using LIMS.Debugging;
using LIMS.Logic;
using LIMS.Logic.Events;
using LIMS.Logic.ImageLoading;
using LIMS.Vendor;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LIMS.UI.Panels
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="PreviewPanel"]/*'/>
    public partial class PreviewPanel : UserControl
    {
        private ObservableCollection<ImageListItem> imageListItems = new();

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
            ImageListBox.ItemsSource = imageListItems;
            ImageListBox.SelectionChanged += ImageSelectionChanged;
        }


        /// <summary>
        /// Adds an image to the preview panel and updates the UI list.
        /// </summary>
        /// <param name="fullPath">The path to  the image to add.</param>
        public void AddImage(string fullPath)
        {
            imageListItems.Add(new ImageListItem(fullPath));
        }


        /// <summary>
        /// Clears all images from the panel and resets the preview display.
        /// </summary>
        public void ClearImages()
        {
            imageListItems.Clear();
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
        /// Deletes an image specified by the user in the UI.
        /// </summary>
        private void DeleteSpecificImage(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.DataContext is ImageListItem item && tabContext != null)
                {
                    tabContext.Storage.TryRemoveImage(item.FullPath, out bool removed);
                    if (removed)
                    {
                        imageListItems.Remove(item);
                        UpdatePreviewAsync();
                    }
                    else
                    {
                        string warningMessage = "Attempting to remove a none existing image.";
                        MessageBox.Show(warningMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        Logger.Warning(warningMessage);
                    }
                    imageListItems.Remove(item);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("An error occurred while attempting to remove an image.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error($"An error occurred while attempting to remove an image: {ex.Message}");
            }
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
                if (ImageListBox.SelectedIndex < 0 || tabContext == null)
                {
                    PreviewImage.Source = null;
                    return;
                }

                ImageListItem? selected = ImageListBox.SelectedItem as ImageListItem;
                string? fullPath = selected?.FullPath;
                if (selected == null || fullPath == null)
                {
                    PreviewImage.Source = null;
                    return;
                }

                tabContext.Storage.TryGetImage(fullPath, out ImageDataContainer? sourceImage);

                if (sourceImage != null && sourceImage.RawBytes != null)
                {
                    byte[] originalBytes = (byte[])sourceImage.RawBytes.Clone();
                    var toolsToApply = tabContext.ToolsManager.Tools.Where(t => t.Enabled).ToList();

                    BusyStateChangedEvent.RaiseBusyStateChanged(true);

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
                MessageBox.Show("An error occurred while updating the image preview.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error($"Error while updating the image preview: {ex.Message}");
            }
            finally
            {
                BusyStateChangedEvent.RaiseBusyStateChanged(false);
            }
        }
    }

    public class ImageListItem
    {
        public string FullPath { get; }
        public string FileName { get; }

        public ImageListItem(string fullPath)
        {
            FullPath = fullPath;
            FileName = Path.GetFileName(fullPath);
        }
    }

}