using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;

namespace LIMS.Logic.Core
{
    public class ToolProcessor
    {
        private readonly ToolsManager toolsManager;
        private readonly DataStorage storage;

        public ToolProcessor(ToolsManager toolsManager, DataStorage storage)
        {
            this.toolsManager = toolsManager;
            this.storage = storage;
        }

        public void Run()
        {
            List<ImageDataContainer> images = storage.GetAllImages();

            Parallel.ForEach(images, image =>
            {
                foreach (ToolBase tool in toolsManager.Tools)
                {
                    if (tool.Enabled)
                    {
                        tool.Apply(image);
                    }
                }
            });
        }
    }
}
