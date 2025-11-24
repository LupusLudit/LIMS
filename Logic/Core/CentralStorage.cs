using LIMS.Logic.ImageLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Logic.Core
{
    public class CentralStorage
    {

        private static CentralStorage? instance = null;

        public static CentralStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CentralStorage();
                }
                return instance;
            }
        }
        private readonly Dictionary<string, ImageDataContainer> images = new Dictionary<string, ImageDataContainer>();
        private readonly object lockObject = new object();
        CentralStorage() 
        {
        }

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

        public IEnumerable<ImageDataContainer> GetAllImages()
        {
            lock (lockObject)
            {
                return images.Values.ToList();
            }
        }
    }
}
