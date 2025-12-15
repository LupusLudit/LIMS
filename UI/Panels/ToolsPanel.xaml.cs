using LIMS.Debugging;
using LIMS.Logic;
using LIMS.Logic.Tools;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace LIMS.UI.Panels
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolsPanel"]/*'/>
    public partial class ToolsPanel : UserControl
    {
        private TabContext? tabContext;
        private WatermarkTool? watermarkTool;
        private ScaleTool? scaleTool;
        private ResizeTool? resizeTool;
        private FlipTool? flipTool;
        private BrightnessTool? brightnessTool;

        public required TabContext TabContext
        {
            get => tabContext!;
            set
            {
                tabContext = value;
                RegisterTools();
            }
        }

        public required PreviewPanel PreviewPanelReference { get; set; }

        public ToolsPanel() => InitializeComponent();

        /// <summary>
        /// Registers all tools with the TabContext ToolsManager.
        /// </summary>
        private void RegisterTools()
        {
            watermarkTool = tabContext!.ToolsManager.RegisterTool(new WatermarkTool());
            scaleTool = tabContext.ToolsManager.RegisterTool(new ScaleTool());
            resizeTool = tabContext.ToolsManager.RegisterTool(new ResizeTool());
            flipTool = tabContext.ToolsManager.RegisterTool(new FlipTool());
            brightnessTool = tabContext.ToolsManager.RegisterTool(new BrightnessTool());
        }

        /// <summary>
        /// Toggles a tool and refreshes the preview.
        /// </summary>
        private void ToggleTool(ToolBase? tool, bool active)
        {
            if (tool == null) return;

            try
            {
                tool.Enabled = active;
                PreviewPanelReference?.RefreshPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while toggling {tool.GetType().Name}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Safely applies a property change to a tool and refreshes the preview.
        /// </summary>
        private void ApplyToolValue(Action applyAction, string errorMessage)
        {
            try
            {
                applyAction();
                PreviewPanelReference?.RefreshPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.Error($"{errorMessage}: {ex.Message}");
            }
        }

        private void EnableWatermark(object sender, RoutedEventArgs e) => ToggleTool(watermarkTool, true);
        private void DisableWatermark(object sender, RoutedEventArgs e) => ToggleTool(watermarkTool, false);

        private void OnBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            ApplyToolValue(() =>
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
            }, "An error occurred while selecting the watermark image.");
        }

        private void WatermarkPositionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyToolValue(() =>
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
                }
            }, "An error occurred while positioning the watermark.");
        }

        private void OpacitySliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) 
        {
            ApplyToolValue(() => 
            {
                if (watermarkTool != null)
                {
                    watermarkTool.Opacity = Convert.ToSingle(e.NewValue);
                }
            }, "An error occurred while changing the watermark opacity.");
        }

        private void EnableScaling(object sender, RoutedEventArgs e) => ToggleTool(scaleTool, true);
        private void DisableScaling(object sender, RoutedEventArgs e) => ToggleTool(scaleTool, false);

        private void ScaleValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ApplyToolValue(() =>
            {
                if (scaleTool != null && e.NewValue != null)
                {
                    scaleTool.ScaleValue = Convert.ToSingle(e.NewValue);
                }
            }, "An error occurred while changing the scale value.");
        }

        private void EnableResize(object sender, RoutedEventArgs e) => ToggleTool(resizeTool, true);
        private void DisableResize(object sender, RoutedEventArgs e) => ToggleTool(resizeTool, false);

        private void ResizeWidthChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ApplyToolValue(() =>
            {
                if (resizeTool != null && e.NewValue != null)
                {
                    resizeTool.Width = Convert.ToInt32(e.NewValue);
                }
            }, "An error occurred while changing the width.");
        }

        private void ResizeHeightChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ApplyToolValue(() =>
            {
                if (resizeTool != null && e.NewValue != null)
                {
                    resizeTool.Height = Convert.ToInt32(e.NewValue);
                }
            }, "An error occurred while changing the height.");
        }

        private void EnableFlip(object sender, RoutedEventArgs e) => ToggleTool(flipTool, true);
        private void DisableFlip(object sender, RoutedEventArgs e) => ToggleTool(flipTool, false);

        private void FlipHorizontalChanged(object sender, RoutedEventArgs e)
        {
            ApplyToolValue(() =>
            {
                if (flipTool != null && sender is CheckBox comboBox)
                {
                    flipTool.FlipHorizontal = comboBox.IsChecked == true;
                }
            }, "An error occurred while flipping horizontally.");
        }

        private void FlipVerticalChanged(object sender, RoutedEventArgs e)
        {
            ApplyToolValue(() =>
            {
                if (flipTool != null && sender is CheckBox comboBox)
                {
                    flipTool.FlipVertical = comboBox.IsChecked == true;

                }
            }, "An error occurred while flipping vertically.");
        }

        private void EnableBrightness(object sender, RoutedEventArgs e) => ToggleTool(brightnessTool, true);
        private void DisableBrightness(object sender, RoutedEventArgs e) => ToggleTool(brightnessTool, false);

        private void BrightnessChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ApplyToolValue(() =>
            {
                if (brightnessTool != null && e.NewValue != null)
                {
                    brightnessTool.Brightness = Convert.ToSingle(e.NewValue);

                }
            }, "An error occurred while changing the brightness.");
        }

    }
}
