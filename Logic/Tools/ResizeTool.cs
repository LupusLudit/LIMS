using LIMS.Logic.ImageLoading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ResizeTool"]/*'/>
    public class ResizeTool : ToolBase
    {
        public int Width
        {
            get { return width; }
            set
            {
                if (InValidRange(value))
                {
                    width = value;
                } 
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                if (InValidRange(value))
                {
                    height = value;
                }
            }
        }

        /*
         * Setting width and height to 0 at the beginning.
         * The program will ignore the value 0 for the width and height
         * and will not perform the resizing until the user selects
         * valid values.
         */
        private int width = 0;
        private int height = 0;

        /// <summary>
        /// Applies resizing to the provided image if tool is enabled and width/height are valid.
        /// </summary>
        /// <param name="image">The target <see cref="ImageDataContainer"/> to modify.</param>
        public override void Apply(ImageDataContainer image)
        {
            if (!Enabled || !InValidRange(width) || !InValidRange(height)) return;

            using (Image resizedImage = Image.Load<Rgba32>(image.RawBytes))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                resizedImage.Mutate(context =>
                {
                    context.Resize(width, height);
                    resizedImage.SaveAsPng(memoryStream);
                    image.RawBytes = memoryStream.ToArray();
                });
            }
        }

        /// <summary>
        /// Checks whether the tool is in a valid state for execution.
        /// </summary>
        /// <param name="errorMessage">Outputs an error message if width or height is invalid.</param>
        /// <returns><c>true</c> if width and height are valid or tool is disabled; otherwise <c>false</c>.</returns>
        public override bool IsInValidState(out string? errorMessage)
        {
            if (Enabled && (!InValidRange(width) || !InValidRange(height)))
            {
                errorMessage = "Width or Height must be greater than zero!";
                return false;
            }
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Checks if the value (for the width/height) is in the valid range (between 1 and 10000).
        /// </summary> 
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if in valid range; otherwise <c>false</c>.</returns>
        private bool InValidRange(int value)
        {
            return value >= 1 && value <= 10000;
        }
    }
}
