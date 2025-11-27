namespace LIMS.Logic.Tools
{
    public class ToolsManager
    {
        private List<ToolBase> tools = new List<ToolBase>();
        public IEnumerable<ToolBase> Tools => tools;

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
