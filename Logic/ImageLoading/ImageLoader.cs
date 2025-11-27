using LIMS.Logic.Core;
using System.IO;


namespace LIMS.Logic.ImageLoading
{
    public static class ImageLoader
    {

        public static async Task LoadImagesAsync(IEnumerable<string> filePaths, DataStorage storage)
        {
            IEnumerable<Task> tasks = filePaths.Select(path => Task.Run(async () =>
                {
                    ImageDataContainer image = new ImageDataContainer(path);
                    image.RawBytes = await File.ReadAllBytesAsync(path);
                    //TODO: Add preview image loading

                    storage.AddImage(image);
                }
            ));

            await Task.WhenAll(tasks);
        }

    }
}
