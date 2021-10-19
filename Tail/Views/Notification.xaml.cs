using Tail.ViewModels;
namespace Tail.Views
{
    public partial class Notification : AppPageBase
    {
        NotificationViewModel _vModel;
        public Notification()
        {
            InitializeComponent();
            _vModel = new NotificationViewModel();
            BindingContext = _vModel;
        }
    }
}
