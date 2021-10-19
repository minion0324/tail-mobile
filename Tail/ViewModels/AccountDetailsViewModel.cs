using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class AccountDetailsViewModel : PageViewModelBase
    {
        Command _clickToChangeCommand;
        Command _loadMoreCommand;
        private bool _isLoadMoreVisible;
        private bool _noDataVisible;
        int _earningsBalance;
        string _totalEarnings;
        string _sliderValue;
        string _achivementText;
        ObservableCollection<PayOutModel> _payOutList;
        public ObservableCollection<TransactionModel> Transactions { get; set; }
        public ObservableCollection<TransactionModel> VisibleTransactions { get; set; }

        public ObservableCollection<PayOutModel> PayOutList
        {
            get => _payOutList;
            set => SetProperty(ref _payOutList, value);
        }
        public int PayOutCurrentPage { get; set; }
        public int PayOutTotalPages { get; set; }
        public string TotalEarnings
        {
            get => _totalEarnings;
            set => SetProperty(ref _totalEarnings, value);
        }
        public bool IsLoadMoreVisible
        {
            get => _isLoadMoreVisible;
            set => SetProperty(ref _isLoadMoreVisible, value);
        }
        public int EarningsBalance
        {
            get => _earningsBalance;
            set => SetProperty(ref _earningsBalance, value);
        }
        public string SliderValue
        {
            get => _sliderValue;
            set => SetProperty(ref _sliderValue, value);
        }
        public string AchivementText
        {
            get => _achivementText;
            set => SetProperty(ref _achivementText, value);
        }
        public bool NoDataVisible
        {
            get => _noDataVisible;
            set => SetProperty(ref _noDataVisible, value);
        }
        public AccountDetailsViewModel()
        {
            PayOutCurrentPage = 1;
            PayOutList = new ObservableCollection<PayOutModel>();
            Task.Run(async () => await GetEarnigs());
            Task.Run(async () => await GetPayOutListAsync(PayOutCurrentPage));
           
        }
        public async Task GetPayOutListAsync(int pageNumber)
        {
            var response = await TailDataServiceProvider.Instance.GetPayoutHistory(pageNumber);
            Debug.WriteLine(response.ResponseData);
            Device.BeginInvokeOnMainThread(() =>
            {
                if (response.ErrorCode == 200 && response.ResponseData != null && response.ResponseData[0].PayOutHistories != null && response.ResponseData[0].PayOutHistories.Count > 0)
                {

                    foreach (var item in response.ResponseData[0].PayOutHistories)
                    {
                        PayOutList.Add(item);
                    }

                    PayOutTotalPages = response.ResponseData[0].PageInfo[0].totalPages;
                    PayOutCurrentPage = response.ResponseData[0].PageInfo[0].currentPage;

                }

                if (PayOutList.Count > 0)
                    NoDataVisible = false;
                else
                    NoDataVisible = true;
                if (PayOutTotalPages == PayOutCurrentPage)
                    IsLoadMoreVisible = false;
                else
                    IsLoadMoreVisible = true;
            });
        }
        public Command ClickToChangeCommand => _clickToChangeCommand ?? (_clickToChangeCommand = new Command(async () => await Handle_ClickToChangeCommand()));
        private async Task Handle_ClickToChangeCommand()
        {
            await NavigationService.NavigateWithInTabToAsync<AddAccountDetails>();
        }
        public Command LoadMoreCommand => _loadMoreCommand ?? (_loadMoreCommand = new Command(() => Handle_LoadMoreCommand()));
        private async void Handle_LoadMoreCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            PayOutCurrentPage = PayOutCurrentPage + 1;
            await GetPayOutListAsync(PayOutCurrentPage);
            Device.BeginInvokeOnMainThread(() =>
            {
                if (PayOutTotalPages == PayOutCurrentPage)
                    IsLoadMoreVisible = false;
                else
                    IsLoadMoreVisible = true;
            });
            IsBusy = false;
        }
        private async Task GetEarnigs()
        {
            var response = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                EarningsBalance = response.ResponseData.EarningBalance;
                CommonSingletonUtility.SharedInstance.CoinBalance = response.ResponseData.BalanceCoins;
                Device.BeginInvokeOnMainThread(() =>
                {
                    TotalEarnings = "$" + EarningsBalance;
                    SetProgressBar();
                });

            }
            else
                await ShowAlert(AppResources.AppName, response.Message);
        }
        void SetProgressBar()
        {

            if (EarningsBalance > 20)
            {
                SliderValue = "10";
                AchivementText = string.Format(AppResources.AchivedPercentageText, "100");
            }
            else
            {
                if (EarningsBalance == 0)
                    SliderValue = "0";
                else
                {

                    double sliderVal = (double)EarningsBalance * 5 / 10;
                    SliderValue = sliderVal.ToString();


                }
                AchivementText = string.Format(AppResources.AchivedPercentageText, EarningsBalance.ToString());
            }

        }
    }
}
