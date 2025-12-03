namespace LIMS.Logic.ImageLoading
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="ImageDataContainer"]/*'/>
    public class ImageDataContainer
    {
        public string FilePath { get; }

        public byte[] RawBytes { get; set; }

        public ImageDataContainer(string filePath, byte[] rawBytes)
        {
            FilePath = filePath;
            RawBytes = rawBytes;
        }
    }
}
