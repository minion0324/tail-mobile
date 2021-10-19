
namespace Tail.Common
{
    public static class Constants
    {


        public const string BaseUrl = "{Enter Base Url here}";
        public const string S3Path = "{Enter S3 Url here}";
        public const string S3BucketForProfileImage = "{Enter S3 Bucket Path For ProfileImage here}";
        public const string S3BucketForPostImage = "{Enter S3 Bucket Path For PostImage here}";
        public const string S3BucketForPostVideo = "{Enter S3 Bucket Path For PostVideo here}";
        public const string AWSCredential = "{Enter AWS Pool ID here}";
        public const string ApiKey = "{Enter API Key here}";
        public const string ApiKeyValue = "{Enter API key Value here}";


        public const string BaseIpAddress = BaseUrl;
        public const string FAQ_URL = BaseUrl+ "/web/faq";
        public const string TERMS_AND_CONDITION_URL = BaseUrl + "/web/termsCondition";
        public const string ABOUT_US_URL = BaseUrl + "/web/aboutUs";
        public const string TEAM_LOGO_PATH = S3Path;
        public const string ProfileImageUrl = S3Path + "/users/";
        public const string PostImageUrl = S3Path + "/posts/images/";
        public const string PostVideoUrl = S3Path + "/posts/videos/";
        public const string ENCRPTION_KEY = ApiKeyValue;
        public const string AuthorizationKey = "Authorization";
        public const string SessionKey = "SessionKey";
        public const int IPHONEX_NOTCH_HEIGHT = 20;
        public const string LANGUAGE_CHANGE_MESSAGE_CONTRACT = "AppLanguageChanged";
        public const string APPLANGUAGE_ENGLISH_CULTURE = "en-US";
        public const string CHECKBOX_DEFAULT = "checkbox";
        public const string CHECKBOX_SELECTED = "checkbox_selected";
        public const string Like_DEFAULT = "like";
        public const string Like_SELECTED = "like_selected";
        public const string DisLike_DEFAULT = "dislike";
        public const string DisLike_SELECTED = "dislike_selected";
        public const string LOGGEDIN_USERNAME = "John Doe";
        public const string LOGGEDIN_USERIMAGE = "placeholder.png";
        public const int LOGGEDIN_USERID = 1;
        public const string IOS_TYPE = "1";
        public const string ANDROID_TYPE = "2";
        public const string DEFAULT_ADD_PROFILE_IMAGE = "add_profile_pic";
        public const short IMAGE_UPLOAD_BATCH_COUNT = 3;
        public const short REFRESH_TOKEN_ERROR = 407;
        public const short INVALID_LOGIN_ERROR = 406;
        public const short API_KEY_ERROR = 403;
        public const short PAGINATION_COUNT = 20;
        public const string DEFAULT_USERIMAGE = "placeholder.png";
        public const string PostDirectoryName = "TailFeedPost";
        public const string MediaDirectoryName = "temp";
        public const string SavedCameraImageName = "Tail.jpg";
        public const float PostThumbImageWidth = 686f;
        public const float PostThumbImageHeight = 650f;
        public const float UserThumbImageWidth = 100f;
        public const float UserThumbImageHeight = 100f;
        public const string PRIVACY_POLICY_URL = BaseUrl + "/web/privacyPolicy";
        public const int CurrentAppVersionNumber = 0;
        public const string AppStore_URL = "itms-apps://itunes.apple.com/app/{0}{1}";
        public const string AppStoreId = "id1534173808";
        public const string PlayStore_URL = "https://play.google.com/store/apps/details?id={0}";
        public const string PlayStoreId = "com.tailnetwork.tailapp";
        public const string Help_FAQ_Url = BaseUrl + "/web/help";
        public const string NotificationMessage = "NotificationMessage";
        public const string AppstoreLink = "https://itunes.apple.com/in/app/id1534173808";
        public const string PlayStoreLink = "https://play.google.com/store/apps/details?id=com.tailnetwork.tailapp";
        
    }
}
