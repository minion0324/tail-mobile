using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PaymentMethodsViewModel : PageViewModelBase
    {
        Command _selectPaymentMethodCommand;
        Command _walletSelectionCommand;
        Command _addNewCardCommand;
        Command _accountDetailsCommand;
        Command _myPurchasesCommand;
        private ObservableCollection<CardDetailsModel> _savedCards;
        private bool _isWalletActive;

        public ObservableCollection<CardDetailsModel> SavedCards
        {
            get => _savedCards;
            set => SetProperty(ref _savedCards, value);
        }
        public bool IsWalletActive
        {
            get => _isWalletActive;
            set => SetProperty(ref _isWalletActive, value);
        }
        public bool IsPagePopped
        {
            get;
            set;
        }
        public PaymentMethodsViewModel()
        {
           
            IsPagePopped = true;
            SavedCards = new ObservableCollection<CardDetailsModel>
            {
                new CardDetailsModel
                {
                    CardNumber = "3163", CardType = "Master", ExpiryDate = "09/2025", IsActive = false, NameOnCard = "Mike John"
                },
                new CardDetailsModel
                {
                    CardNumber = "4568", CardType = "Visa", ExpiryDate = "03/2024", IsActive = false, NameOnCard = "Mike John"
                }
            };
           var selectedpaymetMethod = CommonSingletonUtility.SharedInstance.SelectedCardDetails;
            if(selectedpaymetMethod.CardType == "Wallet")
            {
                IsWalletActive = true;
            }
            else
            {
                IsWalletActive = false;
                var alreadySelectedItem = SavedCards.FirstOrDefault(x => x.CardNumber == selectedpaymetMethod.CardNumber);
                if (alreadySelectedItem != null)
                    alreadySelectedItem.IsActive = true;
            }
        }
        public Command SelectPaymentMethodCommand => _selectPaymentMethodCommand ?? (_selectPaymentMethodCommand = new Command((item) =>
       {
           var cardItem = (CardDetailsModel)item;
           Handle_SelectPaymentMethodCommand(cardItem);
       }));
      

      

        public Command WalletSelectionCommand => _walletSelectionCommand ?? (_walletSelectionCommand = new Command(() => Handle_WalletSelectionCommand()));
        public Command AddNewCardCommand => _addNewCardCommand ?? (_addNewCardCommand = new Command(async () => await Handle_AddNewCardCommandAsync()));
        public Command AccountDetailsCommand => _accountDetailsCommand ?? (_accountDetailsCommand = new Command(async () => await Handle_AccountDetailsCommandAsync()));
        public Command MyPurchasesCommand => _myPurchasesCommand ?? (_myPurchasesCommand = new Command( () =>  Handle_MyPurchasesCommandAsync()));

        private void Handle_MyPurchasesCommandAsync()
        {
            IsPagePopped = false;
            CommonSingletonUtility.SharedInstance.IsFromPaymentMethods = true;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            SettingsService.Instance.CurrentTabIndex = 3;
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            if (currentTabbedPage != null)
                currentTabbedPage.CurrentPage = currentTabbedPage.Children[3];
           
        }

        private async Task Handle_AccountDetailsCommandAsync()
        {
            IsPagePopped = false;
            await NavigationService.NavigateWithInTabToAsync<AccountDetails>();
            
        }

        private async Task Handle_AddNewCardCommandAsync()
        {
            IsPagePopped = false;
            await NavigationService.NavigateWithInTabToAsync<AddNewCard>();
        }

        private void Handle_WalletSelectionCommand()
        {
            var alreadySelectedItem = SavedCards.FirstOrDefault(x => x.IsActive);
            if (alreadySelectedItem != null)
                alreadySelectedItem.IsActive = false;
            IsWalletActive = true;
        }

        void Handle_SelectPaymentMethodCommand(CardDetailsModel cardDetails)
        {
            var alreadySelectedItem = SavedCards.FirstOrDefault(x => x.IsActive);
            if (alreadySelectedItem != null)
                alreadySelectedItem.IsActive = false;
            var item = SavedCards.FirstOrDefault(x => x.CardNumber == cardDetails.CardNumber);
            if (item != null)
                item.IsActive = true;
            IsWalletActive = false;
        }
        public override void OnPageAppearing()
        {
            IsPagePopped = true;
            base.OnPageAppearing();
        }
        public override void OnPageDisappearing()
        {
            base.OnPageDisappearing();
            if(IsPagePopped)
            {
                var SelectedCard = new CardDetailsModel();
                if (IsWalletActive)
                {
                    SelectedCard = new CardDetailsModel
                    {
                        CardNumber = "0000",
                        CardType = "Wallet",
                        ExpiryDate = "",
                        IsActive = true,
                        NameOnCard = "Tail Wallet"
                    };
                }
                else
                {
                    var SelectedItem = SavedCards.FirstOrDefault(x => x.IsActive);
                    if (SelectedItem != null)
                    {
                        SelectedCard = SelectedItem;
                    }
                }

                MessagingCenter.Send<CardDetailsModel>(SelectedCard, "ShowPickPurchasePopup");
            }
            
        }
    }
}
