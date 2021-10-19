using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Tail.Views.TabViews;
using Xamarin.Forms;
using Tail.Services.Helper;
using Tail.Services.LocalStorage;
using System.IO;
using Tail.Services.Interfaces;
using System.Diagnostics;

namespace Tail.ViewModels
{
    public class MyProfileViewModel : PageViewModelBase
    {
        #region private members
        bool _infoVisible = false;

        bool _isFollowingInfoLabelVisible = false;
        bool _isFollowersInfoLabelVisible = false;
        bool _isPurchaseInfoLabelVisible = false;
        bool _isRefreshingPost;
        bool _scrollEnd = false;
        int _prevTabIndex;
        Command _editProfileCommand;
        Command _followUnfollow;
        Command _paymentMethodCommand;
        Command _accountDetailsCommand;
        Command _filterCommand;
        Command<TabItemsModel> _tabSelectedCommand;
        private ObservableCollection<TabItemsModel> _predictionItems;
        private ObservableCollection<ContentView> _predictionViews;
        private ObservableCollection<TabItemsModel> _userTabItems;
        private ObservableCollection<ContentView> _userTabViews;
        private ProfileDetails _userProfileDetails;
        ObservableCollection<PostDetailsMainModel> _postDetailsList;
        ObservableCollection<PostDetailsMainModel> _purchaseDetailsList;
        ObservableCollection<PostDetailsMainModel> _pickDetailsList;
        ObservableCollection<PostDetailsMainModel> _shareDetailsList;
        ObservableCollection<Predictions> _lifetimePredictionList;
        ObservableCollection<Predictions> _last15PredictionList;
        ObservableCollection<FollowingDetails> _followerList;
        ObservableCollection<FollowingDetails> _followingList;
        ObservableCollection<PostDetailsMainModel> _fullPickDetailsList;
        List<SportType> _previousSelectedList;
        List<AccuracySports> _sportsAccuracy;
        private bool _isFollow;
        private bool _isOtherProfile = false;
        private long _userId;
        bool _isPostInfoLabelVisible = false;
        bool _isPickInfoLabelVisible = true;
        bool _isShareInfoLabelVisible = false;
        #endregion

        #region public members
        private bool _isRefreshingFollowing = false;
        public bool IsRefreshingFollowing
        {
            get { return _isRefreshingFollowing; }

            set
            {
                SetProperty(ref _isRefreshingFollowing, value);
            }
        }


        private bool _isRefreshingFollower = false;
        public bool IsRefreshingFollowers
        {
            get { return _isRefreshingFollower; }

            set
            {
                SetProperty(ref _isRefreshingFollower, value);
            }
        }
        private bool _isFullDataLoaded = false;
        public bool IsFullDataLoaded
        {
            get { return _isFullDataLoaded; }

            set
            {
                SetProperty(ref _isFullDataLoaded, value);
            }
        }
        private bool _initialLoad = false;
        public bool InitialLoad
        {
            get { return _initialLoad; }

            set
            {
                SetProperty(ref _initialLoad, value);
            }
        }
        public bool IsRefreshingPost
        {
            get => _isRefreshingPost;

            set => SetProperty(ref _isRefreshingPost, value);
        }
        public bool ScrollEnd
        {
            get => _scrollEnd;

            set => SetProperty(ref _scrollEnd, value);
        }

        public long UserId
        {
            get => _userId;
            set
            {
                SetProperty(ref _userId, value);
                if (UserId == 0 || UserId == SettingsService.Instance.LoggedUserDetails.UserId)
                    _isOtherProfile = false;
                else
                    _isOtherProfile = true;
            }
        }

        public bool IsOtherProfile
        {
            get => _isOtherProfile;

            set => SetProperty(ref _isOtherProfile, value);
        }
        public int PrevTabIndex
        {
            get => _prevTabIndex;

            set => SetProperty(ref _prevTabIndex, value);
        }

        public bool InfoVisible
        {
            get => _infoVisible;
            set => SetProperty(ref _infoVisible, value);
        }
        public bool IsPostInfoLabelVisible
        {
            get => _isPostInfoLabelVisible;
            set => SetProperty(ref _isPostInfoLabelVisible, value);
        }

        public bool IsPickInfoLabelVisible
        {
            get => _isPickInfoLabelVisible;
            set => SetProperty(ref _isPickInfoLabelVisible, value);
        }
        public bool IsShareInfoLabelVisible
        {
            get => _isShareInfoLabelVisible;
            set => SetProperty(ref _isShareInfoLabelVisible, value);
        }

        public bool IsPurchaseInfoLabelVisible
        {
            get => _isPurchaseInfoLabelVisible;
            set => SetProperty(ref _isPurchaseInfoLabelVisible, value);
        }
        public bool IsFollowingInfoLabelVisible
        {
            get => _isFollowingInfoLabelVisible;
            set => SetProperty(ref _isFollowingInfoLabelVisible, value);
        }
        string _followingInfoLabelText;
        public string FollowingInfoLabelText
        {
            get => _followingInfoLabelText;
            set => SetProperty(ref _followingInfoLabelText, value);
        }
        public bool IsFollowersInfoLabelVisible
        {
            get => _isFollowersInfoLabelVisible;
            set => SetProperty(ref _isFollowersInfoLabelVisible, value);
        }
        string _followersInfoLabelText;
        public string FollowersInfoLabelText
        {
            get => _followersInfoLabelText;
            set => SetProperty(ref _followersInfoLabelText, value);
        }
        public bool IsFollow
        {
            get => _isFollow;
            set => SetProperty(ref _isFollow, value);
        }

        public ObservableCollection<PostDetailsMainModel> PickDetailsList
        {
            get => _pickDetailsList;
            set => SetProperty(ref _pickDetailsList, value);
        }
        public ObservableCollection<PostDetailsMainModel> ShareDetailsList
        {
            get => _shareDetailsList;
            set => SetProperty(ref _shareDetailsList, value);
        }
        public ObservableCollection<PostDetailsMainModel> FullPickDetailsList
        {
            get => _fullPickDetailsList;
            set => SetProperty(ref _fullPickDetailsList, value);
        }
        public ObservableCollection<PostDetailsMainModel> PostDetailsList
        {
            get => _postDetailsList;
            set => SetProperty(ref _postDetailsList, value);
        }

        public ObservableCollection<TabItemsModel> PredictionItems
        {
            get
            {
                return _predictionItems;
            }
            set
            {
                SetProperty(ref _predictionItems, value);
            }
        }

        public ObservableCollection<ContentView> PredictionViews
        {
            get
            {
                return _predictionViews;
            }
            set
            {
                SetProperty(ref _predictionViews, value);
            }
        }

        public ObservableCollection<TabItemsModel> UserTabItems
        {
            get
            {
                return _userTabItems;
            }
            set
            {
                SetProperty(ref _userTabItems, value);
            }
        }

        public ObservableCollection<ContentView> UserTabViews
        {
            get
            {
                return _userTabViews;
            }
            set
            {
                SetProperty(ref _userTabViews, value);
            }
        }
        public ObservableCollection<Predictions> Last15Predictions
        {
            get
            {
                return _last15PredictionList;
            }
            set
            {
                SetProperty(ref _last15PredictionList, value);
            }
        }
        public ObservableCollection<Predictions> LifetimePredictions
        {
            get
            {
                return _lifetimePredictionList;
            }
            set
            {
                SetProperty(ref _lifetimePredictionList, value);
            }
        }
        public ProfileDetails UserProfileDetails
        {
            get
            {
                return _userProfileDetails;
            }
            set
            {
                SetProperty(ref _userProfileDetails, value);
            }
        }

