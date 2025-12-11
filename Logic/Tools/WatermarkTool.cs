using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;


namespace LIMS.Logic.Tools
{
    public enum WatermarkPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center,
    }

    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolsManager"]/*'/>
    public class WatermarkTool : ToolBase
    {
        public string WatermarkPath
        { 
            get { return watermarkPath!; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    watermarkPath = value;
                }
            }
        }
        public float Opacity
        {
            get { return opacity; }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    opacity = value;
                }
            } 
        }

        public WatermarkPosition Position { get; set; } = WatermarkPosition.Center;

        private float opacity;
        private string? watermarkPath = null;

        public WatermarkTool(): base()
        {
            Opacity = 0.7f;
        }

        /// <summary>
        /// Applies the watermark to the provided <see cref="ImageDataContainer"/>.
        /// </summary>
        /// <param name="image">The target image on which the watermark should be applied.</param>
        /// <remarks>
        /// The watermark is resized proportionally to fit within a quarter of the target image.
        /// </remarks>
        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled || watermarkPath == null) return;

            using (Image<Rgba32> originalBaseImage = Image.Load<Rgba32>(image.RawBytes))
            using (Image<Rgba32> originalWatermarkImage = Image.Load<Rgba32>(watermarkPath))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int maxWatermarkWidth = originalBaseImage.Width / 4;
                int maxWatermarkHeight = originalBaseImage.Height / 4;

                float widthScalingFactor = (float)maxWatermarkWidth / originalWatermarkImage.Width;
                float heightScalingFactor = (float)maxWatermarkHeight / originalWatermarkImage.Height;

                float scalingFactor = Math.Min(widthScalingFactor, heightScalingFactor);

                Image<Rgba32> watermark = originalWatermarkImage.Clone(context =>
                {
                    if (scalingFactor <= 1.0f)
                    {
                        int resizedWidth = (int) (originalWatermarkImage.Width * scalingFactor);
                        int resizedHeight = (int)(originalWatermarkImage.Height * scalingFactor);
                        context.Resize(resizedWidth, resizedHeight);
                    }
                });

                int x = calculateWatermarkXPosition(originalBaseImage, watermark);
                int y = calculateWatermarkYPosition(originalBaseImage, watermark);

                originalBaseImage.Mutate(context =>
                {
                    context.DrawImage(watermark, new Point(x, y), opacity);
                });

                originalBaseImage.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Calculates the X coordinate for the watermark based on <see cref="Position"/>.
        /// </summary>
        /// <param name="original">The base image.</param>
        /// <param name="watermark">The watermark image.</param>
        /// <returns>The X coordinate where the watermark should be placed.</returns>
        private int calculateWatermarkXPosition(Image<Rgba32> original, Image<Rgba32> watermark)
        {
            int x = Position switch
            {
                WatermarkPosition.TopLeft => 0,
                WatermarkPosition.TopRight => original.Width - watermark.Width,
                WatermarkPosition.BottomLeft => 0,
                WatermarkPosition.BottomRight => original.Width - watermark.Width,
                WatermarkPosition.Center => (original.Width - watermark.Width) / 2,
                _ => 0
            };
            return x;
        }

        /// <summary>
        /// Calculates the Y coordinate for the watermark based on <see cref="Position"/>.
        /// </summary>
        /// <param name="original">The base image.</param>
        /// <param name="watermark">The watermark image.</param>
        /// <returns>The Y coordinate where the watermark should be placed.</returns>
        private int calculateWatermarkYPosition(Image<Rgba32> original, Image<Rgba32> watermark)
        {
            int y = Position switch
            {
                WatermarkPosition.TopLeft => 0,
                WatermarkPosition.TopRight => 0,
                WatermarkPosition.BottomLeft => original.Height - watermark.Height,
                WatermarkPosition.BottomRight => original.Height - watermark.Height,
                WatermarkPosition.Center => (original.Height - watermark.Height) / 2,
                _ => 0
            };
            return y;
        }


        /// <summary>
        /// Checks whether the tool is in a valid state for execution.
        /// </summary>
        /// <param name="errorMessage"> Outputs an error message if the tool is not ready; otherwise <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the tool is enabled and a watermark path has been set; otherwise <c>false</c>.
        /// </returns>
        public override bool IsInValidState(out string? errorMessage)
        {
            if (Enabled && string.IsNullOrEmpty(watermarkPath)) 
            {
                errorMessage = "No watermark image has been selected!";
                return false;
            }
            errorMessage = null;
            return true;
        }
    }
}
