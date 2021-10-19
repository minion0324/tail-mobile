using System.Threading.Tasks;
using System.Collections.Generic;
using Tail.Services.Responses;
using Tail.Models;

namespace Tail.Services.Interfaces
{
    public interface ITailDataService
    {
        Task<ServiceResponse<LoginResponseInfo>> VerifyLogin(LoginRequestInfo LoginInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifyAppleLogin(AppleLoginRequestInfo LoginInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifyFBLogin(FBLoginRequestInfo LoginInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifySignUp(SignUpRequestInfo SignUpInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifyAppleSignUp(AppleSignUpRequestInfo SignUpInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifyFBSignUp(FBSignUpRequestInfo SignUpInfo);
        Task<ServiceResponse<LoginResponseInfo>> VerifyOTP(OtpRequestInfo OTPInfo);
        Task<ServiceResponseBase> ResendOTP(OtpResendInfo OTPInfo);
        Task<ServiceResponseBase> ForgotPassword(ForgotPasswordRequestInfo ForgotPasswordInfo);
        Task<byte[]> DownloadImageFromUrl(string ImageUrl);
        Task<ServiceResponseBase> Logout(LogoutRequestInfo LogoutInfo);
        Task<ServiceResponse<PostResponse>> GetPostDetails(int PageNumber);
        Task<ServiceResponse<ProfileDetails>> GetProfileDetails();
        Task<ServiceResponse<ProfileDetails>> GetProfileDetails(long userId);
        Task<ServiceResponse<CommentsMain>> GetComments(string PostID, string PreviousCommentSyncTime);
        Task<ServiceResponse<IList<Comments>>> AddComment(CommentRequestInfo commentObj);
        Task<ServiceResponse<List<StepsDetails>>> GetGameDetails();
        Task<ServiceResponse<List<RecommendedFollowers>>> GetRecommendedFollowers();
        Task<ServiceResponse<LeagueDetailsResponse>> GetLeagueDetails(int SportID);
        Task<ServiceResponse<TrendingModel>> GetTrendingDetails();
        Task<ServiceResponse<TrendingModel>> GetSearchDetails();
        Task<ServiceResponseBase> UpdatePassword(UpdatePasswordInfo updatePasswordInfo);
        Task<ServiceResponseBase> RemoveAccount();
        Task<ServiceResponse<List<InterestListResponseItems>>> GetInterestList();
        Task<ServiceResponse<List<int>>> GetUserInterestList();
        Task<ServiceResponseBase> UpdateInterestList(UpdateInterestListRequestInfo updateInterestListInfo);
        Task<ServiceResponseBase> AddPost(AddPostRequestInfo addPostInfo);
        Task<ServiceResponseBase> LikeAPost(PostStatusRequest PostInfo);
        Task<ServiceResponseBase> DisLikeAPost(PostStatusRequest PostInfo);
        Task<ServiceResponseBase> HideAPost(PostStatusRequest PostInfo);
        Task<ServiceResponseBase> ReportAPost(ReportPostRequestInfo ReportInfo);
        Task<ServiceResponseBase> UpdateProfile(UpdateProileRequestInfo updateProfileInfo);
        Task<ServiceResponse<LoginResponseInfo>> GetUserDetails(int userId);
        Task<ServiceResponseBase> DeleteComment(DeleteCommentRequestInfo CommentInfo);
        Task<ServiceResponseBase> DeletePost(DeletePostRequestInfo PostInfo);
        Task<ServiceResponse<PostResponse>> GetPosts(string PostID, string UserID);
        Task<ServiceResponse<List<GameSchedule>>> GetGameSchedules(int LeagueId, string EventDate);
        Task<ServiceResponse<List<RecommendedFollowersMain>>> GetUserSuggestions(int PageNumber);
        Task<ServiceResponseBase> FollowUser(FollowRequest requestObj);
        Task<ServiceResponseBase> UnFollowUser(FollowRequest requestObj);
        Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowingList(string followId,string UserID);
        Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowersList(string followId,string UserID);
        Task<ServiceResponseBase> AddPick(AddPickRequestInfo addPickInfo);
        Task<ServiceResponse<PostResponse>> GetPicks(string PickID, string UserID);
        Task<ServiceResponseBase> ContactUs(ContactUsRequestInfo contactInfo);
        Task<ServiceResponseBase> ReportAProblem(ReportAProblemRequestInfo problemInfo);
        Task<ServiceResponseBase> VerifyInappPurchase(VerifyPurchase verifyPurchase);
        Task<ServiceResponseBase> AddCoin(AddCoinRequestInfo coinRequestInfo);
        Task<ServiceResponse<List<CoinHistoryResponseInfo>>> GetCoinHistory(int PageNumber);
        Task<ServiceResponse<CoinDetails>> GetCoinDetails();
        Task<ServiceResponse<int>> GetAppMinimumVersion();
        Task<ServiceResponse<PostResponse>> GetPurchases(string purchaseId, string userId);

        Task<ServiceResponseBase> PickPurchase(PickPurchaseRequestInfo pickPurchaseRequest);
        Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetPurchaseHistory(int PageNo);
        Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetEarningsHistory(int PageNo);
        Task<ServiceResponse<List<PayoutHistoryResponse>>> GetPayoutHistory(int PageNo);
        Task<ServiceResponseBase> AddShare(ShareRequest PostInfo);
        Task<ServiceResponse<TrendResponse>> GetTrendPickList(int PageNo, int Limit, string ListType, int UserId);

        Task<ServiceResponse<GetSettingsResponse>> GetSettingsDetails();

        Task<ServiceResponseBase> SaveSettings(SettingsInfo settingsInfo);
        Task<ServiceResponseBase> UpdateDeviceToken(UpdateDeviceTokenInfo updateToken);
  
        Task<ServiceResponse<SearchResponse>> GetSearchResult(string SearchKey, int Limit, string ListType);

        Task<ServiceResponse<PostDetails>> GetPickDetails(int Type, string PostId);
        Task<ServiceResponseBase> ClearAllNotifications();
        Task<ServiceResponse<NotificationResponseInfo>> GetNotifications(int PageNo);
        Task<ServiceResponseBase> ReadNotification(ReadNotificationInfo notificationInfo);
        Task<ServiceResponse<PostResponse>> GetShare(string ShareID, string UserID);
        Task<ServiceResponseBase> DeleteShare(DeleteShareRequestInfo PostInfo);
        Task<ServiceResponse<int>> GetUnreadCount();
        Task<ServiceResponseBase> ReadAllNotification();

        Task<ServiceResponseBase> VerifyInappPurchaseAndroid(VerifyPurchaseAndroid verifyPurchase);
    }
}
