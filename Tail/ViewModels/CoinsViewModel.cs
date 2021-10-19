using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class CoinsViewModel : PageViewModelBase
    {
        private int _coinBalance;
        private int _earningsBalance;
        Command _loadMoreCommand;
        Command _addCoinsCommand;
        private bool _isLoadMoreVisible;
        private bool _noDataVisible;
        ObservableCollection<CoinHistoryModel> _historyDataList;
        ObservableCollection<PurchaseHistoryModel> _purchasesHistoryList;
        ObservableCollection<TranssactionHistoryModel> _transactionsList;
        ObservableCollection<PurchaseHistoryModel> _earingsHistoryList;
        public ObservableCollection<TransactionModel> Transactions { get; set; }
        public ObservableCollection<TransactionModel> VisibleTransactions { get; set; }
        public string TotalEarnings { get; set; }
        public int CoinsCurrentPage { get; set; }
        public int CoinsTotalPages { get; set; }
        public int PurchasesCurrentPage { get; set; }
        public int PurchasesTotalPages { get; set; }
        public int EarningsCurrentPage { get; set; }
        public int EarningsTotalPages { get; set; }
        public int CoinBalance { get => _coinBalance; set => SetProperty(ref _coinBalance, value); }
        public int EarningsBalance { get => _earningsBalance; set => SetProperty(ref _earningsBalance, value); }
        public bool IsLoadMoreVisible
        {
            get => _isLoadMoreVisible;
            set => SetProperty(ref _isLoadMoreVisible, value);
        }
        public bool NoDataVisible
        {
            get => _noDataVisible;
            set => SetProperty(ref _noDataVisible, value);
        }
        public ObservableCollection<CoinHistoryModel> HistoryDataList
        {
            get => _historyDataList;
            set => SetProperty(ref _historyDataList, value);
        }
        public ObservableCollection<PurchaseHistoryModel> PurchasesHistoryList
        {
            get => _purchasesHistoryList;
            set => SetProperty(ref _purchasesHistoryList, value);
        }
        public ObservableCollection<TranssactionHistoryModel> TransactionsList
        {
            get => _transactionsList;
            set => SetProperty(ref _transactionsList, value);
        }
        public ObservableCollection<PurchaseHistoryModel> EaringsHistoryList
        {
            get => _earingsHistoryList;
            set => SetProperty(ref _earingsHistoryList, value);
        }
        public CoinsViewModel()
        {


        }
        public override void OnPageAppearing()
        {
            base.OnPageAppearing();


            Task.Run(async () => await GetTotalCoins());
            Task.Run(async () => await GetListData());
        }

        public async Task GetListData()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            TransactionsList = new ObservableCollection<TranssactionHistoryModel>();
            await GetCoinHistory(0);
            await GetPurchasesHistoryAsync(0);
            await GetEarningsHistoryAsync(0);
            Device.BeginInvokeOnMainThread(() =>
            {

                HistoryData();
                PurchaseHistoryData();
                EarningsHistoryData();
                TransactionsList = new ObservableCollection<TranssactionHistoryModel>(TransactionsList.OrderByDescending(i => i.Date));
                if (TransactionsList.Count > 0)
                    NoDataVisible = false;
                else
                    NoDataVisible = true;
                if (PurchasesTotalPages == PurchasesCurrentPage && CoinsTotalPages == CoinsCurrentPage)
                    IsLoadMoreVisible = false;
                else
                    IsLoadMoreVisible = true;
            });

            IsBusy = false;
        }

        private void HistoryData()
        {
            if (HistoryDataList != null)
            {
                foreach (var item in HistoryDataList)
                {
                    var transactionitem = new TranssactionHistoryModel();
                    transactionitem.Title = item.Title;
                    transactionitem.DateString = item.PurchaseDate;
                    transactionitem.Coins = item.Coins;
                    transactionitem.Date = DateTime.Parse(item.PurchaseDate);
                    transactionitem.IsSuccess = item.PurchaseStatus;
                    transactionitem.IsRefunded = false;
                    TransactionsList.Add(transactionitem);
                }
            }
        }
        private void PurchaseHistoryData()
        {
            if (PurchasesHistoryList != null)
            {
                foreach (var item in PurchasesHistoryList)
                {
                    var transactionitem = new TranssactionHistoryModel();
                    transactionitem.Title = "Pick purchase - " + item.PickDetails.PickUserName;
                    transactionitem.Date = DateTime.Parse(item.PurchaseDate);
                    transactionitem.DateString = item.PurchaseDate;
                    transactionitem.Coins = item.CoinsPaid;
                    transactionitem.IsSuccess = true;
                    transactionitem.IsRefunded = item.IsRefund;
                    TransactionsList.Add(transactionitem);
                }
            }
        }
        private void EarningsHistoryData()
        {
            if (EaringsHistoryList != null)
            {
                foreach (var item in EaringsHistoryList)
                {
                    var transactionitem = new TranssactionHistoryModel();
                    if (item.IsRefund)
                        transactionitem.Title = "Coins refunded - " + item.PurchaseUserName;
                    else
                        transactionitem.Title = "Coins earned - " + item.PurchaseUserName;
                    transactionitem.Date = DateTime.Parse(item.PurchaseDate);
                    transactionitem.DateString = item.PurchaseDate;
                    transactionitem.Coins = item.CoinsPaid;
                    transactionitem.IsSuccess = true;
                    transactionitem.IsRefunded = item.IsRefund;
                    TransactionsList.Add(transactionitem);
                }
            }



        }
        public async Task GetPurchasesHistoryAsync(int pageNumber)
        {
            var response = await TailDataServiceProvider.Instance.GetPurchaseHistory(pageNumber);
            Debug.WriteLine(response.ResponseData);
            if (response.ErrorCode == 200 && response.ResponseData != null && response.ResponseData[0].PurchaseHistories != null && response.ResponseData[0].PurchaseHistories.Count > 0)
            {
                PurchasesHistoryList = response.ResponseData[0].PurchaseHistories;
                PurchasesTotalPages = response.ResponseData[0].PageInfo[0].totalPages;
                PurchasesCurrentPage = response.ResponseData[0].PageInfo[0].currentPage;
                if (response.ResponseData[0].PurchaseHistories != null && response.ResponseData[0].PurchaseHistories.Count > 0)
                {
                    PurchasesHistoryList = response.ResponseData[0].PurchaseHistories;
                    PurchasesTotalPages = response.ResponseData[0].PageInfo[0].totalPages;
                    PurchasesCurrentPage = response.ResponseData[0].PageInfo[0].currentPage;
                }

            }
        }
        public async Task GetEarningsHistoryAsync(int pageNumber)
        {
            var response = await TailDataServiceProvider.Instance.GetEarningsHistory(pageNumber);
            Debug.WriteLine(response.ResponseData);
            if (response.ErrorCode == 200 && response.ResponseData != null && response.ResponseData[0].PurchaseHistories != null && response.ResponseData[0].PurchaseHistories.Count > 0)
            {
                EaringsHistoryList = response.ResponseData[0].PurchaseHistories;
                EarningsTotalPages = response.ResponseData[0].PageInfo[0].totalPages;
                EarningsCurrentPage = response.ResponseData[0].PageInfo[0].currentPage;
            }
        }
        public async Task GetCoinHistory(int pageNumber)
        {

            var response = await TailDataServiceProvider.Instance.GetCoinHistory(pageNumber);
            Debug.WriteLine(response.ResponseData);
            if (response.ErrorCode == 200 && response.ResponseData != null && response.ResponseData[0].CoinHistories != null && response.ResponseData[0].CoinHistories.Count > 0)
            {
                HistoryDataList = response.ResponseData[0].CoinHistories;
                CoinsTotalPages = response.ResponseData[0].PageInfo[0].totalPages;
                CoinsCurrentPage = response.ResponseData[0].PageInfo[0].currentPage;
            }

        }
        public async Task GetTotalCoins()
        {
            var response = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                CoinBalance = response.ResponseData.BalanceCoins;
                EarningsBalance = response.ResponseData.EarningBalance;
                CommonSingletonUtility.SharedInstance.CoinBalance = CoinBalance;
            }
            else
                await ShowAlert(AppResources.AppName, response.Message);
        }
        public Command LoadMoreCommand => _loadMoreCommand ?? (_loadMoreCommand = new Command(async () => await Handle_LoadMoreCommand()));
        public Command AddCoinsCommand => _addCoinsCommand ?? (_addCoinsCommand = new Command(async () => await Handle_AddCoinsCommand()));

        private async Task Handle_AddCoinsCommand()
        {
            if (IsBusy)
                return;
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowAlert(AppResources.AppName, AppResources.NoConnectionMessage);
                return;
            }
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<AddCoins>();
            IsBusy = false;
        }

        private async Task Handle_LoadMoreCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (EarningsCurrentPage != EarningsTotalPages)
            {
                await GetEarningsHistoryAsync(EarningsCurrentPage + 1);
                Device.BeginInvokeOnMainThread(() =>
                {
                    EarningsHistoryData();
                    TransactionsList = new ObservableCollection<TranssactionHistoryModel>(TransactionsList.OrderByDescending(i => i.Date));
                });
            }
            if (PurchasesCurrentPage != PurchasesTotalPages)
            {
                await GetPurchasesHistoryAsync(PurchasesCurrentPage + 1);
                Device.BeginInvokeOnMainThread(() =>
                {
                    PurchaseHistoryData();

                    TransactionsList = new ObservableCollection<TranssactionHistoryModel>(TransactionsList.OrderByDescending(i => i.Date));
                });
            }

            if (CoinsCurrentPage != CoinsTotalPages)
            {
                await GetCoinHistory(CoinsCurrentPage + 1);
                Device.BeginInvokeOnMainThread(() =>
                {
                    HistoryData();

                    TransactionsList = new ObservableCollection<TranssactionHistoryModel>(TransactionsList.OrderByDescending(i => i.Date));
                });
            }

            if (PurchasesTotalPages == PurchasesCurrentPage && CoinsTotalPages == CoinsCurrentPage)
                IsLoadMoreVisible = false;
            else
                IsLoadMoreVisible = true;
            IsBusy = false;
        }
    }
}
