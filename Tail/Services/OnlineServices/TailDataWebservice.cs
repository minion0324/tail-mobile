
using System.Collections.Generic;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.Interfaces;
using Tail.Services.Responses;
using Tail.Services.ServiceProviders;

namespace Tail.Services.OnlineServices
{
    public class TailDataWebservice : WebserviceBase, ITailDataService
    {

        const string kAddPostUri = "https://ukuj4fjv16.execute-api.us-east-2.amazonaws.com/prod/AddPost";
        const string kAddPickUri = "https://r0eb23r8s0.execute-api.us-east-2.amazonaws.com/prod/AddPick";
        const string kTrendUri = "https://gn98fxbf3h.execute-api.us-east-2.amazonaws.com/prod/TrendingListProduction";
        const string kSearchUri ="https://4pyssap8h8.execute-api.us-east-2.amazonaws.com/prod/TailProductionGeneralSearch";
        const string kLoginUri = "users/userLogin";
        const string kAppleLoginUri = "users/appleLogin";
        const string kFBLoginUri = "users/fbLogin";
        const string kSignUpUri = "users/signUp";
        const string kAppleSignUpUri = "users/appleSignUp";
        const string kFBSignUpUri = "users/fbSignUp";
        const string kOTPUri = "users/verify/phone";
        const string kForgotPasswordUri = "users/forgotPassword";
        const string kResendOTPUri = "users/resendOTP";
        const string kUpdatePasswordUri = "users/updatePassword";
        const string kRemoveAccountUri = "users/removeAccount";
        const string kLogoutUri = "users/logout";
        const string kInterestListUri = "sports/interestList";
        const string kUserInterestListUri = "sports/userInterestList";
        const string kUpdateInterestListUri = "sports/updateInterestList";
        const string kGetLeagueUri = "sports/getLeague";
        const string kLikeUri = "post/likePost";
        const string kHideUri = "post/hidePost";
        const string kReportPostUri = "post/reportPost";
        const string kDisLikeUri = "post/dislikePost";
        const string kUpdateProfile = "users/updateProfile";
        const string kGetUserdetails = "users/getUserDetails";

        const string kGetCommentUri = "comment/getCommentList";
        const string kAddCommentUri = "comment/addNewComment";
        const string kDeleteCommentUri = "comment/deleteComment";
        const string kDeletePostUri = "post/deletePost";
        const string kGetPostUri = "post/getPostList";
        const string kGetGameScheduletUri = "pick/getSchedule";
        const string kGetUserSuggestionsUri = "users/getUserSuggestion";
        const string kFollowUserUri = "follow/addFollow";
        const string kUnFollowUserUri = "follow/removeFollow";
        const string kGetFollowingUri = "follow/getFollowingList";
        const string kGetFollowersUri = "follow/getFollowerList";
        
        const string kGetPickUri = "pick/getPickList";
        const string kGetHomeFeedUri = "feed/home";
        const string kContactUsUri = "users/contactUs";
        const string kReportProblemUri = "post/reportPblm";
        const string kVerifyPurchaseUri = "coin/appleTokenValidation";
        const string kVerifyPurchaseAndroidUri = "coin/googleTokenValidation";
        const string kAddCoinUri = "coin/addCoin";
        const string kGetCoinHistoryUri = "coin/getCoinHistory";
        const string kGetCoinDetailsUri = "coin/getCoinDetails";

        const string kGetPurchasesUri = "pick/getPurchasePickList";
        const string kGetAppMinimumVersionUri = "users/getMinimumVersion";
        const string kPickPurchaseUri = "pick/pickPurchase";
        const string kGetPUrchaseHistoryUri = "pick/getPurchaseHistory";
        const string kGetEarningsHistoryUri = "userWeb/getSoldHistory";
        const string kGetPayOutHistoryUri = "userWeb/payout-list";
        const string kAddShare = "share/addSharePost";
        const string kSaveSettings = "settings/saveSettings";
        const string kGetSettings = "settings/getUserSettings";
        const string kDeviceToken = "token/deviceToken";
        const string kGetPickDetailsUri = "pick/getPickDetail";
        const string kReadMessage = "notify/readMsg";
        const string kReadAllMessage = "notify/readAllNotify";
        
        const string kGetNotification = "notify/getNotifyList";
        const string kClearAll = "notify/clearAll";

        const string kGetShareUri = "share/getSharedList";
        const string kDeleteShareUri = "share/deleteSharePost";
        const string kUnreadNotificationUri = "notify/unreadMsgCount";

