using Xamarin.Forms;

namespace Tail.Controls
{
    public class CustomListView : ListView
    {
        public bool IsAllowSelection
        {
            get;
            set;
        }
        public bool ScrollEnabled { get; set; } = true;
      
    }
}
