using LIMS.Logic;
using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ActionPanel.xaml
    /// </summary>
    public partial class ActionPanel : UserControl
    {
        public required PreviewPanel PreviewPanelReference { get; set; }
        public required TabContext TabContext { get; set; }

        public ActionPanel()
        {
            InitializeComponent();
        }

        public async void OnImportFilesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"Image Files|*{string.Join(";*", TabContext.AllowedExtensions)}",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                await ImageLoader.LoadImagesAsync(dialog.FileNames, TabContext.Storage);

                SendPathsToPreviewPanel(dialog.FileNames);
            }
        }

        public async void OnImportFromFolderClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = Path.GetDirectoryName(dialog.FolderName);
                if (selectedFolder != null)
                {
                    List<string> loadedPaths = Directory.EnumerateFiles(selectedFolder)
                        .Where(file => TabContext.AllowedExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();

                    await ImageLoader.LoadImagesAsync(loadedPaths, TabContext.Storage);
                    
                    SendPathsToPreviewPanel(loadedPaths);
                }
            }
        }

        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            string? errorMessage = null;
            if (!TabContext.ToolsInValidStates(out errorMessage))
            {
                MessageBox.Show(errorMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OpenFolderDialog dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                string destinationFolder = dialog.FolderName;
                if (!string.IsNullOrEmpty(destinationFolder))
                {
                    TabContext.ProcessAllTools();
                    SaveAllImages(destinationFolder);
                    TabContext.Storage.Clear();

                    MessageBox.Show("Images exported successfully!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void SendPathsToPreviewPanel(IEnumerable<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                PreviewPanelReference.AddImage(path);
            }
        }

        private void SaveAllImages(string folder)
        {
            foreach (var img in TabContext.Storage.GetAllImages())
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
