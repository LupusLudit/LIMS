using LIMS.Logic;
using LIMS.Logic.Tools;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Controls
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolsPanel"]/*'/>
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

        /// <summary>
        /// Enables the watermark tool and refreshes the preview.
        /// </summary>
        private void EnableWatermark(object sender, RoutedEventArgs e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Enabled = true;
                PreviewPanelReference?.RefreshPreview();
            }
        }

        /// <summary>
        /// Disables the watermark tool and refreshes the preview.
        /// </summary>
        private void DisableWatermark(object sender, RoutedEventArgs e)
        {
            if (watermarkTool != null)
            {
                watermarkTool.Enabled = false;
                PreviewPanelReference?.RefreshPreview();
            }
        }


        /// <summary>
        /// Handles the Browse button click to select a watermark image file.
        /// Sets the selected file as the watermark path for <see cref="WatermarkTool"/>.
        /// </summary>
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

        /// <summary>
        /// Handles changes in the watermark position ComboBox.
        /// Updates <see cref="WatermarkTool.Position"/> and refreshes the preview.
        /// </summary>
        private void WatermarkPositionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (watermarkTool != null && WatermarkPositionComboBox.SelectedItem is ComboBoxItem item)
            {
                watermarkTool.Position = item.Content.ToString() switch
                {
                    "Top-left" => WatermarkPosition.TopLeft,
                    "Top-right" => WatermarkPosition.TopRight,
                    "Bottom-left" => WatermarkPosition.BottomLeft,
                    "Bottom-right" => WatermarkPosition.BottomRight,
                    "Center" => WatermarkPosition.Center,
                    _ => WatermarkPosition.Center,
                };

                PreviewPanelReference?.RefreshPreview();
            }
        }

        /// <summary>
        /// Handles changes in the opacity slider.
        /// Updates <see cref="WatermarkTool.Opacity"/> and refreshes the preview.
        /// </summary>
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
