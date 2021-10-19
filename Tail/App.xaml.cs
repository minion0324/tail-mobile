using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;
namespace Tail
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SettingsService.Instance.DeviceToken = DependencyService.Get<IDeviceHelper>().GetDeviceId();
            SettingsService.Instance.AppLanguage = Constants.APPLANGUAGE_ENGLISH_CULTURE;
            AppResources.Culture = new CultureInfo(SettingsService.Instance.AppLanguage);
            StyleLoader.LoadStyles();
            ConverterLoader.LoadConverters();
            AppNavigationService.GetInstance().InitializeAsync();
            if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
            CommonSingletonUtility.SharedInstance.AppForegroundStatus = true;
            Task.Run(async () => await GetAppversionAsync());
        }
        public App(PushStructure PushData)
        {
            InitializeComponent();
            SettingsService.Instance.DeviceToken = DependencyService.Get<IDeviceHelper>().GetDeviceId();
            SettingsService.Instance.AppLanguage = Constants.APPLANGUAGE_ENGLISH_CULTURE;
            AppResources.Culture = new CultureInfo(SettingsService.Instance.AppLanguage);
            StyleLoader.LoadStyles();
            ConverterLoader.LoadConverters();
            AppNavigationService.GetInstance().InitializeAsync();
            CommonSingletonUtility.SharedInstance.AppForegroundStatus = true;
            if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
            if (PushData != null && PushData.UserId != 0)
            {
                if (SettingsService.Instance.NotificationCount > 0)
                    SettingsService.Instance.NotificationCount = SettingsService.Instance.NotificationCount - 1;
                MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
                Task.Run(async () => await TailDataServiceProvider.Instance.ReadNotification(new ReadNotificationInfo { notifyId = PushData.NotificationId }));

            
                if (PushData.Ptype > 0)
                {
                    PostEventsArgs paramObj = new PostEventsArgs()
                    {
                        serviceRequired = true,
                        postObj = new PostDetails()
                        {
                            PostId = PushData.PostId,
                            PType = PushData.Ptype
                        }
                    };
                    AppNavigationService.GetInstance().NavigateWithInTabToAsync<Home_Comments>(paramObj);
                }
                else if (PushData.Ptype == 0)
                {
                    SettingsService.Instance.IsOtherUserProfile = true;
                    AppNavigationService.GetInstance().NavigateWithInTabToAsync<MyProfile>(PushData.UserId);
                }
            }

           
        }

        public static Action<string> PostSuccessFacebookAction { get; set; }

        protected override void OnResume()
        {
            base.OnResume();
            Task.Run(async () => await GetAppversionAsync());
            MessagingCenter.Send(this, "OnResumed");
            CommonSingletonUtility.SharedInstance.AppForegroundStatus = true;
        }
        public async Task GetAppversionAsync()
        {
            var unreadResponse = await TailDataServiceProvider.Instance.GetUnreadCount();
            if (unreadResponse.ResponseData >= 0)
            {
                SettingsService.Instance.NotificationCount = unreadResponse.ResponseData;
                MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
            }

            var response = await TailDataServiceProvider.Instance.GetAppMinimumVersion();
            if (response.ResponseData > Constants.CurrentAppVersionNumber)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    int count = PopupNavigation.Instance.PopupStack.Count;
                    if (count > 0)
                    {
                        var popuppage = PopupNavigation.Instance.PopupStack.LastOrDefault().ToString();
                        if (popuppage != "Tail.Views.ForceUpdatePopup")
                        {
                            await PopupNavigation.Instance.PushAsync(new ForceUpdatePopup());
                        }
                    }
                    else
                        await PopupNavigation.Instance.PushAsync(new ForceUpdatePopup());
                });
            }

           


        }
    }
}
