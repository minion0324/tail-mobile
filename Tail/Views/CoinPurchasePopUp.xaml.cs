using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class CoinPurchasePopUp : PopupPage
    {
        readonly CoinPurchasePopupViewModel _vModel;
        public CoinPurchasePopUp(Action popUpCloseCallback, string CoinValue, string price)
        {
            _vModel = new CoinPurchasePopupViewModel();
            _vModel.PopupCloseCallback = popUpCloseCallback;
            _vModel.CoinPack = CoinValue;
            _vModel.PurchaseAmount = price;
            BindingContext = _vModel;
            InitializeComponent();
            BuyButton.ButtonText = "Buy " + CoinValue + " Coins for " + price;
        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
