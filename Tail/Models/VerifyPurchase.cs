
namespace Tail.Models
{
    public class VerifyPurchase: BaseModel
    {
        public string signedData { get; set; }
    }
    public class VerifyPurchaseAndroid : BaseModel
    {
        public string signedData { get; set; }
        public string packageName { get; set; }
        public string productId { get; set; }
        public long purchaseTime { get; set; } = 0;
        public int purchaseState { get; set; } = 0;
        public string orderId { get; set; }
        public string purchaseToken { get; set; }
    }
}
