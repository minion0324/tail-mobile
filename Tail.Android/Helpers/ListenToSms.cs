using System.Diagnostics;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Tasks;
using Tail.Droid.Helpers;
using Tail.Services.Helper;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(ListenToSms))]
namespace Tail.Droid.Helpers
{
    public class ListenToSms : IListenToSmsRetriever
    {
        public void ListenToSmsRetriever()
        {

            SmsRetrieverClient client = SmsRetriever.GetClient(Application.Context);
            var task = client.StartSmsRetriever();
            task.AddOnSuccessListener(new SuccessListener());
            task.AddOnFailureListener(new FailureListener());
        }
        private class SuccessListener : Java.Lang.Object, IOnSuccessListener
        {
            

            public void OnSuccess(Java.Lang.Object result)
            {
                Debug.WriteLine("OnSuccess");
            }

            
        }
        private class FailureListener : Java.Lang.Object, IOnFailureListener
        {
           

            public void OnFailure(Java.Lang.Exception e)
            {
                Debug.WriteLine("OnFailure");
            }
        }
    }
}
