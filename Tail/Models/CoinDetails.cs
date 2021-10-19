
using Newtonsoft.Json;

namespace Tail.Models
{
    public class CoinDetails : BaseModel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("totalCoins")]
        public int TotalCoins { get; set; }
        [JsonProperty("balCoins")]
        public int BalanceCoins { get; set; }
        [JsonProperty("balEarn")]
        public int EarningBalance { get; set; }
    }
}
