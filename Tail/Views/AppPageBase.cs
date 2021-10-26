using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
namespace Tail.Views
{
    public class AppPageBase : ContentPage
    {
        public static bool IsAndroid => Device.RuntimePlatform == Device.Android;

        public AppPageBase()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            BackgroundColor = Color.White;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(false);
        }

       

        protected void AdjustNotchArea(Grid contentGrid)
        {
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    contentGrid.Margin = new Thickness(0, Constants.IPHONEX_NOTCH_HEIGHT, 0, 0);
                }
            }
        }

        protected void AdjustNotchArea(StackLayout contentStack)
        {
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    contentStack.Margin = new Thickness(0, Constants.IPHONEX_NOTCH_HEIGHT, 0, 0);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PageViewModelBase pageViewModel)
            {
                pageViewModel.OnPageAppearing();
            }
        }

        /// <summary>
        /// This method is called when page is removed from stack
        /// </summary>
        public virtual void OnPageDestroy()
        {
            if (BindingContext is PageViewModelBase pageViewModel)
            {
                pageViewModel.OnPageDestroy();
            }
            
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is PageViewModelBase pageViewModel)
            {
                pageViewModel.OnPageDisappearing();
            }

            base.OnDisappearing();
        }

        public async Task<string> ShowActionSheet(string title, string cancel, string destructions, params string[] buttons)
        {
            return await DisplayActionSheet(title, cancel, destructions, buttons);
        }
      
        protected override bool OnBackButtonPressed()
        {
            try
            {
                return base.OnBackButtonPressed();
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }

            return false;
        }
    }
}
