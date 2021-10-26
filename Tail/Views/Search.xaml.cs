using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class Search : AppPageBase
    {
        readonly SearchViewModel _vModel;
        public Search()
        {
            InitializeComponent();
           _vModel = new SearchViewModel();
            BindingContext = _vModel;
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

        void ScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            if (!(sender is ScrollView scrollView) || _vModel.IsResult)
                return;
            double _yValue = 0;
            if (Device.RuntimePlatform == Device.iOS)
                _yValue = -50;
            if (e.ScrollY <= _yValue)
            {
                if (_vModel.ScrollStart)
                    return;
                _vModel.ScrollStart = true;
                
                Task.Run(async () => await _vModel.GetTrendPickList());
                

            }
          
        }
    }
}
