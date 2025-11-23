using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ActionPanel.xaml
    /// </summary>
    public partial class ActionPanel : UserControl
    {
        public ActionPanel()
        {
            InitializeComponent();
        }

        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };

        public static HashSet<string> LoadedPaths = new HashSet<string>();


        public void OnImportFilesClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"Image Files|*{string.Join(";*", allowedExtensions)}",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                LoadedPaths = dialog.FileNames.ToHashSet();
            }

            foreach (string path in LoadedPaths)
            {
                System.Diagnostics.Debug.WriteLine(path);
            }
        }

        public void OnImportFromFolderClick(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            if (dialog.ShowDialog() == true)
            {
                string? selectedFolder = System.IO.Path.GetDirectoryName(dialog.FolderName);
                if (selectedFolder != null)
                {
                    LoadedPaths = Directory.EnumerateFiles(selectedFolder)
                        .Where(f => allowedExtensions.Contains(System.IO.Path.GetExtension(f).ToLower())).ToHashSet();
                }
            }

            foreach (string path in LoadedPaths)
            {
                System.Diagnostics.Debug.WriteLine(path);
            }
        }

    }
}
