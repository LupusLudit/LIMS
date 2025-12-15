using LIMS.Debugging;
using LIMS.Logic.ImageLoading;

namespace LIMS.Logic.Core
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="DataStorage"]/*'/>
    public class DataStorage
    {
        private readonly object lockObject = new object();

        private Dictionary<string, ImageDataContainer> images = new Dictionary<string, ImageDataContainer>();

        public DataStorage() { }

        /// <summary>
        /// Adds an image to the storage (images dictionary) if it has not been added before.
        /// </summary>
        /// <param name="image">The <see cref="ImageDataContainer"/> instance to store.</param>
        /// <remarks>
        /// If an image with the same file path already exists, the method does nothing.
        /// Thread-safe.
        /// </remarks>

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

        /// <summary>
        /// Attempts to retrieve an image from storage using its file path.
        /// </summary>
        /// <param name="filePath">The key used to find the stored image.</param>
        /// <param name="image">The retrieved <see cref="ImageDataContainer"/> instance, or null if not found.</param>
        /// <remarks>
        /// Thread-safe.
        /// </remarks>
        public void TryGetImage(string filePath, out ImageDataContainer? image)
        {
            lock (lockObject)
            {
                images.TryGetValue(filePath, out image);
            }
        }

        /// <summary>
        /// Removes all stored images from memory.
        /// </summary>
        /// <remarks>
        /// Thread-safe.
        /// </remarks>
        public void Clear()
        {
            lock (lockObject)
            {
                images.Clear();
            }
        }

        /// <summary>
        /// Tries to remove an image from the storage.
        /// Returns a boolean providing information
        /// about the success of the operation.
        /// </summary>
        /// <param name="filePath">The file path to the image to be deleted</param>
        /// <param name="removed">if set to <c>true</c> the image was successfully removed; otherwise <c>false</c>.</param>
        public void TryRemoveImage(string filePath, out bool removed)
        {
            lock (lockObject)
            {
                removed = images.Remove(filePath);
            }
        }

        /// <summary>
        /// Retrieves a list of all stored images.
        /// </summary>
        /// <returns>A list containing all <see cref="ImageDataContainer"/> objects currently stored.</returns>
        /// <remarks>
        /// Thread-safe.
        /// </remarks>
        public List<ImageDataContainer> GetAllImages()
        {
            lock (lockObject)
            {
                return images.Values.ToList();
            }
        }
    }
}
