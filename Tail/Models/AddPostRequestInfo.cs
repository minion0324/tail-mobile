
using System.Collections.Generic;

namespace Tail.Models
{
    public class AddPostRequestInfo : BaseModel
    {
        public string postContent { get; set; }
        public int userId { get; set; }
        public List<ImageData> imageUrl { get; set; }
        public List<VideoData> videoUrl { get; set; }
    }
    public class ImageData : BaseModel
    {
        public string fileUrl { get; set; }
        public string fileText { get; set; }
    }
    public class VideoData : BaseModel
    {
        public string fileUrl { get; set; }
        public string fileText { get; set; }
    }
}
