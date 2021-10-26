using System;
using System.Linq;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class CreateAPost : AppPageBase
    {
         CreateApostViewModel _vModel;
        public CreateAPost()
        {
            _vModel = new CreateApostViewModel();
            BindingContext = _vModel;
            InitializeComponent();
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
        public override void OnPageDestroy()
        {
            PostTemplate.DeregisterOnMediaPicked();


            base.OnPageDestroy();
        }
    }
}
