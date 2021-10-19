using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PurchaseHistoryResponse : BaseModel
    {
        ObservableCollection<PurchaseHistoryModel> _purchaseHistories;
        [JsonProperty("resultData")]
        public ObservableCollection<PurchaseHistoryModel> PurchaseHistories
        {
            get => _purchaseHistories;
            set => SetProperty(ref _purchaseHistories, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }
    public class PurchaseHistoryModel : BaseModel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("purUserId")]
        public int PurchaseUserId { get; set; }
        [JsonProperty("purUName")]
        public string PurchaseUserName { get; set; }
        [JsonProperty("purUImg")]
        public string PurchaseUserImage { get; set; }
        [JsonProperty("postId")]
        public string PostId { get; set; }
        [JsonProperty("coinsPaid")]
        public int CoinsPaid { get; set; }
        [JsonProperty("amtPaid")]
        public float AmountPaid { get; set; }
        [JsonProperty("pickInfo")]
        public PickInfo PickDetails { get; set; }
        [JsonProperty("purDate")]
        public string PurchaseDate { get; set; }
        [JsonProperty("addOn")]
        public string AddOnDate { get; set; }
        [JsonProperty("editOn")]
        public string EditOnDate { get; set; }
        [JsonProperty("isRefund")]
        public bool IsRefund { get; set; }
    }
    public class PickInfo :BaseModel
    {
        [JsonProperty("mainSportId")]
        public int MainSportId { get; set; }
        [JsonProperty("sportId")]
        public int SportId { get; set; }
        [JsonProperty("sName")]
        public string SportName { get; set; }
        [JsonProperty("eventId")]
        public string EventId { get; set; }
        [JsonProperty("eventDate")]
        public string EventDate { get; set; }
        [JsonProperty("pickUserId")]
        public int PickUserId { get; set; }
        [JsonProperty("pickUName")]
        public string PickUserName { get; set; }
        [JsonProperty("pickUImg")]
        public string PickUserImage { get; set; }
        [JsonProperty("accPerc")]
        public string AccountPercentage { get; set; }
    }
}
