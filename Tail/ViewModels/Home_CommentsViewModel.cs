using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Tail.Services.Helper;
using Xamarin.Forms;
using System.Collections.Generic;
using System.IO;
using Tail.Services.Interfaces;

namespace Tail.ViewModels
{
    public class HomeCommentsViewModel : PageViewModelBase
    {
        #region private members
        bool _isPickPost;
        bool _loadMoreVisible = false;
        bool _isLoadMoreBusy = false;
        bool _isCommentVisible = false;
        bool _initialLoad = false;

        int _pageNumber;
        int _totalPage;

        string _loggedInUserImage;
        string _enterCommentText;
        string _lastSyncTime;
        string _previousCommentSyncTime;

        ObservableCollection<Comments> _commentDetailsList;

        PostDetails _postItem;



        Command<PostDetails> _moreOptionCommand;
        Command<PostDetails> _likeCommand;
        Command<PostDetails> _disLikeCommand;
        Command<PostDetails> _pickPurchase;
        Command _sendComment;
        Command _previousComments;
        #endregion
        #region public members

        public bool IsPickPost
        {
            get => _isPickPost;
            set => SetProperty(ref _isPickPost, value);
        }
        public bool InitialLoad
        {
            get => _initialLoad;
            set => SetProperty(ref _initialLoad, value);
        }

        public bool LoadMoreVisible
        {
            get => _loadMoreVisible;
            set => SetProperty(ref _loadMoreVisible, value);
        }
        public bool IsLoadMoreBusy
        {
            get => _isLoadMoreBusy;
            set => SetProperty(ref _isLoadMoreBusy, value);
        }
        public bool IsCommentVisible
        {
            get => _isCommentVisible;
            set => SetProperty(ref _isCommentVisible, value);
        }
        public int PageNumber
        {
            get => _pageNumber;
            set => SetProperty(ref _pageNumber, value);
        }
        public int TotalPage
        {
            get => _totalPage;
            set => SetProperty(ref _totalPage, value);
        }
        public string LoggedInUserImage
        {
            get => _loggedInUserImage;
            set => SetProperty(ref _loggedInUserImage, value);
        }
        public string EnterCommentText
        {
            get => _enterCommentText;
            set => SetProperty(ref _enterCommentText, value);
        }

        public string LastSyncTime
        {
            get => _lastSyncTime;
            set => SetProperty(ref _lastSyncTime, value);
        }
        public string PreviousCommentSyncTime
        {
            get => _previousCommentSyncTime;
            set => SetProperty(ref _previousCommentSyncTime, value);
        }
        public ObservableCollection<Comments> CommentDetailsList
        {
            get => _commentDetailsList;
            set => SetProperty(ref _commentDetailsList, value);
        }

        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }

        public Command<PostDetails> MoreOptionCommand => _moreOptionCommand ?? (_moreOptionCommand = new Command<PostDetails>(async (item) => await Handle_MoreOptionCommand(item)));
        public Command<PostDetails> LikeCommand => _likeCommand ?? (_likeCommand = new Command<PostDetails>(async (item) => await Handle_LikeCommand(item)));
        public Command<PostDetails> DisLikeCommand => _disLikeCommand ?? (_disLikeCommand = new Command<PostDetails>(async (item) => await Handle_DisLikeCommand(item)));
        public Command<PostDetails> PickPurchase => _pickPurchase ?? (_pickPurchase = new Command<PostDetails>(async (item) => await Handle_PickPurchase(item)));
        public Command SendComment => _sendComment ?? (_sendComment = new Command(async () => await Handle_sendComment()));
        public Command PreviousComments => _previousComments ?? (_previousComments = new Command(async () => await Handle_PreviousComments()));


