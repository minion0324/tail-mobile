using System;
using Foundation;
using Rg.Plugins.Popup;
using Tail.iOS.DataHelpers;
using UIKit;
using Tail.Services.ApplicationServices;
using LabelHtml.Forms.Plugin.iOS;
using ProgressRingControl.Forms.Plugin.iOS;
using FFImageLoading;
using FFImageLoading.Config;
using System.Net.Http;
using UserNotifications;
using Firebase.CloudMessaging;
using Tail.Services.Helper;
using Xamarin.Forms;
using Tail.Views;
using Tail.Models;
using Tail.ViewModels;
using System.Diagnostics;
using Tail.Services.ServiceProviders;
using System.Threading.Tasks;

namespace Tail.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {

        private const string FacebookAppId = "2682417875356647";
        private const string FacebookAppName = "Tail Network";
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = new HttpClient(new NSUrlSessionHandler()
                {

                })
            });
            Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            Facebook.CoreKit.ApplicationDelegate.SharedInstance.FinishedLaunching(uiApplication, launchOptions);
            HtmlLabelRenderer.Initialize();
            var multiMediaPickerService = new MultiMediaPickerService();
            CommonSingletonUtility.SharedInstance.MultiMediaPicker = multiMediaPickerService;
            Facebook.CoreKit.Settings.AppId = FacebookAppId;
            Facebook.CoreKit.Settings.DisplayName = FacebookAppName;
            RegisterForRemoteNotifications();
            if (Messaging.SharedInstance != null)
                Messaging.SharedInstance.Delegate = this;

            Firebase.Core.App.Configure();
            LoadApplication(new App());
            ProgressRingRenderer.Init();

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {

                var token = Messaging.SharedInstance.FcmToken;
                TailUtils.SetpushToken(token ?? "");
                // if you want to send notification per user, use this token
                System.Diagnostics.Debug.WriteLine(token);
            });

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
        private void RegisterForRemoteNotifications()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>
                {
                    Console.WriteLine(granted);
                    await System.Threading.Tasks.Task.Delay(500);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            if (Messaging.SharedInstance != null)
                Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = SettingsService.Instance.NotificationCount;
        }
        public override void OnActivated(UIApplication uiApplication)
        {


            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications(); // To remove all delivered notifications
            }
            else
            {
                UIApplication.SharedApplication.CancelAllLocalNotifications();
            }
            base.OnActivated(uiApplication);
            // We need to properly handle activation of the application with regards to SSO
            // (e.g., returning from iOS 6.0 authorization dialog or from fast app switching).

        }
        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications(); // To remove all delivered notifications
            }
            else
            {
                UIApplication.SharedApplication.CancelAllLocalNotifications();
            }
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
            TailUtils.SetpushToken(Messaging.SharedInstance.FcmToken ?? "");
            SettingsService.Instance.DeviceToken = Messaging.SharedInstance.FcmToken;

        }
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Debug.WriteLine("Failed To Register For RemoteNotifications");
        }
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);
            NSDictionary dataDic = (userInfo["aps"] as NSDictionary);

            if (userInfo.ObjectForKey(new NSString("isLogout")) != null && App.Current != null)
            {
                SessionOut();
            }
            else
            {
                if (dataDic != null && dataDic.ObjectForKey(new NSString("badge")) != null)
                {
                    SetBadge(dataDic);
                }
                string postId = string.Empty;
                string userId = string.Empty;
                string postType = string.Empty;
                string notificationId = string.Empty;

                if (userInfo.ObjectForKey(new NSString("userId")) != null)
                {
                    userId = userInfo.ValueForKey(new NSString("userId")) as NSString;
                    Debug.WriteLine("UserId" + userId);

                }
                if (userInfo.ObjectForKey(new NSString("postId")) != null)
                {
                    postId = userInfo.ValueForKey(new NSString("postId")) as NSString;
                    Debug.WriteLine("postId" + postId);

                }
                if (userInfo.ObjectForKey(new NSString("pType")) != null)
                {
                    postType = userInfo.ValueForKey(new NSString("pType")) as NSString;
                    Debug.WriteLine("postType" + postType);
                }
                if (userInfo.ObjectForKey(new NSString("notifyId")) != null)
                {
                    notificationId = userInfo.ValueForKey(new NSString("notifyId")) as NSString;
                    Debug.WriteLine("notificationId" + notificationId);
                }
                 ShowNotificationPage(postId, postType, userId, notificationId);
              

            }




        }
        private void SetBadge(NSDictionary dataDic)
        {
            var badge = dataDic.ValueForKey(new NSString("badge")).ToString();
            int badgeInt = 0;
            if (int.TryParse(badge, out badgeInt))
            {
                SettingsService.Instance.NotificationCount = badgeInt;
            }
            MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
        }

        private void SessionOut()
        {
            PageViewModelBase pageViewModelBase = null;
            Xamarin.Forms.TabbedPage appPageBase = null;
            if (App.Current.MainPage is NavigationPage navigationPage)
            {
                appPageBase = navigationPage.CurrentPage as Xamarin.Forms.TabbedPage;

                if (appPageBase != null)
                {
                    pageViewModelBase = appPageBase.BindingContext as PageViewModelBase;
                }
            }
            if (pageViewModelBase != null)
            {

                pageViewModelBase.SesionOut.Execute(null);

            }
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            UNUserNotificationCenter.Current.GetDeliveredNotificationsAsync();
            System.Diagnostics.Debug.WriteLine($"Firebase registration token: {fcmToken}");
            if (!string.IsNullOrWhiteSpace(fcmToken))
            {
                TailUtils.SetpushToken(fcmToken);
            }



        }
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            try
            {
                var apsData = notification.Request.Content.UserInfo["aps"] as NSDictionary;
                var badge = apsData.ValueForKey(new NSString("badge")).ToString();
                int badgeInt = 0;
                if (int.TryParse(badge, out badgeInt))
                {
                    SettingsService.Instance.NotificationCount = badgeInt;
                    MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
                }
                completionHandler(UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.None);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private async void ShowNotificationPage(string postId, string postType, string userId, string notificationId)
        {

            PostDetails postItem = new PostDetails { PostId = postId, PType = Convert.ToInt32(postType) };

            PageViewModelBase pageViewModelBase = null;
            Xamarin.Forms.TabbedPage appPageBase = null;
            if (App.Current.MainPage is NavigationPage navigationPage)
            {
                appPageBase = navigationPage.CurrentPage as Xamarin.Forms.TabbedPage;
                if (appPageBase != null)
                {
                    pageViewModelBase = appPageBase.BindingContext as PageViewModelBase;
                }
            }

            if (appPageBase != null && pageViewModelBase != null)
            {
                await TailDataServiceProvider.Instance.ReadNotification(new ReadNotificationInfo { notifyId = notificationId });
                if (SettingsService.Instance.NotificationCount > 0)
                    SettingsService.Instance.NotificationCount = SettingsService.Instance.NotificationCount - 1;
                MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);

                await PageNavigation(pageViewModelBase, postItem, userId);
            }
            else
            {

                Page currentPage = new Views.Notification();
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(currentPage);
            }
        }
        private async Task PageNavigation(PageViewModelBase pageViewModelBase, PostDetails postItem, string userId)
        {
            if (postItem.PostId != null && postItem.PType > 0)
            {
                PostEventsArgs paramObj = new PostEventsArgs()
                {
                    serviceRequired = true,
                    postObj = postItem
                };
                await pageViewModelBase.NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);
            }
            else
            {
                await pageViewModelBase.Handle_UserDetailsCommand(Convert.ToInt32(userId));
            }
        }
    }
}
