using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace Tail.Models
{
    public class LoginRequestInfo
    {
        public string emailId { get; set; }
        public string password { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
    }
    public class AppleLoginRequestInfo
    {
        public string appleToken { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
        public string email { get; set; }
    }
    public class FBLoginRequestInfo
    {
        public string fbToken { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
        public string email { get; set; }
    }
    public class LoggedInUser
    {
        public int IsSportsAdded { get; set; }
        public int IsPhoneVerified { get; set; }
        public string Id { get; set; }
        [PrimaryKey]
        public int UserTypeId { get; set; }
        public bool IsAdmin { get; set; }
        public int UserStatus { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Dob { get; set; }
        public string Phone { get; set; }
        public string UserImage { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string AboutMe { get; set; }
        public string CountryCode { get; set; }

    }

    public class LoginResponseInfo : BaseModel
    {
        int _isSportsAdded;
        int _isPhoneVerified;
        string _id;
        int _userTypeId;
        bool _isAdmin;
        int _userStatus;
        string _userName;
        string _email;
        string _dob;
        string _phone;
        string _userImage;
        int _userId;
        string _refreshToken;
        string _accessToken;

        string _aboutMe;
        string _countryCode;
       
        bool _isfollowing;
        int _postCount;
        int _pickCount;
        int _followersCount;
        int _followingCount;
        int _purchaseCount;
        int _shareCount;
        int _unreadMsgCount;
        [JsonProperty("isSportsAdded")]
        public int IsSportsAdded
        {
            get => _isSportsAdded;
            set => SetProperty(ref _isSportsAdded, value);
        }

        [JsonProperty("isPhoneVerified")]
        public int IsPhoneVerified
        {
            get => _isPhoneVerified;
            set => SetProperty(ref _isPhoneVerified, value);
        }
        [JsonProperty("_id")]
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        [PrimaryKey]
        [JsonProperty("userTypeId")]
        public int UserTypeId
        {
            get => _userTypeId;
            set => SetProperty(ref _userTypeId, value);
        }
        [JsonProperty("isAdmin")]
        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }
        [JsonProperty("userStatus")]
        public int UserStatus
        {
            get => _userStatus;
            set => SetProperty(ref _userStatus, value);
        }
        [JsonProperty("uName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        [JsonProperty("email")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        [JsonProperty("dob")]
        public string Dob
        {
            get => _dob;
            set => SetProperty(ref _dob, value);
        }
        [JsonProperty("phone")]
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        [JsonProperty("userImg")]
        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        [JsonProperty("userId")]
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        [JsonProperty("refreshToken")]
        public string RefreshToken
        {
            get => _refreshToken;
            set => SetProperty(ref _refreshToken, value);
        }
        [JsonProperty("accessToken")]
        public string AccessToken
        {
            get => _accessToken;
            set => SetProperty(ref _accessToken, value);
        }
        [JsonProperty("aboutMe")]
        public string AboutMe
        {
            get => _aboutMe;
            set => SetProperty(ref _aboutMe, value);
        }
        [JsonProperty("country")]
        public string CountryCode
        {
            get => _countryCode;
            set => SetProperty(ref _countryCode, value);
        }
        [JsonProperty("isfollowing")]
        public bool IsFollowing
        {
            get => _isfollowing;
            set => SetProperty(ref _isfollowing, value);
        }
        [JsonProperty("postCount")]
        public int PostCount
        {
            get => _postCount;
            set => SetProperty(ref _postCount, value);
        }
        [JsonProperty("pickCount")]
        public int PickCount
        {
            get => _pickCount;
            set => SetProperty(ref _pickCount, value);
        }
        [JsonProperty("fwrCnt")]
        public int FollowersCount
        {
            get => _followersCount;
            set => SetProperty(ref _followersCount, value);
        }
        [JsonProperty("fwgCnt")]
        public int FollowingCount
        {
            get => _followingCount;
            set => SetProperty(ref _followingCount, value);
        }
        [JsonProperty("purCnt")]
        public int PurchaseCount
        {
            get => _purchaseCount;
            set => SetProperty(ref _purchaseCount, value);
        }
        [JsonProperty("shareCnt")]
        public int ShareCount
        {
            get => _shareCount;
            set => SetProperty(ref _shareCount, value);
        }
        [JsonProperty("unreadMsgCount")]
        public int UnreadMsgCount
        {
            get => _unreadMsgCount;
            set => SetProperty(ref _unreadMsgCount, value);
        }
        
        [JsonProperty("wUnitTot")]
        public int WUnitTot
        {
            get;
            set;
        }
       
        [JsonIgnore]
        public string LifeTimeUnit
        {
            get
            {
                if (WUnitTot >0)
                {

                    return "+"+ WUnitTot;
                }
                else
                {
                    return WUnitTot.ToString();
                }

            }
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

                    return "+" + WlastTot;
                }
                else
                {
                    return WlastTot.ToString();
                }

            }
        }
     
        [JsonProperty("accuracy")]
        public IList<AccuracySports> MySportsAccuracy { get; set; }
    }
    public class AccuracySports
    {

        [JsonProperty("mainSportId")]
        public int SportId
        {
            get;
            set;
        }
        [JsonProperty("accPerc")]
        public int? AccuracyPrediction
        {
            get;
            set;
        }
        [JsonProperty("lfpPerc")]
        public int? AccuracyLastFew
        {
            get;
            set;
        }

        [JsonProperty("lastNWon")]
        public int? AccuracyGoodCount
        {
            get;
            set;
        }
        [JsonProperty("lastNLost")]
        public int? AccuracyBadCount
        {
            get;
            set;
        } 
        [JsonIgnore]
        public string AccuracyGoodValue
        {
            get
            {
                if (AccuracyGoodCount != null)
                {

                    return " ("+ AccuracyGoodCount + ") ";
                }
                else
                {
                    return "";
                }

            }
        }
        [JsonIgnore]
        public string AccuracyBadValue
        {
            get
            {
                if (AccuracyBadCount != null)
                {

                    return " (" + AccuracyBadCount + ") ";
                }
                else
                {
                    return "";
                }

            }
        }


        [JsonIgnore]
        public double? AccuracySliderRange
        {
            get
            {
                if (AccuracyPrediction != null)
                {

                    return Convert.ToDouble(AccuracyPrediction)/10;
                }
                else
                {
                    return null;
                }

            }
        }

    
        [JsonIgnore]
        public string AccuracyInGoodPrediction
        {
            get
            {
                if (AccuracyLastFew != null)
                {

                    return AccuracyLastFew + "%";
                }
                else
                {
                    return "NA";
                }

            }
        }
        [JsonIgnore]
        public string AccuracyInBadPrediction
        {
            get
            {
                if (AccuracyLastFew != null)
                {

                    return (100- Convert.ToInt32(AccuracyLastFew)) + "%";
                }
                else
                {
                    return "NA";
                }

            }
        }

    }
}
