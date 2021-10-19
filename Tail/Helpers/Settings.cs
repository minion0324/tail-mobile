using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Tail.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string FacebookTokenKey = "facebookTokenKey";
        private const string FCMTokenKey = "fcmToken_key";

        #endregion


        public static string FaceBookToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(FacebookTokenKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FacebookTokenKey, value);
            }
        }
        public static string FCMToken
        {
            get => AppSettings.GetValueOrDefault(FCMTokenKey, string.Empty);
            set => AppSettings.AddOrUpdateValue(FCMTokenKey, value);
        }
    }
}


