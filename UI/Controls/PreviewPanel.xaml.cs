using System.Windows.Controls;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl
    {

        private HashSet<string> filePaths = new HashSet<string>();
        public PreviewPanel()
        {
            InitializeComponent();
        }

        private void LoadImageNamesIntoUI()
        {
            ImageListBox.ItemsSource = filePaths.Select(System.IO.Path.GetFileName);
            ImageListBox.SelectedIndex = 0;
        }

        public void AddImage(string filePath)
        {
            if (filePath != null)
            {
                filePaths.Add(filePath);
                LoadImageNamesIntoUI();
            }
        }
    }
}
