namespace Tail.Models
{
    public class PickerItem : BaseModel
    {
        string _itemName;

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }
    }
}
