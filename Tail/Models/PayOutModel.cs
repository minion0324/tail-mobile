using System;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class PayOutModel : BaseModel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("withdrawAmount")]
        public int WithdrawAmount { get; set; }

        [JsonProperty("tailFee")]
        public double TailFee { get; set; }

        [JsonProperty("transferAmount")]
        public double TransferAmount { get; set; }

        [JsonProperty("payoutAmount")]
        public double PayoutAmount { get; set; }

        [JsonProperty("payoutReference")]
        public string PayoutReference { get; set; }

        [JsonProperty("earningStatus")]
        public int EarningStatus { get; set; }

        [JsonProperty("payoutStatus")]
        public string PayoutStatus { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("payDate")]
        public string PayDate { get; set; }

        [JsonProperty("transferReference")]
        public string TransferReference { get; set; }

        [JsonProperty("addOn")]
        public string AddOn { get; set; }

        [JsonProperty("editOn")]
        public string EditOn { get; set; }

        [JsonProperty("v")]
        public int V { get; set; }

        public string PayDateString
        {
            get
            {
                DateTime dateTime = DateTime.Parse(PayDate);
                var dateString = dateTime.ToString("MMM dd, yyyy");
                return dateString;
            }
        }
    }
}
