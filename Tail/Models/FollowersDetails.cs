using Newtonsoft.Json;
using Tail.Common;

namespace Tail.Models
{
    public class FollowersDetails: FollowingDetails
    {


        [JsonProperty("FollowersCount")]
        public int FollowersCountValue
        {
            get;
            set;
        }

        [JsonIgnore]
        public string DisplayFollowersValue
        {
            get
            {

                return string.Format(AppResources.FollowersCount, FollowersCount);
            }
        }
        [JsonIgnore]
        public bool IsFollowValue
        {
            get;
            set;
        }
    }
}
