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
        public List<ImageDataContainer> LoadedImages = new List<ImageDataContainer>();

        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };

        public ActionPanel()
        {
            InitializeComponent();
        }

        public void OnImportFilesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"Image Files|*{string.Join(";*", allowedExtensions)}",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                LoadedImages = dialog.FileNames.Select(path => new ImageDataContainer(path)).ToList();
                ShowLoadedImagesNamesInUI();
            }
        }

        public void OnImportFromFolderClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = Path.GetDirectoryName(dialog.FolderName);
                if (selectedFolder != null)
                {
                    LoadedImages = Directory.EnumerateFiles(selectedFolder)
                        .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                        .Select(path => new ImageDataContainer(path)).ToList();

                    ShowLoadedImagesNamesInUI();
                }
            }
        }

        private void ShowLoadedImagesNamesInUI()
        {
            HashSet<string> loadedPaths = LoadedImages.Select(i => i.FilePath).ToHashSet();
            PreviewPanelReference.LoadImageNames(loadedPaths);
        }

    }
}
