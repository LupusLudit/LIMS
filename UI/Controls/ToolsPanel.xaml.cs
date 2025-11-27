using LIMS.Logic;
using LIMS.Logic.Tools;
using Microsoft.Win32;
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

        public required PreviewPanel PreviewPanelReference { get; set; }

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
                PreviewPanelReference?.RefreshPreview();
            }
        }

        private void DisableWatermark(object sender, RoutedEventArgs e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Enabled = false;
                PreviewPanelReference?.RefreshPreview();
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

        private void WatermarkPositionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (watermarkTool != null && WatermarkPositionComboBox.SelectedItem is ComboBoxItem item)
            {
                string positionText = item.Content.ToString();

                switch (positionText)
                {
                    case "Top-left":
                        watermarkTool.Position = WatermarkPosition.TopLeft;
                        break;
                    case "Top-right":
                        watermarkTool.Position = WatermarkPosition.TopRight;
                        break;
                    case "Bottom-left":
                        watermarkTool.Position = WatermarkPosition.BottomLeft;
                        break;
                    case "Bottom-right":
                        watermarkTool.Position = WatermarkPosition.BottomRight;
                        break;
                    case "Center":
                        watermarkTool.Position = WatermarkPosition.Center;
                        break;
                    default:
                        watermarkTool.Position = WatermarkPosition.Center;
                        break;
                }

                PreviewPanelReference?.RefreshPreview();
            }
        }


        private void OpacitySliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Opacity = (float)e.NewValue;
                PreviewPanelReference?.RefreshPreview();
            }
        }


    }
}
