
using Newtonsoft.Json;

namespace Tail.Models
{
    public class SettingsInfo
    {
        [JsonProperty("isEmailNotifyOn")]
        public bool IsEmailNotify { get; set; }
        [JsonProperty("isAppNotifyOn")]
        public bool IsAppNotify { get; set; }
        [JsonProperty("isPostLikeDisOn")]
        public bool IsPostLikeDislike { get; set; }
        [JsonProperty("isPostCommentOn")]
        public bool IsPostComment { get; set; }
        [JsonProperty("isPickLikeDisOn")]
        public bool IsPickLikeDislike { get; set; }
        [JsonProperty("isPickCommentOn")]
        public bool IsPickComment { get; set; }
        [JsonProperty("isPickPurOn")]
        public bool IsPickPurchase { get; set; }
        [JsonProperty("isFwgPostOn")]
        public bool IsFollowingPos { get; set; }
        [JsonProperty("isFwgCommentOn")]
        public bool IsFollowingComment { get; set; }
        [JsonProperty("isFwrNewOn")]
        public bool IsFollowerNew { get; set; }

    }

}
