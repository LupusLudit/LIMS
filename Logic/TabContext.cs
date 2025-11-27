using LIMS.Logic.Core;
using LIMS.Logic.Tools;
namespace LIMS.Logic
{
    public class TabContext
    {
        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };
        public DataStorage Storage { get; } = new DataStorage();
        public ToolsManager ToolsManager { get; } = new ToolsManager();
        public string[] AllowedExtensions => allowedExtensions;

        public void ProcessAllTools()
        {
            ToolProcessor processor = new ToolProcessor(ToolsManager, Storage);
            processor.Run();
        }

        public bool ToolsInValidStates(out string? errorMessage)
        {
            foreach (ToolBase tool in ToolsManager.Tools)
            {
                if (!tool.IsInValidState(out errorMessage))
                {
                    return false;
                }
            }
            errorMessage = null;
            return true;
        }
    }
}
