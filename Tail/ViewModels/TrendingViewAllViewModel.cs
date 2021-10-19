using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class TrendingViewAllViewModel : PageViewModelBase
    {

        #region private members
        private bool _initialLoad = false;
        bool _isPeople = false;
        bool _isPicks = false;
        bool _infoVisible = false;
        string _titleText;
        string _searchText;
        TrendingModel _trendingDetails;
        TrendingType _trend_Type;
        ObservableCollection<TrendPostMain> _trendList;
        ObservableCollection<TrendingUserBase> _peopleList;
        ObservableCollection<TrendPostMain> _fullTrendList;
        FilterReturnAgrument _filterSelectedList;
        Command _filterCommand;
        #endregion
        #region public members
        public bool InitialLoad
        {
            get { return _initialLoad; }

            set
            {
                SetProperty(ref _initialLoad, value);
            }
        }
        public string TitleText
        {
            get => _titleText;
            set => SetProperty(ref _titleText, value);
        }
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        public bool IsPeople
        {
            get => _isPeople;
            set => SetProperty(ref _isPeople, value);
        }
        public bool InfoVisible
        {
            get => _infoVisible;
            set => SetProperty(ref _infoVisible, value);
        }
        public bool IsPicks
        {
            get => _isPicks;
            set => SetProperty(ref _isPicks, value);
        }
        public TrendingModel TrendingDetails
        {
            get => _trendingDetails;
            set => SetProperty(ref _trendingDetails, value);
        }
        public ObservableCollection<TrendPostMain> TrendList
        {
            get => _trendList;
            set => SetProperty(ref _trendList, value);
        }
        public ObservableCollection<TrendPostMain> FullTrendList
        {
            get => _fullTrendList;
            set => SetProperty(ref _fullTrendList, value);
        }
        public ObservableCollection<TrendingUserBase> PeopleList
        {
            get => _peopleList;
            set => SetProperty(ref _peopleList, value);
        }
        public TrendingType Trend_Type
        {
            get => _trend_Type;
            set => SetProperty(ref _trend_Type, value);
        }
        public FilterReturnAgrument FilterSelectedList
        {
            get => _filterSelectedList;
            set => SetProperty(ref _filterSelectedList, value);
        }
        public Command FilterCommand => _filterCommand ?? (_filterCommand = new Command(async () => await Handle_FilterCommand()));

        #endregion


        public TrendingViewAllViewModel()
        {
            TrendingDetails = new TrendingModel();
        }
        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {
                IsPeople = false;
                TrendArgument paramArgs = (TrendArgument)parameter;
                SearchText = paramArgs.SearchText;
                Trend_Type = paramArgs.TrendType;
                if (Trend_Type == TrendingType.Pick)
                {
                    TitleText = "Trending Picks";
                    IsPicks = true;
                    await GetTrendPickList();
                }
                else if (Trend_Type == TrendingType.Post)
                {
                    TitleText = "Trending Posts";
                    await GetTrendPostList();

                }
                else if (Trend_Type == TrendingType.People)
                {
                    TitleText = "Trending People";
                    IsPeople = true;
                    await GetTrendUserList();
                }
           
                else if (Trend_Type == TrendingType.PeopleResult)
                {
                    TitleText = "Search People";
                    IsPeople = true;
                    await GetSearchResultUser();
                }
                
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Initialize View All Trend. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

       
        async Task<bool> GetSearchResultUser()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<TrendingUserBase> _tempUserPost = new ObservableCollection<TrendingUserBase>();

                var pickResponse = await TailDataServiceProvider.Instance.GetSearchResult(SearchText, 20, TrendingResultType.User.ToString());
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (TrendingUsers userInfo in pickResponse.ResponseData.ResultData.Users)
                    {

                        TrendingUserBase userObj = new TrendingUserBase
                        {
                            UserItem = userInfo,
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempUserPost.Add(userObj);

                        });
                    }


                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    PeopleList = new ObservableCollection<TrendingUserBase>(_tempUserPost);

                    InitialLoad = false;
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> GetTrendPickList()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<TrendPostMain> _tempDataPick = new ObservableCollection<TrendPostMain>();
             
                var pickResponse = await TailDataServiceProvider.Instance.GetTrendPickList(1, 20, TrendingResultType.Pick.ToString(), SettingsService.Instance.LoggedUserDetails.UserId);
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (TrendPost postInfo in pickResponse.ResponseData.ResultData.PickItem)
                    {
                        if (!string.IsNullOrEmpty(postInfo.GameDateTime))
                        {
                            DateTime _eventDateTime = Convert.ToDateTime(postInfo.GameDateTime);
                            string _displayFormate = _eventDateTime.ToString("ddd, MMM d yyyy");
                            postInfo.GameDateDisplay = _displayFormate;
                        }
                        if (!string.IsNullOrEmpty(postInfo.GameDateTime))
                        {
                            DateTime _eventDateTime = Convert.ToDateTime(postInfo.GameDateTime);
                            string _displayFormate = _eventDateTime.ToString("hh:mm tt");
                            postInfo.GameTimeDisplay = _displayFormate;
                        }
                        postInfo.PType = 2;
                        TrendPostMain postObj = new TrendPostMain
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<TrendPost>(async (item) => await Handle_CommentCommandPick(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))

                        };
                    
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempDataPick.Add(postObj);

                        });
                    }


                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    TrendList = new ObservableCollection<TrendPostMain>(_tempDataPick);
                    FullTrendList = new ObservableCollection<TrendPostMain>(_tempDataPick);
                    InitialLoad = false;
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> GetTrendPostList()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<TrendPostMain> _tempDataPick = new ObservableCollection<TrendPostMain>();

                var pickResponse = await TailDataServiceProvider.Instance.GetTrendPickList(1, 20, TrendingResultType.Post.ToString(), SettingsService.Instance.LoggedUserDetails.UserId);
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (TrendPost postInfo in pickResponse.ResponseData.ResultData.PostItem)
                    {

                        TrendPostMain postObj = new TrendPostMain
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<TrendPost>(async (item) => await Handle_CommentCommand(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempDataPick.Add(postObj);

                        });
                    }


                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    TrendList = new ObservableCollection<TrendPostMain>(_tempDataPick);
                    FullTrendList = new ObservableCollection<TrendPostMain>(_tempDataPick);
                    InitialLoad = false;
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> GetTrendUserList()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<TrendingUserBase> _tempUserPost = new ObservableCollection<TrendingUserBase>();

                var pickResponse = await TailDataServiceProvider.Instance.GetTrendPickList(1, 20, TrendingResultType.User.ToString(), SettingsService.Instance.LoggedUserDetails.UserId);
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (TrendingUsers userInfo in pickResponse.ResponseData.ResultData.Users)
                    {

                        TrendingUserBase userObj = new TrendingUserBase
                        {
                            UserItem = userInfo,
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempUserPost.Add(userObj);

                        });
                    }


                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    PeopleList = new ObservableCollection<TrendingUserBase>(_tempUserPost);

                    InitialLoad = false;
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task Handle_CommentCommandPick(TrendPost postItem)
        {
            PostDetails postObj = new PostDetails()
            {
                PostId = postItem.PostId,
                PType = 2
            };
            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired = true,
                postObj = postObj
            };
            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);

        }
        async Task Handle_CommentCommand(TrendPost postItem)
        {
            PostDetails postObj = new PostDetails()
            {
                PostId = postItem.PostId,
                PType = 1
            };
            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired = true,
                postObj = postObj
            };
            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);

        }
        async Task Handle_FilterCommand()
        {

            await PopupNavigation.Instance.PushAsync(new ViewAllFilterPopUp(FilterSelectedList, (items) => Handle_FilterPopUpClosed(items)));


        }
        void Handle_FilterPopUpClosed(FilterReturnAgrument SelectedList)
        {
            Debug.WriteLine("FilterPopUpClosed : " + SelectedList);
        }

    }
}
