using LIMS.Logic.ImageLoading;
using LIMS.Logic.Tools;

namespace LIMS.Logic.Core
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolProcessor"]/*'/>
    public class ToolProcessor
    {
        private readonly ToolsManager toolsManager;
        private readonly DataStorage storage;

        public ToolProcessor(ToolsManager toolsManager, DataStorage storage)
        {
            this.toolsManager = toolsManager;
            this.storage = storage;
        }

        /// <summary>
        /// Executes all enabled tools on every image currently stored in <see cref="DataStorage"/>.
        /// </summary>
        /// <remarks>
        /// The processing is performed in parallel for improved efficiency.
        /// Each enabled tool in <see cref="ToolsManager.Tools"/> is applied to each image.
        /// </remarks>
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
