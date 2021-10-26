using System;
using Tail.ViewModels;
using Xamarin.Forms;
using System.Linq;
using Tail.Common;
using Tail.Services.ApplicationServices;

namespace Tail.Views
{
    public partial class PostYourPick : AppPageBase
    {
        PostYourPickViewModel _vModel;
        public PostYourPick()
        {
            try
            {
                InitializeComponent();
                _vModel = new PostYourPickViewModel();
                BindingContext = _vModel;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }
            
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is PostYourPickViewModel)
            {
                _vModel = BindingContext as PostYourPickViewModel;

                _vModel.OnPageSwitched += () =>
                {
                    var lastChild = OuterStck.Children.FirstOrDefault();
                    if (lastChild != null)
                        ContentScroll.ScrollToAsync(lastChild, ScrollToPosition.Start, false);

                };
            }
        }
        protected override bool OnBackButtonPressed()
        {
            try
            {
                var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
                SettingsService.Instance.CurrentTabIndex = 0;
                TabbedPage currentTabbedPage = currentPage as TabbedPage;
                if (currentTabbedPage != null)
                    currentTabbedPage.CurrentPage = currentTabbedPage.Children[0];
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
