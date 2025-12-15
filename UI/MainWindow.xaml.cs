using LIMS.Logic;
using System.Windows;
using LIMS.UI.Panels;
using LIMS.Logic.Events;

namespace LIMS.UI
{
    /// <include file='../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="MainWindow"]/*'/>
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
            BusyStateChangedEvent.OnBusyStateChanged += SetBusyBar;

            ActionPanelControl.PreviewPanelReference = PreviewPanelControl;
            ActionPanelControl.TabContext = tabContext;

            ToolsPanelControl.PreviewPanelReference = PreviewPanelControl;
            ToolsPanelControl.TabContext = tabContext;

            PreviewPanelControl.TabContext = tabContext;

        }

        /// <summary>
        /// Assigns certain values to the global busy bar.
        /// </summary>
        /// <param name="isBusy">if set to <c>true</c> busy bar is shown; otherwise it will be hidden.</param>
        /// <param name="message">The text content to be displayed inside of the busy bar.</param>
        public void SetBusyBar(bool isBusy, string message = "Processing...")
        {
            Dispatcher.Invoke(() =>
            {
                GlobalBusyBar.IsBusy = isBusy;
                GlobalBusyBar.BusyContent = message;
            });
        }

        /// <summary>
        /// OnClosed override ensuring that the <see cref="BusyStateChangedEvent"/>
        /// unsubscribes from the SetBusyBar method.
        /// This method exists in case UI resets will be necessary
        /// or in case new Windows will have to be added in the future.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            BusyStateChangedEvent.OnBusyStateChanged -= SetBusyBar;
            base.OnClosed(e);
        }


    }
}