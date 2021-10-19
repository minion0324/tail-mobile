using System;
using Foundation;
using StoreKit;
using Tail.iOS.Services;
using Tail.Services.Interfaces;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(AppRating))]
namespace Tail.iOS.Services
{
    public class AppRating : IAppRating
    {
        public void RateApp()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                SKStoreReviewController.RequestReview();
            else
            {
                string storeUrl = Tail.Common.Constants.AppStore_URL+ Tail.Common.Constants.AppStoreId+ "?action=write-review";
             

                try
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(storeUrl));
                }
                catch (Exception ex)
                {
                    // Here you could show an alert to the user telling that App Store was unable to launch

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
