using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class PickPurchasePopup : PopupPage
    {
        readonly PickPurchasePopupViewModel _vModel;
        public PickPurchasePopup(Action popUpCloseCallback, PostDetails selectedPostDetails)
        {
            _vModel = new PickPurchasePopupViewModel();
            BindingContext = _vModel;
            _vModel.PickDetails = selectedPostDetails;
            _vModel.BuyText = string.Format(AppResources.PayCoinsText, selectedPostDetails.PickInfo[0].PickPrice.ToString("0"));
            
            _vModel.PopupCloseCallback = popUpCloseCallback;
            _vModel.TitleText = AppResources.ConfirmPickPurchaceText + selectedPostDetails.UserName;
            InitializeComponent();
          
        }

        async void DismissNotification_Clicked(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

   
    }
}