        public Action OnCommentUpdated
        {
            get;
            set;
        }
        public Action OnLoadMoreUpdated
        {
            get;
            set;
        }
        public Action AddNewCommentUpdated
        {
            get;
            set;
        }
        #endregion
        public string FileChacheFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.PostDirectoryName);


        public HomeCommentsViewModel()
        {
            try
            {
                CommentDetailsList = new ObservableCollection<Comments>();
                PageNumber = 1;

            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await ShowAlert(AppResources.AppName, "Error While Dismiss Comment. \nERROR : " + ex.Message);
                });
            }

        }
        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {
                PostEventsArgs _args = (PostEventsArgs)parameter;
                int pType = (_args.postObj.Post_Type == PostType.Free) ? 1 : 2;
                if (!_args.serviceRequired || !await GetPicksDetails(pType, _args.postObj.PostId))
                {
                    PostItem = _args.postObj;

                }
                else
                {
                    if (PostItem.PickInfo != null && PostItem.PickInfo.Count != 0)
                        UpdatePickUI(PostItem);
                }


                if (PostItem.PickInfo != null && PostItem.PickInfo.Count != 0)
                {
                    IsCommentVisible = PostItem.PickInfo[0].IsPickPurchase;
                }
                else
                    IsCommentVisible = true;
                //PostItem.PostId = "5fbf5c6b9a25e8cfbe668a92"; // Remove this(Only for Debug Purpose)
                if (PostItem.PostedAttachments != null && PostItem.PostedAttachments.Count != 0)
                {
                    foreach (PostAttchment attachObj in PostItem.PostedAttachments)
                    {
                        attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                    }
                    PostItem.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));

                }
                InitialLoad = false;
                IsPickPost = (PostItem.Post_Type == PostType.Pick);
                if (PostItem.PickInfo == null || PostItem.PickInfo.Count == 0 || (PostItem.PickInfo != null && PostItem.PickInfo.Count != 0 && PostItem.PickInfo[0].IsPickPurchase))
                {
                    IsBusy = true;
                    LoggedInUserImage = (SettingsService.Instance.LoggedUserDetails != null && !string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.UserImage)) ? TailUtils.GetThumbProfileImage(SettingsService.Instance.LoggedUserDetails.UserImage) : Constants.DEFAULT_USERIMAGE;
                    if (await GetCommentDetails()&& CommentDetailsList.Count > 0)
                    {
                        OnCommentUpdated?.Invoke();
                    }

                    IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Initialize Comments. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
      
        async Task<bool> GetPicksDetails(int type, string postId)
        {
            bool hasSuccessResponse = false;
            InitialLoad = true;
            try
            {

                var postResponse = await TailDataServiceProvider.Instance.GetPickDetails(type, postId);
                if (postResponse.ErrorCode == 200 && postResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    PostDetails postInfo = postResponse.ResponseData;

                    if (postInfo.PickInfo != null)
                    {
                        UpdatePickUI(postInfo);
                        if (postInfo.PostedAttachments != null)
                        {
                            foreach (PostAttchment attachObj in postInfo.PostedAttachments)
                            {
                                attachObj.AttachmentTap = new Command<PostAttchment>(async (item) => await Handle_AttachmentTapCommand(item));
                            }
                        }


                        if (postInfo.PickInfo != null && postInfo.PickInfo.Count != 0 && (postInfo.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postInfo.UserId))
                        {
                            postInfo.PickInfo[0].IsPickPurchase = true;
                        }
                    

                        if (postInfo.IsPurchased)
                            postInfo.PickInfo[0].IsPickPurchase = true;
                    }
                    else
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
                    }

                    PostItem = postInfo;
                    IsBusy = false;



                }
                else
                {
                    if (postResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, postResponse.Message);
                }


            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

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
                if (menuIndex == 2)
                {
                    Back.Execute(null);
                }
                else if (menuIndex == 1 && postItem.UserId == SettingsService.Instance.LoggedUserDetails.UserId && await DeletePost(postItem))
                {

                    Back.Execute(null);

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
        async Task Handle_AttachmentTapCommand(PostAttchment Attachment)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (Attachment == null)
                return;

            List<PostAttchment> detailsObj = new List<PostAttchment>(PostItem.PostedAttachments);
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
        async Task Handle_LikeCommand(PostDetails postItem)
        {
            if (await LikeUpdate(postItem.PostId))
            {
                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount + 1;
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount;
                postItem.PostLikedStatus = !postItem.PostLikedStatus;
                postItem.PostDisLikedStatus = false;
                PostItem = postItem;
            }
        }

        async Task Handle_DisLikeCommand(PostDetails postItem)
        {
            if (await DisLikeUpdate(postItem.PostId))
            {
                postItem.DisLikeCount = (postItem.LikedStatus == LikeStatus.Dislike) ? postItem.DisLikeCount - 1 : postItem.DisLikeCount + 1;
                postItem.LikeCount = (postItem.LikedStatus == LikeStatus.Like) ? postItem.LikeCount - 1 : postItem.LikeCount;
                postItem.PostDisLikedStatus = !postItem.PostDisLikedStatus;
                postItem.PostLikedStatus = false;
                PostItem = postItem;
            }
        }
        async Task Handle_DeleteCommand(string CommentID)
        {
            bool confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmCommentDelete);
            if (confirmation)
            {
                var itemToRemove = CommentDetailsList.SingleOrDefault(r => r.ID == CommentID);
                if (itemToRemove != null && !CommentID.StartsWith("temp_"))
                {
                    itemToRemove.IsBusy = true;
                    if (await DeleteComment(CommentID))
                    {
                        itemToRemove.IsBusy = false;
                        CommentDetailsList.Remove(itemToRemove);
                        PostItem.CommentCount = PostItem.CommentCount - 1;
                    }
                    else
                    {
                        itemToRemove.IsBusy = false;
                    }
                }
                else
                {
                    if (itemToRemove != null)
                        CommentDetailsList.Remove(itemToRemove);
                }

                OnLoadMoreUpdated?.Invoke();
            }
        }


        async Task Handle_sendComment()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(EnterCommentText))
            {
                EnterCommentText = "";
                IsBusy = false;
                return;
            }

            CommentDetailsList.Add(new Comments
            {
                ID = "temp_" + Guid.NewGuid().ToString(),
                UserID = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.UserId : 0,
                PostId = PostItem.PostId,
                PostedDateTime = DateTime.Now.ToString(),
                CommentText = EnterCommentText.Trim(),
                UserName = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.UserName : "",
                UserImage = LoggedInUserImage,
                IsBusy = true,
                IsSuccess = true

            });

            AddNewCommentUpdated?.Invoke();
            string _comment = EnterCommentText;
            EnterCommentText = "";
            PostItem.CommentCount = PostItem.CommentCount + 1;
            if (await AddComment(_comment, CommentDetailsList.Count - 1))
                AddNewCommentUpdated?.Invoke();
            IsBusy = false;
        }
        async Task Handle_PreviousComments()
        {
            if (IsLoadMoreBusy)
                return;
            IsLoadMoreBusy = true;
            if (TotalPage > PageNumber)
            {
                PageNumber += 1;
                if (await GetCommentDetails())
                {
                    LoadMoreVisible = (PageNumber != TotalPage);
                    OnLoadMoreUpdated?.Invoke();
                }
            }
            IsLoadMoreBusy = false;
        }
        private async Task<bool> GetCommentDetails()
        {
            bool hasSuccessResponse = false;
            try
            {
                if (string.IsNullOrEmpty(PreviousCommentSyncTime))
                {
                    PreviousCommentSyncTime = "0";
                }
                var commentsResponse = await TailDataServiceProvider.Instance.GetComments(PostItem.PostId, PreviousCommentSyncTime);
                if (commentsResponse.ErrorCode == 200 && commentsResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    foreach (var commentItem in commentsResponse.ResponseData.CommentsData)
                    {
                        var checkItem = CommentDetailsList.SingleOrDefault(r => r.ID == commentItem.ID);
                        if (checkItem != null)
                            continue;
                        if (commentItem.Status != 3)
                        {
                            commentItem.UserImage = TailUtils.GetThumbProfileImage(commentItem.UserImage);
                            commentItem.UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID));
                            commentItem.DeleteCommand = new Command<string>(async (commentID) => await Handle_DeleteCommand(commentID));
                        }
                        else
                        {
                            commentItem.UserImage = "";
                            commentItem.UserName = AppResources.AnonymousUser;
                            commentItem.UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(0));
                            
                        }
                        CommentDetailsList.Insert(0, commentItem);
                        if ((PageNumber == 1 && string.IsNullOrEmpty(LastSyncTime)))
                        {
                            LastSyncTime = commentItem.PostedDateTime;
                        }
                        PreviousCommentSyncTime = commentItem.PostedDateTime;
                    }
                    if (commentsResponse.ResponseData.PageInfo.Count > 0)
                    {
                        if (PageNumber == 1 && commentsResponse.ResponseData.PageInfo[0].totalPages > 1)
                        {
                            TotalPage = commentsResponse.ResponseData.PageInfo[0].totalPages;
                            PostItem.CommentCount = commentsResponse.ResponseData.PageInfo[0].totalRecords;
                        }

                        LoadMoreVisible = (PageNumber == 1 && TotalPage > 1);
                    }


                }
                else
                {
                    if (commentsResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, commentsResponse.Message);
                }


            }
            catch (Exception ex)
            {
                IsLoadMoreBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            return hasSuccessResponse;
        }

        private async Task<bool> AddComment(string CommentText, int ItemIndex)
        {
            bool hasSuccessResponse = false;
            try
            {
                CommentRequestInfo reqObj = new CommentRequestInfo()
                {
                    postId = PostItem.PostId,
                    comment = CommentText.Trim(),
                    lastAddedOn = string.IsNullOrEmpty(LastSyncTime) ? "0" : LastSyncTime
                };
                var commentsResponse = await TailDataServiceProvider.Instance.AddComment(reqObj);
                if (commentsResponse.ErrorCode == 200 && commentsResponse.ResponseData != null)
                {

                    foreach (var NewCommentsItem in commentsResponse.ResponseData.Reverse())
                    {
                        var existItem = CommentDetailsList.LastOrDefault(r => r.ID == NewCommentsItem.ID);
                        if (existItem != null)
                            continue;

                        LastSyncTime = NewCommentsItem.PostedDateTime;
                        var checkItem = CommentDetailsList.Last(r => r.UserID == NewCommentsItem.UserID && r.CommentText == NewCommentsItem.CommentText);
                        if (checkItem != null)
                            CommentDetailsList.Remove(checkItem);

                        NewCommentsItem.UserImage = TailUtils.GetThumbProfileImage(NewCommentsItem.UserImage);
                        NewCommentsItem.UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID));
                        NewCommentsItem.DeleteCommand = new Command<string>(async (commentID) => await Handle_DeleteCommand(commentID));


                        CommentDetailsList.Add(NewCommentsItem);

                    }

                    hasSuccessResponse = true;
                }
                else
                {
                    CommentDetailsList[ItemIndex].IsBusy = false;
                    CommentDetailsList[ItemIndex].IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            return hasSuccessResponse;
        }

        private async Task<bool> DeleteComment(string CommentID)
        {
            bool hasSuccessResponse = false;
            try
            {
                DeleteCommentRequestInfo reqObj = new DeleteCommentRequestInfo()
                {
                    commentId = CommentID,
                    postId = PostItem.PostId,

                };
                var commentsResponse = await TailDataServiceProvider.Instance.DeleteComment(reqObj);
                if (commentsResponse.ErrorCode == 200)
                {

                    hasSuccessResponse = true;
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            return hasSuccessResponse;
        }

    }
}
