using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.Common;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Models
{
    public class RecommendedFollowersMain : BaseModel
    {
        IList<RecommendedFollowers> _followersData;
        [JsonProperty("resultData")]
        public IList<RecommendedFollowers> FollowersData
        {
            get => _followersData;
            set => SetProperty(ref _followersData, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
    }

    public class RecommendedFollowers : BaseModel
    {
        [JsonProperty("_id")]
        public string ID { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("uName")]
        public string UserName { get; set; }
        [JsonProperty("userImg")]
        public string ImageName { get; set; }
        public string UserImage
        {
            get
            {
                if (!string.IsNullOrEmpty(ImageName) && ImageName != "string")
                {
                    return TailUtils.GetThumbProfileImage(ImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }
                
            }
        }

        [JsonProperty("predSports")]
        public FollowersSport MySports { get; set; }
        bool _isFollow=false;
        public bool IsFollow
        {
            get => _isFollow;
            set => SetProperty(ref _isFollow, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
        Command<int> _followUnfollow;
        public Command<int> FollowUnfollow
        {
            get => _followUnfollow;
            set => SetProperty(ref _followUnfollow, value);
        }
    }
    public class FollowersSport{

        [JsonProperty("mainSportId")]
        public int SportId
        {
            get;
            set;
        }
        [JsonProperty("accPerc")]
        public int AccuracyPrediction
        {
            get;
            set;
        }
        [JsonIgnore]
        public string PickByPrediction
        {
            get
            {
                if(AccuracyPrediction == 0)
                {
                    return "NA";
                }
                else
                {
                    return AccuracyPrediction + "%";
                }
               
            }
        }
      
        [JsonIgnore]
        public string SportName
        {
            get
            {
                return TailUtils.GameTypeToName((SportType)SportId);
            }
        }
        [JsonIgnore]
        public string SportIcon
        {
            get
            {
                return TailUtils.GameTypeToImage_Follow((SportType)SportId);
            }
        }

    }
}
