using System;
using Android.Content;
using Android.Content.PM;
using Tail.Services.Interfaces;
using Xamarin.Forms;
using Tail.Droid.Helpers;

[assembly: Dependency(typeof(AppRatiing))]
namespace Tail.Droid.Helpers
{
    public class AppRatiing : IAppRating
    {
        public void RateApp()
        {
            var activity = Android.App.Application.Context;
            var url = $"market://details?id={(activity)?.PackageName}";
            if (activity != null)
            {
                try
                {
                    activity.PackageManager.GetPackageInfo("com.android.vending", PackageInfoFlags.Activities);


                    Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                    activity.StartActivity(intent);
                }
                catch (PackageManager.NameNotFoundException ex)
                {
                    // this won't happen. But catching just in case the user has downloaded the app without having Google Play installed.

                    Console.WriteLine(ex.Message);
                }
                catch (ActivityNotFoundException)
                {
                    // if Google Play fails to load, open the App link on the browser 

                    string playStoreUrl = string.Format(Tail.Common.Constants.PlayStore_URL, Tail.Common.Constants.PlayStoreId);

                    var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(playStoreUrl));
                    browserIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                    activity.StartActivity(browserIntent);
                }
            }
        }
    }
}

