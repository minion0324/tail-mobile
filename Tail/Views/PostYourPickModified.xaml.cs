using System;
using System.Linq;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class PostYourPickModified : AppPageBase
    {
        PostYourPickModifiedViewModel _vModel;
        public PostYourPickModified()
        {
            InitializeComponent();
            _vModel = new PostYourPickModifiedViewModel();
            BindingContext = _vModel;
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

        void EntryPrice_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var typedString = e.NewTextValue;
            if (typedString != string.Empty)
            {
                int intValue;
                var isparsed = int.TryParse(typedString, out intValue);
                if (isparsed)
                {
                    if (intValue != 0)
                    {
                        _vModel.IsHideCaptionEnabled = true;
                        PostTemplate.IsHideCaptionEnabled = true;
                    }
                    else
                    {
                        _vModel.IsHideCaptionEnabled = false;
                        PostTemplate.IsHideCaptionEnabled = false;
                        PostTemplate.IsHideSelected = false;
                    }
                }
            }
            else
            {
                _vModel.StepsDataList[1].BettingDetails.PickPrice = 0;
            }
          
        }
    }
}
