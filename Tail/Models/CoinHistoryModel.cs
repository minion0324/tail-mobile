using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class CoinHistoryResponseInfo : BaseModel
    {
        ObservableCollection<CoinHistoryModel>  _coinHistories;
        [JsonProperty("resultData")]
        public ObservableCollection<CoinHistoryModel> CoinHistories
        {
            get => _coinHistories;
            set => SetProperty(ref _coinHistories, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }
    public class CoinHistoryModel : BaseModel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("coins")]
        public int Coins { get; set; }
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("purchaseId")]
        public string PurchaseId { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        string _purchaseDate;
        [JsonProperty("purDate")]
        public string PurchaseDate {
            get => _purchaseDate;
            set
            {
                SetProperty(ref _purchaseDate, value);
            }
        }
        [JsonProperty("addOn")]
        public string AddOnDate { get; set; }
        [JsonProperty("editOn")]
        public string EditOnDate { get; set; }
        [JsonProperty("purStatus")]
        public bool PurchaseStatus { get; set; }
        public string Title { get => "Coin purchased"; }
    }
}
