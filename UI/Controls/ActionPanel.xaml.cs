using LIMS.Logic.ImageLoading;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ActionPanel.xaml
    /// </summary>
    public partial class ActionPanel : UserControl
    {
        public required PreviewPanel PreviewPanelReference { get; set; }

        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };

        public ActionPanel()
        {
            InitializeComponent();
        }

        public async void OnImportFilesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"Image Files|*{string.Join(";*", allowedExtensions)}",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                await ImageLoader.LoadImagesAsync(dialog.FileNames);

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
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();

                    await ImageLoader.LoadImagesAsync(loadedPaths);
                    
                    SendPathsToPreviewPanel(loadedPaths);
                }
            }
        }

        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SendPathsToPreviewPanel(IEnumerable<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                PreviewPanelReference.AddImage(path);
            }
        }

    }
}
