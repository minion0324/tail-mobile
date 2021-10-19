using System;
using System.Diagnostics;
using Firebase.Iid;
using Tail.Common;
using Tail.Droid.DataHelpers;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceHelper))]
namespace Tail.Droid.DataHelpers
{
    public class DeviceHelper : IDeviceHelper
    {
        public float DeviceHeight
        {
            get => (float)(0);
        }
        public string GetDeviceId()
        {
           return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);           
        }

        public DeviceModel GetDeviceModel()
        {
            throw new System.NotSupportedException();
        }

        public double GetDeviceHight()
        {
            throw new System.NotSupportedException();
        }
        public void QuitApp()
        {
            try
            {
                if (SplashActivity.Instance != null)
                    SplashActivity.Instance.FinishAffinity();

                if (Forms.Context != null)
                {
                    var context = Forms.Context as MainActivity;
                    if (context != null)
                        context.FinishAffinity();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in QuitApp:" + ex.Message);
            }
        }
        public void UpdateToken()
        {
            if (!string.IsNullOrWhiteSpace(FirebaseInstanceId.Instance.Token))
            {
                TailUtils.SetpushToken(FirebaseInstanceId.Instance.Token);
            }
        }
        }
    }
