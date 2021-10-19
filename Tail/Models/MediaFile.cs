
namespace Tail.Models
{
    public class MediaFile
    {
        public int ImageID { get; set; }
        public string PreviewPath { get; set; }
        public string Path { get; set; }
        public MediaFileType Type { get; set; }
        public bool IsUploading { get; set; }
        public string UploadedName { get; set; }
        public decimal Progress { get; set; }
        public bool IsVideo
        {
            get
            {
                if (Type == MediaFileType.Video)
                    return true;
                else
                    return false;
            }
        }
    }

    public enum MediaFileType
    {
        Image,
        Video
    }
}
