using LIMS.Logic.ImageLoading;

namespace LIMS.Logic.Core
{
    public class DataStorage
    {
        private readonly object lockObject = new object();

        private Dictionary<string, ImageDataContainer> images = new Dictionary<string, ImageDataContainer>();

        public DataStorage() { }

        public void AddImage(ImageDataContainer image)
        {
            lock (lockObject)
            {
                if (!images.ContainsKey(image.FilePath))
                {
                    images[image.FilePath] = image;
                }
            }
        }

        public void TryGetImage(string filePath, out ImageDataContainer? image)
        {
            lock (lockObject)
            {
                images.TryGetValue(filePath, out image);
            }
        }

        public void Clear()
        {
            lock (lockObject)
            {
                images.Clear();
            }
        }

        public List<ImageDataContainer> GetAllImages()
        {
            lock (lockObject)
            {
                return images.Values.ToList();
            }
        }
    }
}
