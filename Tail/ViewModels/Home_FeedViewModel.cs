using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ServiceProviders;
using Tail.Models;
using Tail.Views;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;
using System;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace Tail.ViewModels
{
    public class HomeFeedViewModel : PageViewModelBase
    {
        #region private members
        bool _isInfoLabelVisible = false;
        bool _isRefreshingPost;
        bool _scrollEnd = false;
        private CardDetailsModel _selectedCard;
        ObservableCollection<PostDetailsMainModel> _postDetailsList;
        ObservableCollection<PostDetailsMainModel> _newFeedList;
        Command _changePaymentMethodCommand;
        Command _payCommand;
        Command _refreshPostCommand;
        int _currentPage = 1;
       
        #endregion
        #region public members

        Action _popupCloseCallback;
        public Action PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }
        private int _coinBalance;
        private int _earningsBalance;
        public int CoinBalance { get => _coinBalance; set => SetProperty(ref _coinBalance, value); }
        public int EarningsBalance { get => _earningsBalance; set => SetProperty(ref _earningsBalance, value); }
        public bool IsInfoLabelVisible
        {
            get => _isInfoLabelVisible;
            set => SetProperty(ref _isInfoLabelVisible, value);
        }
        public bool IsRefreshingPost
        {
            get => _isRefreshingPost;

            set => SetProperty(ref _isRefreshingPost, value);
        }
        public ObservableCollection<PostDetailsMainModel> PostDetailsList
        {
            get => _postDetailsList;
            set => SetProperty(ref _postDetailsList, value);
        }
        public ObservableCollection<PostDetailsMainModel> NewFeedList
        {
            get => _newFeedList;
            set => SetProperty(ref _newFeedList, value);
        }
        public CardDetailsModel SelectedCard
        {
            get => _selectedCard;
            set => SetProperty(ref _selectedCard, value);
        }
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }
     
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
        private bool _initialLoad = false;
        public bool InitialLoad
        {
            get { return _initialLoad; }

            set
            {
                SetProperty(ref _initialLoad, value);
            }
        }
        private bool _isListUpdate = false;
        public bool IsListUpdate
        {
            get { return _isListUpdate; }

            set
            {
                SetProperty(ref _isListUpdate, value);
            }
        }
        
        private bool _isPullEnable = false;
        public bool IsPullEnable
        {
            get { return _isPullEnable; }

            set
            {
                SetProperty(ref _isPullEnable, value);
            }
        }
        bool _topIndicator = false;
        public bool TopIndicator
        {
            get { return _topIndicator; }

            set
            {
                SetProperty(ref _topIndicator, value);
            }
        }
        bool _bottomIndicator = false;
        public bool BottomIndicator
        {
            get { return _bottomIndicator; }

            set
            {
                SetProperty(ref _bottomIndicator, value);
            }
        }
        public Command RefreshPostCommand => _refreshPostCommand ?? (_refreshPostCommand = new Command(() => Handle_RefreshPostCommand()));
        string _fileChacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.PostDirectoryName);
        public string FileChacheFolder
        {
            get => _fileChacheFolder;
            set => SetProperty(ref _fileChacheFolder, value);
        }
        public Action ListMoveToTop
        {
            get;
            set;
        }
        public Action ListMoveDown
        {
            get;
            set;
        }
        public DateTime LastUpdatedTime
        {
            get;
            set;
        }
        public bool ScrollEndReached
        {
            get;
            set;
        } = false;
        #endregion

        public HomeFeedViewModel()
        {
            PostDetailsList = new ObservableCollection<PostDetailsMainModel>();
            if (!Directory.Exists(FileChacheFolder))
            {
                Directory.CreateDirectory(FileChacheFolder);
            }
            InitialLoad = true;
            ScrollEndReached = false;
            Task.Run(async () => await GetPostDetails(1));
            MessagingCenter.Unsubscribe<CardDetailsModel>(this, "ShowPickPurchasePopup");
            MessagingCenter.Subscribe<CardDetailsModel>(this, "ShowPickPurchasePopup", async (val) =>
            {
                SelectedCard = val;
                var postItem = CommonSingletonUtility.SharedInstance.SelectedPostDetails;
                TimeSpan _timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(postItem.PickInfo[0].GameDateTime));
                if (_timeSpan.Seconds > 0)
                {
                    postItem.PickInfo[0].IsPaidPickPurchaseEnable = false;
                    postItem.PickInfo[0].DisplyPickPrice = AppResources.WaitingForResult;
                    return;
                }
               
                await PopupNavigation.Instance.PushAsync(new PickPurchasePopup(async () => await Handle_PickPurchasePopUpClosedAsync(), postItem));
            });
            MessagingCenter.Unsubscribe<string>(this, "SharedAPost");
            MessagingCenter.Subscribe<string>(this, "SharedAPost", async (val) =>
            {
                IsListUpdate = true;
                CommonSingletonUtility.SharedInstance.IsNewPostAdded = true;
                await RefreshPost();
            });
            SelectedCard = new CardDetailsModel
            {
                CardNumber = "0000",
                CardType = "Wallet",
                ExpiryDate = "",
                IsActive = true,
                NameOnCard = "Tail Wallet"
            };
            Task.Delay(7000).ContinueWith(t => PullRefreshEnable());
        
              
        }
        public void PullRefreshEnable()
        {
            IsPullEnable = true;
        }
        ///<summary>
        ///Get Feedline Posts and Picks.
        ///</summary>
        public async Task GetPostDetails(int pageNumber)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                var postResponse = await TailDataServiceProvider.Instance.GetPostDetails(pageNumber);
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {

                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        bool _isMore = true;
                        PostDetails _postInfo= postInfo;
                        if(postInfo.Post_Type==PostType.SharedFree || postInfo.Post_Type == PostType.SharedPick)
                        {
                            if (postInfo.SharedUserDetails == null)
                                continue;
                            if (postInfo.SharedUserDetails.ShareUserId != SettingsService.Instance.LoggedUserDetails.UserId)
                                _isMore = false;
                            if (postInfo.SharedData!=null)
                                _postInfo = postInfo.SharedData;
                            _postInfo.ShareText = postInfo.ShareText;
                            _postInfo.SharePostedDate = postInfo.PostedDate;
                            _postInfo.SharedUserDetails = postInfo.SharedUserDetails;
                            _postInfo.ShareId = postInfo.PostId;
                            _postInfo.IsShare = true;
                        }
                        if (_postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in _postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                        }
                        _postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        if (_postInfo.Post_Type == PostType.Pick && _postInfo.PickInfo != null && _postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(_postInfo);
                          

                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = _postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID)),
                            MoreVisible= _isMore
                        };


                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0 && (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId))
                        {
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        if (postObj.PostItem.IsPurchased)
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;

                        var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postInfo.PostId);
                        if (_existingItem != null)
                        {
                            if (!_existingItem.PostItem.IsShare)
                            {
                                int indexValue = PostDetailsList.IndexOf(_existingItem);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    PostDetailsList[indexValue] = postObj;
                                });
                            }

                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                PostDetailsList.Add(postObj);
                            });
                        }



                    }
                    if (postResponse.ResponseData.PostItem.Count<Constants.PAGINATION_COUNT)
                    {
                        ScrollEndReached = true;
                    }
                  


                }
                else
                {
                    if (postResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Task.Run(async () => await ShowAlert(AppResources.AppName, postResponse.Message));
                        });
                    }
                    else
                    {
                        IsBusy = false;
                        await GetPostDetails(CurrentPage);
                        return;
                    }

                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (PostDetailsList.Count > 0)
                        IsInfoLabelVisible = false;
                    else
                        IsInfoLabelVisible = true;
                    InitialLoad = false;
                    BottomIndicator = false;

                    IsBusy = false;
                    ScrollEnd = false;
                    LastUpdatedTime = DateTime.Now;
                     
                });

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                    ScrollEnd = false;
                    ScrollStart = false;
                    TopIndicator = false;
                    IsListUpdate = false;
                    Task.Run(async () => await ShowAlert(AppResources.AppName, ex.Message));
                });
            }
        }
      

        async Task Handle_MoreOptionCommand(PostDetails postItem)
        {
            await PopupNavigation.Instance.PushAsync(new Home_MorePopUp(postItem, async (item, isShare, menuIndex) => await Handle_MoreOptionPopUpClosed(postItem, isShare, menuIndex)));


        }
        async Task Handle_MoreOptionPopUpClosed(PostDetails postItem, bool isShare, int menuIndex)
        {
            if (!isShare)
            {
                if (menuIndex == 1 && postItem.IsShare)
                {
                    if (!await DeleteShare(postItem))
                        return;
                    var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.sharePostId == postItem.sharePostId);
                    if (_existingItem != null)
                        PostDetailsList.Remove(_existingItem);

                }
                else
                {
                    var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                    if (menuIndex == 2 && _existingItem != null)
                    {
                        PostDetailsList.Remove(_existingItem);
                    }
                    else if (menuIndex == 1 && postItem.UserId == SettingsService.Instance.LoggedUserDetails.UserId && await DeletePost(postItem) && _existingItem != null)
                    {
                        PostDetailsList.Remove(_existingItem);
                    }
                }
            }
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

        ///<summary>
        ///Handle like button click.
        ///</summary>
        async Task Handle_LikeCommand(PostDetails postItem)
        {
            if (IsBusy || postItem.IsShareNotAvailable)
                return;
            IsBusy = true;
            if (await LikeUpdate(postItem.PostId))
            {
                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount + 1;
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount;
                postItem.PostLikedStatus = !postItem.PostLikedStatus;
                postItem.PostDisLikedStatus = false;
                var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                _existingItem.PostItem = postItem;
            }
            IsBusy = false;

        }
        async Task Handle_CommentCommand(PostDetails postItem)
        {
            if (IsBusy || postItem.IsShareNotAvailable)
                return;
            IsBusy = true;
            PostEventsArgs paramObj = new PostEventsArgs()
            {
                serviceRequired=false,
                postObj=postItem
            };

            await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);
            IsBusy = false;

        }
        ///<summary>
        ///Handle dis like button click.
        ///</summary>
        async Task Handle_DisLikeCommand(PostDetails postItem)
        {
            if (IsBusy || postItem.IsShareNotAvailable)
                return;
            IsBusy = true;
            if (await DisLikeUpdate(postItem.PostId))
            {
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount + 1;
                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount;
                postItem.PostDisLikedStatus = !postItem.PostDisLikedStatus;
                postItem.PostLikedStatus = false;

                var _existingItem = PostDetailsList.FirstOrDefault(p => p.PostItem.PostId == postItem.PostId);
                _existingItem.PostItem = postItem;
            }
            IsBusy = false;
        }

        async Task Handle_PickPurchasePopUpClosedAsync()
        {
            await PopupNavigation.Instance.PopAllAsync();
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
            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);
        }
        void Handle_RefreshPostCommand()
        {
            if (IsBusy)
                return;
            IsRefreshingPost = true;
            TopIndicator = true;
            Task.Run(async () => await RefreshPost());
            IsRefreshingPost = false;
        }

        async Task RefreshPost()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                CurrentPage = 1;
                Debug.WriteLine("Home Feed : Refresh");
                ScrollEndReached = false;
                var postResponse = await TailDataServiceProvider.Instance.GetPostDetails(1);
                List<PostDetailsMainModel> _tempPostData = new List<PostDetailsMainModel>();
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {


                    foreach (PostDetails postInfo in postResponse.ResponseData.PostItem)
                    {
                        bool _isMore = true;
                        PostDetails _postInfo = postInfo;
                        if (postInfo.Post_Type == PostType.SharedFree || postInfo.Post_Type == PostType.SharedPick)
                        {
                            if (postInfo.SharedUserDetails == null)
                                continue;
                            if (postInfo.SharedUserDetails.ShareUserId != SettingsService.Instance.LoggedUserDetails.UserId)
                                _isMore = false;
                            if (postInfo.SharedData != null)
                                _postInfo = postInfo.SharedData;
                            _postInfo.ShareText = postInfo.ShareText;
                            _postInfo.ShareId = postInfo.PostId;
                            _postInfo.SharePostedDate = postInfo.PostedDate;
                            _postInfo.SharedUserDetails = postInfo.SharedUserDetails;
                            _postInfo.IsShare = true;
                        }
                        if (_postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in _postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                            }
                        }
                        _postInfo.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                        if (_postInfo.Post_Type == PostType.Pick && _postInfo.PickInfo != null && _postInfo.PickInfo.Count != 0)
                        {
                            UpdatePickUI(_postInfo);


                        }
                        PostDetailsMainModel postObj = new PostDetailsMainModel
                        {
                            PostItem = _postInfo,
                            MoreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)),
                            LikeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)),
                            CommentCommand = new Command<PostDetails>(async (item) => await Handle_CommentCommand(item)),
                            DisLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)),
                            PickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)),
                            UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID)),
                            MoreVisible=_isMore
                        };



                        if (postObj.PostItem.PickInfo != null && postObj.PostItem.PickInfo.Count != 0 && (postObj.PostItem.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postObj.PostItem.UserId))
                        {
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        }
                        if (postObj.PostItem.IsPurchased)
                            postObj.PostItem.PickInfo[0].IsPickPurchase = true;
                        _tempPostData.Add(postObj);


                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PostDetailsList = new ObservableCollection<PostDetailsMainModel>(_tempPostData);

                    });
                }
                else
                {
                    if (postResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Task.Run(async () => await ShowAlert(AppResources.AppName, postResponse.Message));
                        });
                    } 
                    else
                    {
                        IsBusy = false;
                        await RefreshPost();

                        return;
                    }
                }


                Device.BeginInvokeOnMainThread(() =>
                {

                    if (CommonSingletonUtility.SharedInstance.IsNewPostAdded)
                    {
                        CommonSingletonUtility.SharedInstance.IsNewPostAdded = false;
                        ListMoveToTop?.Invoke();
                    }
                    else
                    {
                        if (Device.RuntimePlatform != Device.iOS)
                            ListMoveDown?.Invoke();
                    }
                    if (PostDetailsList.Count > 0)
                        IsInfoLabelVisible = false;
                    else
                        IsInfoLabelVisible = true;
                    IsBusy = false;
                    ScrollEnd = false;
                    ScrollStart = false;
                    TopIndicator = false;
                    IsListUpdate = false;
                    LastUpdatedTime = DateTime.Now;
                });

            }
            catch (Exception ex)
            {
              
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                    ScrollEnd = false;
                    ScrollStart = false;
                    TopIndicator = false;
                    IsListUpdate = false;
                    Task.Run(async () => await ShowAlert(AppResources.AppName, ex.Message));
                });
            }
        }
        public override void OnPageAppearing()
        {



            try
            {

                NotificationCount = SettingsService.Instance.NotificationCount;
                
                if (IsBusy)
                    return;
                if ((DateTime.Now - LastUpdatedTime).TotalSeconds > 60 || CommonSingletonUtility.SharedInstance.IsNewPostAdded)
                {
                    IsListUpdate = true;
                    Task.Run(async () => await RefreshPost());
                }
                    

                base.OnPageAppearing();
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        public async Task GetCoinBalance()
        {
            var response = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                CoinBalance = response.ResponseData.BalanceCoins;
                CommonSingletonUtility.SharedInstance.CoinBalance = CoinBalance;
                EarningsBalance = response.ResponseData.EarningBalance;
            }
        }
    }
}
