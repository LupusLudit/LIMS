using System.Windows.Controls;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for PreviewPanel.xaml
    /// </summary>
    public partial class PreviewPanel : UserControl
    {
        public PreviewPanel()
        {
            InitializeComponent();
        }

        public void LoadImageNames(HashSet<string> imagePaths)
        {
            ImageListBox.ItemsSource = imagePaths.Select(System.IO.Path.GetFileName);
            ImageListBox.SelectedIndex = 0;
        }
    }
}
