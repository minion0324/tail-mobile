using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class Menu : AppPageBase
    {
        readonly MenuViewModel _vModel;
        public Menu()
        {
            _vModel = new MenuViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () => await ShowAppExitMessage());
               

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }

            return true;
        }
        public async Task ShowAppExitMessage()
        {
            var res = await AppNavigationService.GetInstance().ShowConfirmAlertAsync(AppResources.AppName, AppResources.AppExitMessage);
            if (res)
            {
                await Task.Delay(500);
                DependencyService.Get<IDeviceHelper>().QuitApp();
            }
        }
    }
}
