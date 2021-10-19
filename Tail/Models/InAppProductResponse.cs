using System.Collections.Generic;
using Plugin.InAppBilling;

namespace Tail.Models
{
    public class InAppProductResponse : BaseModel
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<InAppBillingProduct> products { get; set; }
    }
}
