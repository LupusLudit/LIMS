using LIMS.Logic.ImageLoading;
namespace LIMS.Logic.Tools
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ToolBase"]/*'/>
    public abstract class ToolBase
    {
        public bool Enabled { get; set; } = false;
        protected ToolBase() { }
        public abstract void Apply(ImageDataContainer image);
        public abstract bool IsInValidState(out string? errorMessage);
    }
}
