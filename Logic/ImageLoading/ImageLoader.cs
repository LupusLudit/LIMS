using LIMS.Logic.Core;
using System.IO;


namespace LIMS.Logic.ImageLoading
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ImageLoader"]/*'/>
    public static class ImageLoader
    {
        /// <summary>
        /// Asynchronously loads raw bytes from multiple image files and stores them in the provided <see cref="DataStorage"/> instance.
        /// </summary>
        /// <param name="filePaths"> A collection of file system paths pointing to image files.</param>
        /// <param name="storage"> The <see cref="DataStorage"/> used to store loaded <see cref="ImageDataContainer"/> objects.</param>
        /// <remarks>
        /// Each image is loaded in a separate task to improve performance.
        /// </remarks>
        public static async Task LoadImagesAsync(IEnumerable<string> filePaths, DataStorage storage)
        {
            IEnumerable<Task> tasks = filePaths.Select(path => Task.Run(async () =>
                {
                    byte[] bytes = await File.ReadAllBytesAsync(path);
                    ImageDataContainer image = new ImageDataContainer(path, bytes);

                    storage.AddImage(image);
                }
            ));

            await Task.WhenAll(tasks);
        }

    }
}
