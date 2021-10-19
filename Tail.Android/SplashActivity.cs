using System.Threading.Tasks;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Support.V7.App;
using Android.Content.PM;
using Tail.Services.ApplicationServices;

namespace Tail.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Immersive = true, WindowSoftInputMode = SoftInput.AdjustResize, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : AppCompatActivity
	{
        
        public static Activity Instance { get; set; }

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
            base.OnCreate(savedInstanceState, persistentState);

            Instance = this;
            SetContentView(Resource.Layout.SplashLayout);
        }

        protected override void OnResume()
        {
            base.OnResume();

            SetContentView(Resource.Layout.SplashLayout);

            Task.Run(async () =>
            {
                await ShowSplash();
            });
        }



        async Task ShowSplash()
        {
            CommonSingletonUtility.SharedInstance.AppForegroundStatus = true;
            await Task.Delay(700);

            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            if (Intent.Extras != null)
            {
                Bundle bundle = Intent.Extras;
                mainActivityIntent.PutExtra("customdata", bundle);
            }
           
            mainActivityIntent.AddFlags(ActivityFlags.NoAnimation);
           // mainActivityIntent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(mainActivityIntent);
        }



    }
}
