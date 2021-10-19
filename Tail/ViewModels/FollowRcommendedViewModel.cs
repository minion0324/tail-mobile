using System;
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
    public class FollowRcommendedViewModel : PageViewModelBase
    {
        #region private members

        ObservableCollection<RecommendedFollowers> _recommendedList;
        Command _nextCommand;
        Command _skipCommand;
        int _totalRecords=0;
        int _totalPages = 0;
        int _currentPage=1;
        int _bottomBarHeight = 74;
        bool _noRecordsLabel=false;

        #endregion
        #region public members
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }

            set
            {
                SetProperty(ref _isRefreshing, value);
            }

        }
        public bool NoRecordsLabel
        {
            get { return _noRecordsLabel; }

            set
            {
                SetProperty(ref _noRecordsLabel, value);
            }
        }

        private bool _isFromSignup;
        public bool IsFromSignup
        {
            get => _isFromSignup;
            set => SetProperty(ref _isFromSignup, value);
        }

        public ObservableCollection<RecommendedFollowers> RecommendedList
        {
            get => _recommendedList;
            set => SetProperty(ref _recommendedList, value);
        }

         public Command NextCommand => _nextCommand ?? (_nextCommand = new Command(async () => await Handle_NextCommand()));
        public Command SkipCommand => _skipCommand ?? (_skipCommand = new Command(async () => await Handle_NextCommand()));
        public Command RefreshCommand => new Command(() => ExecuteRefreshCommand());

      
        public int TotalRecords
        {
            get => _totalRecords;
            set => SetProperty(ref _totalRecords, value);
        }
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }
        public int TotalPages
        {
            get => _totalPages ;
            set => SetProperty(ref _totalPages, value);
        }
        public int BottomBarHeight
        {
            get => _bottomBarHeight;
            set => SetProperty(ref _bottomBarHeight, value);
        }
        #endregion

        public FollowRcommendedViewModel()
        {
            IsFromSignup = !SettingsService.Instance.FromSettings;
            if (!IsFromSignup)
                BottomBarHeight = 10;
            RecommendedList = new ObservableCollection<RecommendedFollowers>();
            Task.Run(async () => await GetRecommendedFollowers());

        }
        public async Task Handle_UserDetailsWithoutTabCommand(int userID)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            SettingsService.Instance.IsOtherUserProfile = true;
            await NavigationService.NavigateToAsync<MyProfile>(userID);
            IsBusy = false;
        }
        async Task Handle_FollowUnfollowCommand(int userID)
        {
            IsBusy = true;
            var _existingItem = RecommendedList.FirstOrDefault(p => p.UserId == userID);
            if (_existingItem == null)
                return;
            if (_existingItem.IsFollow)
            {
                if (!await UnFollowUser(userID))
                    return;
            }
            else
            {
                if (!await FollowUser(userID))
                    return;
            }
            _existingItem.IsFollow = !_existingItem.IsFollow;
            IsBusy = false;
        }
        async Task Handle_NextCommand()
        {
            IsBusy = true;
            await NavigationService.SetTabAsMainPageAsync<Home>();
            IsBusy = false;
        }
        public async Task GetRecommendedFollowers()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            var recomentedResponse = await TailDataServiceProvider.Instance.GetUserSuggestions(CurrentPage);
            if (recomentedResponse.ErrorCode == 200 && recomentedResponse.ResponseData != null)
            {
                if(CurrentPage==1)
                    RecommendedList = new ObservableCollection<RecommendedFollowers>();
                foreach (RecommendedFollowersMain followerInfo in recomentedResponse.ResponseData)
                {
                    foreach (RecommendedFollowers item in followerInfo.FollowersData)
                    {
                        item.UserDetails = new Command<int>(async (UserID) => await Handle_UserDetailsWithoutTabCommand(item.UserId));
                        item.FollowUnfollow = new Command<int>(async (UserID) => await Handle_FollowUnfollowCommand(item.UserId));
                        if (!RecommendedList.Contains(item))
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                RecommendedList.Add(item);
                            });
                           
                        }
                       
                    }
                    foreach (PaginationDetails pageItem in followerInfo.PageInfo)
                    {
                        if(TotalRecords==0)
                            TotalRecords = pageItem.totalRecords;
                        if (TotalPages == 0)
                            TotalPages = pageItem.totalPages;
                    }
                       
                }
                if (CurrentPage <= TotalPages)
                {
                    CurrentPage++;
                }
                


            }
            else
            {
                if (recomentedResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                    await ShowAlert(AppResources.AppName, recomentedResponse.Message);
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                if (RecommendedList == null || RecommendedList.Count == 0)
                    NoRecordsLabel = true;
                else
                    NoRecordsLabel = false;
                IsBusy = false;
            });
           
        }
        


        public void ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Task.Run(async () => {
                IsRefreshing = true;
                CurrentPage = 1;
                await GetRecommendedFollowers();
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsRefreshing = false;
                });
            });
           
            IsBusy = false;


        }

    }
}
