using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace LIMS.Logic.Tools
{
    public class ResizeTool : ToolBase
    {
        public float ResizeValue
        {
            get { return resizeValue; }
            set
            {
                if (value > 0 && value <= 2f)
                {
                    resizeValue = value;
                }
            }
        }
        private float resizeValue = 1f;

        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled) return;
            if (ResizeValue <= 0) return;

            using (Image<Rgba32> originalImage = Image.Load<Rgba32>(image.RawBytes))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int targetWidth = (int)(originalImage.Width * ResizeValue);
                int targetHeight = (int)(originalImage.Height * ResizeValue);

                originalImage.Mutate(context =>
                {
                    context.Resize(targetWidth, targetHeight);
                });

                originalImage.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }
        }

        public override bool IsInValidState(out string? errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
