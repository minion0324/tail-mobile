
namespace Tail.Models
{
    public class TransactionModel : BaseModel
    {
        public string TransationName
        {
            get;
            set;
        }
        public string TransationDate
        {
            get;
            set;
        }
        public string TransationFromDate
        {
            get;
            set;
        }
        public string TransationToDate
        {
            get;
            set;
        }
        public string TransationAmount
        {
            get;
            set;
        }
    }
}
