using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class CoinPurchasePopupViewModel : PageViewModelBase
    {
        Command _cancelCommand;
        Command _buyCoinsCommand;
        Action _popupCloseCallback;
        public Action PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }
        string _coinPack;
        public string CoinPack
        {
            get => _coinPack;
            set => SetProperty(ref _coinPack, value);
        }
        string _purchaseAmount;
        public string PurchaseAmount
        {
            get => _purchaseAmount;
            set => SetProperty(ref _purchaseAmount, value);
        }
        public CoinPurchasePopupViewModel()
        {
        }
        public Command CancelCommand => _cancelCommand ?? (_cancelCommand = new Command( () =>  Handle_CancelCommand()));
        public Command BuyCoinsCommand => _buyCoinsCommand ?? (_buyCoinsCommand = new Command(() => Handle_BuyCoinsCommandAsync()));

        private void  Handle_BuyCoinsCommandAsync()
        {

            Debug.WriteLine("Coin Purchase");
        }

        private void Handle_CancelCommand()
        {
            PopupCloseCallback.Invoke();
        }
    }
}
