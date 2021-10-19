using System;
using Android.App;
using Firebase.Iid;
using Tail.Services.Helper;

namespace Tail.Droid.FCM
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]

    public class FirebaseInstanceIDService : FirebaseInstanceIdService
    {
        const string TAG = "TailFirebaseIIDService";

        // [START refresh_token]
  
        public override void OnTokenRefresh()
        {
            // Get updated InstanceID token.
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Android.Util.Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            System.Diagnostics.Debug.WriteLine($"######Token######  :  {refreshedToken}");
            System.Diagnostics.Debug.WriteLine("Refreshed token: " + FirebaseInstanceId.Instance.Token);
            TailUtils.SetpushToken(refreshedToken);

        }
        // [END refresh_token] 
    }
}
