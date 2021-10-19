using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PickPurchasePopupViewModel : PageViewModelBase
    {
        string _buyText;
        string _titleText;
        Command _changePaymentMethodCommand;
        Command _payCommand;

        public Action PopupCloseCallback { get; set; }
        
        public string BuyText
        {
            get => _buyText;
            set => SetProperty(ref _buyText, value);
        }
        public string TitleText
        {
            get => _titleText;
            set => SetProperty(ref _titleText, value);
        }
        public PostDetails PickDetails
        {
            get;
            set;
        }

        public PickPurchasePopupViewModel()
        {
        }
        public Command ChangePaymentMethodCommand => _changePaymentMethodCommand ?? (_changePaymentMethodCommand = new Command(async () => await Handle_ChangePaymentMethodCommand()));

        private async Task Handle_ChangePaymentMethodCommand()
        {
            PopupCloseCallback.Invoke();
          
            await NavigationService.NavigateWithInTabToAsync<AddCoins>();
        }
        public Command PayCommand => _payCommand ?? (_payCommand = new Command(async () => await Handle_PayCommand()));

        private async Task Handle_PayCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PickPurchaseRequestInfo pickPurchaseRequest = new PickPurchaseRequestInfo();
            pickPurchaseRequest.postId = PickDetails.PostId;
            pickPurchaseRequest.coinsPaid = PickDetails.PickInfo[0].PickPrice;
            pickPurchaseRequest.amtPaid = PickDetails.PickInfo[0].PickPrice;
            var Response = await TailDataServiceProvider.Instance.PickPurchase(pickPurchaseRequest);
            if(Response.ErrorCode == 200)
            {
                MessagingCenter.Send<PostDetails>(PickDetails, "PickPurchased");
                PickDetails.PickInfo[0].IsPickPurchase = true;
                if (PickDetails.PickInfo[0].IsContentHide)
                {
                    PickDetails.PickInfo[0].IsAttachmentEnable = true;
                    if (PickDetails.PostedAttachments != null && PickDetails.PostedAttachments.Count != 0)
                    {
                        PickDetails.PickInfo[0].AttachmentHeight = 280;
                    }
                }
               
            }
            else
            {
                await ShowAlert(AppResources.AppName, Response.Message);
            }
            var coinDetailsResponse = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (coinDetailsResponse.ErrorCode == 200 && coinDetailsResponse.ResponseData != null)
            {
                var TotalCoins = coinDetailsResponse.ResponseData.BalanceCoins;
                CommonSingletonUtility.SharedInstance.CoinBalance = TotalCoins;
            }

            PopupCloseCallback.Invoke();
            IsBusy = false;
        }
    }
}
