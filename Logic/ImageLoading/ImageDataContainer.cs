namespace LIMS.Logic.ImageLoading
{
    public class ImageDataContainer
    {
        public string FilePath { get; }

        public byte[]? RawBytes { get; set; }

        public ImageDataContainer(string filePath)
        {
            FilePath = filePath;
        }
    }
}
