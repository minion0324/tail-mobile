
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class ProfileDetails : BaseModel
    {

        [JsonProperty("UserId")]
        public int UserId
        {
            get;
            set;
        }
        string _userName;
        [JsonProperty("UserName")]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        string _userImage;
        [JsonProperty("UserImage")]
        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        string _aboutMe;
        [JsonProperty("AboutMe")]
        public string AboutMe
        {
            get => _aboutMe;
            set => SetProperty(ref _aboutMe, value);
        }

        [JsonProperty("LifeTimePredictions")]
        public List<Predictions> LifetimePredictions
        {
            get;
            set;
        }
        [JsonProperty("Last15Predictions")]
        public List<Predictions> Last15Predictions
        {
            get;
            set;
        }
        [JsonProperty("UserPosts")]
        public List<PostDetails> UserPosts
        {
            get;
            set;
        }
        [JsonProperty("UserPicks")]
        public List<PostDetails> UserPicks
        {
            get;
            set;
        }

        [JsonProperty("UserFollowers")]
        public List<FollowersDetails> UserFollowers
        {
            get;
            set;
        }
        [JsonProperty("UserFollowing")]
        public List<FollowersDetails> UserFollowing
        {
            get;
            set;
        }
        [JsonProperty("UserPurchases")]
        public List<PostDetails> UserPurchases
        {
            get;
            set;
        }
        [JsonProperty("PostCount")]
        public int PostCount
        {
            get;
            set;
        }
        [JsonProperty("PicksCount")]
        public int PicksCount
        {
            get;
            set;
        }
        [JsonProperty("FollowersCount")]
        public int FollowersCount
        {
            get;
            set;
        }
        [JsonProperty("FollowingCount")]
        public int FollowingCount
        {
            get;
            set;
        }
        [JsonProperty("PurchaseCount")]
        public int PurchaseCount
        {
            get;
            set;
        }
        [JsonProperty("IsFollowing")]
        public bool IsFollowing
        {
            get;
            set;
        }
    }
}
