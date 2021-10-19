using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Models;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class ShareOptionPopup : PopupPage
    {
        ShareOptionPopupViewModel _vModel;
        
        public ShareOptionPopup(PostDetails PostItem, Action popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new ShareOptionPopupViewModel();
            BindingContext = _vModel;
            _vModel.PostItem = PostItem;
        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }

}
