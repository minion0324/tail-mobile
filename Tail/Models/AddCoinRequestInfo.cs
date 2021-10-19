
namespace Tail.Models
{
    public class AddCoinRequestInfo: BaseModel
    {
        public int coins { get; set; }
        public int amount { get; set; }
        public string productId { get; set; }
        public string currType { get; set; }
        public double localAmt { get; set; }
        public string purId { get; set; }
    }
}
