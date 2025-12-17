using LIMS.Debugging;
using LIMS.Logic;
using LIMS.Safety;
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

        /// <summary>
        /// Retrieves the size of the font.
        /// </summary>
        /// <returns>The font size (double).</returns>
        private double GetAppFontSize()
        {
            if (Application.Current.MainWindow != null)
            {
                return (double)Application.Current.Resources["GlobalFontSize"];
            }
            return 12.0;
        }

        /// <summary>
        /// Performs the zoom in action (making the font larger) on click.
        /// </summary>
        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            SafeExecutor.Execute(
                () =>
                {
                    double fontSize = GetAppFontSize();
                    if (fontSize < 22)
                    {
                        Application.Current.Resources["GlobalFontSize"] = fontSize + 1;
                    }
                },
                "An error occurred while zooming in."
            );
        }

        /// <summary>
        /// Performs the zoom out action (making the font smaller) on click.
        /// </summary>
        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {

            SafeExecutor.Execute(
                () =>
                {
                    double fontSize = GetAppFontSize();
                    if (fontSize > 10.0)
                    {
                        Application.Current.Resources["GlobalFontSize"] = fontSize - 1;
                    }
                },
                "An error occurred while zooming out."
            );
        }

        /// <summary>
        /// Resets the zoom value to default size on click.
        /// </summary>
        private void ZoomResetClick(object sender, RoutedEventArgs e)
        {
            SafeExecutor.Execute(
                () =>
                {
                    Application.Current.Resources["GlobalFontSize"] = 16.0;
                },
                "An error occurred while resetting the zoom value."
            );
        }
    }
}
