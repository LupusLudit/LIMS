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

        public async void OnImportFilesClick(object sender, RoutedEventArgs e)
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

        public async void OnImportFromFolderClick(object sender, RoutedEventArgs e)
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

        public void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            string? errorMessage = null;
            if (!tabContext.ToolsInValidStates(out errorMessage))
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
                    tabContext.ProcessAllTools();
                    SaveAllImages(destinationFolder);
                    tabContext.Storage.Clear();
                    PreviewPanelReference.ClearImages();

                    MessageBox.Show("Images exported successfully!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            tabContext!.Storage.Clear();
            PreviewPanelReference.ClearImages();
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
