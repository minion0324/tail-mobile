
using Newtonsoft.Json;
namespace Tail.Models
{
    public class TestModel
    {
        [JsonProperty("NotificationId")]
        public string TestId
        {
            get;
            set;
        }

        [JsonProperty("NotificationTitle")]
        public string TestTitle
        {
            get;
            set;
        }
    }
}
