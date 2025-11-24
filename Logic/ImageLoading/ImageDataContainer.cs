using System.IO;
using System.Windows.Media.Imaging;

namespace LIMS.Logic.ImageLoading
{
    public class ImageDataContainer
    {
        public string FilePath { get; }

        public BitmapImage? PreviewImage { get; set; }

        public byte[]? RawBytes { get; set; }

        public ImageDataContainer(string filePath)
        {
            FilePath = filePath;
        }
    }
}
