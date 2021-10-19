
namespace Tail.Models
{
    public class MenuModel : BaseModel
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        public string Name { get; set; }
    }
}
