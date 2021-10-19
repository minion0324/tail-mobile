
using System.Collections.Generic;
using Tail.ViewModels;
using Tail.Common;
using Newtonsoft.Json;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Models
{
    public class TrendingModel : ViewModelBase
    {
        IList<TrendingPeopleModel> _trendingPeopleList;
        public IList<TrendingPeopleModel> TrendingPeopleList
        {
            get => _trendingPeopleList;
            set => SetProperty(ref _trendingPeopleList, value);
        }
        IList<PostDetails> _trendingPicks;
        public IList<PostDetails> TrendingPicks
        {
            get => _trendingPicks;
            set => SetProperty(ref _trendingPicks, value);
        }
        IList<PostDetails> _trendingPosts;
        public IList<PostDetails> TrendingPosts
        {
            get => _trendingPosts;
            set => SetProperty(ref _trendingPosts, value);
        }
        bool _isResult=false;
        public bool IsResult
        {
            get => _isResult;
            set => SetProperty(ref _isResult, value);
        }
        bool _isViewAllEnable = true;
        public bool IsViewAllEnable
        {
            get => _isViewAllEnable;
            set => SetProperty(ref _isViewAllEnable, value);
        }
     
    }
    public class TrendingPeopleModel : ViewModelBase
    {
        int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        string _userImage;
        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        string _userAccuracy;
        public string UserAccuracy
        {
            get => _userAccuracy;
            set => SetProperty(ref _userAccuracy, value);
        }
        SportType _userSportType;
        public SportType  UserSportType
        {
            get => _userSportType;
            set => SetProperty(ref _userSportType, value);
        }
        [JsonIgnore]
        public string UserSportImage
        {
            get
            {
                return TailUtils.GameTypeToImage_Follow(UserSportType);
            }
        }

        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }
        string _baseballAccuracy;
        public string BaseballAccuracy
        {
            get => _baseballAccuracy;
            set => SetProperty(ref _baseballAccuracy, value);
        }
        string _basketballAccuracy;
        public string BasketballAccuracy
        {
            get => _basketballAccuracy;
            set => SetProperty(ref _basketballAccuracy, value);
        }

        string _footballAccuracy;
        public string FootballAccuracy
        {
            get => _footballAccuracy;
            set => SetProperty(ref _footballAccuracy, value);
        }
        string _hockyAccuracy;
        public string HockyAccuracy
        {
            get => _hockyAccuracy;
            set => SetProperty(ref _hockyAccuracy, value);
        }
        string _mmaAccuracy;
        public string MmaAccuracy
        {
            get => _mmaAccuracy;
            set => SetProperty(ref _mmaAccuracy, value);
        }
        bool _isFollow=false;
        public bool IsFollow
        {
            get => _isFollow;
            set => SetProperty(ref _isFollow, value);
        }
        TrendingType _trendType;
        public TrendingType TrendType
        {
            get => _trendType;
            set => SetProperty(ref _trendType, value);
        }
    }
    public class TrendArgument : ViewModelBase
    {
        TrendingType _trendType;
        public TrendingType TrendType
        {
            get => _trendType;
            set => SetProperty(ref _trendType, value);
        }
        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
    }


}
