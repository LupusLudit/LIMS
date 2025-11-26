using LIMS.Logic.ImageLoading;
namespace LIMS.Logic.Tools
{
    public abstract class ToolBase
    {
        public bool Enabled { get; set; } = false;
        protected ToolBase() { }
        public abstract void Apply(ImageDataContainer image);
    }
}
