using System.IO;
using System.Windows.Media.Imaging;

namespace LIMS.Vendor
{
    /// <summary>
    /// This class contains externally sourced (non-authored) code.
    /// </summary>
    public static class BitmapLoader
    {   
        /// <summary>
        /// Copied / adapted from StackOverflow – isolated to satisfy the project rules: https://stackoverflow.com/questions/5346727/convert-memory-stream-to-bitmapimage
        /// Loads the bitmap image.
        /// </summary>
        /// <param name="imageBytes">The image bytes.</param>
        /// <returns>
        /// The bitmap image.
        /// </returns>
        public static BitmapImage LoadBitmapImage(byte[] imageBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
