using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class NotificationResponseInfo:BaseModel
    {
        IList<NotificationInfo> _notificationData;
        [JsonProperty("resultData")]
        public IList<NotificationInfo> NotificationData
        {
            get => _notificationData;
            set => SetProperty(ref _notificationData, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }
    public class NotificationInfo:BaseModel
    {

        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("postId")]
        public string PostId { get; set; }
        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty("isRead")]
        public bool IsRead { get; set; }
        [JsonProperty("isSilentPush")]
        public bool IsSilentPush { get; set; }
        [JsonProperty("pushStatus")]
        public int PushStatus { get; set; }
        [JsonProperty("pushAttempts")]
        public int PushAttempts { get; set; }
        [JsonProperty("readOn")]
        public bool ReadOn { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("recById")]
        public int RecById { get; set; }
        [JsonProperty("nType")]
        public int NType { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("uImg")]
        public string UserImg { get; set; }
        [JsonProperty("notifyTxt")]
        public string NotifyTxt { get; set; }
        [JsonProperty("addOn")]
        public string AddOn { get; set; }
        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }
        [JsonProperty("pType")]
        public int PostType { get; set; }
        

    }
}
