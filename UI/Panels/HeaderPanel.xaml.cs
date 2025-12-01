using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Panels
{

    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="HeaderPanel"]/*'/>

    public partial class HeaderPanel : UserControl
    {
        public HeaderPanel()
        {
            InitializeComponent();
        }

        private void PreferencesClick(object sender, RoutedEventArgs e)
        {
            if (Resources["PreferencesMenu"] is ContextMenu menu)
            {
                menu.PlacementTarget = PreferencesButton;
                menu.IsOpen = true;
            }
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