        public ObservableCollection<FollowingDetails> FollowerList
        {
            get => _followerList;
            set => SetProperty(ref _followerList, value);
        }
        public ObservableCollection<FollowingDetails> FollowingList
        {
            get => _followingList;
            set => SetProperty(ref _followingList, value);
        }
        public ObservableCollection<PostDetailsMainModel> PurchaseDetailsList
        {
            get => _purchaseDetailsList;
            set => SetProperty(ref _purchaseDetailsList, value);
        }
        public List<SportType> PreviousSelectedList
        {
            get => _previousSelectedList;
            set => SetProperty(ref _previousSelectedList, value);
        }
        int _postCount = 0;
        public int PostCount
        {
            get => _postCount;

            set => SetProperty(ref _postCount, value);
        }

        int _pickCount = 0;
        public int PickCount
        {
            get => _pickCount;

            set => SetProperty(ref _pickCount, value);
        }
        int _purchaseCount = 0;
        public int PurchaseCount
        {
            get => _purchaseCount;

            set => SetProperty(ref _purchaseCount, value);
        }
        int _shareCount = 0;
        public int ShareCount
        {
            get => _shareCount;

            set => SetProperty(ref _shareCount, value);
        }


        int _followersCount = 0;
        public int FollowersCount
        {
            get => _followersCount;

            set => SetProperty(ref _followersCount, value);
        }
        int _followingCount = 0;
        public int FollowingCount
        {
            get => _followingCount;

            set => SetProperty(ref _followingCount, value);
        }

        string _lifeTimeUnit = "0";
        public string LifeTimeUnit
        {
            get => _lifeTimeUnit;

            set => SetProperty(ref _lifeTimeUnit, value);
        }
        string _last10Unit = "0";
        public string Last10Unit
        {
            get => _last10Unit;

            set => SetProperty(ref _last10Unit, value);
        }
        bool _isLoadedMore;
        public bool IsLoadedMore
        {
            get => _isLoadedMore;
            set => SetProperty(ref _isLoadedMore, value);
        }
        public List<AccuracySports> SportsAccuracy
        {
            get => _sportsAccuracy;
            set => SetProperty(ref _sportsAccuracy, value);
        }

        public Action OnTabAdded
        {
            get;
            set;
        }
        public Action<int> OnTabRefresh
        {
            get;
            set;
        }

        public Action OnFilterApplied
        {
            get;
            set;
        }
        public Action<int> TabSelection
        {
            get;
            set;
        }
        public Action<bool> AboutMeUpdate
        {
            get;
            set;
        }
        public Action OnPostRefresh
        {
            get;
            set;
        }
        public Command EditProfileCommand => _editProfileCommand ?? (_editProfileCommand = new Command(async () => await Handle_EditProfileCommand()));
        public Command FollowUnfollow => _followUnfollow ?? (_followUnfollow = new Command(async () => await Handle_FollowUnfollowProfileCommand()));
        public Command AccountDetailsCommand => _accountDetailsCommand ?? (_accountDetailsCommand = new Command(async () => await Handle_AccountDetailsCommand()));
        public Command PaymentMethodCommand => _paymentMethodCommand ?? (_paymentMethodCommand = new Command(async () => await Handle_PaymentMethodCommand()));
        public Command FilterCommand => _filterCommand ?? (_filterCommand = new Command(async () => await Handle_FilterCommand()));
        public Command RefreshFollowerCommand => new Command(() => ExecuteRefreshFollowerCommand());
        public Command RefreshFollowingCommand => new Command(() => ExecuteRefreshFollowingCommand());
        public Command<TabItemsModel> TabSelectedCommand => _tabSelectedCommand ?? (_tabSelectedCommand = new Command<TabItemsModel>((item) => Handle_TabSelection(item)));
        public Command RefreshPostCommand => new Command(() => ExecuteRefreshPostCommand());

        public Action<TabItemsModel> OnUserTabIndexChanged
        {
            get;
            set;
        }
        public Action<TabItemsModel> OnPredictionTabIndexChanged
        {
            get;
            set;
        }

        #endregion
        public string FileChacheFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.PostDirectoryName);


