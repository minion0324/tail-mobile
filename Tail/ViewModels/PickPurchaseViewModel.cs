using System;
using System.Threading.Tasks;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PickPurchaseViewModel: PageViewModelBase
    {
        Command _changePaymentMethodCommand;
        public Action PopupCloseCallback { get; set; }
        public PickPurchaseViewModel()
        {
        }
        public Command ChangePaymentMethodCommand => _changePaymentMethodCommand ?? (_changePaymentMethodCommand = new Command(async () => await Handle_ChangePaymentMethodCommand()));

        private async Task Handle_ChangePaymentMethodCommand()
        {
            PopupCloseCallback.Invoke();
            await NavigationService.NavigateWithInTabToAsync<PaymentMenthods>();
        }
    }
}
