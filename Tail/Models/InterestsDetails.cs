
namespace Tail.Models
{
    public class InterestsDetails : BaseModel
    {
        private string _sportName;
        public string SportName
        {
            get => _sportName;
            set => SetProperty(ref _sportName, value);
        }

        private string _sportImage;
        public string SportImage
        {
            get => _sportImage;
            set => SetProperty(ref _sportImage, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
