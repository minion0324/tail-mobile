
namespace Tail.Models
{
    public class CardDetailsModel : BaseModel
    {
        public string NameOnCard
        {
            get;
            set;
        }
        public string CardNumber
        {
            get;
            set;
        }
        public string ExpiryDate
        {
            get;
            set;
        }
        public string CardType
        {
            get;
            set;
        }
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
    }
}
