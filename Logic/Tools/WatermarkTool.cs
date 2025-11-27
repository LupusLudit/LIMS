using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;


namespace LIMS.Logic.Tools
{
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

        private float opacity;
        private string? watermarkPath = null;

        public WatermarkTool(): base()
        {
            Opacity = 0.7f;
        }

        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled || watermarkPath == null) return;

            using (Image<Rgba32> original = Image.Load<Rgba32>(image.RawBytes))
            using (Image<Rgba32> watermark = Image.Load<Rgba32>(watermarkPath))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int x = (original.Width - watermark.Width) / 2;
                int y = (original.Height - watermark.Height) / 2;

                original.Mutate(context =>
                {
                    context.DrawImage(watermark, new Point(x, y), opacity);
                });

                original.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }
        }

        public override bool IsInValidState(out string? errorMessage)
        {
            if (!Enabled || (Enabled && !string.IsNullOrEmpty(watermarkPath))) 
            {
                errorMessage = null;
                return true;
            }
            else
            {
                errorMessage = "No watermark image has been selected!";
                return false;
            }
        }
    }
}
