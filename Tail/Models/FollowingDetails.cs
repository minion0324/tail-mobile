
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Models
{
    public class FollowingDetailsMain : BaseModel
    {
        IList<FollowingDetails> _followersData;
        [JsonProperty("resultData")]
        public IList<FollowingDetails> FollowersData
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

    public class FollowingDetails : BaseModel
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
                if (!string.IsNullOrEmpty(ImageName))
                {
                    return TailUtils.GetThumbProfileImage(ImageName);
                }
                else
                {
                    return Constants.DEFAULT_USERIMAGE;
                }

            }
        }
        [JsonIgnore]
        public bool FollowButtonVisible
        {
            get
            {
                if(UserId== SettingsService.Instance.LoggedUserDetails.UserId)
                {
                    return false;
                }
                else
                {
                    return true;
                }
               
            }
        }
        [JsonProperty("fwgCnt")]
        public int FollowingCount { get; set; }
        [JsonProperty("fwrCnt")]
        public int FollowersCount { get; set; }
        public bool IsDataRefresh { get; set; }
        [JsonProperty("accuracy")]
        public IList<FollowersSport> MySports { get; set; }
        [JsonIgnore]
        public string DisplayFollowing
        {
            get
            {

                return string.Format(AppResources.FollowingCount, FollowingCount);
            }
        }
        [JsonIgnore]
        public string DisplayFollowers
        {
            get
            {

                return string.Format(AppResources.FollowersCount, FollowersCount);
            }
        }
        Command<int> _folloUnFollowCommand;
        public Command<int> FolloUnFollowCommand
        {
            get => _folloUnFollowCommand;
            set => SetProperty(ref _folloUnFollowCommand, value);
        }
        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
        bool _isFollow ;
        public bool IsFollow
        {
            get => _isFollow;
            set => SetProperty(ref _isFollow, value);
        }
        bool _isFollowedBack;
        [JsonProperty("isFollowedBack")]
        public bool IsFollowedBack
        {
            get => _isFollowedBack;
            set => SetProperty(ref _isFollowedBack, value);
        }
    }
}
