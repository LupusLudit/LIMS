using System.Windows.Controls;
using LIMS.Logic;
using LIMS.Logic.ImageLoading;
using System.IO;
using LIMS.Vendor;
using LIMS.Debugging;
using System.Windows.Media.Imaging;

namespace LIMS.UI.Panels
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="PreviewPanel"]/*'/>
    public partial class PreviewPanel : UserControl
    {
        private HashSet<string> filePaths = new HashSet<string>();
        private TabContext? tabContext;

        private CancellationTokenSource? cancellationTokenSource;

        public required TabContext TabContext
        {
            get { return tabContext!; }
            set
            {
                tabContext = value;
            }
        }

        public PreviewPanel()
        {
            InitializeComponent();
            ImageListBox.SelectionChanged += ImageSelectionChanged;
        }

        /// <summary>
        /// Updates the UI list with the names of the currently loaded images.
        /// Automatically selects the first image if any exist.
        /// </summary>
        private void LoadImageNamesIntoUI()
        {
            ImageListBox.ItemsSource = filePaths.Select(Path.GetFileName).ToList();
            if (ImageListBox.Items.Count > 0) ImageListBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Adds an image to the preview panel and updates the UI list.
        /// </summary>
        /// <param name="filePath">The full file path of the image to add.</param>
        public void AddImage(string filePath)
        {
            if (filePath != null)
            {
                filePaths.Add(filePath);
                LoadImageNamesIntoUI();
            }
        }

        /// <summary>
        /// Clears all images from the panel and resets the preview display.
        /// </summary>
        public void ClearImages()
        {
            filePaths.Clear();
            LoadImageNamesIntoUI();
            PreviewImage.Source = null;
        }

        /// <summary>
        /// Refreshes the preview image by re-applying all enabled tools to the currently selected image.
        /// </summary>
        public void RefreshPreview()
        {
            UpdatePreviewAsync();
        }

        /// <summary>
        /// Handles the selection change in the image list.
        /// </summary>
        private void ImageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePreviewAsync();
        }

        /// <summary>
        /// Asynchronously updates the preview image.
        /// Cancels any previous ongoing updates to ensure UI responsiveness.
        /// </summary>
        private async void UpdatePreviewAsync()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            try
            {
                if (ImageListBox.SelectedIndex < 0 || tabContext == null) return;

                string? fileName = ImageListBox.SelectedItem?.ToString();
                string fullPath = filePaths.FirstOrDefault(file => Path.GetFileName(file) == fileName)!;
                if (fileName == null || fullPath == null) return;

                tabContext.Storage.TryGetImage(fullPath, out ImageDataContainer? sourceImage);

                if (sourceImage != null && sourceImage.RawBytes != null)
                {
                    byte[] originalBytes = (byte[])sourceImage.RawBytes.Clone();
                    var toolsToApply = tabContext.ToolsManager.Tools.Where(t => t.Enabled).ToList();
                    BitmapImage? resultImage = await Task.Run(() =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            return null;
                        }

                        ImageDataContainer tempContainer = new ImageDataContainer(fullPath, originalBytes);

                        foreach (var tool in toolsToApply)
                        {
                            if (token.IsCancellationRequested)
                            {
                                return null;
                            }

                            tool.Apply(tempContainer);
                        }

                        if (tempContainer.RawBytes == null || token.IsCancellationRequested)
                        {
                            return null;
                        }

                        return BitmapLoader.LoadBitmapImage(tempContainer.RawBytes);

                    }, CancellationToken.None);

                    if (resultImage != null && !token.IsCancellationRequested)
                    {
                        PreviewImage.Source = resultImage;
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Logger.Error($"Error updating preview: {ex.Message}");
            }
        }
    }
}