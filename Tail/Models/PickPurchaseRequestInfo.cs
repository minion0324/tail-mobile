
namespace Tail.Models
{
    public class PickPurchaseRequestInfo : BaseModel
    {
        public string postId { get; set; }
        public float coinsPaid { get; set; }
        public float amtPaid { get; set; }
    }
}
