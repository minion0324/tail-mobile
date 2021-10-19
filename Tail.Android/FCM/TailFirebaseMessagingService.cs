using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Firebase.Messaging;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;
using static Android.App.ActivityManager;

namespace Tail.Droid.FCM
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class TailFirebaseMessagingService : FirebaseMessagingService
    {
        public const string PRIMARY_CHANNEL = "Tail_FCM_Notification";
        // [START receive_message]
        public override void OnMessageReceived(RemoteMessage p0)
        {

            try
            {
                if (!SettingsService.Instance.Authenticated)
                    return;
                IDictionary<string, string> data = p0.Data;
                if (data != null && data.Count > 0 && data.ContainsKey("isLogout"))
                {
                    bool isLogout = Convert.ToBoolean(data["isLogout"]);
                    if (App.Current == null || !isLogout)
                        return;
                    SessionOut();
                    return;
                }
                else if (data != null && data.Count > 0 && data.ContainsKey("badge"))
                {
                    SettingsService.Instance.NotificationCount = Convert.ToInt32(data["badge"]);
                }

                MessagingCenter.Send(new NotificationInfo(), Tail.Common.Constants.NotificationMessage);
                if (!CommonSingletonUtility.SharedInstance.AppForegroundStatus)
                {
                    SendNotifications(p0);

                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Push Received Exception: " + ex.Message);
            }

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
                Device.BeginInvokeOnMainThread(() =>
                {
                    pageViewModelBase.SesionOut.Execute(null);
                });

            }
        }

        public void SendNotifications(RemoteMessage message)
        {
            try

            {

                var intent = new Intent(this, typeof(MainActivity)); intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("customParam", message.GetNotification().Body);
                intent.PutExtra("google.message_id", message.MessageId);
                intent.PutExtra("google.sent_time", message.SentTime);
                foreach (var key in message.Data.Keys)
                {
                    intent.PutExtra(key, message.Data[key]);
                }
                var fullScreenPendingIntent = PendingIntent.GetActivity(this, SettingsService.Instance.NotificationCount, intent, PendingIntentFlags.OneShot);
                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                NotificationCompat.Builder notification;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var chan1 = new NotificationChannel(PRIMARY_CHANNEL,
                     new Java.Lang.String("Primary"), NotificationImportance.High);
                    chan1.LightColor = Android.Graphics.Color.Green;
                    manager.CreateNotificationChannel(chan1);
                    notification = new NotificationCompat.Builder(this, PRIMARY_CHANNEL);
                }
                else
                {
                    notification = new NotificationCompat.Builder(this);

                }
                notification.SetContentIntent(fullScreenPendingIntent)
                         .SetContentTitle(message.GetNotification().Title)
                         .SetContentText(message.GetNotification().Body)
                         .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.notification_icon))
                         .SetSmallIcon(Resource.Drawable.notification_icon)

                         .SetStyle((new NotificationCompat.BigTextStyle()))
                         .SetPriority(NotificationCompat.PriorityHigh)

                         .SetAutoCancel(true);
                manager.Notify(SettingsService.Instance.NotificationCount, notification.Build());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Send Notification Exception: " + ex.Message);
            }
        }
    }
}
