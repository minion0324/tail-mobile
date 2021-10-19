using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.Common;
using Tail.ViewModels;
using Xamarin.Forms;
using Tail.Services.Helper;
using Tail.Services.ApplicationServices;

namespace Tail.Models
{
    public class PostDetailsMainModel : ViewModelBase
    {

        PostDetails _postItem;
        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        bool _moreVisible=true;
        public bool MoreVisible
        {
            get => _moreVisible;
            set => SetProperty(ref _moreVisible, value);
        }
        Command<PostDetails> _moreOptionCommand;
        public Command<PostDetails> MoreOptionCommand
        {
            get => _moreOptionCommand;
            set => SetProperty(ref _moreOptionCommand, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }

        Command<PostDetails> _likeCommand;
        public Command<PostDetails> LikeCommand
        {
            get => _likeCommand;
            set => SetProperty(ref _likeCommand, value);
        }
        Command<PostDetails> _disLikeCommand;
        public Command<PostDetails> DisLikeCommand
        {
            get => _disLikeCommand;
            set => SetProperty(ref _disLikeCommand, value);
        }
        Command<PostDetails> _commentCommand;
        public Command<PostDetails> CommentCommand
        {
            get => _commentCommand;
            set => SetProperty(ref _commentCommand, value);
        }

        Command<PostDetails> _pickPurchase;
        public Command<PostDetails> PickPurchase
        {
            get => _pickPurchase;
            set => SetProperty(ref _pickPurchase, value);
        }
     
    }
    public class PostResponse : ViewModelBase
    {
        List<PostDetails> _postItem;
        [JsonProperty("resultData")]
        public List<PostDetails> PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        List<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public List<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }


    }
    public class TrendResponse : ViewModelBase
    {
        TrendDetail _resultData;
        [JsonProperty("resultData")]
        public TrendDetail ResultData
        {
            get => _resultData;
            set => SetProperty(ref _resultData, value);
        }
        List<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public List<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }


    }

    public class SearchResponse : ViewModelBase
    {
        SearchDetail _resultData;
        [JsonProperty("resultData")]
        public SearchDetail ResultData
        {
            get => _resultData;
            set => SetProperty(ref _resultData, value);
        }
        List<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public List<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }


    }

    public class SearchDetail : ViewModelBase
    {
        List<PostDetails> _pickItem;
        [JsonProperty("Picks")]
        public List<PostDetails> PickItem
        {
            get => _pickItem;
            set => SetProperty(ref _pickItem, value);
        }
        List<PostDetails> _postItem;
        [JsonProperty("Posts")]
        public List<PostDetails> PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        List<TrendingUsers> _users;
        [JsonProperty("Users")]
        public List<TrendingUsers> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
    }
    public class TrendDetail : ViewModelBase
    {
        List<TrendPost> _pickItem;
        [JsonProperty("Picks")]
        public List<TrendPost> PickItem
        {
            get => _pickItem;
            set => SetProperty(ref _pickItem, value);
        }
        List<TrendPost> _postItem;
        [JsonProperty("Posts")]
        public List<TrendPost> PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        List<TrendingUsers> _users;
        [JsonProperty("Users")]
        public List<TrendingUsers> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
    }
    public class TrendingUserBase : ViewModelBase
    {
        TrendingUsers _userItem;
        public TrendingUsers UserItem
        {
            get => _userItem;
            set => SetProperty(ref _userItem, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
    }
        public class TrendingUsers : ViewModelBase
    {
        string _id;
        [JsonProperty("_id")]
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        int _userId;
        [JsonProperty("userId")]
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        string _userName;
        [JsonProperty("uName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        string _userImageName;
        [JsonProperty("userImg")]
        public string UserImageName
        {
            get => _userImageName;
            set => SetProperty(ref _userImageName, value);
        }
        [JsonIgnore]
        public string UserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(UserImageName) && UserImageName != "string")
                {

                    return TailUtils.GetThumbProfileImage(UserImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }


        TrendUserSports _predSports;
        [JsonProperty("predSports")]
        public TrendUserSports PredSports
        {
            get => _predSports;
            set => SetProperty(ref _predSports, value);
        }
    }
    public class TrendUserSports : ViewModelBase
    {
        int _sportId;
        [JsonProperty("mainSportId")]
        public int SportId
        {
            get => _sportId;
            set => SetProperty(ref _sportId, value);
        }
        [JsonIgnore]
        public string UserSportImage
        {
            get
            {
                return TailUtils.GameTypeToImage_Follow((SportType)SportId);
            }
        }
        int? _accPerc;
        [JsonProperty("accPerc")]
        public int? AccPerc
        {
            get => _accPerc;
            set => SetProperty(ref _accPerc, value);
        }
        [JsonIgnore]
        public string UserAccuracy
        {
            get
            {
                if (AccPerc !=null)
                {

                    return AccPerc +"%";
                }
                else
                {
                    return "NA";
                }

            }
        }

        int? _lfpPerc;
        [JsonProperty("lfpPerc")]
        public int? LfpPerc
        {
            get => _lfpPerc;
            set => SetProperty(ref _lfpPerc, value);
        }
        bool _isInterest;
        [JsonProperty("isInterest")]
        public bool IsInterest
        {
            get => _isInterest;
            set => SetProperty(ref _isInterest, value);
        }

    }
    public class SharedUserInfo : ViewModelBase
    {
        string _shareUserName;
        [JsonProperty("uName")]
        public string ShareUserName
        {
            get => _shareUserName;
            set => SetProperty(ref _shareUserName, value);
        }
        int _shareUserId;
        [JsonProperty("userId")]
        public int ShareUserId
        {
            get => _shareUserId;
            set => SetProperty(ref _shareUserId, value);
        }
        string _shareUserImageName;
        [JsonProperty("userImg")]
        public string ShareUserImageName
        {
            get => _shareUserImageName;
            set => SetProperty(ref _shareUserImageName, value);
        }
        [JsonIgnore]
        public string ShareUserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(ShareUserImageName) && ShareUserImageName != "string")
                {

                    return TailUtils.GetThumbProfileImage(ShareUserImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }

    }
    public class PostDetails : ViewModelBase
    {
        PostDetails _sharedData;
        [JsonProperty("parent")]
        public PostDetails SharedData
        {
            get => _sharedData;
            set => SetProperty(ref _sharedData, value);
        }
      
        

        SharedUserInfo _sharedUserDetails;
        [JsonProperty("sharedBy")]
        public SharedUserInfo SharedUserDetails
        {
            get => _sharedUserDetails;
            set => SetProperty(ref _sharedUserDetails, value);
        }
        bool _isShare;
        public bool IsShare
        {
            get => _isShare;
            set => SetProperty(ref _isShare, value);
        }
        string _sharePostId;
        [JsonProperty("parentId")]
        public string sharePostId
        {
            get => _sharePostId;
            set => SetProperty(ref _sharePostId, value);
        }
        string _shareId;
        [JsonProperty("sid")]
        public string ShareId
        {
            get => _shareId;
            set => SetProperty(ref _shareId, value);
        }
        string _shareText;
        [JsonProperty("sharedDesc")]
        public string ShareText
        {
            get => _shareText;
            set => SetProperty(ref _shareText, value);
        }
        string _sharedText;
        [JsonProperty("sDesc")]
        public string SharedText
        {
            get => _sharedText;
            set => SetProperty(ref _sharedText, value);
        }
       

        string _shareUserName;
        [JsonProperty("sUName")]
        public string ShareUserName
        {
            get => _shareUserName;
            set => SetProperty(ref _shareUserName, value);
        }
        int _shareUserId;
        [JsonProperty("sUId")]
        public int ShareUserId
        {
            get => _shareUserId;
            set => SetProperty(ref _shareUserId, value);
        }
        string _shareUserImageName;
        [JsonProperty("sUImg")]
        public string ShareUserImageName
        {
            get => _shareUserImageName;
            set => SetProperty(ref _shareUserImageName, value);
        }
        [JsonIgnore]
        public string ShareUserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(ShareUserImageName) && ShareUserImageName != "string")
                {

                    return TailUtils.GetThumbProfileImage(ShareUserImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }
        
        string _sharePostedDate;
        [JsonProperty("saddOn")]
        public string SharePostedDate
        {
            get => _sharePostedDate;
            set => SetProperty(ref _sharePostedDate, value);
        }
       

       
        [JsonIgnore]
        public string ShareDisplayPostDate
        {
            get
            {
                if (!string.IsNullOrEmpty(SharePostedDate))
                {
                    DateTime _postDateTime = Convert.ToDateTime(SharePostedDate);
                    return TailUtils.FindDisplayTime(_postDateTime);
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        string _postId;
        [JsonProperty("_id")]
        public string PostId
        {
            get => _postId;
            set => SetProperty(ref _postId, value);
        }

        string _pickId;
        [JsonProperty("pickId")]
        public string PickId
        {
            get => _pickId;
            set => SetProperty(ref _pickId, value);
        }

        int _userId;
        [JsonProperty("userId")]
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        string _userName;
        [JsonProperty("uName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        [JsonIgnore]
        public string FirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    string[] _firstNameArr = UserName.Split(null);
                    return _firstNameArr[0] + ":";
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        string _userImageName;
        [JsonProperty("userImg")]
        public string UserImageName
        {
            get => _userImageName;
            set => SetProperty(ref _userImageName, value);
        }

        [JsonIgnore]
        public string UserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(UserImageName) && UserImageName != "string")
                {

                    return TailUtils.GetThumbProfileImage(UserImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }


        string _postedDate;
        [JsonProperty("addOn")]
        public string PostedDate
        {
            get => _postedDate;
            set => SetProperty(ref _postedDate, value);
        }
        [JsonIgnore]
        public string DisplayPostDate
        {
            get
            {
                if (!string.IsNullOrEmpty(PostedDate))
                {
                    DateTime _postDateTime = Convert.ToDateTime(PostedDate);
                    return TailUtils.FindDisplayTime(_postDateTime);
                   
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        string _postText;
        [JsonProperty("pText")]
        public string PostText
        {
            get => _postText;
            set => SetProperty(ref _postText, value);
        }
        [JsonIgnore]
        public string PostTextSmall
        {
            get
            {
                if (!string.IsNullOrEmpty(PostText))
                {
                    return PostText.Length <= 150 ? PostText : PostText.Substring(0, 150) + "...";
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        int _pType;
        [JsonProperty("pType")]
        public int PType
        {
            get => _pType;
            set => SetProperty(ref _pType, value);
        }
        public PostType Post_Type
        {
            get
            {
                if (PType == 4)
                {
                    return PostType.SharedPick;
                }
                else if (PType == 3)
                {
                    return PostType.SharedFree;
                }
                else if (PType == 2)
                {
                    return PostType.Pick;
                }
                else
                {
                    return PostType.Free;
                }

            }
        }
        [JsonIgnore]
        public bool IsShareNotAvailable
        {
            get
            {
                if ((PType==4 || PType==3) && SharedData == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        int _puchaseCount;
        [JsonProperty("purCnt")]
        public int PuchaseCount
        {
            get => _puchaseCount;
            set => SetProperty(ref _puchaseCount, value);
        }
        int _userStatus;
        [JsonProperty("uStatus")]
        public int UserStatus
        {
            get => _userStatus;
            set => SetProperty(ref _userStatus, value);
        }

        IList<PostAttchment> _postedAttachmentst;
        [JsonProperty("postfiles")]
        public IList<PostAttchment> PostedAttachments
        {
            get => _postedAttachmentst;
            set => SetProperty(ref _postedAttachmentst, value);
        }
        [JsonIgnore]
        public bool IsPlayEnable
        {
            get
            {

                if (PostedAttachments != null && PostedAttachments.Count!=0&& PostedAttachments[0].FileType == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsCarouselVisible
        {
            get
            {
                if (PostedAttachments!=null && PostedAttachments.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public bool PlayVisible
        {
            get
            {
                if (PostedAttachments!=null &&  PostedAttachments.Count==1 && PostedAttachments[0].FileType==2)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public string PlayImage
        {
            get
            {
               
                return "play";

            }
        }
       

        public int AttachmentHeight
        {
            get
            {
                if (PostedAttachments != null && PostedAttachments.Count != 0)
                {
                    return 280;
                }
                else
                {
                    return 0;
                }

            }
        }
        public bool IsCarousal
        {
            get
            {
                if (PostedAttachments!=null&& AttachmentHeight > 0 && PostedAttachments.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public int OffsetHeight
        {
            get
            {
                if (PostedAttachments != null && PostedAttachments.Count != 0)
                {
                    return 200;
                }
                else
                {
                    return 0;
                }

            }
        }

      
        int _likeCount;
        [JsonProperty("likeCnt")]
        public int LikeCount
        {
            get => _likeCount;
            set => SetProperty(ref _likeCount, value);
        }
        int _disLikeCount;
        [JsonProperty("dislikeCnt")]
        public int DisLikeCount
        {
            get => _disLikeCount;
            set => SetProperty(ref _disLikeCount, value);
        }

        int _commentCount;
        [JsonProperty("commentCnt")]
        public int CommentCount
        {
            get => _commentCount;
            set => SetProperty(ref _commentCount, value);
        }
        bool _postLikedStatus;
        [JsonProperty("isPostLiked")]
        public bool PostLikedStatus
        {
            get => _postLikedStatus;
            set => SetProperty(ref _postLikedStatus, value);
        }
        bool _postDisLikedStatus;
        [JsonProperty("isPostDisliked")]
        public bool PostDisLikedStatus
        {
            get => _postDisLikedStatus;
            set => SetProperty(ref _postDisLikedStatus, value);
        }
        Command<PostAttchment> _attachmentTap;
        public Command<PostAttchment> AttachmentTap
        {
            get => _attachmentTap;
            set => SetProperty(ref _attachmentTap, value);
        }
        [JsonIgnore]
        public LikeStatus LikedStatus
        {
            get
            {
                if (PostLikedStatus)
                {
                    return LikeStatus.Like;
                }
                else if (PostDisLikedStatus)
                {
                    return LikeStatus.Dislike;
                }
                else
                {
                    return LikeStatus.None;
                }

            }
        }
        [JsonIgnore]
        public string LikeImage
        {
            get
            {
                if (LikedStatus == LikeStatus.Like)
                {
                    return Constants.Like_SELECTED;
                }
                else
                {
                    return Constants.Like_DEFAULT;
                }

            }
        }
        [JsonIgnore]
        public string DisLikeImage
        {
            get
            {
                if (LikedStatus == LikeStatus.Dislike)
                {
                    return Constants.DisLike_SELECTED;
                }
                else
                {
                    return Constants.DisLike_DEFAULT;
                }

            }
        }

        int? _result;
        [JsonProperty("result")]
        public int? Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }
        [JsonIgnore]
        public ResultType Result_Type
        {
            get
            {
                switch (Result)
                {
                    case -1:
                        return ResultType.Push;
                    case 0:
                        return ResultType.Lost;
                    case 1:
                        return ResultType.Won;
                    default:
                        return ResultType.None;

                }

            }
        }
        string _resultTextHome;
        public string ResultTextHome
        {
            get => _resultTextHome;
            set => SetProperty(ref _resultTextHome, value);
        }
        string _resultTextAway;
        public string ResultTextAway
        {
            get => _resultTextAway;
            set => SetProperty(ref _resultTextAway, value);
        }
        [JsonIgnore]
        public string ResultText
        {
            get
            {
                switch (Result)
                {
                    case 0:
                        return AppResources.LostText;
                    case 1:
                        return AppResources.WonText;
                    default:
                        return "";

                }

            }
        }
        List<PickInfoDetails> _pickInfo;
        [JsonProperty("pickInfo")]
        public List<PickInfoDetails> PickInfo
        {
            get => _pickInfo;
            set => SetProperty(ref _pickInfo, value);
        }

        AccuracySports _accuracy;
        [JsonProperty("accuracy")]
        public AccuracySports Accuracy
        {
            get => _accuracy;
            set => SetProperty(ref _accuracy, value);
        }

       

        List<HideUserDetails> _hideUsers;
        [JsonProperty("hideUsers")]
        public List<HideUserDetails> HideUsers
        {
            get => _hideUsers;
            set => SetProperty(ref _hideUsers, value);
        }
      
        Command<PostDetails> _postTapCommand;
        public Command<PostDetails> PostTapCommand
        {
            get => _postTapCommand;
            set => SetProperty(ref _postTapCommand, value);
        }
     

        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
        bool _createImage = false;
        public bool CreateImage
        {
            get => _createImage;
            set => SetProperty(ref _createImage, value);
        }
        bool _isReport;
        [JsonProperty("isReport")]
        public bool IsReport
        {
            get => _isReport;
            set => SetProperty(ref _isReport, value);
        }
        double _baseScore;
        [JsonProperty("baseScore")]
        public double BaseScore
        {
            get => _baseScore;
            set => SetProperty(ref _baseScore, value);
        }
        bool _isTrend;
        [JsonProperty("isTrend")]
        public bool IsTrend
        {
            get => _isTrend;
            set => SetProperty(ref _isTrend, value);
        }
        List<PostUserDetails> _createdUserDetails;
        [JsonProperty("userDetails")]
        public List<PostUserDetails> CreatedUserDetails
        {
            get => _createdUserDetails;
            set => SetProperty(ref _createdUserDetails, value);
        }
        bool _isFollowingBack;
        [JsonProperty("isFollowing")]
        public bool IsFollowingBack
        {
            get => _isFollowingBack;
            set => SetProperty(ref _isFollowingBack, value);
        }
        bool _isPurchased;
        [JsonProperty("isPurchased")]
        public bool IsPurchased
        {
            get => _isPurchased;
            set => SetProperty(ref _isPurchased, value);
        }
        int _wagerUnit;
        [JsonProperty("wagerUnit")]
        public int WagerUnit
        {
            get => _wagerUnit;
            set => SetProperty(ref _wagerUnit, value);
        }
        [JsonProperty("wlastTot")]
        public int WlastTot
        {
            get;
            set;
        }
        [JsonIgnore]
        public string LastFewUnit
        {
            get
            {
                if (WlastTot > 0)
                {

                    return "+" + WlastTot +" U";
                }
                else
                {
                    return WlastTot + " U";
                }

            }
        }
    }
    public class PostAttchment : ViewModelBase
    {
        
        string _fileID;
        [JsonProperty("_id")]
        public string FileID
        {
            get => _fileID;
            set => SetProperty(ref _fileID, value);
        }

        int _fileType;
        [JsonProperty("ptFileType")]
        public int FileType
        {
            get => _fileType;
            set => SetProperty(ref _fileType, value);
        }
       

        string _imageName;
        [JsonProperty("ptFileUrl")]
        public string ImageName
        {
            get => _imageName;
            set => SetProperty(ref _imageName, value);
        }
        [JsonIgnore]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName))
                {
                    if (FileType == 1)
                    {
                        return TailUtils.GetThumbPostImage(ImageName);
                    }
                    else
                    {
                        return TailUtils.GetThumbPostVideo(ImageName);
                    }
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        [JsonIgnore]
        public string ImageActualUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName))
                {
                    if (FileType == 1)
                    {
                        return TailUtils.GetOrginalPostImage(ImageName);
                    }
                    else
                    {
                        return TailUtils.GetOrginalPostVideo(ImageName);
                    }
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        [JsonIgnore]
        public bool IsPlayEnable
        {
            get
            {

                if (FileType == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Command<PostAttchment> _attachmentTap;
        public Command<PostAttchment> AttachmentTap
        {
            get => _attachmentTap;
            set => SetProperty(ref _attachmentTap, value);
        }
    }
    public class PickInfoDetails : ViewModelBase
    {
        float _pickPrice;
        [JsonProperty("pickFee")]
        public float PickPrice
        {
            get => _pickPrice;
            set => SetProperty(ref _pickPrice, value);
        }
        string _displyPickPrice;
        public string DisplyPickPrice
        {
            get => _displyPickPrice;
            set => SetProperty(ref _displyPickPrice, value);
        }
       

        double _hspScore;
        [JsonProperty("hSpScore")]
        public double HspScore
        {
            get => _hspScore;
            set => SetProperty(ref _hspScore, value);
        }
        double _aspScore;
        [JsonProperty("aSpScore")]
        public double AspScore
        {
            get => _aspScore;
            set => SetProperty(ref _aspScore, value);
        }


        double _hPtSpMoney;
        [JsonProperty("hPtSpMoney")]
        public double HPtSpMoney
        {
            get => _hPtSpMoney;
            set => SetProperty(ref _hPtSpMoney, value);
        }
        double _aPtSpMoney;
        [JsonProperty("aPtSpMoney")]
        public double APtSpMoney
        {
            get => _aPtSpMoney;
            set => SetProperty(ref _aPtSpMoney, value);
        }

        double _hmoScore;
        [JsonProperty("hMoScore")]
        public double HmoScore
        {
            get => _hmoScore;
            set => SetProperty(ref _hmoScore, value);
        }
        double _amoScore;
        [JsonProperty("aMoScore")]
        public double AmoScore
        {
            get => _amoScore;
            set => SetProperty(ref _amoScore, value);
        }
        double _overScore;
        [JsonProperty("oScore")]
        public double OverScore
        {
            get => _overScore;
            set => SetProperty(ref _overScore, value);
        }
        public double OScore
        {
            get
            {
                return Math.Round(OverScore, 2);
            }
        }
        double _underScore;
        [JsonProperty("uScore")]
        public double UnderScore
        {
            get => _underScore;
            set => SetProperty(ref _underScore, value);
        }

        public double UScore
        {
            get
            {
                return Math.Round(UnderScore, 2);
            }
        }

        double _tlOvMoney;
        [JsonProperty("tlOvMoney")]
        public double TlOvMoney
        {
            get => _tlOvMoney;
            set => SetProperty(ref _tlOvMoney, value);
        }
        double _tlUnMoney;
        [JsonProperty("tlUnMoney")]
        public double TlUnMoney
        {
            get => _tlUnMoney;
            set => SetProperty(ref _tlUnMoney, value);
        }


        double _htScore;
        [JsonProperty("htScore")]
        public double HtScore
        {
            get => _htScore;
            set => SetProperty(ref _htScore, value);
        }
        double _atScore;
        [JsonProperty("atScore")]
        public double AtScore
        {
            get => _atScore;
            set => SetProperty(ref _atScore, value);
        }

        string _finalScoreText;
        public string FinalScoreText
        {
            get => _finalScoreText;
            set => SetProperty(ref _finalScoreText, value);
        }

        string _betPointHome;
        public string BetPointHome
        {
            get => _betPointHome;
            set => SetProperty(ref _betPointHome, value);
        }
        string _betPointAway;
        public string BetPointAway
        {
            get => _betPointAway;
            set => SetProperty(ref _betPointAway, value);
        }


        string _betValueHome;
        public string BetValueHome
        {
            get => _betValueHome;
            set => SetProperty(ref _betValueHome, value);
        }
        string _betValueAway;
        public string BetValueAway
        {
            get => _betValueAway;
            set => SetProperty(ref _betValueAway, value);
        }

        int _mainSportId;
        [JsonProperty("mainSportId")]
        public int MainSportId
        {
            get => _mainSportId;
            set => SetProperty(ref _mainSportId, value);
        }
        [JsonIgnore]
        public SportType Sport_Type
        {
            get
            {
                return (SportType)MainSportId;
            }
        }
        [JsonIgnore]
        public string TeamDefaultLogo
        {
            get
            {
                if (Sport_Type == SportType.Baseball)
                    return "baseball_placeholder.png";
                else if (Sport_Type == SportType.Basketball)
                    return "basketball_placeholder.png";
                else if (Sport_Type == SportType.Football)
                    return "football_placeholder.png";
                else if (Sport_Type == SportType.Hocky)
                    return "hocky_placeholder.png";
                else if (Sport_Type == SportType.MMA)
                    return "mma_or_boxing_placeholder.png";
                else
                    return "baseball_placeholder.png";

            }
        }
        string _selectedSpotName;
        public string SelectedSpotName
        {
            get => _selectedSpotName;
            set => SetProperty(ref _selectedSpotName, value);
        }
        string _selectedSpotImage;
        public string SelectedSpotImage
        {
            get => _selectedSpotImage;
            set => SetProperty(ref _selectedSpotImage, value);
        }
       

        string _accuracyText;
        public string AccuracyText
        {
            get => _accuracyText;
            set => SetProperty(ref _accuracyText, value);
        }
        int _sportId;
        [JsonProperty("sportId")]
        public int SportId
        {
            get => _sportId;
            set => SetProperty(ref _sportId, value);
        }
        string _selectedEventName;
        [JsonProperty("sName")]
        public string SelectedEventName
        {
            get => _selectedEventName;
            set => SetProperty(ref _selectedEventName, value);
        }
        string _selectedEventImage;
        public string SelectedEventImage
        {
            get => _selectedEventImage;
            set => SetProperty(ref _selectedEventImage, value);
        }
        string _last10PredText;
        public string Last10PredText
        {
            get => _last10PredText;
            set => SetProperty(ref _last10PredText, value);
        }

        string _eventId;
        [JsonProperty("eventId")]
        public string EventId
        {
            get => _eventId;
            set => SetProperty(ref _eventId, value);
        }
        string _gameDateTime;
        [JsonProperty("eventDate")]
        public string GameDateTime
        {
            get => _gameDateTime;
            set => SetProperty(ref _gameDateTime, value);
        }
        string _gameDate;
        public string GameDate
        {
            get => _gameDate;
            set => SetProperty(ref _gameDate, value);
        }

        string _gameTime;
        public string GameTime
        {
            get => _gameTime;
            set => SetProperty(ref _gameTime, value);
        }
        string _displayGameDateTime;
        public string DisplayGameDateTime
        {
            get => _displayGameDateTime;
            set => SetProperty(ref _displayGameDateTime, value);
        }

      
        string _firstTeamName;
        [JsonProperty("htName")]
        public string FirstTeamName
        {
            get => _firstTeamName;
            set => SetProperty(ref _firstTeamName, value);
        }

        string _firstTeamAbbr;
        [JsonProperty("htAbbr")]
        public string FirstTeamAbbr
        {
            get => _firstTeamAbbr;
            set => SetProperty(ref _firstTeamAbbr, value);
        }
        string _htLogo;
        [JsonProperty("htLogo")]
        public string HtLogo
        {
            get => _htLogo;
            set => SetProperty(ref _htLogo, value);
        }
        string _firstTeamImage;
        public string FirstTeamImage
        {
            get => _firstTeamImage;
            set => SetProperty(ref _firstTeamImage, value);
        }
        string _firstTeamColor; 
        [JsonProperty("htHomeColorCode")]
        public string FirstTeamColor
        {
            get => _firstTeamColor;
            set => SetProperty(ref _firstTeamColor, value);
        }
        string _firstTeamSecondaryColor;
        [JsonProperty("htAwayColorCode")]
        public string FirstTeamSecondaryColor
        {
            get => _firstTeamSecondaryColor;
            set => SetProperty(ref _firstTeamSecondaryColor, value);
        }
        [JsonIgnore]
        public bool IsDefaultLogoHome
        {
            get
            {
                if (string.IsNullOrEmpty(FirstTeamColor) || Sport_Type == SportType.MMA)
                    return true;
                else
                    return false;

            }
        }
        string _secondTeamName;
        [JsonProperty("atName")]
        public string SecondTeamName
        {
            get => _secondTeamName;
            set => SetProperty(ref _secondTeamName, value);
        }

        string _secondTeamAbbr;
        [JsonProperty("atAbbr")]
        public string SecondTeamAbbr
        {
            get => _secondTeamAbbr;
            set => SetProperty(ref _secondTeamAbbr, value);
        }
        string _atLogo;
        [JsonProperty("atLogo")]
        public string AtLogo
        {
            get => _atLogo;
            set => SetProperty(ref _atLogo, value);
        }
        string _secondTeamImage;
        public string SecondTeamImage
        {
            get => _secondTeamImage;
            set => SetProperty(ref _secondTeamImage, value);
        }

        string _secondTeamColor;
        [JsonProperty("atHomeColorCode")]
        public string SecondTeamColor
        {
            get => _secondTeamColor;
            set => SetProperty(ref _secondTeamColor, value);
        }
        string _secondTeamSecondaryColor;
        [JsonProperty("atAwayColorCode")]
        public string SecondTeamSecondaryColor
        {
            get => _secondTeamSecondaryColor;
            set => SetProperty(ref _secondTeamSecondaryColor, value);
        }
        [JsonIgnore]
        public bool IsDefaultLogoAway
        {
            get
            {
                if (string.IsNullOrEmpty(SecondTeamColor) || Sport_Type == SportType.MMA)
                    return true;
                else
                    return false;

            }
        }
        int _betType;
        [JsonProperty("betType")]
        public int BetType
        {
            get => _betType;
            set => SetProperty(ref _betType, value);
        }
        PickPurchaseType _pickPurchase_Type;
        public PickPurchaseType PickPurchase_Type
        {
            get => _pickPurchase_Type;
            set => SetProperty(ref _pickPurchase_Type, value);
        }

     
        public PickType Pick_Type
        {
            get
            { 
              return (PickType)BetType;  
            }
        }

        bool _isPickPurchase;
        public bool IsPickPurchase
        {
            get => _isPickPurchase;
            set => SetProperty(ref _isPickPurchase, value);
        }
       
        bool _isGameComplete=false;
        public bool IsGameComplete
        {
            get => _isGameComplete;
            set => SetProperty(ref _isGameComplete, value);
        }
     
        int? _typeToWin;
        [JsonProperty("tTypeToWin")]
        public int? TypeToWin
        {
            get => _typeToWin;
            set => SetProperty(ref _typeToWin, value);
        }
        TeamType _predictionTeam;
        public TeamType PredictionTeam
        {
            get => _predictionTeam;
            set => SetProperty(ref _predictionTeam, value);
        }

        bool _homeBorderVisible;
        public bool HomeBorderVisible
        {
            get => _homeBorderVisible;
            set => SetProperty(ref _homeBorderVisible, value);
        }
       
        string _homeBackground;
        public string HomeBackground
        {
            get => _homeBackground;
            set => SetProperty(ref _homeBackground, value);
        }
        string _awayBackground;
        public string AwayBackground
        {
            get => _awayBackground;
            set => SetProperty(ref _awayBackground, value);
        }
        double _opacityHome;
        public double OpacityHome
        {
            get => _opacityHome;
            set => SetProperty(ref _opacityHome, value);
        }
        double _opacityMoneyTextyHome;
        public double OpacityMoneyTextyHome
        {
            get => _opacityMoneyTextyHome;
            set => SetProperty(ref _opacityMoneyTextyHome, value);
        }

       
        double _opacityAway;
        public double OpacityAway
        {
            get => _opacityAway;
            set => SetProperty(ref _opacityAway, value);
        }
        double _opacityMoneyTextAway;
        public double OpacityMoneyTextAway
        {
            get => _opacityMoneyTextAway;
            set => SetProperty(ref _opacityMoneyTextAway, value);
        }
        string _homeTextColor;
        public string HomeTextColor
        {
            get => _homeTextColor;
            set => SetProperty(ref _homeTextColor, value);
        }

       
        string _homeMoneyTextColor;
        public string HomeMoneyTextColor
        {
            get => _homeMoneyTextColor;
            set => SetProperty(ref _homeMoneyTextColor, value);
        }
       
        string _awayTextColor;
        public string AwayTextColor
        {
            get => _awayTextColor;
            set => SetProperty(ref _awayTextColor, value);
        }
       
        string _awayMoneyTextColor;
        public string AwayMoneyTextColor
        {
            get => _awayMoneyTextColor;
            set => SetProperty(ref _awayMoneyTextColor, value);
        }

        string _selectedTextHome;
        public string SelectedTextHome
        {
            get => _selectedTextHome;
            set => SetProperty(ref _selectedTextHome, value);
        }

        string _selectedTextAway;
        public string SelectedTextAway
        {
            get => _selectedTextAway;
            set => SetProperty(ref _selectedTextAway, value);
        }

        TeamType _winnerTeam;
        public TeamType WinnerTeam
        {
            get => _winnerTeam;
            set => SetProperty(ref _winnerTeam, value);
        }
        string _predictionTeamName;
        [JsonProperty("tNameToWin")]
        public string PredictionTeamName
        {
            get => _predictionTeamName;
            set => SetProperty(ref _predictionTeamName, value);
        }
       

        bool _isMyPurchasedPick = false;
        public bool IsMyPurchasedPick
        {
            get => _isMyPurchasedPick;
            set => SetProperty(ref _isMyPurchasedPick, value);
        }
        string _pickPurchaseDate;
        public string PickPurchaseDate
        {
            get => _pickPurchaseDate;
            set => SetProperty(ref _pickPurchaseDate, value);
        }


        bool _isPaidPickPurchaseEnable = true;
        public bool IsPaidPickPurchaseEnable
        {
            get => _isPaidPickPurchaseEnable;
            set => SetProperty(ref _isPaidPickPurchaseEnable, value);
        }
        bool _isContentHide;
        [JsonProperty("isCnet")]
        public bool IsContentHide
        {
            get => _isContentHide;
            set => SetProperty(ref _isContentHide, value);
        }
        bool _isAttachmentEnable;
        public bool IsAttachmentEnable
        {
            get => _isAttachmentEnable;
            set => SetProperty(ref _isAttachmentEnable, value);
        }
        int _attachmentHeight;
        public int AttachmentHeight
        {
            get => _attachmentHeight;
            set => SetProperty(ref _attachmentHeight, value);
        }
       

    }
    public class HideUserDetails
    {


    }
    public class PostEventsArgs
    {
        public bool serviceRequired { get; set; } = false;
        public PostDetails postObj { get; set; }

    }
    public class PostUserDetails : ViewModelBase
    {
        string _id;
        [JsonProperty("_id")]
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        string _uName;
        [JsonProperty("uName")]
        public string UName
        {
            get => _uName;
            set => SetProperty(ref _uName, value);
        }
        string _userImg;
        [JsonProperty("userImg")]
        public string UserImg
        {
            get => _userImg;
            set => SetProperty(ref _userImg, value);
        }
    }
    public class DeletePostRequestInfo
    {
        public string postId { get; set; }
        public int pType { get; set; }

    }
    public class DeleteShareRequestInfo
    {
        public string shareId { get; set; }

    }
    public class AddPickRequestInfo
    {
        public int userId { get; set; }
        public int mainSportId { get; set; }
        public int sportId { get; set; }
        public string sName { get; set; }
        public string eventId { get; set; }
        public string eventDate { get; set; }
        public string htName { get; set; }
        public string htAbbr { get; set; }
        public string htLogo { get; set; }
        public string atName { get; set; }
        public string atAbbr { get; set; }
        public string atLogo { get; set; }
        public double pickFee { get; set; }
        public int betType { get; set; }
        public int teamIdToWin { get; set; }
        public string tNameToWin { get; set; }
        public double hSpScore { get; set; }
        public double aSpScore { get; set; }
        public double hPtSpMoney { get; set; }
        public double aPtSpMoney { get; set; }
        public double hMoScore { get; set; }
        public double aMoScore { get; set; }
        public double oScore { get; set; }
        public double uScore { get; set; }
        public double tlOvMoney { get; set; }
        public double tlUnMoney { get; set; }
        public string postContent { get; set; }
        public List<ImageData> imageUrl { get; set; }
        public List<VideoData> videoUrl { get; set; }
        public int tTypeToWin { get; set; }
        public int wagerUnit { get; set; }
        public bool isContent { get; set; }

        public string htHomeColorCode { get; set; }
        public string htAwayColorCode { get; set; }
        public string atHomeColorCode { get; set; }
        public string atAwayColorCode { get; set; }
    }
    public class TrendPost : ViewModelBase
    {
        string _postId;
        [JsonProperty("PostId")]
        public string PostId
        {
            get => _postId;
            set => SetProperty(ref _postId, value);
        }
        int _userId;
        [JsonProperty("UserId")]
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        string _userName;
        [JsonProperty("UserName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        [JsonIgnore]
        public string FirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    string[] _firstNameArr = UserName.Split(null);
                    return _firstNameArr[0] + ":";
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        string _userImageName;
        [JsonProperty("UserImage")]
        public string UserImageName
        {
            get => _userImageName;
            set => SetProperty(ref _userImageName, value);
        }

        [JsonIgnore]
        public string UserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(UserImageName) && UserImageName != "string" && UserImageName != "NOFILE")
                {

                    return TailUtils.GetThumbProfileImage(UserImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }


        string _postedDate;
        [JsonProperty("PostedOn")]
        public string PostedDate
        {
            get => _postedDate;
            set => SetProperty(ref _postedDate, value);
        }
        [JsonIgnore]
        public string DisplayPostDate
        {
            get
            {
                if (!string.IsNullOrEmpty(PostedDate))
                {
                    DateTime _postDateTime = Convert.ToDateTime(PostedDate);
                    return TailUtils.FindDisplayTime(_postDateTime);
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        string _postText;
        [JsonProperty("Title")]
        public string PostText
        {
            get => _postText;
            set => SetProperty(ref _postText, value);
        }

        string _gameDateTime;
        [JsonProperty("GameDate")]
        public string GameDateTime
        {
            get => _gameDateTime;
            set => SetProperty(ref _gameDateTime, value);
        }
        string _gameDateDisplay;
        public string GameDateDisplay
        {
            get => _gameDateDisplay;
            set => SetProperty(ref _gameDateDisplay, value);
        }

        string _gameTimeDisplay;
        public string GameTimeDisplay
        {
            get => _gameTimeDisplay;
            set => SetProperty(ref _gameTimeDisplay, value);
        }

        [JsonIgnore]
        public string PostTextSmall
        {
            get
            {
                if (!string.IsNullOrEmpty(PostText))
                {
                    return PostText.Length <= 150 ? PostText : PostText.Substring(0, 150) + "...";
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        int _pType=1;
        public int PType
        {
            get => _pType;
            set => SetProperty(ref _pType, value);
        }
        public PostType Post_Type
        {
            get
            {
                if(PType==2)
                    return PostType.Pick;
                else
                    return PostType.Free;

            }
        }

        int _fileType=1;
        public int FileType
        {
            get => _fileType;
            set => SetProperty(ref _fileType, value);
        }

        string _imageName;
        [JsonProperty("PostFile")]
        public string ImageName
        {
            get => _imageName;
            set => SetProperty(ref _imageName, value);
        }
        [JsonIgnore]
        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName) && ImageName != "NOFILE")
                {
                    if (FileType == 1)
                    {
                        return TailUtils.GetThumbPostImage(ImageName);
                    }
                    else
                    {
                        return TailUtils.GetThumbPostVideo(ImageName);
                    }
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        [JsonIgnore]
        public string ImageActualUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName))
                {
                    if (FileType == 1)
                    {
                        return TailUtils.GetOrginalPostImage(ImageName);
                    }
                    else
                    {
                        return TailUtils.GetOrginalPostVideo(ImageName);
                    }
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        [JsonIgnore]
        public bool IsPlayEnable
        {
            get
            {

                if (FileType == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Command<PostAttchment> _attachmentTap;
        public Command<PostAttchment> AttachmentTap
        {
            get => _attachmentTap;
            set => SetProperty(ref _attachmentTap, value);
        }


    }

    public class TrendPostMain : ViewModelBase
    {
        TrendPost _postItem;
        public TrendPost PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }

        Command<TrendPost> _commentCommand;
        public Command<TrendPost> CommentCommand
        {
            get => _commentCommand;
            set => SetProperty(ref _commentCommand, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }

    }

}
