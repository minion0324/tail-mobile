using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Tail.Services.Responses;

namespace Tail.Services.MockServices
{
    public class TailDataMockService : ITailDataService
    {
        public Task<ServiceResponse<LoginResponseInfo>> VerifyLogin(LoginRequestInfo LoginInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyAppleLogin(AppleLoginRequestInfo LoginInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyFBLogin(FBLoginRequestInfo LoginInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifySignUp(SignUpRequestInfo SignUpInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyAppleSignUp(AppleSignUpRequestInfo SignUpInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyFBSignUp(FBSignUpRequestInfo SignUpInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponse<LoginResponseInfo>> VerifyOTP(OtpRequestInfo OTPInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> ResendOTP(OtpResendInfo OTPInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> ForgotPassword(ForgotPasswordRequestInfo ForgotPasswordInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> Logout(LogoutRequestInfo LogoutInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<byte[]> DownloadImageFromUrl(string ImageUrl)
        {
            throw new System.NotImplementedException();
        }
    
        public Task<ServiceResponse<PostResponse>> GetPostDetails(int PageNumber)
        {
            throw new System.NotImplementedException();
            
        }
        public Task<ServiceResponse<ProfileDetails>> GetProfileDetails()
        {
            ServiceResponse<ProfileDetails> serviceResponse = new ServiceResponse<ProfileDetails>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new ProfileDetails
                {
                    UserId = Constants.LOGGEDIN_USERID,
                    UserImage = "Rectangle_7.png",
                    IsFollowing = false,
                    AboutMe = "Lorem ipsum dolor sit amet, consectetur adipiscing elit,  sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                    FollowersCount = 220,
                    FollowingCount = 300,
                    UserName = Constants.LOGGEDIN_USERNAME,
                    PicksCount = 20,
                    Last15Predictions = GetPredictions(),
                    LifetimePredictions = GetPredictions(),
                    UserFollowing = new List<FollowersDetails>(),
                    PostCount = 45,
                    PurchaseCount = 23,
                    UserPicks = new List<PostDetails>(),
                    UserPurchases = new List<PostDetails>(),
                    UserPosts = new List<PostDetails>(),
                    UserFollowers = new List<FollowersDetails>()
                   

                }

            };

            return Task.FromResult(serviceResponse);
        }
        public Task<ServiceResponse<ProfileDetails>> GetProfileDetails(long userId)
        {
            ServiceResponse<ProfileDetails> serviceResponse = new ServiceResponse<ProfileDetails>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new ProfileDetails
                {
                    UserId = 4,
                    UserImage = "mike.png",
                    IsFollowing = false,
                    AboutMe = "Lorem ipsum dolor sit amet, consectetur adipiscing elit,  sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam",
                    FollowersCount = 220,
                    FollowingCount = 300,
                    UserName = "Mike John",
                    PicksCount = 20,
                    Last15Predictions = GetPredictions(),
                    LifetimePredictions = GetPredictions(),               
                    UserFollowing = new List<FollowersDetails>(),
                    PostCount = 45,
                    UserPicks = new List<PostDetails>(),
                    UserPosts = new List<PostDetails>(),
                    UserFollowers = new List<FollowersDetails>(),
                }

            };

            return Task.FromResult(serviceResponse);
        }
        public Task<ServiceResponse<CommentsMain>> GetComments(string PostID, string PreviousCommentSyncTime)
        {
            ServiceResponse<CommentsMain> serviceResponse = new ServiceResponse<CommentsMain>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new CommentsMain
                {
                    CommentsData = new List<Comments>
                    {
                        new Comments
                        {
                            UserID=5,
                            PostId="1",
                            UserName="Jake Ray",
                            UserImage="ben",
                            CommentText="Consectetur adipiscing elit.",
                            PostedDateTime=DateTime.Now.AddHours(-5).ToString()

                        },
                        new Comments
                        {
                            UserID=6,
                            PostId="2",
                            UserName="Tom Hardy",
                            UserImage="tomhardy.png",
                            CommentText="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor",
                            PostedDateTime=DateTime.Now.AddHours(-3).ToString()
                        },
                        new Comments
                        {
                            UserID=7,
                            PostId="3",
                            UserName="Alisa",
                            UserImage="alisa",
                            CommentText="Adipiscing elit, sed do eiusmod tempor",
                            PostedDateTime=DateTime.Now.AddHours(-2).ToString()
                        }

                    },
                    PageInfo = new List<PaginationDetails>
                    {
                        new PaginationDetails
                        {
                            currentPage=1,
                            totalPages=1,
                            totalRecords=3,
                        }
                    }


                }
            };

            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<List<StepsDetails>>> GetGameDetails()
        {
           
            List<PickerItem> _spotOptions = new List<PickerItem>();
            var _sportTypeArray = Enum.GetValues(typeof(SportType)).Cast<SportType>();
            for (int i = 1; i < _sportTypeArray.Count(); i++)
            {
                _spotOptions.Add(new PickerItem
                {
                    ItemName = TailUtils.GameTypeToName((SportType)i)
                });
            }

            ServiceResponse<List<StepsDetails>> serviceResponse = new ServiceResponse<List<StepsDetails>>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new List<StepsDetails>
                {
                    new StepsDetails
                    {
                       // UpcomingGames=_upcomingList,
                        SpotOptions=_spotOptions,
                        BettingDetails=new BettingTypeDetails(),
                        LeagueOptions=new List<PickerItem>(),
                        IsStep1=true,
                        SelectedTabIndex=1
                    },
                     new StepsDetails
                    {
                       // UpcomingGames=new UpcomingGameDetails(),
                        SpotOptions=new List<PickerItem>(),
                        BettingDetails= new BettingTypeDetails(),
                        LeagueOptions=new List<PickerItem>(),
                        IsStep1=false,
                        SelectedTabIndex=1
                    }
                }



            };

            return Task.FromResult(serviceResponse);
        }


        public Task<ServiceResponse<List<RecommendedFollowers>>> GetRecommendedFollowers()
        {
            ServiceResponse<List<RecommendedFollowers>> serviceResponse = new ServiceResponse<List<RecommendedFollowers>>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new List<RecommendedFollowers>
                {
                    new RecommendedFollowers
                    {
                        UserId=5,

                        UserName="Jake Ray",
                        //UserImage="ben",
                        //SportIcon="mma_follow.png",
                       // PickByPrediction=98,
                        IsFollow = true

                    },
                    new RecommendedFollowers
                    {
                        UserId=6,
                        UserName="Tom Hardy",
                        //UserImage="tomhardy.png",
                        //SportIcon="ice_hockey_follow.png",
                        //PickByPrediction="93%",
                        IsFollow = false
                    },
                     new RecommendedFollowers
                    {
                        UserId=7,
                        UserName="Alisa",
                       // UserImage="alisa",
                       //SportIcon="basketball_follow.png",
                       // PickByPrediction="90%"
                    }

                }
            };

            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<LeagueDetailsResponse>> GetLeagueDetails(int SportID)
        {
            ServiceResponse<LeagueDetailsResponse> serviceResponse = new ServiceResponse<LeagueDetailsResponse>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new LeagueDetailsResponse(),
                
            };

            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<List<NotificationModel>>> GetNotifications()
        {
            ServiceResponse<List<NotificationModel>> serviceResponse = new ServiceResponse<List<NotificationModel>>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new List<NotificationModel>
                {
                    new NotificationModel()
                    {
                       UserId=4,
                       UserImage="mike.png",
                       IsNotificationRead=false,
                       NotificationDateTime=DateTime.Now.AddHours(-14).ToString(),
                       NotificationContent="Mike John has liked your post",
                       PostItem= new PostDetails(),

                    },
                   new NotificationModel()
                    {
                       UserId=4,
                       UserImage="mike.png",
                       IsNotificationRead=false,
                       NotificationDateTime=DateTime.Now.AddHours(-15).ToString(),
                       NotificationContent="Mike John has posted a pick in NBA",
                       PostItem= new PostDetails(),

                    },
                     new NotificationModel()
                    {
                       UserId=2,
                       UserImage="alisa.png",
                       IsNotificationRead=true,
                       NotificationDateTime=DateTime.Now.AddHours(-16).ToString(),
                       NotificationContent="Alisa Devine has liked your post",
                       PostItem= new PostDetails(),

                    },
                     new NotificationModel()
                    {
                       UserId=3,
                       UserImage="mark.png",
                       IsNotificationRead=true,
                       NotificationDateTime=DateTime.Now.AddHours(-18).ToString(),
                       NotificationContent="Mark Ruffalo has posted a pick in NBA",
                       PostItem= new PostDetails(),

                    },


                }
            };

            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<TrendingModel>> GetTrendingDetails()
        {

            List<TrendingPeopleModel> _peopleList = new List<TrendingPeopleModel>()
            {
                new TrendingPeopleModel()
                {
                    UserId=4,
                    UserImage="mike.png",
                    UserName="Mike John",
                    UserAccuracy="98%",
                    UserSportType=SportType.Football,
                    TrendType=TrendingType.People,

                },
                new TrendingPeopleModel()
                {

                    UserId=2,
                    UserImage="alisa.png",
                    UserName="Alisa Devine",
                    UserAccuracy="97%",
                    UserSportType=SportType.Hocky,
                    TrendType=TrendingType.People,

                },
                new TrendingPeopleModel()
                {

                    UserId=3,
                    UserImage="mark.png",
                    UserName="Mark Ruffalo",
                    UserAccuracy="89%",
                    UserSportType=SportType.Baseball,
                    TrendType=TrendingType.People,

                },
             };

            List<PostDetails> _picksList = new List<PostDetails>();
            List<PostDetails> _postList = new List<PostDetails>();
            ServiceResponse<TrendingModel> serviceResponse = new ServiceResponse<TrendingModel>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new TrendingModel
                {
                    TrendingPeopleList = _peopleList,
                    TrendingPicks = _picksList,
                    TrendingPosts = _postList,
                }
            };

            return Task.FromResult(serviceResponse);
        }

        public Task<ServiceResponse<TrendingModel>> GetSearchDetails()
        {

            List<TrendingPeopleModel> _peopleList = new List<TrendingPeopleModel>()
            {
                new TrendingPeopleModel()
                {
                    UserId=5,
                    UserImage="ben.png",
                    UserName="Jake Ray",
                    UserAccuracy="77%",
                    UserSportType=SportType.Basketball,
                    BaseballAccuracy="80%",
                    BasketballAccuracy="77%",
                    FootballAccuracy="45%",
                    HockyAccuracy="10%",
                    MmaAccuracy="0",
                    TrendType=TrendingType.PeopleResult,

                },
                new TrendingPeopleModel()
                {

                    UserId=6,
                    UserImage="tomhardy.png",
                    UserName="Tom Hardy",
                    UserAccuracy="83%",
                    UserSportType=SportType.Football,
                     BaseballAccuracy="60%",
                    BasketballAccuracy="87%",
                    FootballAccuracy="52%",
                    HockyAccuracy="30%",
                    MmaAccuracy="10%",
                    TrendType=TrendingType.PeopleResult,

                },

             };

            List<PostDetails> _picksList = new List<PostDetails>();
            List<PostDetails> _postList = new List<PostDetails>();
            ServiceResponse<TrendingModel> serviceResponse = new ServiceResponse<TrendingModel>
            {
                ErrorCode = 0,
                Message = string.Empty,
                ResponseData = new TrendingModel
                {
                    TrendingPeopleList = _peopleList,
                    TrendingPicks = _picksList,
                    TrendingPosts = _postList,
                }
            };

            return Task.FromResult(serviceResponse);
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
                predictionList.Add(sportPrediction);
            }
            return predictionList;
        }

        public Task<ServiceResponseBase> UpdatePassword(UpdatePasswordInfo updatePasswordInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> RemoveAccount()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<InterestListResponseItems>>> GetInterestList()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<int>>> GetUserInterestList()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> UpdateInterestList(UpdateInterestListRequestInfo updateInterestListInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> LikeAPost(PostStatusRequest PostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> DisLikeAPost(PostStatusRequest PostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> HideAPost(PostStatusRequest PostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> ReportAPost(ReportPostRequestInfo ReportInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> UpdateProfile(UpdateProileRequestInfo updateProfileInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<LoginResponseInfo>> GetUserDetails(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<IList<Comments>>> AddComment(CommentRequestInfo commentObj)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> DeleteComment(DeleteCommentRequestInfo CommentInfo)
        {
            throw new System.NotImplementedException();
        }
        public Task<ServiceResponseBase> DeletePost(DeletePostRequestInfo PostInfo)
        {
            throw new System.NotImplementedException();
        }



        public Task<ServiceResponseBase> AddPost(AddPostRequestInfo addPostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<PostResponse>> GetPosts(string PostID, string UserID)
        {
            throw new NotImplementedException();
	    }

        Task<ServiceResponse<LoginResponseInfo>> ITailDataService.GetUserDetails(int userId)
        {
            throw new NotImplementedException();

        }
        public Task<ServiceResponse<List<GameSchedule>>> GetGameSchedules(int LeagueId, string EventDate)
        {
            throw new NotImplementedException();
        }
        public  Task<ServiceResponse<List<RecommendedFollowersMain>>> GetUserSuggestions(int PageNumber)
        {
            throw new NotImplementedException();
        }
        public  Task<ServiceResponseBase> FollowUser(FollowRequest requestObj)
        {
            throw new NotImplementedException();
        }
        public  Task<ServiceResponseBase> UnFollowUser(FollowRequest requestObj)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowingList(string followId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowersList(string followId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> AddPick(AddPickRequestInfo addPickInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<PostResponse>> GetPicks(string PickID, string UserID)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowingList(string followId, string UserID)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<FollowingDetailsMain>>> GetFollowersList(string followId, string UserID)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> ContactUs(ContactUsRequestInfo contactInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> ReportAProblem(ReportAProblemRequestInfo problemInfo)
        {
            throw new NotImplementedException();

        }

        public Task<ServiceResponseBase> VerifyInappPurchase(VerifyPurchase verifyPurchase)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> AddCoin(AddCoinRequestInfo coinRequestInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<CoinHistoryResponseInfo>>> GetCoinHistory(int PageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<CoinDetails>> GetCoinDetails()
        {
            throw new NotImplementedException();
        }


        public Task<ServiceResponse<List<PurchaseDetails>>> GetPurchases(int PageNumber)
        { 
              throw new NotImplementedException();
    }
    public Task<ServiceResponseBase> PickPurchase(PickPurchaseRequestInfo pickPurchaseRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetPurchaseHistory(int PageNo)

        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> AddShare(ShareRequest PostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> AddShare(PostStatusRequest PostInfo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<TrendResponse>> GetTrendPickList(int PageNo, int Limit, string ListType, int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<PostResponse>> GetPurchases(string purchaseId, string userId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<GetSettingsResponse>> GetSettingsDetails()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> SaveSettings(SettingsInfo settingsInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponseBase> UpdateDeviceToken(UpdateDeviceTokenInfo updateToken)
{
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<int>> GetAppMinimumVersion()
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<SearchResponse>> GetSearchResult(string SearchKey, int Limit, string ListType)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponse<PostDetails>> GetPickDetails(int Type, string PostId)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> ClearAllNotifications()
        {
            throw new NotImplementedException();
        }

     
        public Task<ServiceResponseBase> ReadNotification(ReadNotificationInfo notificationInfo)
        {
            throw new NotImplementedException();
        }

        Task<ServiceResponse<NotificationResponseInfo>> ITailDataService.GetNotifications(int PageNo)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<PurchaseHistoryResponse>>> GetEarningsHistory(int PageNo)
		{
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<PostResponse>> GetShare(string ShareID, string UserID)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> DeleteShare(DeleteShareRequestInfo PostInfo)
        {
            throw new NotImplementedException();
        }

      
        public Task<ServiceResponse<int>> GetUnreadCount()
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> ReadAllNotification()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<PayoutHistoryResponse>>> GetPayoutHistory(int PageNo)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResponseBase> VerifyInappPurchaseAndroid(VerifyPurchaseAndroid verifyPurchase)
        {
            throw new NotImplementedException();
        }
    }
}
