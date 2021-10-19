
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PickDetailInfo
    {

        [JsonProperty("type")]
        public int PickType
        {
            get;
            set;
        }
        [JsonProperty("sportId")]
        public int SportId
        {
            get;
            set;
        }
        [JsonProperty("sportName")]
        public int SportName
        {
            get;
            set;
        }
        [JsonProperty("eventId")]
        public string EventId
        {
            get;
            set;
        }
        [JsonProperty("eventDate")]
        public string EventDate
        {
            get;
            set;
        }
        [JsonProperty("pickUserId")]
        public string PickUserId
        {
            get;
            set;
        }
        [JsonProperty("pickUName")]
        public string PickUserName
        {
            get;
            set;
        }
        [JsonProperty("pickUImg")]
        public string PickUserImage
        {
            get;
            set;
        }
        [JsonProperty("accPerc")]
        public string AccPerc
        {
            get;
            set;
        }
        [JsonProperty("lfpPerc")]
        public string LfpPerc
        {
            get;
            set;
        }
        
    }
}
