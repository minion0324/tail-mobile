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
    public class SearchResultViewAllViewModel : PageViewModelBase
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
        ObservableCollection<PostDetailsMainModel> _trendList;
        ObservableCollection<TrendingUserBase> _peopleList;
        ObservableCollection<PostDetailsMainModel> _fullTrendList;
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
        public ObservableCollection<PostDetailsMainModel> TrendList
        {
            get => _trendList;
            set => SetProperty(ref _trendList, value);
        }
        public ObservableCollection<PostDetailsMainModel> FullTrendList
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
        public SearchResultViewAllViewModel()
        {
        }
        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {
                IsPeople = false;
                TrendArgument paramArgs = (TrendArgument)parameter;
                SearchText = paramArgs.SearchText;
                Trend_Type = paramArgs.TrendType;


                if (Trend_Type == TrendingType.PostResult)
                {
                    TitleText = "Search Posts";
                    await GetSearchResultPost();
                }
                else if (Trend_Type == TrendingType.PickResult)
                {
                    TitleText = "Search Picks";
                    IsPicks = true;
                    await GetSearchResultPick();
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
        async Task<bool> GetSearchResultPick()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<PostDetailsMainModel> _tempDataPick = new ObservableCollection<PostDetailsMainModel>();

                var pickResponse = await TailDataServiceProvider.Instance.GetSearchResult(SearchText, 20, TrendingResultType.Pick.ToString());
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (PostDetails postInfo in pickResponse.ResponseData.ResultData.PickItem)
                    {
                        if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDateTime))
                        {
                            DateTime _eventDateTime = Convert.ToDateTime(postInfo.PickInfo[0].GameDateTime);
                            string _displayFormate = _eventDateTime.ToString("ddd, MMM d yyyy");
                            postInfo.PickInfo[0].GameDate = _displayFormate;
                        }
                        if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDateTime))
                        {
                            DateTime _eventDateTime = Convert.ToDateTime(postInfo.PickInfo[0].GameDateTime);
                            string _displayFormate = _eventDateTime.ToString("hh:mm tt");
                            postInfo.PickInfo[0].GameTime = _displayFormate;
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))

                        };
                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0 && (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId))
                        {
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        if (postObj.PostItem.IsPurchased)
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
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
                    TrendList = new ObservableCollection<PostDetailsMainModel>(_tempDataPick);
                    FullTrendList = new ObservableCollection<PostDetailsMainModel>(_tempDataPick);
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
        async Task<bool> GetSearchResultPost()
        {
            bool hasSuccessResponse = false;
            if (TrendList == null)
                InitialLoad = true;
            try
            {

                ObservableCollection<PostDetailsMainModel> _tempDataPick = new ObservableCollection<PostDetailsMainModel>();

                var pickResponse = await TailDataServiceProvider.Instance.GetSearchResult(SearchText, 20, TrendingResultType.Post.ToString());
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    foreach (PostDetails postInfo in pickResponse.ResponseData.ResultData.PostItem)
                    {

                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
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
                    TrendList = new ObservableCollection<PostDetailsMainModel>(_tempDataPick);
                    FullTrendList = new ObservableCollection<PostDetailsMainModel>(_tempDataPick);
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
        async Task Handle_CommentCommand(PostDetails postItem)
        {
          
            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired = true,
                postObj = postItem
            };
            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);

        }
        async Task Handle_FilterCommand()
        {

            await PopupNavigation.Instance.PushAsync(new ViewAllFilterPopUp(FilterSelectedList, (items) => Handle_FilterPopUpClosed(items)));


        }
        void Handle_FilterPopUpClosed(FilterReturnAgrument SelectedList)
        {
            Debug.WriteLine("FilterPopUpClosed : "+ SelectedList);
        }

    }
}
