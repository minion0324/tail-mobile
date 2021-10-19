using Plugin.InAppBilling;

namespace Tail.Models
{
    public class InAppPurchaseResponse : BaseModel
    {
        public string PurchaseId { get; set; }
        public string PurchaseToken { get; set; }
        public PurchaseState State { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
