using Newtonsoft.Json;

namespace Tail.Services.Responses
{
    public class ServiceResponseBase
    {
        [JsonProperty("Error")]
        public int ErrorCode
        {
            get;
            set;
        }
        [JsonProperty("Message")]
        public string Message
        {
            get;
            set;
        }

       
    }
}
