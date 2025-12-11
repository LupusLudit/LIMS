using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="FlipTool"]/*'/>
    public class FlipTool : ToolBase
    {
        public bool FlipHorizontal
        {
            get { return flipHorizontal; }
            set { flipHorizontal = value; }
        }

        public bool FlipVertical
        {
            get { return flipVertical; }
            set { flipVertical = value; }
        }

        private bool flipHorizontal = false;
        private bool flipVertical = false;

        /// <summary>
        /// Applies flipping to the provided image if the tool is enabled.
        /// </summary>
        /// <param name="image">The target <see cref="ImageDataContainer"/> to modify.</param>
        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled || (!flipHorizontal && !flipVertical)) return;

            using (Image<Rgba32> img = Image.Load<Rgba32>(image.RawBytes))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                img.Mutate(context =>
                {
                    if (flipHorizontal)
                    {
                        context.Flip(SixLabors.ImageSharp.Processing.FlipMode.Horizontal);

                    }

                    if (flipVertical) 
                    {
                        context.Flip(SixLabors.ImageSharp.Processing.FlipMode.Vertical);
                    }
                });

                img.SaveAsPng(memoryStream);
                image.RawBytes = memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Checks if at least one flip direction is enabled when the tool is active.
        /// </summary>
        /// <param name="errorMessage">Outputs an error message if no direction is selected.</param>
        /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
        public override bool IsInValidState(out string? errorMessage)
        {
            if (Enabled && !flipHorizontal && !flipVertical)
            {
                errorMessage = "At least one flip direction must be enabled!";
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}