        public Task<ServiceResponse<LoginResponseInfo>> VerifyLogin(LoginRequestInfo LoginInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kLoginUri), LoginInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyAppleLogin(AppleLoginRequestInfo LoginInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kAppleLoginUri), LoginInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyFBLogin(FBLoginRequestInfo LoginInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kFBLoginUri), LoginInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifySignUp(SignUpRequestInfo SignUpInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kSignUpUri), SignUpInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyAppleSignUp(AppleSignUpRequestInfo SignUpInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kAppleSignUpUri), SignUpInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyFBSignUp(FBSignUpRequestInfo SignUpInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kFBSignUpUri), SignUpInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyOTP(OtpRequestInfo OTPInfo)
        {
            return PostAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kOTPUri), OTPInfo);
        }
        public Task<ServiceResponseBase> ResendOTP(OtpResendInfo OTPInfo)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kResendOTPUri), OTPInfo);
        }
        public  Task<ServiceResponseBase> ForgotPassword(ForgotPasswordRequestInfo ForgotPasswordInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kForgotPasswordUri), ForgotPasswordInfo);
        }
        public Task<ServiceResponseBase> UpdatePassword(UpdatePasswordInfo updatePasswordInfo)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUpdatePasswordUri), updatePasswordInfo);
        }
        public Task<ServiceResponseBase> RemoveAccount()
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kRemoveAccountUri), null);
        }
        public Task<ServiceResponse<List<InterestListResponseItems>>> GetInterestList()
        {
            return GetAsync<ServiceResponse<List<InterestListResponseItems>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kInterestListUri), string.Empty);
        }
        public Task<ServiceResponse<List<int>>> GetUserInterestList()
        {
            return GetAsync<ServiceResponse<List<int>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUserInterestListUri), string.Empty);
        }
        public Task<ServiceResponseBase> UpdateInterestList(UpdateInterestListRequestInfo updateInterestListInfo)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUpdateInterestListUri), updateInterestListInfo);
        }

        public Task<ServiceResponseBase> UpdateProfile(UpdateProileRequestInfo updateProfileInfo)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUpdateProfile), updateProfileInfo);
}
        public Task<ServiceResponseBase> AddPost(AddPostRequestInfo addPostInfo)
        {
            return PostAsync<ServiceResponseBase>(kAddPostUri, addPostInfo);
        }
        public Task<byte[]> DownloadImageFromUrl(string ImageUrl)
        {
            return DownloadImageAsync(ImageUrl);
        }
        public Task<ServiceResponseBase> Logout(LogoutRequestInfo LogoutInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kLogoutUri), LogoutInfo);
        }
        public Task<ServiceResponse<PostResponse>> GetPostDetails(int PageNumber)
        {
            string param = string.Format(@"/{0}", PageNumber);
            return GetAsync<ServiceResponse<PostResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetHomeFeedUri), param);
        }
        public Task<ServiceResponse<ProfileDetails>> GetProfileDetails()
        {
            return TailDataServiceProvider.Instance.GetProfileDetails();
        }
        public Task<ServiceResponse<ProfileDetails>> GetProfileDetails(long userId)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<CommentsMain>> GetComments(string PostID, string PreviousCommentSyncTime)
        {
            string param = string.Format(@"/{0}/{1}", PreviousCommentSyncTime, PostID);
            return GetAsync<ServiceResponse<CommentsMain>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetCommentUri), param);
        }
        public Task<ServiceResponse<IList<Comments>>> AddComment(CommentRequestInfo commentObj)
        {
            return PostAsync<ServiceResponse<IList<Comments>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kAddCommentUri), commentObj);
        }
        public Task<ServiceResponse<List<StepsDetails>>> GetGameDetails()
        {
             throw new System.NotImplementedException();
        }

        public Task<ServiceResponse<List<RecommendedFollowers>>> GetRecommendedFollowers()
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LeagueDetailsResponse>> GetLeagueDetails(int SportID)
        {
            string param = string.Concat("?mainSportId=", SportID);
            return GetAsync<ServiceResponse<LeagueDetailsResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetLeagueUri), param);
            
        }
        public Task<ServiceResponse<List<NotificationModel>>> GetNotifications()
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<TrendingModel>> GetTrendingDetails()
        {
            throw new System.NotImplementedException();
        }

       
        public  Task<ServiceResponse<TrendingModel>> GetSearchDetails()
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> LikeAPost(PostStatusRequest PostInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kLikeUri), PostInfo);
        }
        public Task<ServiceResponseBase> DisLikeAPost(PostStatusRequest PostInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kDisLikeUri), PostInfo);
        }
        public Task<ServiceResponseBase> HideAPost(PostStatusRequest PostInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kHideUri), PostInfo);
        }
        public Task<ServiceResponseBase> ReportAPost(ReportPostRequestInfo ReportInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kReportPostUri), ReportInfo);
        }
        public Task<ServiceResponse<LoginResponseInfo>> GetUserDetails(int userId)
        {
            string param = string.Concat("?userId=", userId);
            return GetAsync<ServiceResponse<LoginResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetUserdetails), param);
        }
        public Task<ServiceResponseBase> DeleteComment(DeleteCommentRequestInfo CommentInfo)
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kDeleteCommentUri), CommentInfo);
        }
        public Task<ServiceResponseBase> DeletePost(DeletePostRequestInfo PostInfo)
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kDeletePostUri), PostInfo);
        }
        public Task<ServiceResponse<PostResponse>> GetPosts(string PostID,string UserID)
        {
            string param = string.Format(@"/{0}/{1}", PostID, UserID);
            return GetAsync<ServiceResponse<PostResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPostUri), param);
           
        }
        public Task<ServiceResponse<List<GameSchedule>>> GetGameSchedules(int LeagueId, string EventDate)
        {
            string param = string.Format(@"/{0}/{1}", LeagueId, EventDate);
            return GetAsync<ServiceResponse<List<GameSchedule>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetGameScheduletUri), param);
        }
         public  Task<ServiceResponse<List<RecommendedFollowersMain>>> GetUserSuggestions(int PageNumber)
        {
            string param = string.Format(@"/{0}", PageNumber);
            return GetAsync<ServiceResponse<List<RecommendedFollowersMain>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetUserSuggestionsUri), param);
        }
        public Task<ServiceResponseBase> FollowUser(FollowRequest requestObj)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kFollowUserUri), requestObj);
        }
        public Task<ServiceResponseBase> UnFollowUser(FollowRequest requestObj)
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUnFollowUserUri), requestObj);
        }
        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowingList(string followId,string UserID)
        {
            string param = string.Format(@"/{0}/{1}",  UserID, followId);
            return GetAsync<ServiceResponse<List<FollowingDetailsMain>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetFollowingUri), param);
        }
        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowersList(string followId,string UserID)
        {
            string param = string.Format(@"/{0}/{1}",  UserID, followId);
            return GetAsync<ServiceResponse<List<FollowingDetailsMain>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetFollowersUri), param);
        }
        public Task<ServiceResponseBase> AddPick(AddPickRequestInfo addPickInfo)
        {
            return PostAsync<ServiceResponseBase>(kAddPickUri, addPickInfo);
        }
        public Task<ServiceResponse<PostResponse>> GetPicks(string PickID, string UserID)
        {
            string param = string.Format(@"/{0}/{1}", PickID, UserID);
            return GetAsync<ServiceResponse<PostResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPickUri), param);
        }
        public Task<ServiceResponseBase> ContactUs(ContactUsRequestInfo contactInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kContactUsUri), contactInfo);
        }
        public Task<ServiceResponseBase> ReportAProblem(ReportAProblemRequestInfo problemInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kReportProblemUri), problemInfo);

        }

        public Task<ServiceResponseBase> VerifyInappPurchase(VerifyPurchase verifyPurchase)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kVerifyPurchaseUri), verifyPurchase);
        }

        public Task<ServiceResponseBase> AddCoin(AddCoinRequestInfo coinRequestInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kAddCoinUri), coinRequestInfo);
        }

        public Task<ServiceResponse<List<CoinHistoryResponseInfo>>> GetCoinHistory(int PageNumber)
        {
            string param = string.Format(@"/{0}", PageNumber);
            return GetAsync<ServiceResponse<List<CoinHistoryResponseInfo>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetCoinHistoryUri), param);
        }

        public Task<ServiceResponse<CoinDetails>> GetCoinDetails()
        {
            string param = string.Empty;
            return GetAsync<ServiceResponse<CoinDetails>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetCoinDetailsUri),param);
        }

        public Task<ServiceResponse<PostResponse>> GetPurchases(string purchaseId, string userId)
        {
            string param = string.Format(@"/{0}/{1}", purchaseId, userId);
            return GetAsync<ServiceResponse<PostResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPurchasesUri), param);

        }

        public Task<ServiceResponseBase> PickPurchase(PickPurchaseRequestInfo pickPurchaseRequest)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kPickPurchaseUri), pickPurchaseRequest);
        }

        public Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetPurchaseHistory(int PageNo)
        {
            string param = string.Format(@"/{0}", PageNo);
            return GetAsync<ServiceResponse<List<PurchaseHistoryResponse>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPUrchaseHistoryUri), param);
        }
        public Task<ServiceResponseBase> AddShare(ShareRequest PostInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kAddShare), PostInfo);
        }
        public Task<ServiceResponseBase> AddShare(PostStatusRequest PostInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<TrendResponse>> GetTrendPickList(int PageNo, int Limit, string ListType, int UserId)
        {
            string param = string.Format(@"?page={0}&limit={1}&listType={2}&userId={3}", PageNo, Limit, ListType, UserId);
            return GetAsync<ServiceResponse<TrendResponse>>(kTrendUri, param);
        }

        public Task<ServiceResponse<GetSettingsResponse>> GetSettingsDetails()

        {
            string param = string.Empty;
            return GetAsync<ServiceResponse<GetSettingsResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetSettings), param);
        }
        public Task<ServiceResponseBase> SaveSettings(SettingsInfo settingsInfo)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kSaveSettings), settingsInfo);
        }
        public Task<ServiceResponseBase> UpdateDeviceToken(UpdateDeviceTokenInfo updateToken)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kDeviceToken), updateToken);
}

        public Task<ServiceResponse<int>> GetAppMinimumVersion()
        {
            string param = string.Empty;
            return GetAsync<ServiceResponse<int>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetAppMinimumVersionUri), param);
        }
        public Task<ServiceResponse<SearchResponse>> GetSearchResult(string SearchKey, int Limit, string ListType)
        {
            string param = string.Format(@"?searchkey={0}&limit={1}&listType={2}", SearchKey, Limit, ListType);
            return GetAsync<ServiceResponse<SearchResponse>>(kSearchUri, param);
        }
        public Task<ServiceResponse<PostDetails>> GetPickDetails(int Type, string PostId)
        {
            string param = string.Format(@"/{0}/{1}", Type, PostId);
            return GetAsync<ServiceResponse<PostDetails>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPickDetailsUri), param);

        }
        public Task<ServiceResponseBase> ClearAllNotifications()
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kClearAll), null);
        }
        public Task<ServiceResponse<NotificationResponseInfo>> GetNotifications(int PageNo)
        {
            string param = string.Format(@"/{0}", PageNo);
            return GetAsync<ServiceResponse<NotificationResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetNotification), param);
        }
        public Task<ServiceResponseBase> ReadNotification(ReadNotificationInfo notificationInfo)
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kReadMessage), notificationInfo);
        }

        public Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetEarningsHistory(int PageNo)
        {
            string param = string.Format(@"/{0}", PageNo);
            return GetAsync<ServiceResponse<List<PurchaseHistoryResponse>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetEarningsHistoryUri), param);
		}
        public  Task<ServiceResponse<PostResponse>> GetShare(string ShareID, string UserID)
        {
            string param = string.Format(@"/{0}/{1}", ShareID, UserID);
            return GetAsync<ServiceResponse<PostResponse>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetShareUri), param);

        }
        public Task<ServiceResponseBase> DeleteShare(DeleteShareRequestInfo PostInfo)
        {
            return DeleteAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kDeleteShareUri), PostInfo);
        }

       
        public  Task<ServiceResponse<int>> GetUnreadCount()
        {
            string param = string.Empty;
            return GetAsync<ServiceResponse<int>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kUnreadNotificationUri), param);
        }
        public  Task<ServiceResponseBase> ReadAllNotification()
        {
            return PutAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kReadAllMessage), null);
        }

        public Task<ServiceResponse<List<PayoutHistoryResponse>>> GetPayoutHistory(int PageNo)
        {
            string param = string.Format(@"?page={0}", PageNo);
            return GetAsync<ServiceResponse<List<PayoutHistoryResponse>>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kGetPayOutHistoryUri), param);
        }
        public Task<ServiceResponseBase> VerifyInappPurchaseAndroid(VerifyPurchaseAndroid verifyPurchase)
        {
            return PostAsync<ServiceResponseBase>(string.Format(@"{0}/{1}", Constants.BaseUrl, kVerifyPurchaseAndroidUri), verifyPurchase);
        }
    }
}
