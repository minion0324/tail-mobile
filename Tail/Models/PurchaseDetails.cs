
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PurchaseDetails
    {

        [JsonProperty("purUserId")]
        public int UserId
        {
            get;
            set;
        }

        [JsonProperty("purUName")]
        public string UserName
        {
            get;
            set;
        }

        [JsonProperty("purDate")]
        public string PurchaseDate
        {
            get;
            set;
        }

        [JsonProperty("purUImg")]
        public string UserImage
        {
            get;
            set;
        }

        [JsonProperty("postId")]
        public string PostId
        {
            get;
            set;
        }
        [JsonProperty("coinsPaid")]
        public string CoinsPaid
        {
            get;
            set;
        }
        [JsonProperty("amtPaid")]
        public string AmountPaid
        {
            get;
            set;
        }
        [JsonProperty("pickInfo")]
        public PickDetailInfo PickItemDetails
        {
            get;set;
        }
    }
}