        public MyProfileViewModel()
        {

            UserProfileDetails = new ProfileDetails();
            PostDetailsList = new ObservableCollection<PostDetailsMainModel>();
            FollowingList = new ObservableCollection<FollowingDetails>();
            FollowerList = new ObservableCollection<FollowingDetails>();
            PickDetailsList = new ObservableCollection<PostDetailsMainModel>();
            ShareDetailsList = new ObservableCollection<PostDetailsMainModel>();
            FullPickDetailsList = new ObservableCollection<PostDetailsMainModel>();
            PurchaseDetailsList = new ObservableCollection<PostDetailsMainModel>();
            Last15Predictions = new ObservableCollection<Predictions>();
            LifetimePredictions = new ObservableCollection<Predictions>();

            IsOtherProfile = SettingsService.Instance.IsOtherUserProfile;
            if (!IsOtherProfile)
            {
                InitialiseTab(false, 0);
            }
            OnUserTabIndexChanged += TabSelection_Changed;

            OnPredictionTabIndexChanged += (TabItemsModel selectedItem) =>
            {
                switch (selectedItem.Id)
                {
                    case 1:

                        break;
                    case 2:

                        break;
                }
            };

         
           
        }
        async void TabSelection_Changed(TabItemsModel selectedItem)
        {
            try
            {
                if (PrevTabIndex == selectedItem.Id - 1)
                    return;

                await LoadSelectedTab(selectedItem.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
        }
        async Task LoadSelectedTab(int tabId)
        {

            switch (tabId)
            {
                case 1:
                    if (!IsOtherProfile)
                    {
                        if (PostDetailsList.Count == 0)
                        {
                            await GetPosts("0", SettingsService.Instance.LoggedUserDetails.UserId.ToString());

                        }
                        else
                        {
                            await RefreshPosts();
                        }
                    }
                    else
                    {
                        await GetPosts("0", UserId.ToString());
                    }
                    break;
                case 2:
                    if (!IsOtherProfile)
                    {
                        if (PickDetailsList.Count == 0)
                        {
                            await GetPicks("0", SettingsService.Instance.LoggedUserDetails.UserId.ToString());

                        }
                        else
                        {

                            await RefreshPicks();
                        }
                    }
                    else
                    {
                        await GetPicks("0", UserId.ToString());
                    }
                    break;
                case 3:
                    await GetFollowers("0");
                    break;
                case 4:
                    await GetFollowing("0");
                    break;
                case 5:
                    if (PurchaseDetailsList.Count == 0)
                    {
                        await GetPurchases("0");
                    }
                    else
                    {

                        await RefreshPurchases();
                    }
                    break;
                case 6:
                    if (!IsOtherProfile)
                    {

                        if (ShareDetailsList.Count == 0)
                        {
                            await GetShare("0", SettingsService.Instance.LoggedUserDetails.UserId.ToString());
                        }
                        else
                        {
                            await RefreshShare();
                        }


                    }
                    else
                    {
                        await GetShare("0", UserId.ToString());

                    }
                    break;
            }
            PrevTabIndex = tabId - 1;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
               
                Device.BeginInvokeOnMainThread(() =>
                {
                    InitialiseTab(true, PrevTabIndex);
                });
            }
            else
            {
                if (PrevTabIndex != 5 && PrevTabIndex != 4)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitialiseTab(true, PrevTabIndex);
                    });
                }
            }
        }
        public async Task<bool> GetPurchases(string purchaseId)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (PurchaseDetailsList == null || PurchaseDetailsList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                var postResponse = await TailDataServiceProvider.Instance.GetPurchases(purchaseId, UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.PostItem.Count > 0 )
                        IsLoadedMore = true;
                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        if (postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                            }
                        }
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);
                        }
                        postInfo.PostId = postInfo.PickId;
                        if (postInfo.UserStatus == 3)
                        {
                            postInfo.UserImageName = "";
                            postInfo.UserName = AppResources.AnonymousUser;
                            postInfo.UserId = 0;
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),

                        };


                        if (postObj.PostItem.PickInfo != null && !IsOtherProfile)
                        {
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            var _existingItem = PurchaseDetailsList.FirstOrDefault(p => p.PostItem.PostId == postInfo.PostId);
                            if (_existingItem != null)
                            {
                                int indexValue = PurchaseDetailsList.IndexOf(_existingItem);
                                PurchaseDetailsList[indexValue] = postObj;
                            }
                            else
                                PurchaseDetailsList.Add(postObj);
                        });


                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                if (PurchaseDetailsList != null && PurchaseDetailsList.Count > 0)
                {
                    IsPurchaseInfoLabelVisible = false;
                }
                else
                    IsPurchaseInfoLabelVisible = true;
            });
            IsBusy = false;
            return hasSuccessResponse;

        }

        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {

                UserId = (int)parameter;

                if (UserId == 0 || UserId == SettingsService.Instance.LoggedUserDetails.UserId)
                    IsOtherProfile = false;
                else
                    IsOtherProfile = true;

                int _tabWidth = Convert.ToInt32(Application.Current.MainPage.Width / 2);
                PredictionItems = new ObservableCollection<TabItemsModel> { new TabItemsModel { Id = 1, Name = AppResources.LifetimePredictions, Count = "NA", TabHeaderWidth = _tabWidth }, new TabItemsModel { Id = 2, Name = AppResources.Last15Predictions, Count = "NA", TabHeaderWidth = _tabWidth } };

                if (IsOtherProfile)
                {
                    IsBusy = true;
                    InitialLoad = true;
                    await GetUserProfileDetails(Convert.ToInt32(UserId));

                    await GetPosts("0", UserId.ToString());

                    InitialiseTab(false, 0);
                    IsFullDataLoaded = true;
                    InitialLoad = false;
                    IsBusy = false;
                }
                else
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Initialize MyProfile. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadMore()
        {
            switch (PrevTabIndex)
            {
                case 0:
                    if (IsOtherProfile)
                    {
                        await GetPosts(PostDetailsList[PostDetailsList.Count - 1].PostItem.PostId, UserId.ToString());
                    }
                    else
                    {
                        await GetPosts(PostDetailsList[PostDetailsList.Count - 1].PostItem.PostId, SettingsService.Instance.LoggedUserDetails.UserId.ToString());
                    }
                    ScrollEnd = false;
                    break;
                case 1:
                    if (IsOtherProfile)
                    {
                        await GetPicks(PickDetailsList[PickDetailsList.Count - 1].PostItem.PostId, UserId.ToString());
                    }
                    else
                    {
                        await GetPicks(PickDetailsList[PickDetailsList.Count - 1].PostItem.PostId, SettingsService.Instance.LoggedUserDetails.UserId.ToString());

                    }

                    ScrollEnd = false;
                    break;
                case 2:
                    await GetFollowers(FollowerList[FollowerList.Count - 1].ID);
                    ScrollEnd = false;
                    break;
                case 3:
                    await GetFollowing(FollowingList[FollowingList.Count - 1].ID);
                    ScrollEnd = false;
                    break;
                case 4:
                    await GetPurchases(PurchaseDetailsList[PickDetailsList.Count - 1].PostItem.PostId);
                    ScrollEnd = false;
                    break;
                case 5:
                    if (IsOtherProfile)
                    {
                        await GetShare(PickDetailsList[PickDetailsList.Count - 1].PostItem.PostId, UserId.ToString());
                    }
                    else
                    {
                        await GetShare(PickDetailsList[PickDetailsList.Count - 1].PostItem.PostId, SettingsService.Instance.LoggedUserDetails.UserId.ToString());

                    }


                    ScrollEnd = false;
                    break;


            }

            if (IsLoadedMore)
            {
                IsLoadedMore = false;
                if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitialiseTab(true, PrevTabIndex);
                    });
                }
                else
                {
                   
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            InitialiseTab(true, PrevTabIndex);
                        });
                }
            }

        }
        public async Task<bool> GetPosts(string PostID, string userid)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (PostDetailsList == null || PostDetailsList.Count == 0)
                    IsBusy = true;

                var postResponse = await TailDataServiceProvider.Instance.GetPosts(PostID, userid.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.PostItem.Count > 0)
                        IsLoadedMore = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                        {
                            attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                            if (attachObj.FileType == 1)
                            {
                                var imageLocalPath = Path.Combine(FileChacheFolder, attachObj.ImageName);
                                if (!File.Exists(imageLocalPath))
                                {
                                    string _bucketName = Constants.S3BucketForPostImage;
                                    DependencyService.Get<IAwsBucketService>().DownlaodFile(imageLocalPath, attachObj.ImageName, _bucketName);

                                }
                            }
                        }
                        postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                            // AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item))
                        };
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postInfo.PostId);
                            if (_existingItem != null)
                            {
                                int indexValue = PostDetailsList.IndexOf(_existingItem);
                                PostDetailsList[indexValue] = postObj;
                            }
                            else
                                PostDetailsList.Add(postObj);
                        });

                    }

                }
                else
                {
                    if (postResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnPostRefresh?.Invoke();
                });
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                    if (PostDetailsList.Count > 0)
                        IsPostInfoLabelVisible = false;
                    else
                        IsPostInfoLabelVisible = true;
                });



            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> GetPicks(string PostID, string userid)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (PostDetailsList == null || PostDetailsList.Count == 0)
                    IsBusy = true;
                var postResponse = await TailDataServiceProvider.Instance.GetPicks(PostID, userid.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.PostItem.Count > 0)
                        IsLoadedMore = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        if (postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                            }
                        }
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),

                        };

                        if (postObj.PostItem.PickInfo != null)
                        {
                            if (!IsOtherProfile)
                            {
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                            }
                            else
                            {
                                if (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free)
                                    postObj.PostItem.PickInfo[0].IsPickPurchase = true;

                            }
                            if (postObj.PostItem.IsPurchased)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            var _existingItem = PickDetailsList.FirstOrDefault(p => p.PostItem.PostId == postInfo.PostId);
                            if (_existingItem != null)
                            {
                                int indexValue = PickDetailsList.IndexOf(_existingItem);
                                PickDetailsList[indexValue] = postObj;
                            }
                            else
                                PickDetailsList.Add(postObj);
                        });


                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {

                    if(PreviousSelectedList==null || PreviousSelectedList.Count==0)
                         FullPickDetailsList = new ObservableCollection<PostDetailsMainModel>(PickDetailsList);
                    IsBusy = false;
                    if (PickDetailsList.Count > 0)
                    {
                        IsPickInfoLabelVisible = true;
                        InfoVisible = false;
                    }
                    else
                    {
                        IsPickInfoLabelVisible = false;
                        InfoVisible = true;
                    }

                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        async Task<bool> GetShare(string PostID, string userid)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (ShareDetailsList == null || ShareDetailsList.Count == 0)
                    IsBusy = true;
                var postResponse = await TailDataServiceProvider.Instance.GetShare(PostID, userid.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.PostItem.Count > 0)
                        IsLoadedMore = true;
                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        postInfo.SharedUserDetails = new SharedUserInfo()
                        {
                            ShareUserId=postInfo.ShareUserId,
                            ShareUserImageName = postInfo.ShareUserImageName,
                            ShareUserName= postInfo.ShareUserName
                        };
                        postInfo.ShareText = postInfo.SharedText;
                        postInfo.IsShare = true;
                        if (postInfo.PostId == null || postInfo.UserStatus==3)
                            postInfo.PType = 3;
                        Debug.WriteLine("Post Details : " + postInfo.PostId + " " + postInfo.UserName + " " + postInfo.UserId);
                        if (postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                        }
                        postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);


                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                        };

                        
                        if (postObj.PostItem.ShareUserId != SettingsService.Instance.LoggedUserDetails.UserId)
                            postObj.MoreVisible = false;
                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0)
                        {
                            if (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;

                            if (postObj.PostItem.IsPurchased)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            var _existingItem = ShareDetailsList.FirstOrDefault(p => p.PostItem.PostId == postInfo.PostId);
                            if (_existingItem != null)
                            {
                                int indexValue = ShareDetailsList.IndexOf(_existingItem);
                                ShareDetailsList[indexValue] = postObj;
                            }
                            else
                                ShareDetailsList.Add(postObj);
                        });


                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {

                    IsBusy = false;
                    if (ShareDetailsList.Count > 0)
                    {
                        IsShareInfoLabelVisible = true;
                        InfoVisible = false;
                    }
                    else
                    {
                        IsShareInfoLabelVisible = false;
                        InfoVisible = true;
                    }

                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public void Handle_TabSelection(TabItemsModel selectedItem)
        {
            Debug.WriteLine(selectedItem);
        }

        public async Task<bool> GetFollowing(string id)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (FollowingList == null || FollowingList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<FollowingDetails> _tempFollowingData = new List<FollowingDetails>();
                var postResponse = await TailDataServiceProvider.Instance.GetFollowingList(id, UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.Count > 0 && postResponse.ResponseData[0].FollowersData.Count > 0)
                        IsLoadedMore = true;
                    foreach (FollowingDetailsMain followInfo in postResponse.ResponseData)
                    {
                        foreach (FollowingDetails followObj in followInfo.FollowersData)
                        {

                            followObj.UserDetails = new Command<int>(async (UserID) => await Handle_UserDetailsCommand(UserID));
                            followObj.FolloUnFollowCommand = new Command<int>(async (UserID) => await Handle_UnFollowCommand(UserID));


                            var sportTypes = Enum.GetValues(typeof(SportType)).Cast<SportType>();
                            for (int i = 1; i < sportTypes.Count(); i++)
                            {
                                if (followObj.MySports != null)
                                {
                                    var _existingItem = followObj.MySports.FirstOrDefault(p => p.SportId == i);
                                    if (_existingItem != null)
                                        continue;
                                }
                                if (followObj.MySports == null)
                                    followObj.MySports = new List<FollowersSport>();

                                FollowersSport newSpot = new FollowersSport()
                                {
                                    SportId = i,
                                    AccuracyPrediction = 0,
                                };
                                followObj.MySports.Add(newSpot);
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (id == "0")
                                    _tempFollowingData.Add(followObj);
                                else
                                    FollowingList.Add(followObj);
                            });


                        }

                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (id == "0")
                        FollowingList = new ObservableCollection<FollowingDetails>(_tempFollowingData);

                    if (!IsOtherProfile)
                    {
                        FollowingInfoLabelText = AppResources.FollowingInfo;
                    }
                    else
                    {
                        FollowingInfoLabelText = UserProfileDetails.UserName + AppResources.FollowerInfoOtherUser;
                    }
                    if (FollowingList.Count > 0)
                        IsFollowingInfoLabelVisible = false;
                    else
                        IsFollowingInfoLabelVisible = true;
                });


            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsBusy = false;
            return hasSuccessResponse;

        }
        public async Task<bool> GetFollowers(string Id)
        {
            bool hasSuccessResponse = false;

            try
            {
                if (FollowerList == null || FollowerList.Count == 0)
                {
                    IsBusy = true;

                    FollowerList = new ObservableCollection<FollowingDetails>();
                }
                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<FollowingDetails> _tempFollowersData = new List<FollowingDetails>();
                var postResponse = await TailDataServiceProvider.Instance.GetFollowersList(Id, UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    if (postResponse.ResponseData.Count > 0 && postResponse.ResponseData[0].FollowersData.Count>0)
                        IsLoadedMore = true;

                    foreach (FollowingDetailsMain followInfo in postResponse.ResponseData)
                    {
                        foreach (FollowingDetails followObj in followInfo.FollowersData)
                        {
                            followObj.IsDataRefresh = false;
                            followObj.UserDetails = new Command<int>(async (UserID) => await Handle_UserDetailsCommand(UserID));
                            followObj.FolloUnFollowCommand = new Command<int>(async (item) => await Handle_FollowUnfollowCommand(item));


                            var sportTypes = Enum.GetValues(typeof(SportType)).Cast<SportType>();
                            for (int i = 1; i < sportTypes.Count(); i++)
                            {
                                if (followObj.MySports != null)
                                {
                                    var _existingItem = followObj.MySports.FirstOrDefault(p => p.SportId == i);
                                    if (_existingItem != null)
                                        continue;
                                }
                                if (followObj.MySports == null)
                                    followObj.MySports = new List<FollowersSport>();
                                FollowersSport newSpot = new FollowersSport()
                                {
                                    SportId = i,
                                    AccuracyPrediction = 0,
                                };
                                followObj.MySports.Add(newSpot);
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (Id == "0")
                                    _tempFollowersData.Add(followObj);
                                else
                                    FollowerList.Add(followObj);
                            });




                        }

                    }

                }
                else
                {
                    if (postResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Id == "0")
                        FollowerList = new ObservableCollection<FollowingDetails>(_tempFollowersData);
                    if (!IsOtherProfile)
                    {
                        FollowersInfoLabelText = AppResources.FollowerInfo;
                    }
                    else
                    {
                        FollowersInfoLabelText = UserProfileDetails.UserName + AppResources.FollowerInfoOtherUser;
                    }


                    if (FollowerList.Count > 0)
                        IsFollowersInfoLabelVisible = false;
                    else
                        IsFollowersInfoLabelVisible = true;
                    IsBusy = false;
                });


            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> GetUserProfileDetails(int userid)
        {
            bool hasSuccessResponse = false;

            try
            {

                var proileResponse = await TailDataServiceProvider.Instance.GetUserDetails(userid);

                if (proileResponse.ErrorCode == 200 && proileResponse.ResponseData != null)
                {
                    PostCount = proileResponse.ResponseData.PostCount;
                    PickCount = proileResponse.ResponseData.PickCount;
                    FollowersCount = proileResponse.ResponseData.FollowersCount;
                    FollowingCount = proileResponse.ResponseData.FollowingCount;
                    PurchaseCount = proileResponse.ResponseData.PurchaseCount;
                    ShareCount = proileResponse.ResponseData.ShareCount;
                    LifeTimeUnit = proileResponse.ResponseData.LifeTimeUnit;
                    Last10Unit = proileResponse.ResponseData.LastFewUnit;
                    SportsAccuracy = new List<AccuracySports>(proileResponse.ResponseData.MySportsAccuracy);

                    List<Predictions> PredObj = GetPredictions();
                    LifetimePredictions = new ObservableCollection<Predictions>(PredObj);
                    List<Predictions> PredObj10 = GetLast10Predictions();
                    Last15Predictions = new ObservableCollection<Predictions>(PredObj10);

                    if (!IsOtherProfile)
                    {

                        UserInfo _userDetails = SettingsService.Instance.LoggedUserDetails;

                        _userDetails.UserName = proileResponse.ResponseData.UserName;
                        _userDetails.UserImage = (string.IsNullOrEmpty(proileResponse.ResponseData.UserImage) || proileResponse.ResponseData.UserImage == "string") ? string.Empty : proileResponse.ResponseData.UserImage;
                        _userDetails.CountryCode = "+" + proileResponse.ResponseData.CountryCode;
                        _userDetails.Email = proileResponse.ResponseData.Email;
                        _userDetails.Phone = proileResponse.ResponseData.Phone;
                        _userDetails.Dob = proileResponse.ResponseData.Dob;
                        _userDetails.AboutMe = proileResponse.ResponseData.AboutMe;

                        SettingsService.Instance.LoggedUserDetails = _userDetails;

                        LoggedInUser _LoggedDetails = new LoggedInUser()
                        {
                            UserId = proileResponse.ResponseData.UserId,
                            UserName = proileResponse.ResponseData.UserName,
                            UserImage = (string.IsNullOrEmpty(proileResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : proileResponse.ResponseData.UserImage,
                            AccessToken = proileResponse.ResponseData.AccessToken,
                            RefreshToken = proileResponse.ResponseData.RefreshToken,
                            Email = proileResponse.ResponseData.Email,
                            CountryCode = proileResponse.ResponseData.CountryCode,
                            IsSportsAdded = Convert.ToInt32(proileResponse.ResponseData.IsSportsAdded),
                            IsPhoneVerified = proileResponse.ResponseData.IsPhoneVerified,
                            Id = proileResponse.ResponseData.Id,
                            UserTypeId = proileResponse.ResponseData.UserTypeId,
                            IsAdmin = proileResponse.ResponseData.IsAdmin,
                            AboutMe = proileResponse.ResponseData.AboutMe,
                            Dob = proileResponse.ResponseData.Dob,
                            Phone = proileResponse.ResponseData.Phone,
                            UserStatus = proileResponse.ResponseData.UserStatus

                        };
                        DataStorageService.Instance.SaveLoginDetails(_LoggedDetails);
                        RefreshUserBasicInfo();
                    }
                    else
                    {
                      
                        if (!string.IsNullOrEmpty(proileResponse.ResponseData.UserImage) && proileResponse.ResponseData.UserImage != "string")
                        {
                            string _fullurl = TailUtils.GetThumbProfileImage(proileResponse.ResponseData.UserImage);
                            if (UserProfileDetails.UserImage != _fullurl)
                                UserProfileDetails.UserImage = _fullurl;
                        }
                        else
                        {
                            if (UserProfileDetails.UserImage != Constants.DEFAULT_USERIMAGE)
                                UserProfileDetails.UserImage = Constants.DEFAULT_USERIMAGE;
                        }
                        if (UserProfileDetails.AboutMe != proileResponse.ResponseData.AboutMe)
                        {
                            UserProfileDetails.AboutMe = proileResponse.ResponseData.AboutMe;
                            if (string.IsNullOrEmpty(UserProfileDetails.AboutMe))
                            {
                                AboutMeUpdate?.Invoke(false);
                            }
                            else
                            {
                                AboutMeUpdate?.Invoke(true);
                            }
                        }
                        else if (string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.AboutMe))
                        {
                            AboutMeUpdate?.Invoke(false);
                        }
                        if (UserProfileDetails.UserName != proileResponse.ResponseData.UserName)
                            UserProfileDetails.UserName = (proileResponse.ResponseData.UserName.Length > 20) ? proileResponse.ResponseData.UserName.Substring(0, 20) + "..." : proileResponse.ResponseData.UserName;
                        if (IsFollowing != proileResponse.ResponseData.IsFollowing)
                            IsFollowing = proileResponse.ResponseData.IsFollowing;
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        async Task Handle_AttachmentTapCommand(PostAttchment Attachment)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            List<PostAttchment> detailsObj = new List<PostAttchment>();
            if (Attachment == null)
                return;

            foreach (PostDetailsMainModel postItem in PostDetailsList)
            {
                var _existingItem = postItem.PostItem.PostedAttachments.FirstOrDefault(p => p.FileID == Attachment.FileID);
                if (_existingItem != null)
                {
                    detailsObj = new List<PostAttchment>(postItem.PostItem.PostedAttachments);
                    break;
                }
            }
            string actualUrl = "";
            if (Attachment.FileType == 1)
            {
                await PopupNavigation.Instance.PushAsync(new EnlargeImagePopUp(Attachment, detailsObj));
            }
            else
            {
                actualUrl = TailUtils.GetOrginalPostVideo(Attachment.ImageName);
              
                await PopupNavigation.Instance.PushAsync(new VideoView(actualUrl));
            }
            IsBusy = false;
        }
     
        
        List<Predictions> GetPredictions()
        {
            var predictionList = new List<Predictions>();
            var sportTypes = Enum.GetValues(typeof(SportType)).Cast<SportType>();
            for (int i = 1; i < sportTypes.Count(); i++)
            {
                Predictions sportPrediction = new Predictions();
                sportPrediction.SportId = i;
                sportPrediction.SportName = TailUtils.GameTypeToName((SportType)i);
                sportPrediction.SportImage = TailUtils.GameTypeToImage_Follow((SportType)i);
                sportPrediction.PredictionPercentage = "NA";
                if (SportsAccuracy != null && SportsAccuracy.Count != 0)
                {
                    var _existingItem = SportsAccuracy.FirstOrDefault(p => p.SportId == sportPrediction.SportId);
                    if (_existingItem != null)
                    {
                        if (_existingItem.AccuracyPrediction != null)
                        {
                            sportPrediction.PredictionPercentage = _existingItem.AccuracyPrediction + "%";
                        }
                        else
                        {
                            sportPrediction.PredictionPercentage = "NA";
                        }

                    }
                }


                predictionList.Add(sportPrediction);
            }
            return predictionList;
        }
        List<Predictions> GetLast10Predictions()
        {
            var predictionList = new List<Predictions>();
            var sportTypes = Enum.GetValues(typeof(SportType)).Cast<SportType>();
            for (int i = 1; i < sportTypes.Count(); i++)
            {
                Predictions sportPrediction = new Predictions();
                sportPrediction.SportId = i;
                sportPrediction.SportName = TailUtils.GameTypeToName((SportType)i);
                sportPrediction.SportImage = TailUtils.GameTypeToImage_Follow((SportType)i);
                sportPrediction.PredictionPercentage = "NA";
                if (SportsAccuracy != null && SportsAccuracy.Count != 0)
                {
                    var _existingItem = SportsAccuracy.FirstOrDefault(p => p.SportId == sportPrediction.SportId);
                    if (_existingItem != null)
                    {
                        if (_existingItem.AccuracyLastFew != null)
                        {
                            sportPrediction.PredictionPercentage = _existingItem.AccuracyLastFew + "%";
                        }
                        else
                        {
                            sportPrediction.PredictionPercentage = "NA";
                        }

                    }
                }


                predictionList.Add(sportPrediction);
            }
            return predictionList;
        }
     
        async Task Handle_FollowUnfollowCommand(int UserId)
        {

            var _existingItem = FollowerList.FirstOrDefault(p => p.UserId == UserId);
            if (_existingItem == null)
                return;
            if (_existingItem.IsFollowedBack)
            {
                if (!await UnFollowUser(UserId))
                    return;
                if (!IsOtherProfile)
                {
                    var _existingFollowing = FollowingList.FirstOrDefault(p => p.UserId == UserId);
                    if (_existingFollowing != null)
                        FollowingList.Remove(_existingFollowing);
                    FollowingCount = FollowingCount - 1;
                }
            }
            else
            {
                if (!await FollowUser(UserId))
                    return;
                if (!IsOtherProfile)
                {
                    FollowingList.Add(_existingItem);
                    FollowingCount = FollowingCount + 1;
                }
            }
            _existingItem.IsFollowedBack = !_existingItem.IsFollowedBack;
            Device.BeginInvokeOnMainThread(() =>
            {
                InitialiseTab(true, 2);
            });

        }
        async Task Handle_UnFollowCommand(int UserId)
        {
            if (!await UnFollowUser(UserId))
                return;
            var _existingItem = FollowingList.FirstOrDefault(p => p.UserId == UserId);
            if (_existingItem == null)
                return;
            if (!IsOtherProfile)
                FollowingList.Remove(_existingItem);
            if (FollowingList.Count > 0)
                IsFollowingInfoLabelVisible = false;
            else
                IsFollowingInfoLabelVisible = true;
            if (!IsOtherProfile)
                FollowingCount = FollowingCount - 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                InitialiseTab(true, 3);
            });
        }
        async Task Handle_FollowUnfollowProfileCommand()
        {

            if (IsBusy)
                return;
            IsBusy = true;
            if (IsFollowing)
            {
                if (!await UnFollowUser(Convert.ToInt32(UserId)))
                {
                    IsBusy = false;
                    return;
                }

            }
            else
            {
                if (!await FollowUser(Convert.ToInt32(UserId)))
                {
                    IsBusy = false;
                    return;
                }
            }
            if (UserId != 0)
                Task.Run(async () => await GetUserProfileDetails(Convert.ToInt32(UserId))).Wait();
            Device.BeginInvokeOnMainThread(() =>
            {
                InitialiseTab(true, PrevTabIndex);

            });
            IsBusy = false;

        }
        async Task Handle_FilterCommand()
        {

            await PopupNavigation.Instance.PushAsync(new FilterPopUp(PreviousSelectedList, (items) => Handle_FilterPopUpClosed(items)));


        }

        async Task Handle_MoreOptionCommand(PostDetails postItem)
        {


            await PopupNavigation.Instance.PushAsync(new Home_MorePopUp(postItem, async (item, isShare, menuIndex) => await Handle_MorePopUpClosed(postItem, isShare, menuIndex)));

        }
        async Task Handle_MorePopUpClosed(PostDetails postItem, bool isShare, int menuIndex)
        {
            if (isShare)
            {
                postItem.CreateImage = true;
            }
            else
            {
                if (menuIndex == 1 && postItem.IsShare)
                {
                    if (!await DeleteShare(postItem))
                        return;
                    var _existingItem = ShareDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                    if (_existingItem != null)
                        ShareDetailsList.Remove(_existingItem);

                    Task.Run(async () => await GetUserProfileDetails(SettingsService.Instance.LoggedUserDetails.UserId)).Wait();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (ShareDetailsList.Count > 0)
                            IsShareInfoLabelVisible = false;
                        else
                            IsShareInfoLabelVisible = true;

                        InitialiseTab(true, 5);
                    });

                }
                else if (menuIndex == 1 && await DeletePost(postItem))
                {
                    if (postItem.Post_Type == PostType.Free)
                    {
                        var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                        if (_existingItem != null)
                            PostDetailsList.Remove(_existingItem);
                    }
                    else
                    {
                        var _existingItem = PickDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                        if (_existingItem != null)
                            PickDetailsList.Remove(_existingItem);

                        FullPickDetailsList = new ObservableCollection<PostDetailsMainModel>(PickDetailsList);
                    }
                    Task.Run(async () => await GetUserProfileDetails(SettingsService.Instance.LoggedUserDetails.UserId)).Wait();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (postItem.Post_Type == PostType.Free)
                        {
                            if (PostDetailsList.Count > 0)
                                IsPostInfoLabelVisible = false;
                            else
                                IsPostInfoLabelVisible = true;

                            InitialiseTab(true, 0);
                        }
                        else
                        {
                            if (PickDetailsList.Count > 0)
                                IsPickInfoLabelVisible = false;
                            else
                                IsPickInfoLabelVisible = true;

                            InitialiseTab(true, 1);
                        }

                    });
                }

            }
        }
        async Task<bool> DeletePost(PostDetails PostItem)
        {
            bool hasSuccessResponse = false;

            try
            {
                DeletePostRequestInfo requestObj = new DeletePostRequestInfo()
                {
                    postId = PostItem.PostId.ToString(),
                    pType = (PostItem.Post_Type == PostType.Free) ? 1 : 2

                };
                var deleteResponse = await TailDataServiceProvider.Instance.DeletePost(requestObj);
                if (deleteResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                }
                else
                {
                    if (deleteResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, deleteResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> DeleteShare(PostDetails PostItem)
        {
            bool hasSuccessResponse = false;

            try
            {
                DeleteShareRequestInfo requestObj = new DeleteShareRequestInfo()
                {
                    shareId = PostItem.ShareId.ToString(),

                };
                var deleteResponse = await TailDataServiceProvider.Instance.DeleteShare(requestObj);
                if (deleteResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                }
                else
                {
                    if (deleteResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, deleteResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        void Handle_FilterPopUpClosed(List<SportType> SelectedList)
        {
            PreviousSelectedList = new List<SportType>(SelectedList);
            ObservableCollection<PostDetailsMainModel> _tempPickDetailsList = new ObservableCollection<PostDetailsMainModel>();
            IEnumerable<PostDetailsMainModel> _filteredItems;
            foreach (var item in SelectedList)
            {
                _filteredItems = FullPickDetailsList.Where(x => x.PostItem.PickInfo[0].Sport_Type == item);
                foreach (var filteredItem in _filteredItems)
                {
                    _tempPickDetailsList.Add(filteredItem);
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                if (SelectedList.Count != 0)
                    PickDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPickDetailsList);
                else
                    PickDetailsList = new ObservableCollection<PostDetailsMainModel>(PickDetailsList);

                if (PickDetailsList.Count == 0)
                    InfoVisible = true;
                else
                    InfoVisible = false;

                PickCount = PickDetailsList.Count;
                InitialiseTab(true, 1);
            });
        }
        async Task Handle_LikeCommand(PostDetails postItem)
        {
            if (postItem != null && await LikeUpdate(postItem.PostId))
            {

                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount + 1;
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount;
                postItem.PostLikedStatus = !postItem.PostLikedStatus;
                postItem.PostDisLikedStatus = false;
                var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                if (_existingItem != null)
                    _existingItem.PostItem = postItem;
            }
        }
        async Task Handle_CommentCommand(PostDetails postItem)
        {

            if (IsBusy)
                return;
            IsBusy = true;
            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired = false,
                postObj = postItem
            };
            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);
            IsBusy = false;



        }
        async Task Handle_DisLikeCommand(PostDetails postItem)
        {
            if (postItem != null && await DisLikeUpdate(postItem.PostId))
            {
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount + 1;
                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount;
                postItem.PostDisLikedStatus = !postItem.PostDisLikedStatus;
                postItem.PostLikedStatus = false;
                var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                if (_existingItem != null)
                    _existingItem.PostItem = postItem;
            }
        }


        public override void OnPageAppearing()
        {
            try
            {
                NotificationCount = SettingsService.Instance.NotificationCount;
                if (CommonSingletonUtility.SharedInstance.IsFromEditScreen)
                {
                    CommonSingletonUtility.SharedInstance.IsFromEditScreen = false;
                    return;
                }
                  
               
                if (!IsOtherProfile)
                {
                    Task.Run(async () => await GetUserProfileDetails(SettingsService.Instance.LoggedUserDetails.UserId)).Wait();
                    Task.Run(async () => await LoadSelectedTab(PrevTabIndex + 1));

                }
                else
                {
                    if (UserId != 0)
                    {
                        Task.Run(async () => await GetUserProfileDetails(Convert.ToInt32(UserId))).Wait();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            InitialiseTab(true, PrevTabIndex);

                        });
                    }


                }

                if (CommonSingletonUtility.SharedInstance.IsFromPaymentMethods)
                {
                    CommonSingletonUtility.SharedInstance.IsFromPaymentMethods = false;
                    TabSelection?.Invoke(4);
                }
                base.OnPageAppearing();
            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await ShowAlert(AppResources.AppName, "Error While Initialize Search. \nERROR : " + ex.Message);
                });
            }
            finally
            {
                IsBusy = false;
            }

        }
        void RefreshUserBasicInfo()
        {
            try
            {


                if (!string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.UserImage) && SettingsService.Instance.LoggedUserDetails.UserImage != Constants.DEFAULT_USERIMAGE)
                {
                    string _fullurl = TailUtils.GetThumbProfileImage(SettingsService.Instance.LoggedUserDetails.UserImage);
                    if (UserProfileDetails.UserImage != _fullurl)
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Task.Delay(1000);
                            UserProfileDetails.UserImage = _fullurl;
                        });
                }
                else
                {
                    if (UserProfileDetails.UserImage != Constants.DEFAULT_USERIMAGE)
                        UserProfileDetails.UserImage = Constants.DEFAULT_USERIMAGE;
                }
                if (UserProfileDetails.AboutMe != SettingsService.Instance.LoggedUserDetails.AboutMe)
                {
                    UserProfileDetails.AboutMe = SettingsService.Instance.LoggedUserDetails.AboutMe;
                    if (string.IsNullOrEmpty(UserProfileDetails.AboutMe))
                    {
                        AboutMeUpdate?.Invoke(false);
                    }
                    else
                    {
                        AboutMeUpdate?.Invoke(true);
                    }
                }
                else if (string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.AboutMe))
                {
                    AboutMeUpdate?.Invoke(false);
                }
                if (UserProfileDetails.UserName != SettingsService.Instance.LoggedUserDetails.UserName)
                    UserProfileDetails.UserName = (SettingsService.Instance.LoggedUserDetails.UserName.Length > 20) ? SettingsService.Instance.LoggedUserDetails.UserName.Substring(0, 20) + "..." : SettingsService.Instance.LoggedUserDetails.UserName;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        public void InitialiseTab(bool isRefresh, int tabIndex)
        {
            try
            {
                if (Application.Current.MainPage == null || Application.Current.MainPage.Width <= 0)
                    return;

                int _tabWidth = Convert.ToInt32((Application.Current.MainPage.Width+50) / 5);
                if (IsOtherProfile)
                    _tabWidth = Convert.ToInt32(Application.Current.MainPage.Width / 5);

                UserTabItems = new ObservableCollection<TabItemsModel> { new TabItemsModel {Id=1, Name = string.Format( AppResources.Posts,PostCount.ToString()),Count="("+PostCount.ToString()+")",TabHeaderWidth=_tabWidth},
                                       new TabItemsModel {Id=2,Name  = AppResources.Picks,Count="("+PickCount.ToString()+")",TabHeaderWidth=_tabWidth },
                                       new TabItemsModel { Id=3,Name  = AppResources.Followers,Count="("+FollowersCount.ToString()+")",TabHeaderWidth=_tabWidth } ,
                                       new TabItemsModel {Id=4, Name  = AppResources.Following,Count="("+FollowingCount.ToString()+")",TabHeaderWidth=_tabWidth } };
                if (!IsOtherProfile)
                {
                    UserTabItems.Add(new TabItemsModel { Id = 5, Name = AppResources.Purchases, Count = "(" + PurchaseCount.ToString() + ")", TabHeaderWidth = _tabWidth });
                }
                UserTabItems.Add(new TabItemsModel { Id = 6, Name = AppResources.MySharedsText, Count = "(" + ShareCount.ToString() + ")", TabHeaderWidth = _tabWidth });


                if (!IsOtherProfile)
                {
                    UserTabViews = new ObservableCollection<ContentView> { new PostsView(this), new PicksView(this), new FollowersView(this), new FollowingView(this), new PurchaseView(this), new ShareView(this) };
                }
                else
                {
                    UserTabViews = new ObservableCollection<ContentView> { new PostsView(this), new PicksView(this), new FollowersView(this), new FollowingView(this), new ShareView(this) };
                   
                }
                if (PredictionItems == null)
                {
                    int _predTabWidth = Convert.ToInt32(Application.Current.MainPage.Width / 2);
                    PredictionItems = new ObservableCollection<TabItemsModel> { new TabItemsModel { Id = 1, Name = AppResources.LifetimePredictions, Count = "NA", TabHeaderWidth = _predTabWidth }, new TabItemsModel { Id = 2, Name = AppResources.Last15Predictions, Count = "NA", TabHeaderWidth = _predTabWidth } };

                }

                PredictionViews = new ObservableCollection<ContentView> { new LifetimePredictionsView(this), new Last15PredictionsView(this) };
                if (isRefresh)
                    OnTabRefresh?.Invoke(tabIndex);
                else
                    OnTabAdded?.Invoke();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }

        async Task Handle_AccountDetailsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            await NavigationService.NavigateWithInTabToAsync<AccountDetails>();
            IsBusy = false;
        }

        async Task Handle_EditProfileCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            await NavigationService.NavigateWithInTabToAsync<EditProfile>();

            IsBusy = false;
        }
        async Task Handle_PaymentMethodCommand()
        {
            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);
        }


        public void ExecuteRefreshFollowerCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Task.Run(async () =>
            {
                IsRefreshingFollowers = true;

                await GetFollowers("0");
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsRefreshingFollowers = false;
                });
            });
            IsBusy = false;

        }
        public void ExecuteRefreshFollowingCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Task.Run(async () =>
            {
                IsRefreshingFollowing = true;

                await GetFollowing("0");
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsRefreshingFollowing = false;
                });


            });

            IsBusy = false;
        }
        public void ExecuteRefreshPostCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Task.Run(async () =>
            {
                IsRefreshingPost = true;
                await RefreshPosts();
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsRefreshingPost = false;
                });
            });
            IsBusy = false;

        }
        public async Task<bool> RefreshPosts()
        {
            bool hasSuccessResponse = false;

            try
            {
                if (PostDetailsList == null || PostDetailsList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<PostDetailsMainModel> _tempPostData = new List<PostDetailsMainModel>();
                var postResponse = await TailDataServiceProvider.Instance.GetPosts("0", UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                        {
                            attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                            if (attachObj.FileType == 1)
                            {
                                var imageLocalPath = Path.Combine(FileChacheFolder, attachObj.ImageName);
                                if (!File.Exists(imageLocalPath))
                                {
                                    string _bucketName = Constants.S3BucketForPostImage;
                                    DependencyService.Get<IAwsBucketService>().DownlaodFile(imageLocalPath, attachObj.ImageName, _bucketName);

                                }
                            }
                        }
                        postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),

                        };
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempPostData.Add(postObj);

                        });



                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

                Device.BeginInvokeOnMainThread(() =>
                {

                    PostDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPostData);
                    OnPostRefresh?.Invoke();
                    IsBusy = false;
                    if (PostDetailsList.Count > 0)
                    {
                        IsPostInfoLabelVisible = false;
                    }
                    else
                        IsPostInfoLabelVisible = true;
                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }


        public async Task<bool> RefreshPicks()
        {
            bool hasSuccessResponse = false;

            try
            {
                PreviousSelectedList = new List<SportType>();
                if (PickDetailsList == null || PickDetailsList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<PostDetailsMainModel> _tempPickData = new List<PostDetailsMainModel>();
                var postResponse = await TailDataServiceProvider.Instance.GetPicks("0", UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        if (postInfo.PostedAttachments != null && postInfo.PostedAttachments.Count != 0)
                        {

                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                            postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        }
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),

                        };


                        if (postObj.PostItem.PickInfo != null)
                        {
                            if (!IsOtherProfile)
                            {
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                            }
                            else
                            {
                                if (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free)
                                    postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                                else
                                    postObj.PostItem.PickInfo[0].IsPickPurchase = false;
                            }
                            if (postObj.PostItem.IsPurchased)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempPickData.Add(postObj);
                            PickCount = _tempPickData.Count;

                        });



                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

                Device.BeginInvokeOnMainThread(() =>
                {

                    PickDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPickData);
                    FullPickDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPickData);

                    IsBusy = false;

                    if (PostDetailsList.Count > 0)
                    {
                        IsPostInfoLabelVisible = false;
                    }
                    else
                        IsPostInfoLabelVisible = true;
                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public async Task<bool> RefreshShare()
        {
            bool hasSuccessResponse = false;

            try
            {
                PreviousSelectedList = new List<SportType>();
                if (PickDetailsList == null || PickDetailsList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<PostDetailsMainModel> _tempPickData = new List<PostDetailsMainModel>();
                var postResponse = await TailDataServiceProvider.Instance.GetShare("0", UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        postInfo.SharedUserDetails = new SharedUserInfo()
                        {
                            ShareUserId = postInfo.ShareUserId,
                            ShareUserImageName = postInfo.ShareUserImageName,
                            ShareUserName = postInfo.ShareUserName
                        };
                        postInfo.ShareText = postInfo.SharedText;
                        postInfo.IsShare = true;
                        if (postInfo.PostId == null || postInfo.UserStatus == 3)
                            postInfo.PType = 3;
                        if (postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                        }
                        postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);


                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                        };

                        if (postObj.PostItem.SharedUserDetails.ShareUserId != SettingsService.Instance.LoggedUserDetails.UserId)
                            postObj.MoreVisible = false;
                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0)
                        {

                            if (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;

                            if (postObj.PostItem.IsPurchased)
                                postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempPickData.Add(postObj);
                            PickCount = _tempPickData.Count;

                        });



                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

                Device.BeginInvokeOnMainThread(() =>
                {

                    ShareDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPickData);
                    IsBusy = false;
                    if (ShareDetailsList.Count > 0)
                    {
                        IsShareInfoLabelVisible = true;
                        InfoVisible = false;
                    }
                    else
                    {
                        IsShareInfoLabelVisible = false;
                        InfoVisible = true;
                    }
                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public async Task<bool> RefreshPurchases()
        {
            bool hasSuccessResponse = false;

            try
            {
                if (PurchaseDetailsList == null || PurchaseDetailsList.Count == 0)
                    IsBusy = true;

                if (!IsOtherProfile)
                {
                    UserId = SettingsService.Instance.LoggedUserDetails.UserId;
                }
                List<PostDetailsMainModel> _tempPurchaseData = new List<PostDetailsMainModel>();
                var postResponse = await TailDataServiceProvider.Instance.GetPurchases("0", UserId.ToString());
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        if (postInfo.PostedAttachments != null && postInfo.PostedAttachments.Count != 0)
                        {

                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                            postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        }
                        if (postInfo.Post_Type == PostType.Pick && postInfo.PickInfo != null && postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(postInfo);
                        }
                        postInfo.PostId = postInfo.PickId;
                        if (postInfo.UserStatus == 3)
                        {
                            postInfo.UserImageName = "";
                            postInfo.UserName = AppResources.AnonymousUser;
                            postInfo.UserId = 0;
                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                        };

                        if (postObj.PostItem.PickInfo != null && !IsOtherProfile)
                        {

                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _tempPurchaseData.Add(postObj);
                            PurchaseCount = _tempPurchaseData.Count;

                        });



                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }

                Device.BeginInvokeOnMainThread(() =>
                {

                    PurchaseDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPurchaseData);
                    if (PurchaseDetailsList.Count > 0)
                    {
                        IsPurchaseInfoLabelVisible = false;
                    }
                    else
                        IsPurchaseInfoLabelVisible = true;
                    IsBusy = false;

                });

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }



    }
}
