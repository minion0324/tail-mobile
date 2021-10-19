
namespace Tail.Models
{
    public class PostStatusRequest
    {
        public string postId { get; set; }
    }
    public class ShareRequest
    {
        public string postId { get; set; }
        public string sDesc { get; set; }
    }
}
