using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class HomeViewModel : PageViewModelBase
    {
        public HomeViewModel()
        {
            try
            {
                MessagingCenter.Subscribe<App>((App)Application.Current, "PlusClick", async (sender) =>
                {
                    await showPopUp();
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            Task.Run(async () => await GetAppversionAsync());
        }
        public async Task GetAppversionAsync()
        {
            var unreadResponse = await TailDataServiceProvider.Instance.GetUnreadCount();
            if (unreadResponse.ResponseData >= 0)
            {
                SettingsService.Instance.NotificationCount = unreadResponse.ResponseData;
            }

            var response = await TailDataServiceProvider.Instance.GetAppMinimumVersion();
            if (response.ResponseData > Constants.CurrentAppVersionNumber)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await PopupNavigation.Instance.PushAsync(new ForceUpdatePopup());
                });
            }
          
        }

        public async Task showPopUp()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new MakeAPost(async () => await Handle_MakeApostPopUpClosed()));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        async Task Handle_MakeApostPopUpClosed()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
       
    }
}
