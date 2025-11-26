using LIMS.Logic.ImageLoading;


namespace LIMS.Logic.Core
{
    public class CentralStorage
    {
        private static Lazy<CentralStorage> instance = new Lazy<CentralStorage>(() => new CentralStorage());

        public static CentralStorage Instance
        { 
            get { return instance.Value; }
        }

        private readonly object lockObject = new object();

        private Dictionary<string, ImageDataContainer> images = new Dictionary<string, ImageDataContainer>();

        CentralStorage() { }

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

        public List<ImageDataContainer> GetAllImages()
        {
            lock (lockObject)
            {
                return images.Values.ToList();
            }
        }
    }
}
