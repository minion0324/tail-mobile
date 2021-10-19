using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Tail.Services.ApplicationServices;
using Tail.Droid.DataHelpers;
using Android.Content;
using LabelHtml.Forms.Plugin.Droid;
using Plugin.Permissions;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Facebook;
using Xamarin.Forms;
using Android.Gms.Common;
using Firebase;
using Tail.Droid.FCM;
using Firebase.Iid;
using Firebase.Messaging;
using Tail.Services.Helper;
using FFImageLoading;
using Xamarin.Android.Net;
using System.Net;
using FFImageLoading.Config;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using TabbedPage = Xamarin.Forms.TabbedPage;
using Tail.Models;
using Java.IO;

namespace Tail.Droid
{
    [Activity(Label = "Tail", Icon = "@drawable/appicon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = false, Exported = true, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        internal static readonly string CHANNEL_ID = "tail_notification_channel";
        internal static readonly string CHANNEL_NAME = "Tail_FCM_Notification";
        internal static readonly int NOTIFICATION_ID = 100;
        public static ICallbackManager CallbackManager { get; set; } = CallbackManagerFactory.Create();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = new HttpClient(new AndroidClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                })
            });
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            HtmlLabelRenderer.Initialize();
            var metrics = Resources.DisplayMetrics;
            CommonSingletonUtility.SharedInstance.DeviceWidth = ConvertPixelsToDp(metrics.WidthPixels, metrics);
            CommonSingletonUtility.SharedInstance.DeviceHeight = ConvertPixelsToDp(metrics.HeightPixels, metrics);
            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CommonSingletonUtility.SharedInstance.MultiMediaPicker = MultiMediaPickerService.SharedInstance;
            InitOrientationChangeEventSubscription();
            FacebookSdk.SdkInitialize(ApplicationContext);
            CrossCurrentActivity.Current.Activity = this;

            //Start Push Notification

            FirebaseApp.InitializeApp(this);
            if (IsPlayServicesAvailable())
            {
                var backIntent = new Intent(this, typeof(FirebaseInstanceIDService));
                StartService(backIntent);
                var foregroundIntent = new Intent(this, typeof(TailFirebaseMessagingService));
                StartService(foregroundIntent);
                FirebaseMessaging.Instance.SubscribeToTopic("news");
            }

            CreateNotificationChannel();
            PushStructure PushData = new PushStructure();
            if (Intent.Extras != null)
            {
                Bundle passedBundle = Intent.GetBundleExtra("customdata");
                Dictionary<string, object> dict = passedBundle.KeySet()
                                                .ToDictionary<string, string, object>(key => key, key => passedBundle.Get(key));
                if (dict != null && dict.Any() && dict.ContainsKey("userId"))
                {
                    try
                    {
                        PushData.PostId = dict["postId"].ToString();
                        PushData.UserId = int.Parse(dict["userId"].ToString());
                        PushData.Badge = int.Parse(dict["badge"].ToString());
                        PushData.Ptype = int.Parse(dict["pType"].ToString());
                        PushData.IsSessionOut = false;
                        PushData.NotificationId= dict["notifyId"].ToString();
                        SettingsService.Instance.NotificationCount = PushData.Badge;
                        MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Push Structure not common caught successfully and proccessing:" + ex.Message);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(FirebaseInstanceId.Instance.Token))
            {
                TailUtils.SetpushToken(FirebaseInstanceId.Instance.Token);
            }
            //End Push Notification 

            if (Intent != null)
            { 
                if (PushData.UserId!=0)
                {
                  
                    LoadApplication(new App(PushData));
                }
                else
                {
                    LoadApplication(new App());
                }
            }
            else
            {
                LoadApplication(new App());
            }

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

         

        }

        public bool IsPlayServicesAvailable()
        {  
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this); if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //the google play service is not available and result code is User readable
                }
                else
                {
                    //This device is not supported           
                    Finish(); // Kill the activity if you want.         
                }
                return false;
            }
            else
            {
                //Google Play Services is available.         
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,CHANNEL_NAME , NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            System.Diagnostics.Debug.WriteLine("Firebase Cloud Messages appear in this channel");
        }
       
        public static float ConvertPixelsToDp(float px, Android.Util.DisplayMetrics matrix)
        {
            return px / matrix.Density;
        }
        private void InitOrientationChangeEventSubscription()
        {
            MessagingCenter.Subscribe<object>(this, "PlayInFullScreen", (val) =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
            MessagingCenter.Subscribe<object>(this, "FullScreenEnded", (val) =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            MultiMediaPickerService.SharedInstance.OnActivityResult(requestCode, resultCode, data);
        }
        protected override void OnResume()
        {
            //if (new DeviceUtils().isDeviceRooted(ApplicationContext))
            //{
                
            //}

            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
            CommonSingletonUtility.SharedInstance.AppForegroundStatus = false;
        }

    
    }
}