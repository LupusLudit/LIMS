using System.Windows;

namespace LIMS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ActionPanelControl.PreviewPanelReference = PreviewPanelControl;
        }
    }
}