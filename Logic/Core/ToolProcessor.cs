using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;

namespace LIMS.Logic.Core
{
    public class ToolProcessor
    {
        private readonly List<ToolBase> tools = new List<ToolBase>();

        public void AddTool(ToolBase tool)
        {
            if (tool != null && !tools.Contains(tool))
            {
                tools.Add(tool);
            }
        }

        public void Run()
        {
            List<ImageDataContainer> images = CentralStorage.Instance.GetAllImages();

            Parallel.ForEach(images, image =>
            {
                foreach (ToolBase tool in tools)
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
