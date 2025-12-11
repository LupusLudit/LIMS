using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="BrightnessTool"]/*'/>
    public class BrightnessTool : ToolBase
    {
        public float Brightness {
            get { return brightness; }
            set
            {
                if (value > 0 && value <= 3f)
                {
                    brightness = value;
                }
            }
        }

        private float brightness = 1.0f;

        /// <summary>
        /// Applies brightness adjustment to the provided image if the tool is enabled.
        /// </summary>
        /// <param name="image">The target <see cref="ImageDataContainer"/> to modify.</param>
        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled) return;

            using (Image changedImage = Image.Load<Rgba32>(image.RawBytes))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                changedImage.Mutate(context =>
                {
                    context.Brightness(brightness);
                });

                changedImage.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }

        }

        /// <summary>
        /// Checks if the tool is in a valid state.
        /// Since the input value is always checked it is not possible to force an error.
        /// </summary>
        /// <param name="errorMessage">Outputs an error message if invalid; always <c>null</c> in this tool.</param>
        /// <returns>Always <c>true</c>.</returns>
        public override bool IsInValidState(out string? errorMessage)
        {
            errorMessage = null;
            return true;
        }
    }
}
