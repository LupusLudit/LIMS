using LIMS.Logic;
using System.Windows;

namespace LIMS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /*
        In the future, more tabs could be opened at the same time.
        Tab context will provide DataStorage and ToolProcessor for each tab.
        In the future there will be a list of TabContexts and a way to switch between them.
         */
        private TabContext tabContext;
        public MainWindow()
        {
            InitializeComponent();
            tabContext = new TabContext();

            ActionPanelControl.PreviewPanelReference = PreviewPanelControl;
            ActionPanelControl.TabContext = tabContext;

            ToolsPanelControl.PreviewPanelReference = PreviewPanelControl;
            ToolsPanelControl.TabContext = tabContext;

            PreviewPanelControl.TabContext = tabContext;
        }
    }
}