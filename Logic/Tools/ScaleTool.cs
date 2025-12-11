using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ScaleTool"]/*'/>
    public class ScaleTool : ToolBase
    {
        public float ScaleValue
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

        /// <summary>
        /// Applies scaling to the provided image if the tool is enabled.
        /// </summary>
        /// <param name="image">The target <see cref="ImageDataContainer"/> to modify.</param>
        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled) return;

            using (Image<Rgba32> originalImage = Image.Load<Rgba32>(image.RawBytes))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int targetWidth = (int)(originalImage.Width * ScaleValue);
                int targetHeight = (int)(originalImage.Height * ScaleValue);

                originalImage.Mutate(context =>
                {
                    context.Resize(targetWidth, targetHeight);
                });

                originalImage.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Determines whether the tool is in a valid state.
        /// </summary>
        /// <param name="errorMessage">Outputs an error message if the tool is invalid.</param>
        /// <returns>Always <c>true</c> because any valid scale is acceptable.</returns>
        public override bool IsInValidState(out string? errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
