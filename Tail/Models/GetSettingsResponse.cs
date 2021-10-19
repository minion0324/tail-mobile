using Newtonsoft.Json;
using SQLite;

namespace Tail.Models
{
    public class GetSettingsResponse:BaseModel
    {
        [PrimaryKey]
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("addOn")]
        public string AddOn { get; set; }
        [JsonProperty("editOn")]
        public string EditOn { get; set; }
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
        public bool IsFwgCommentOn { get; set; }
        [JsonProperty("isFwrNewOn")]
        public bool IsFollowerNew { get; set; }
              
    }
}
