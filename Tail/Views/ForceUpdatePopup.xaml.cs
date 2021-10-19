using Rg.Plugins.Popup.Pages;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class ForceUpdatePopup : PopupPage
    {
        readonly ForceUpdatePopupViewModel _vModel;
        public ForceUpdatePopup()
        {
            InitializeComponent();
            _vModel = new ForceUpdatePopupViewModel();
            BindingContext = _vModel;
        }
    }
}
