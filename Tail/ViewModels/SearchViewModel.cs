using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class SearchViewModel : PageViewModelBase
    {
        #region private members
        private bool _initialLoad = false;
        private bool _pickVisible = true;
        private bool _postVisible = true;
        private bool _userVisible = true;
        bool _searchPickVisible = false;
        bool _searchPostVisible = false;
        bool _searchUserVisible = false;
        TrendingModel _trendingDetails;
        string _searchText;
        Command _viewAllPost;
        Command _viewAllPicks;
        Command _viewAllPeople;
        ObservableCollection<TrendPostMain> _pickDetailsList;
        ObservableCollection<TrendPostMain> _postDetailsList;
        ObservableCollection<TrendingUserBase> _userDetailsList;
        ObservableCollection<PostDetailsMainModel> _pickSearchList;
        ObservableCollection<PostDetailsMainModel> _postSearchList;

        #endregion
        #region public members
        public TrendingModel TrendingDetails
        {
            get => _trendingDetails;
            set => SetProperty(ref _trendingDetails, value);
        }

        public bool InitialLoad
        {
            get { return _initialLoad; }

            set
            {
                SetProperty(ref _initialLoad, value);
            }
        }
        public bool PickVisible
        {
            get { return _pickVisible; }

            set
            {
                SetProperty(ref _pickVisible, value);
            }
        }
        public bool SearchPickVisible
        {
            get { return _searchPickVisible; }

            set
            {
                SetProperty(ref _searchPickVisible, value);
            }
        }
        public bool PostVisible
        {
            get { return _postVisible; }

            set
            {
                SetProperty(ref _postVisible, value);
            }
        }
        public bool SearchPostVisible
        {
            get { return _searchPostVisible; }

            set
            {
                SetProperty(ref _searchPostVisible, value);
            }
        }
        public bool UserVisible
        {
            get { return _userVisible; }

            set
            {
                SetProperty(ref _userVisible, value);
            }
        }
        public bool SearchUserVisible
        {
            get { return _searchUserVisible; }

            set
            {
                SetProperty(ref _searchUserVisible, value);
            }
        }
        bool _isResult = false;
        public bool IsResult
        {
            get => _isResult;
            set => SetProperty(ref _isResult, value);
        }
        bool _isInfoLabelVisible = false;
        public bool IsInfoLabelVisible
        {
            get => _isInfoLabelVisible;
            set => SetProperty(ref _isInfoLabelVisible, value);
        }
        

        bool _isViewAllPickEnable = false;
        public bool IsViewAllPickEnable
        {
            get => _isViewAllPickEnable;
            set => SetProperty(ref _isViewAllPickEnable, value);
        }
        bool _isViewAllPostEnable = false;
        public bool IsViewAllPostEnable
        {
            get => _isViewAllPostEnable;
            set => SetProperty(ref _isViewAllPostEnable, value);
        }
        bool _isViewAllPeopleEnable = false;
        public bool IsViewAllPeopleEnable
        {
            get => _isViewAllPeopleEnable;
            set => SetProperty(ref _isViewAllPeopleEnable, value);
        }
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        public ObservableCollection<TrendPostMain> PickDetailsList
        {
            get => _pickDetailsList;
            set => SetProperty(ref _pickDetailsList, value);
        }
        public ObservableCollection<TrendPostMain> PostDetailsList
        {
            get => _postDetailsList;
            set => SetProperty(ref _postDetailsList, value);
        }

        public ObservableCollection<PostDetailsMainModel> PickSearchList
        {
            get => _pickSearchList;
            set => SetProperty(ref _pickSearchList, value);
        }
        public ObservableCollection<PostDetailsMainModel> PostSearchList
        {
            get => _postSearchList;
            set => SetProperty(ref _postSearchList, value);
        }

        public ObservableCollection<TrendingUserBase> UserDetailsList
        {
            get => _userDetailsList;
            set => SetProperty(ref _userDetailsList, value);
        }
        bool _scrollEnd = false;
        public bool ScrollEnd
        {
            get => _scrollEnd;

            set => SetProperty(ref _scrollEnd, value);
        }
        bool _scrollStart = false;
        public bool ScrollStart
        {
            get => _scrollStart;

            set => SetProperty(ref _scrollStart, value);
        }

        public Command ViewAllPost => _viewAllPost ?? (_viewAllPost = new Command(async () => await Handle_ViewAllPost()));
        public Command ViewAllPicks => _viewAllPicks ?? (_viewAllPicks = new Command(async () => await Handle_ViewAllPicks()));
        public Command ViewAllPeople => _viewAllPeople ?? (_viewAllPeople = new Command(async () => await Handle_ViewAllPeople()));

        public Action SearchTextChange
        {
            get;
            set;
        }
        public DateTime LastUpdatedTime
        {
            get;
            set;
        }
        #endregion

        public SearchViewModel()
        {
            TrendingDetails = new TrendingModel();
            SearchTextChange += SearchText_Changed;

        }
        public override void OnPageAppearing()
        {
          
            if ((DateTime.Now - LastUpdatedTime).TotalSeconds > 60)
                Task.Run(async () => await GetTrendPickList());
            base.OnPageAppearing();

        }
        async void SearchText_Changed()
        {
            if(string.IsNullOrEmpty(SearchText))
            {
                IsResult = false;
                IsInfoLabelVisible = false;
                await GetTrendPickList();
                return;
            }
            if (SearchText.Length >= 3){
                await GetSearchResult();
            }

        }
        public async Task<bool> GetTrendPickList()
        {
           
           
            bool hasSuccessResponse = false;
            if (PickDetailsList == null && PostDetailsList == null && UserDetailsList == null)
                InitialLoad = true;
            try
            {
               
                ObservableCollection<TrendPostMain> _tempDataPick = new ObservableCollection<TrendPostMain>();
                ObservableCollection<TrendPostMain> _tempDataPost = new ObservableCollection<TrendPostMain>();
                ObservableCollection<TrendingUserBase> _tempUserPost = new ObservableCollection<TrendingUserBase>();
                var pickResponse = await TailDataServiceProvider.Instance.GetTrendPickList(1, 4, TrendingResultType.All.ToString(), SettingsService.Instance.LoggedUserDetails.UserId);
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {
                    hasSuccessResponse = true;
                    if (pickResponse.ResponseData.ResultData.PickItem.Count > 3)
                    {
                        IsViewAllPickEnable = true;
                    }
                    else
                    {
                        IsViewAllPickEnable = false;
                    }
                    if (pickResponse.ResponseData.ResultData.PostItem.Count > 3)
                    {
                        IsViewAllPostEnable = true;
                    }
                    else
                    {
                        IsViewAllPostEnable = false;
                    }
                    if (pickResponse.ResponseData.ResultData.Users.Count > 3)
                    {
                        IsViewAllPeopleEnable = true;
                    }
                    else
                    {
                        IsViewAllPeopleEnable = false;
                    }

 
                    foreach (TrendPost postInfo in pickResponse.ResponseData.ResultData.PickItem)
                    {
                        if (_tempDataPick.Count >= 3)
                        {
                            break;
                        }

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
                      
                        TrendPostMain postObj = new TrendPostMain
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<TrendPost>(async (item) => await Handle_CommentCommandPick(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                       
                        _tempDataPick.Add(postObj);

                    }

                    foreach (TrendPost postInfo in pickResponse.ResponseData.ResultData.PostItem)
                    {
                        if (_tempDataPost.Count >= 3)
                        {
                            break;
                        }
                        TrendPostMain postObj = new TrendPostMain
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<TrendPost>(async (item) => await Handle_CommentCommand(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                        _tempDataPost.Add(postObj);

                    }
                    foreach (TrendingUsers userInfo in pickResponse.ResponseData.ResultData.Users)
                    {
                        if (_tempUserPost.Count >= 3)
                        {
                            break;
                        }
                        TrendingUserBase userObj = new TrendingUserBase
                        {
                            UserItem = userInfo,
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                        _tempUserPost.Add(userObj);
                    }
                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await ShowAlert(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    SearchPickVisible = false;
                    SearchPostVisible = false;
                    SearchUserVisible = false;
                    ScrollStart = false;
                    PickDetailsList = new ObservableCollection<TrendPostMain>(_tempDataPick);
                    PostDetailsList = new ObservableCollection<TrendPostMain>(_tempDataPost);
                    UserDetailsList = new ObservableCollection<TrendingUserBase>(_tempUserPost);
                    IsResult = false;
                    if (PickDetailsList.Count == 0)
                        PickVisible = false;
                    else
                        PickVisible = true;
                    if (PostDetailsList.Count == 0)
                        PostVisible = false;
                    else
                        PostVisible = true;
                    if (UserDetailsList.Count == 0)
                        UserVisible = false;
                    else
                        UserVisible = true;
                    InitialLoad = false;
                    LastUpdatedTime = DateTime.Now;
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        async Task<bool> GetSearchResult()
        {
            bool hasSuccessResponse = false;

            
            try
            {

                ObservableCollection<PostDetailsMainModel> _tempDataPick = new ObservableCollection<PostDetailsMainModel>();
                ObservableCollection<PostDetailsMainModel> _tempDataPost = new ObservableCollection<PostDetailsMainModel>();
                ObservableCollection<TrendingUserBase> _tempUserPost = new ObservableCollection<TrendingUserBase>();
                var pickResponse = await TailDataServiceProvider.Instance.GetSearchResult(SearchText, 4, TrendingResultType.All.ToString());
                if (pickResponse.ErrorCode == 200 && pickResponse.ResponseData != null && pickResponse.ResponseData.ResultData != null)
                {

                    hasSuccessResponse = true;
                    if (pickResponse.ResponseData.ResultData.PickItem.Count > 3)
                    {
                        IsViewAllPickEnable = true;
                    }
                    else
                    {
                        IsViewAllPickEnable = false;
                    }
                    if (pickResponse.ResponseData.ResultData.PostItem.Count > 3)
                    {
                        IsViewAllPostEnable = true;
                    }
                    else
                    {
                        IsViewAllPostEnable = false;
                    }
                    if (pickResponse.ResponseData.ResultData.Users.Count > 3)
                    {
                        IsViewAllPeopleEnable = true;
                    }
                    else
                    {
                        IsViewAllPeopleEnable = false;
                    }

                    foreach (PostDetails postInfo in pickResponse.ResponseData.ResultData.PickItem)
                    {
                        if (_tempDataPick.Count >= 3)
                        {
                            break;
                        }
                        if (postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
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
                            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDate))
                            {
                                postInfo.PickInfo[0].DisplayGameDateTime = postInfo.PickInfo[0].GameDate + " at " + postInfo.PickInfo[0].GameTime;
                            }
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_SearchCommentCommand(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0 && (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId))
                        {
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        if (postObj.PostItem.IsPurchased)
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        _tempDataPick.Add(postObj);

                    }

                    foreach (PostDetails postInfo in pickResponse.ResponseData.ResultData.PostItem)
                    {
                        if (_tempDataPost.Count >= 3)
                        {
                            break;
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_SearchCommentCommand(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                        _tempDataPost.Add(postObj);

                    }
                    foreach (TrendingUsers userInfo in pickResponse.ResponseData.ResultData.Users)
                    {
                        if (_tempUserPost.Count >= 3)
                        {
                            break;
                        }
                        TrendingUserBase userObj = new TrendingUserBase
                        {
                            UserItem = userInfo,
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID))
                        };
                        _tempUserPost.Add(userObj);
                    }
                }
                else
                {
                    if (pickResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await ShowAlert(AppResources.AppName, pickResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        PickVisible = false;
                        PostVisible = false;
                        UserVisible = false;
                        PickSearchList = new ObservableCollection<PostDetailsMainModel>(_tempDataPick);
                        PostSearchList = new ObservableCollection<PostDetailsMainModel>(_tempDataPost);
                        UserDetailsList = new ObservableCollection<TrendingUserBase>(_tempUserPost);
                        IsResult = true;
                        if (PickDetailsList.Count == 0)
                            SearchPickVisible = false;
                        else
                            SearchPickVisible = true;
                        if (PostDetailsList.Count == 0)
                            SearchPostVisible = false;
                        else
                            SearchPostVisible = true;
                        if (UserDetailsList.Count == 0)
                            SearchUserVisible = false;
                        else
                            SearchUserVisible = true;

                        if (!SearchPickVisible && !SearchPostVisible && !SearchUserVisible)
                            IsInfoLabelVisible = true;
                        else
                            IsInfoLabelVisible = false;
                        InitialLoad = false;
                        LastUpdatedTime = DateTime.Now;
                    }
                });
            }
            catch (Exception ex)
            {
                InitialLoad = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

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
        async Task Handle_SearchCommentCommand(PostDetails postItem)
        {

            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired = true,
                postObj = postItem
            };
            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);

        }
        async Task Handle_ViewAllPost()
        {
            TrendingType _type = (IsResult) ? TrendingType.PostResult : TrendingType.Post;

            TrendArgument args = new TrendArgument()
            {
                TrendType = _type,
                SearchText = SearchText
            };
            if (_type == TrendingType.Post)
            {
                await NavigationService.NavigateWithInTabToAsync<TrendingViewAll>(args);
            }
            else
            {
                await NavigationService.NavigateWithInTabToAsync<SearchResultViewAll>(args);
            }
            
        }
        async Task Handle_ViewAllPicks()
        {
            TrendingType _type = (IsResult) ? TrendingType.PickResult : TrendingType.Pick;
            TrendArgument args = new TrendArgument()
            {
                TrendType = _type,
                SearchText = SearchText
            };
            if (_type == TrendingType.Pick)
            {
                await NavigationService.NavigateWithInTabToAsync<TrendingViewAll>(args);
            }
            else
            {
                await NavigationService.NavigateWithInTabToAsync<SearchResultViewAll>(args);
            }
        }
        async Task Handle_ViewAllPeople()
        {
            TrendingType _type = (IsResult) ? TrendingType.PeopleResult : TrendingType.People;
            TrendArgument args = new TrendArgument()
            {
                TrendType = _type,
                SearchText = SearchText
            };
            await NavigationService.NavigateWithInTabToAsync<TrendingViewAll>(args);
        }

    }
}
