using LIMS.Logic.Core;
using LIMS.Logic.Tools;

namespace LIMS.Logic
{
    /// <include file='../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="TabContext"]/*'/>
    public class TabContext
    {
        private readonly string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff" };
        public DataStorage Storage { get; } = new DataStorage();
        public ToolsManager ToolsManager { get; } = new ToolsManager();
        public string[] AllowedExtensions => allowedExtensions;

        /// <summary>
        /// Executes all enabled tools on all images stored in <see cref="Storage"/>.
        /// </summary>
        /// <remarks>
        /// Internally creates a <see cref="ProcessingEngine"/> and runs it.
        /// </remarks>
        public void ProcessAllTools()
        {
            ProcessingEngine processingEngine = new ProcessingEngine(ToolsManager, Storage);
            processingEngine.Run();
        }

        /// <summary>
        /// Checks if all tools in <see cref="ToolsManager"/> are in valid states.
        /// </summary>
        /// <param name="errorMessage">
        /// Outputs the error message of the first invalid tool encountered; otherwise <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if all tools are in valid states; otherwise <c>false</c>.
        /// </returns>
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
