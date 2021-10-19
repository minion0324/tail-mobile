using System;
namespace Tail.Models
{
    public class TabItemsModel : BaseModel
    {
        private bool _isSelected;
        public string Name { get; set; }
        public string Count { get; set; }
        public int TabHeaderWidth { get; set; }
        public int Id { get; set; }
        public int PrevId { get; set; }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
        public string  DisplayCount
        {
            get
            {
                return string.Format("( {0} )",Count );
            }
            
        }
    }
}
