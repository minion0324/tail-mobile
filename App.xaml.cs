using System.Collections.ObjectModel;
using ITRApp.Common;
using ITRApp.Core.Common;
using ITRApp.Core.Helpers;
using ITRApp.Core.Interfaces;
using ITRApp.Core.Models;
using ITRApp.Core.ResourceFiles;
using ITRApp.Core.Services;
using ITRApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ITRApp
{
    public partial class App : Application
    {
        private readonly IITRAppServices _iTRService;
        public ObservableCollection<PresentationDetails> PresentationList
        {
            get; set;
        }
        public App()
        {
            _iTRService = new ITRAppServices();
            Localization.SetLocale();
            AppResources.Culture = DependencyService.Get<ILocale>().GetCurrent();
            Styles.LoadStyles();

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //CrossGeofences.Current.RegionStatusChanged += (sender, args) =>
            //{
            //    var region = args.Region;
            //    var status = args.Status;
            //    var date = (DateTime.Now.Date.Month.ToString("00") + "-" + DateTime.Now.Date.Day.ToString("00") + "-" + DateTime.Now.Date.Year.ToString());
            //    var time = Convert.ToDateTime(DateTime.Now.TimeOfDay.ToString()).ToString("HH:mm");
            //    var dateTime = date + " " + time;
            //    var selectedDateTime = DateTime.ParseExact(dateTime, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
            //    DependencyService.Get<ILocalNotificationService>().Cancel(0);
            //    if (args.Status == GeofenceStatus.Entered)
            //    {
            //        DependencyService.Get<ILocalNotificationService>().LocalNotification(AppResources.AppTitle, AppResources.EnterPresentation, 1, selectedDateTime);
            //    }
            //  if (args.Status == GeofenceStatus.Exited)
            //    {
            //        DependencyService.Get<ILocalNotificationService>().LocalNotification(AppResources.AppTitle, AppResources.ExitPresentation, 2, selectedDateTime);
            //        CrossGeofences.Current.StopMonitoring(args.Region);
            //    }

            //};



            if (Device.RuntimePlatform == Device.Android)
            {
                if (ITRAppSettings.IsInitialLaunch)
                {
                    ITRAppSettings.IsInitialLaunch = false;
                    MainPage = new LocationAccessPage();
                }
                else
                {


                    if (ITRAppSettings.AccessToken != null && ITRAppSettings.AccessToken.Length > 0 && ITRAppSettings.KeepMeLoggedIn)
                    {
                        //Task.Run(async () => { await GetPresentationsAndSetUpGeoFencing(); });

                        MainPage = new NavigationPage(new ITRMasterDetailPage());

                    }

                    else
                        MainPage = new NavigationPage(new PreLogin());
                }
            }
            else
            {
                MainPage = new SplashAnimationPage();
            }
            
           
            //if (ITRAppSettings.AccessToken != null && ITRAppSettings.AccessToken.Length > 0 && ITRAppSettings.KeepMeLoggedIn)
            //{
            //    //Task.Run(async () => { await GetPresentationsAndSetUpGeoFencing(); });

           

            //}

            //else
            //    MainPage = new NavigationPage(new PreLogin());
        }


        //async Task GetPresentationsAndSetUpGeoFencing()
        // {

        //     GetPresentationResponse result = await _iTRService.GetAllPresentations();
        //     if (result.Success)
        //     {
        //         if (result.PresentationList != null && result.PresentationList.Count > 0)
        //         {

        //             PresentationList = new ObservableCollection<PresentationDetails>(result.PresentationList.Where(u => DateTime.Parse(u.EventDate).Date >= DateTime.Now.Date));
        //             foreach (var presentationdtls in PresentationList)
        //             {
        //                 DateTime eventDt = DateTime.Parse(presentationdtls.EventDate);
        //                 if(eventDt.Date== DateTime.Now.Date && presentationdtls.Address != null)
        //                 CrossGeofences.Current.StartMonitoring(new GeofenceRegion (presentationdtls.Name, new Position (Double.Parse(presentationdtls.Address.Latitude), Double.Parse(presentationdtls.Address.Longitude)), Distance.FromMeters(800) ));

        //             }

        //         }
        //     }

        // }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

