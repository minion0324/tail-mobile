
using System.Collections.Generic;

namespace Tail.Models
{
    public class ContactUsRequestInfo
    {
        public string subject { get; set; }
        public string query { get; set; }
        public List<ImageData> imageUrl { get; set; }
       
    }
}
