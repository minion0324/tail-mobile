using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.OnlineServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class AddCoinsViewModel : PageViewModelBase
    {
        Command _payNowCommand;
        Command _addCoinsFromWebCommand;
        Command _faqCommand;
        Action _popupCloseCallback;
        public Action PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }

        int _totalCoins;
        public int TotalCoins
        {
            get => _totalCoins;
            set => SetProperty(ref _totalCoins, value);
        }
        string _tenCoinsPrice;
        public string TenCoinsPrice
        {
            get => _tenCoinsPrice;
            set => SetProperty(ref _tenCoinsPrice, value);
        }
        string _twentyCoinsPrice;
        public string TwentyCoinsPrice
        {
            get => _twentyCoinsPrice;
            set => SetProperty(ref _twentyCoinsPrice, value);
        }
        string _thirtyCoinsPrice;
        public string ThirtyCoinsPrice
        {
            get => _thirtyCoinsPrice;
            set => SetProperty(ref _thirtyCoinsPrice, value);
        }

        string _fourtyCoinsPrice;
        public string FourtyCoinsPrice
        {
            get => _fourtyCoinsPrice;
            set => SetProperty(ref _fourtyCoinsPrice, value);
        }
        string _fiftyCoinsPrice;
        public string FiftyCoinsPrice
        {
            get => _fiftyCoinsPrice;
            set => SetProperty(ref _fiftyCoinsPrice, value);
        }
        string _currencyType;
        public string CurrencyType
        {
            get => _currencyType;
            set => SetProperty(ref _currencyType, value);
        }
        bool _isPurchaseFromWeb;
        public bool IsPurchaseFromWeb
        {
            get => _isPurchaseFromWeb;
            set => SetProperty(ref _isPurchaseFromWeb, value);
        }


        string _productId;
        public string ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }
        string _coinPrice;
        public string CoinPrice
        {
            get => _coinPrice;
            set => SetProperty(ref _coinPrice, value);
        }
        public AddCoinsViewModel()
        {
            IsPurchaseFromWeb = false;
            Task.Run(async () => await GetAllPacks());
            Task.Run(async () => await GetTotalCoins());

        }
        public async Task GetTotalCoins()
        {
            var response = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                TotalCoins = response.ResponseData.BalanceCoins;
                CommonSingletonUtility.SharedInstance.CoinBalance = TotalCoins;
            }
        }
        async Task GetAllPacks()
        {
            try
            {


                IsBusy = true;
                var response = await InAppPurchaseService.Instance.GetCoinPacksFromAppleAsync();
                Debug.WriteLine(response.products);
                if (response.ErrorCode == 200)
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        AssignCoinPackiOS(response);
                    }
                    else
                    {
                        AssignCoinPackAndroid(response);
                    }


                }
                else
                {
                    IsBusy = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        NavigationService.ShowAlertAsync(AppResources.AppName, response.Message);
                    });

                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
        }

        private void AssignCoinPackiOS(InAppProductResponse response)
        {
            foreach (var item in response.products)
            {
                CurrencyType = item.CurrencyCode;
                switch (item.ProductId)
                {
                    case "testTenPack":
                        TenCoinsPrice = item.LocalizedPrice;
                        break;
                    case "testTwentyPack":
                        TwentyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "testThirtyPack":
                        ThirtyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "testFourtyPack":
                        FourtyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "testFiftyPack":
                        FiftyCoinsPrice = item.LocalizedPrice;
                        break;
                }
            }
        }
        private void AssignCoinPackAndroid(InAppProductResponse response)
        {
            foreach (var item in response.products)
            {
                CurrencyType = item.CurrencyCode;
                switch (item.ProductId)
                {
                    case "tenpack":
                        TenCoinsPrice = item.LocalizedPrice;
                        break;
                    case "twentypack":
                        TwentyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "thirtypack":
                        ThirtyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "fortypack":
                        FourtyCoinsPrice = item.LocalizedPrice;
                        break;
                    case "fiftypack":
                        FiftyCoinsPrice = item.LocalizedPrice;
                        break;
                }
            }
        }
        public Command AddCoinsFromWebCommand => _addCoinsFromWebCommand ?? (_addCoinsFromWebCommand = new Command(() => Handle_AddCoinsFromWebCommand()));
        public Command FAQCommand => _faqCommand ?? (_faqCommand = new Command(() => Handle_FAQCommand()));

        private async void Handle_FAQCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<FrequentlyAskedQuestions>();
            IsBusy = false;
        }
        private void Handle_AddCoinsFromWebCommand()
        {
            IsPurchaseFromWeb = true;
          
        }

        public Command PayNowCommand => _payNowCommand ?? (_payNowCommand = new Command(async (param) =>
        {
            var coinValue = param.ToString();
            await Handle_PayNowCommandAsync(coinValue);
        }));
        private async Task Handle_PayNowCommandAsync(string Coins)
        {
            if (IsBusy)
                return;
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowAlert(AppResources.AppName, AppResources.NoConnectionMessage);
                return;
            }
            IsBusy = true;
            ProductId = string.Empty;
            CoinPrice = string.Empty;
            SetCoinPrice(Coins);

            if (string.IsNullOrEmpty(CoinPrice))
            {
                await ShowAlert(AppResources.AppName, "Could not fetch the details of the coin packs. Please try again later.");
                IsBusy = false;
                return;
            }

            var formatedPrice = CoinPrice.Substring(1);
            formatedPrice = formatedPrice.TrimStart();
            formatedPrice = formatedPrice.Replace(",", "");
            if (ProductId != string.Empty)
            {
                var status = await InAppPurchaseService.Instance.PurchaseCoinPackAsync(ProductId);
                if (status.IsSuccess)
                {
                    AddCoinRequestInfo coinRequestInfo = new AddCoinRequestInfo();
                    coinRequestInfo.localAmt = Convert.ToDouble(formatedPrice);
                    coinRequestInfo.coins = Convert.ToInt16(Coins);
                    coinRequestInfo.amount = Convert.ToInt16(Coins);
                    coinRequestInfo.productId = ProductId;
                    coinRequestInfo.currType = CurrencyType;
                    coinRequestInfo.purId = status.PurchaseId;
                    var result = await TailDataServiceProvider.Instance.AddCoin(coinRequestInfo);
                    if (result.ErrorCode != 200)
                        await ShowAlert(AppResources.AppName, result.Message);
                    if (result.ErrorCode == 200)
                    {
                        IsBusy = false;
                        Handle_BackCommand();
                    }
                }
                else
                {
                    await ShowAlert(AppResources.AppName, status.Message);
                }
            }
            await GetTotalCoins();
            IsBusy = false;
        }
        private void SetCoinPrice(string Coins)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                switch (Coins)
                {
                    case "10":
                        CoinPrice = TenCoinsPrice;
                        ProductId = "testTenPack";
                        break;
                    case "20":
                        CoinPrice = TwentyCoinsPrice;
                        ProductId = "testTwentyPack";
                        break;
                    case "30":
                        CoinPrice = ThirtyCoinsPrice;
                        ProductId = "testThirtyPack";
                        break;
                    case "40":
                        CoinPrice = FourtyCoinsPrice;
                        ProductId = "testFourtyPack";
                        break;
                    case "50":
                        CoinPrice = FiftyCoinsPrice;
                        ProductId = "testFiftyPack";
                        break;
                }
            }
            else
            {
                switch (Coins)
                {
                    case "10":
                        CoinPrice = TenCoinsPrice;
                        ProductId = "tenpack";
                        break;
                    case "20":
                        CoinPrice = TwentyCoinsPrice;
                        ProductId = "twentypack";
                        break;
                    case "30":
                        CoinPrice = ThirtyCoinsPrice;
                        ProductId = "thirtypack";
                        break;
                    case "40":
                        CoinPrice = FourtyCoinsPrice;
                        ProductId = "fortypack";
                        break;
                    case "50":
                        CoinPrice = FiftyCoinsPrice;
                        ProductId = "fiftypack";
                        break;
                }
            }
        }
       
        public override void OnPageDisappearing()
        {
            base.OnPageDisappearing();
            MessagingCenter.Unsubscribe<App>(this, "OnResumed");
        }



    }
}
