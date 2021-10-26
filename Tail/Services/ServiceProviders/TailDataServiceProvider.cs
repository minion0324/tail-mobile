using System.Collections.Generic;
using System.Threading.Tasks;
using Tail.Models;
using Tail.Services.Interfaces;
using Tail.Services.MockServices;
using Tail.Services.OnlineServices;
using Tail.Services.Responses;

namespace Tail.Services.ServiceProviders
{
    public class TailDataServiceProvider : ITailDataService
    {
        readonly ITailDataService _tailMockService;
        readonly ITailDataService _tailDataWebservice;    

        static TailDataServiceProvider _instance;

        public static TailDataServiceProvider Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new TailDataServiceProvider();
                }

                return _instance;
            }
        }

        TailDataServiceProvider()
        {
            _tailMockService = new TailDataMockService();
            _tailDataWebservice = new TailDataWebservice();    
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyLogin(LoginRequestInfo LoginInfo)
        {
            return await _tailDataWebservice.VerifyLogin(LoginInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyAppleLogin(AppleLoginRequestInfo LoginInfo)
        {
            return await _tailDataWebservice.VerifyAppleLogin(LoginInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyFBLogin(FBLoginRequestInfo LoginInfo)
        {
            return await _tailDataWebservice.VerifyFBLogin(LoginInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifySignUp(SignUpRequestInfo SignUpInfo)
        {
            return await _tailDataWebservice.VerifySignUp(SignUpInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyAppleSignUp(AppleSignUpRequestInfo SignUpInfo)
        {
            return await _tailDataWebservice.VerifyAppleSignUp(SignUpInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyFBSignUp(FBSignUpRequestInfo SignUpInfo)
        {
            return await _tailDataWebservice.VerifyFBSignUp(SignUpInfo);
        }
        public async Task<ServiceResponse<LoginResponseInfo>> VerifyOTP(OtpRequestInfo OTPInfo)
        {
            return await _tailDataWebservice.VerifyOTP(OTPInfo);
        }
        public async Task<ServiceResponseBase> ResendOTP(OtpResendInfo OTPInfo)
        {
            return await _tailDataWebservice.ResendOTP(OTPInfo);
        }
        public async Task<ServiceResponseBase> ForgotPassword(ForgotPasswordRequestInfo ForgotPasswordInfo)
        {
            return await _tailDataWebservice.ForgotPassword(ForgotPasswordInfo);
        }
        public async Task<byte[]> DownloadImageFromUrl(string ImageUrl)
        {
            return await _tailDataWebservice.DownloadImageFromUrl(ImageUrl);
        }

        public async Task<ServiceResponseBase> Logout(LogoutRequestInfo LogoutInfo)
        {
            return await _tailDataWebservice.Logout(LogoutInfo);
        }

        public async Task<ServiceResponse<PostResponse>> GetPostDetails(int PageNumber)
        {
            return await _tailDataWebservice.GetPostDetails(PageNumber);
        }
        public async Task<ServiceResponse<ProfileDetails>> GetProfileDetails()
        {
            return await _tailMockService.GetProfileDetails();
        }
        public async Task<ServiceResponse<ProfileDetails>> GetProfileDetails(long userId)
        {
            return await _tailMockService.GetProfileDetails(userId);
        }
        public async Task<ServiceResponse<CommentsMain>> GetComments(string PostID, string PreviousCommentSyncTime)
        {
            return await _tailDataWebservice.GetComments(PostID, PreviousCommentSyncTime);
        }
        public async Task<ServiceResponse<IList<Comments>>> AddComment(CommentRequestInfo commentObj)
        {
            return await _tailDataWebservice.AddComment(commentObj);
        }
        public async Task<ServiceResponse<List<StepsDetails>>> GetGameDetails()
        {
            return await _tailMockService.GetGameDetails();
        }

        public async Task<ServiceResponse<List<RecommendedFollowers>>> GetRecommendedFollowers()
        {
            return await _tailMockService.GetRecommendedFollowers();
        }
        public async Task<ServiceResponse<LeagueDetailsResponse>> GetLeagueDetails(int SportID)
        {
            return await _tailDataWebservice.GetLeagueDetails(SportID);
        }
        public async Task<ServiceResponse<NotificationResponseInfo>> GetNotifications(int PageNo)
        {
            return await _tailDataWebservice.GetNotifications(PageNo);
        }
        public async Task<ServiceResponse<TrendingModel>> GetTrendingDetails()
        {
            return await _tailMockService.GetTrendingDetails();
        }
        public async Task<ServiceResponse<TrendingModel>> GetSearchDetails()
        {
            return await _tailMockService.GetSearchDetails();
        }

        public async Task<ServiceResponseBase> UpdatePassword(UpdatePasswordInfo updatePasswordInfo)
        {
            return await _tailDataWebservice.UpdatePassword(updatePasswordInfo);
        }

        public  async Task<ServiceResponseBase> RemoveAccount()
        {
            return await _tailDataWebservice.RemoveAccount();
        }
        public async Task<ServiceResponse<List<InterestListResponseItems>>> GetInterestList()
        {
            return await _tailDataWebservice.GetInterestList();
        }
        public async Task<ServiceResponse<List<int>>> GetUserInterestList()
        {
            return await _tailDataWebservice.GetUserInterestList();
        }
        public async Task<ServiceResponseBase> UpdateInterestList(UpdateInterestListRequestInfo updateInterestListInfo)
        {
            return await _tailDataWebservice.UpdateInterestList(updateInterestListInfo);
        }
        public async Task<ServiceResponseBase> AddPost(AddPostRequestInfo addPostInfo)
        {
            return await _tailDataWebservice.AddPost(addPostInfo);
        }

        public async Task<ServiceResponseBase> LikeAPost(PostStatusRequest PostInfo)
        {
            return await _tailDataWebservice.LikeAPost(PostInfo);
        }
        public async Task<ServiceResponseBase> DisLikeAPost(PostStatusRequest PostInfo)
        {
            return await _tailDataWebservice.DisLikeAPost(PostInfo);
        }
        public async Task<ServiceResponseBase> HideAPost(PostStatusRequest PostInfo)
        {
            return await _tailDataWebservice.HideAPost(PostInfo);
        }
        public async Task<ServiceResponseBase> ReportAPost(ReportPostRequestInfo ReportInfo)
        {
            return await _tailDataWebservice.ReportAPost(ReportInfo);
        }

        public async Task<ServiceResponseBase> UpdateProfile(UpdateProileRequestInfo updateProfileInfo)
        {
            return await _tailDataWebservice.UpdateProfile(updateProfileInfo);
        }

        public async Task<ServiceResponse<LoginResponseInfo>> GetUserDetails(int userId)
        {
            return await _tailDataWebservice.GetUserDetails(userId);
        }
        public async Task<ServiceResponseBase> DeleteComment(DeleteCommentRequestInfo CommentInfo)
        {
            return await _tailDataWebservice.DeleteComment(CommentInfo);
        }
        public async Task<ServiceResponseBase> DeletePost(DeletePostRequestInfo PostInfo)
        {
            return await _tailDataWebservice.DeletePost(PostInfo);
        }
        public async Task<ServiceResponse<PostResponse>> GetPosts(string PostID, string UserID)
        {
            return await _tailDataWebservice.GetPosts(PostID,UserID);
        }
        public async Task<ServiceResponse<List<GameSchedule>>> GetGameSchedules(int LeagueId, string EventDate)
        {
            return await _tailDataWebservice.GetGameSchedules(LeagueId, EventDate);
        }
        public async Task<ServiceResponse<List<RecommendedFollowersMain>>> GetUserSuggestions(int PageNumber)
        {
            return await _tailDataWebservice.GetUserSuggestions(PageNumber);
        }
        public async Task<ServiceResponseBase> FollowUser(FollowRequest requestObj)
        {
            return await _tailDataWebservice.FollowUser(requestObj);
        }
        public async Task<ServiceResponseBase> UnFollowUser(FollowRequest requestObj)
        {
            return await _tailDataWebservice.UnFollowUser(requestObj);
        }
        public async Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowingList(string followId,string UserID)
        {
            return await _tailDataWebservice.GetFollowingList(followId,UserID);
        }
        public async Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowersList(string followId,string UserID)
        {
            return await _tailDataWebservice.GetFollowersList(followId,UserID);
        }
        public async Task<ServiceResponseBase> AddPick(AddPickRequestInfo addPickInfo)
        {
            return await _tailDataWebservice.AddPick(addPickInfo);
        }
        public async Task<ServiceResponse<PostResponse>> GetPicks(string PickID, string UserID)
        {
            return await _tailDataWebservice.GetPicks(PickID, UserID);

        }
        public async Task<ServiceResponseBase> ContactUs(ContactUsRequestInfo contactInfo)
        {
            return await _tailDataWebservice.ContactUs(contactInfo);

        }
        public async Task<ServiceResponseBase> ReportAProblem(ReportAProblemRequestInfo problemInfo)
        {
            return await _tailDataWebservice.ReportAProblem(problemInfo);
        }

        public async Task<ServiceResponseBase> VerifyInappPurchase(VerifyPurchase verifyPurchase)
        {
            return await _tailDataWebservice.VerifyInappPurchase(verifyPurchase);
        }
        public async Task<ServiceResponseBase> VerifyInappPurchaseAndroid(VerifyPurchaseAndroid verifyPurchase)
        {
            return await _tailDataWebservice.VerifyInappPurchaseAndroid(verifyPurchase);
        }
        public async Task<ServiceResponseBase> AddCoin(AddCoinRequestInfo coinRequestInfo)
        {
            return await _tailDataWebservice.AddCoin(coinRequestInfo);
        }

        public async Task<ServiceResponse<List<CoinHistoryResponseInfo>>> GetCoinHistory(int PageNumber)
        {
            return await _tailDataWebservice.GetCoinHistory(PageNumber);
        }

        public async Task<ServiceResponse<CoinDetails>> GetCoinDetails()
        {
            return await _tailDataWebservice.GetCoinDetails();
        }


        public async Task<ServiceResponse<PostResponse>> GetPurchases(string purchaseId, string userId)
        {
            return await _tailDataWebservice.GetPurchases(purchaseId,userId);
        }

        public async Task<ServiceResponseBase> PickPurchase(PickPurchaseRequestInfo pickPurchaseRequest)
        {
            return await _tailDataWebservice.PickPurchase(pickPurchaseRequest);
        }

        public async Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetPurchaseHistory(int PageNo)
        {
            return await _tailDataWebservice.GetPurchaseHistory(PageNo);

        }
        public async Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetEarningsHistory(int PageNo)
        {
            return await _tailDataWebservice.GetEarningsHistory(PageNo);

        }
        public async Task<ServiceResponse<List<PayoutHistoryResponse>>> GetPayoutHistory(int PageNo)
        {
            return await _tailDataWebservice.GetPayoutHistory(PageNo);

        }
        public async Task<ServiceResponseBase> AddShare(ShareRequest PostInfo)
        {
            return await _tailDataWebservice.AddShare(PostInfo);
        }
        public Task<ServiceResponseBase> AddShare(PostStatusRequest PostInfo)
        {
            throw new System.NotImplementedException();
        }
        public async Task<ServiceResponse<TrendResponse>> GetTrendPickList(int PageNo, int Limit, string ListType, int UserId)
        {
            return await _tailDataWebservice.GetTrendPickList(PageNo, Limit, ListType, UserId);
        }

        public async Task<ServiceResponse<GetSettingsResponse>> GetSettingsDetails()
        { 
            return await _tailDataWebservice.GetSettingsDetails();
        }
        public async Task<ServiceResponseBase> SaveSettings(SettingsInfo settingsInfo)
        
            {
            return await _tailDataWebservice.SaveSettings(settingsInfo);
        }

        public async Task<ServiceResponseBase> UpdateDeviceToken(UpdateDeviceTokenInfo updateToken)
        {
            return await _tailDataWebservice.UpdateDeviceToken(updateToken);
}
        public async Task<ServiceResponse<int>> GetAppMinimumVersion()
        {
            return await _tailDataWebservice.GetAppMinimumVersion();
        }
        public async Task<ServiceResponse<SearchResponse>> GetSearchResult(string SearchKey, int Limit, string ListType)
        {
            return await _tailDataWebservice.GetSearchResult(SearchKey, Limit, ListType);
        }

        public async Task<ServiceResponse<PostDetails>> GetPickDetails(int Type, string PostId)
        {
            return await _tailDataWebservice.GetPickDetails(Type, PostId);

        }
        public async Task<ServiceResponseBase> ClearAllNotifications( )
        {
            return await _tailDataWebservice.ClearAllNotifications();
        }
        public async Task<ServiceResponseBase> ReadNotification(ReadNotificationInfo notificationInfo)
        {
            return await _tailDataWebservice.ReadNotification(notificationInfo);
        }
        public async Task<ServiceResponse<PostResponse>> GetShare(string ShareID, string UserID)
        {
            return await _tailDataWebservice.GetShare(ShareID, UserID);

        }
        public async Task<ServiceResponseBase> DeleteShare(DeleteShareRequestInfo PostInfo)
        {
            return await _tailDataWebservice.DeleteShare(PostInfo);
        }

       
        public async Task<ServiceResponse<int>> GetUnreadCount()
        {
            return await _tailDataWebservice.GetUnreadCount();
        }
        public async Task<ServiceResponseBase> ReadAllNotification()
        {
            return await _tailDataWebservice.ReadAllNotification();
        }
    }
}
