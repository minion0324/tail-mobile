using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class Home_Feeds : AppPageBase
    {
         HomeFeedViewModel _vModel;


        public Home_Feeds()
        {
            InitializeComponent();
            _vModel = new HomeFeedViewModel();
            BindingContext = _vModel;

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is HomeFeedViewModel)
            {
                _vModel = BindingContext as HomeFeedViewModel;

                _vModel.ListMoveToTop += () =>
                {
                    PageScroll.ScrollToAsync(TopIndicator, ScrollToPosition.Start, false);

                };
                _vModel.ListMoveDown += () =>
                {
                     PageScroll.ScrollToAsync(0, 10, false);
                };
            }
        }
       

        async void ScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            if (!(sender is ScrollView scrollView))
                return;

            var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;
            double _yValue = 0;
            if (Device.RuntimePlatform == Device.iOS)
                _yValue = -50;
            if (e.ScrollY <= _yValue)
            {
                if (_vModel.ScrollStart)
                    return;
                _vModel.ScrollStart = true;
                _vModel.RefreshPostCommand.Execute(null);
                
            }
            if (scrollingSpace > e.ScrollY)
                return;
            if (_vModel.ScrollEnd)
                return;
            _vModel.ScrollEnd = true;
            _vModel.CurrentPage += 1;
            if (!_vModel.ScrollEndReached)
            {
                _vModel.BottomIndicator = true;
                await _vModel.GetPostDetails(_vModel.CurrentPage);
            }

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
