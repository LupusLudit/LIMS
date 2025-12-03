using LIMS.Debugging;
using LIMS.Logic;
using LIMS.Logic.ImageLoading;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Panels
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ActionPanel"]/*'/>
    public partial class ActionPanel : UserControl
    {
        public required PreviewPanel PreviewPanelReference { get; set; }

        private TabContext? tabContext;
        public required TabContext TabContext
        {
            get { return tabContext!; }
            set
            {
                tabContext = value;
            }
        }

        public ActionPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for importing individual image files.
        /// Opens a file dialog for user selection, loads the images into <see cref="TabContext.Storage"/>,
        /// and updates the <see cref="PreviewPanel"/>.
        /// </summary>
        public async void OnImportFilesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = $"Image Files|*{string.Join(";*", tabContext!.AllowedExtensions)}",
                    Multiselect = true
                };

                if (dialog.ShowDialog() == true)
                {
                    await ImageLoader.LoadImagesAsync(dialog.FileNames, tabContext.Storage);

                    SendPathsToPreviewPanel(dialog.FileNames);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while importing images.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Handles the click event for importing images from a folder.
        /// Filters images by allowed extensions, loads them into <see cref="TabContext.Storage"/>,
        /// and updates the <see cref="PreviewPanel"/>.
        /// </summary>
        public async void OnImportFromFolderClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFolderDialog dialog = new OpenFolderDialog();

                if (dialog.ShowDialog() == true)
                {
                    string? selectedFolder = dialog.FolderName;
                    if (selectedFolder != null)
                    {
                        List<string> loadedPaths = Directory.EnumerateFiles(selectedFolder)
                            .Where(file => tabContext!.AllowedExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();

                        await ImageLoader.LoadImagesAsync(loadedPaths, tabContext!.Storage);

                        SendPathsToPreviewPanel(loadedPaths);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while importing images from this folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Handles the click event for the Start button.
        /// Validates tools, processes all images, saves them to the selected folder,
        /// clears storage, and updates the preview panel.
        /// </summary>
        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            string? errorMessage = null;
            if (!tabContext.ToolsInValidStates(out errorMessage))
            {
                MessageBox.Show(errorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Logger.Warning(errorMessage!);
                return;
            }

            try
            {
                OpenFolderDialog dialog = new OpenFolderDialog();
                if (dialog.ShowDialog() == true)
                {
                    string destinationFolder = dialog.FolderName;
                    if (!string.IsNullOrEmpty(destinationFolder))
                    {
                        tabContext.ProcessAllTools();
                        SaveAllImages(destinationFolder);
                        tabContext.Storage.Clear();
                        PreviewPanelReference.ClearImages();

                        MessageBox.Show("Images exported successfully!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred during image processing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error(ex.Message);
            }

        }

        /// <summary>
        /// Handles the click event for the Clear button.
        /// Clears all images from <see cref="TabContext.Storage"/> and the <see cref="PreviewPanel"/>.
        /// </summary>
        public void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                tabContext!.Storage.Clear();
                PreviewPanelReference.ClearImages();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("An error occurred while clearing the images.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Sends the paths of newly imported images to the <see cref="PreviewPanel"/> for display.
        /// </summary>
        /// <param name="filePaths">The paths of the images to add to the preview panel.</param>
        private void SendPathsToPreviewPanel(IEnumerable<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                PreviewPanelReference.AddImage(path);
            }
        }

        /// <summary>
        /// Saves all images currently stored in <see cref="TabContext.Storage"/> to the specified folder.
        /// </summary>
        /// <param name="folder">The destination folder where images will be saved.</param>
        private void SaveAllImages(string folder)
        {
            foreach (var img in tabContext!.Storage.GetAllImages())
            {
                string fileName = Path.GetFileName(img.FilePath);
                string destinationPath = Path.Combine(folder, fileName);

                if (img.RawBytes != null)
                {
                    File.WriteAllBytes(destinationPath, img.RawBytes);
                }
            }
        }

    }
}
