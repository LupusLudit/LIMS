namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolsManager"]/*'/>
    public class ToolsManager
    {
        private List<ToolBase> tools = new List<ToolBase>();
        public IEnumerable<ToolBase> Tools => tools;

        /// <summary>
        /// Registers a new tool to the manager if it is not already registered.
        /// </summary>
        /// <typeparam name="T">The type of the tool, derived from <see cref="ToolBase"/>.</typeparam>
        /// <param name="tool">The tool instance to register.</param>
        /// <returns>
        /// The registered tool if it was successfully added; otherwise <c>null</c>
        /// </returns>
        public T? RegisterTool<T>(T tool) where T : ToolBase
        {
            if (tool != null && !tools.Contains(tool))
            {
                tools.Add(tool);
                return tool;
            }
            return null;
        }
    }
}
