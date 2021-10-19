using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class PasswordVerificationPopup : PopupPage
    {

        PasswordVerificationViewModel _vModel;


        public PasswordVerificationPopup(Action popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new PasswordVerificationViewModel();
            _vModel.PopupCloseCallback = popUpCloseCallback;
            _vModel.SetFocus = SetFocus;
            BindingContext = _vModel;
           
        }
        void SetFocus()
        {
            try
            {
                PasswordEntry.Focus();

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, AppResources.OKText));
            }
        }
       

        void ShowPassword_Tapped(System.Object sender, System.EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
            ShowPassword.Source = PasswordEntry.IsPassword ? "eye_signup" : "eye_hide";
        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}

