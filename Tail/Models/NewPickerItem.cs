
namespace Tail.Models
{
    public class NewPickerItem : BaseModel
    {
        string _itemName;
        bool _isSelected;
        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
