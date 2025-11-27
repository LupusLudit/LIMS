using LIMS.Logic;
using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Controls
{
    /// <summary>
    /// Interaction logic for ToolsPanel.xaml
    /// </summary>
    public partial class ToolsPanel : UserControl
    {
        private TabContext? tabContext;
        public required TabContext TabContext
        {
            get { return tabContext!; }
            set
            {
                tabContext = value;
                watermarkTool = tabContext.ToolsManager.RegisterTool(new WatermarkTool());
            }
        }

        private WatermarkTool? watermarkTool;
        public ToolsPanel()
        {
            InitializeComponent();
        }

        private void EnableWatermark(object sender, RoutedEventArgs e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Enabled = true;
            }
        }

        private void DisableWatermark(object sender, RoutedEventArgs e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Enabled = false;
            }
        }

        public void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = $"Image Files|*{string.Join(";*", TabContext.AllowedExtensions)}",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true && watermarkTool != null)
            {
                watermarkTool.WatermarkPath = dialog.FileName;
            }
        }

        private void OpacitySliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Opacity = (float)e.NewValue;    
            }
        }


    }
}
