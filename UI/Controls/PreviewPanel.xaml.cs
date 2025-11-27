using System.Windows.Controls;
using LIMS.Logic;
using LIMS.Logic.ImageLoading;
using System.IO;
using LIMS.Vendor;


namespace LIMS.UI.Controls
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="PreviewPanel"]/*'/>
    public partial class PreviewPanel : UserControl
    {
        private HashSet<string> filePaths = new HashSet<string>();
        private TabContext? tabContext;
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
            ImageSelectionChanged(this, null);
        }

        /// <summary>
        /// Handles the selection change in the image list and updates the preview image.
        /// Applies all enabled tools from <see cref="TabContext.ToolsManager"/> to the selected image.
        /// </summary>
        /// <param name="sender">The source of the selection change event.</param>
        /// <param name="e">The selection changed event arguments.</param>
        private void ImageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0 && tabContext != null)
            {
                string fileName = ImageListBox.SelectedItem.ToString()!;
                string fullPath = filePaths.FirstOrDefault(file => Path.GetFileName(file) == fileName)!;

                if (tabContext != null)
                {
                    tabContext.Storage.TryGetImage(fullPath, out ImageDataContainer? image);

                    if (image != null && image.RawBytes != null)
                    {
                        byte[] previewBytes = (byte[])image.RawBytes.Clone();

                        foreach (var tool in tabContext.ToolsManager.Tools)
                        {
                            if (tool.Enabled)
                            {
                                var temp = new ImageDataContainer(fullPath) { RawBytes = previewBytes };
                                tool.Apply(temp);
                                previewBytes = temp.RawBytes!;
                            }
                        }

                        PreviewImage.Source = BitmapLoader.LoadBitmapImage(previewBytes);
                    }

                }
            }
        }
    }
}
