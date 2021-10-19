using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Tail.Models;
using Newtonsoft.Json;


namespace Tail.Services.ApplicationServices
{
    public class SettingsService
    {
        static SettingsService _instance;
        public static SettingsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsService();
                }

                return _instance;
            }
        }

        #region Setting Constants

      
        const string kAuthenticatedKey = "authenticated";
        const string kAppLanguage = "AppLanguageKey";
        const string kCurrentTabIndexKey = "currentTabIndex";
        const string kIsOtherUserProfileKey = "otherUserProfile";
        private const string kLoggedUser = "LoggedUserKey";
        const string kFacebookAccessKey = "FacebookAccessKey";
        const string kDeviceToken = "DeviceTokenKey";
        const string kIsSavedCredentials = "IsSavedCredentialsKey";
        const string kPartialRegstarionUserID = "PartialRegstarionUserIDKey";

        const string kFromEditProfile = "FromEditProfile";
        const string kIsCompletedSignUpProcess = "IsCompletedSignUpProcessKey";
        const string kFromSettings= "FromSettings";
        const string kHasPushMessages = "HasPushMessageskey";
        const string kNotificationCount= "NotificationCount";
        #endregion

        #region Settings Properties


        public string FacebookAccessToken
        {
            get => GetValueOrDefault(kFacebookAccessKey, string.Empty);
            set => AddOrUpdateValue(kFacebookAccessKey, value);
        }


        public bool Authenticated
        {
            get => GetValueOrDefault(kAuthenticatedKey, false);
            set => AddOrUpdateValue(kAuthenticatedKey, value);
        }
        public string AppLanguage
        {
            get => GetValueOrDefault(kAppLanguage, "en-US");
            set => AddOrUpdateValue(kAppLanguage, value);
        }
     
        public int CurrentTabIndex
        {
            get => GetValueOrDefault(kCurrentTabIndexKey, 0);
            set => AddOrUpdateValue(kCurrentTabIndexKey, value);
        }
        public bool IsOtherUserProfile
        {
            get => GetValueOrDefault(kIsOtherUserProfileKey, false);
            set => AddOrUpdateValue(kIsOtherUserProfileKey, value);
        }
        public bool HasPushMessages
        {
            get => GetValueOrDefault(kHasPushMessages, false);
            set => AddOrUpdateValue(kHasPushMessages, value);
        }
        public int NotificationCount
        {
            get => GetValueOrDefault(kNotificationCount, 0);
            set => AddOrUpdateValue(kNotificationCount, value);
        }
        public UserInfo LoggedUserDetails
        {
            get
            {
                string value = Instance.GetValueOrDefault(kLoggedUser, string.Empty);
                UserInfo userData;
                if (string.IsNullOrEmpty(value))
                    userData = new UserInfo();
                else
                    userData = JsonConvert.DeserializeObject<UserInfo>(value);
                return userData;
            }
            set
            {
                if (value != null)
                {
                    string userData = JsonConvert.SerializeObject(value);
                    Instance.AddOrUpdateValue(kLoggedUser, userData);
                }
                else
                {
                    Instance.AddOrUpdateValue(kLoggedUser, null);
                }


            }
        }
        public string DeviceToken
        {
            get => GetValueOrDefault(kDeviceToken, string.Empty);
            set => AddOrUpdateValue(kDeviceToken, value);
        }
        public bool IsSavedCredentials
        {
            get => GetValueOrDefault(kIsSavedCredentials, false);
            set => AddOrUpdateValue(kIsSavedCredentials, value);
        }
        public int PartialRegstarionUserID
        {
            get => GetValueOrDefault(kPartialRegstarionUserID, 0);
            set => AddOrUpdateValue(kPartialRegstarionUserID, value);
        }
        public bool FromEditProfile
        {
            get => GetValueOrDefault(kFromEditProfile, false);
            set => AddOrUpdateValue(kFromEditProfile, value);
        }
        public bool FromSettings
        {
            get => GetValueOrDefault(kFromSettings, false);
            set => AddOrUpdateValue(kFromSettings, value);
        }
        public bool IsCompletedSignUpProcess
        {
            get => GetValueOrDefault(kIsCompletedSignUpProcess, true);
            set => AddOrUpdateValue(kIsCompletedSignUpProcess, value);
        }
        #endregion

        #region Public Methods

        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public int GetValueOrDefault(string key, int defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public double GetValueOrDefault(string key, double defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);        
        public Task AddOrUpdateValue(string key, int value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, double value) => AddOrUpdateValueInternal(key, value);
    
        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;

            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;

            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }

            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        #endregion
    }
}
