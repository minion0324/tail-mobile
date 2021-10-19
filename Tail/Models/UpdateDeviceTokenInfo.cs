
using Newtonsoft.Json;

namespace Tail.Models
{
    public class UpdateDeviceTokenInfo
    {
        [JsonProperty("rToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("dToken")]
        public string DeviceToken { get; set; }
      
    }
}
