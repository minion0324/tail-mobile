using System;
using System.Text.RegularExpressions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class ChangePasswordPopup : PopupPage
    {
        const string passwordRegex = @"(?=.*\d)(?=.*[A-Z])(?=.*[$@$!%*#?&]).{8,}";
        const string letterRegex = @"(?=.*[A-Z])";
        const string numberRegex = @"(?=.*\d)";
        const string specialCharRegex = @"(?=.*[$@$!%*#?&])";
        const string eightCharRegex = @".{8,}";
        ChangePasswordPopupViewModel _vModel;


        public ChangePasswordPopup(Action popUpCloseCallback)
        {
            InitializeComponent();
            _vModel = new ChangePasswordPopupViewModel();
            _vModel.SetFocus = SetFocus;
            BindingContext = _vModel;
            _vModel.PopupCloseCallback = popUpCloseCallback;
            OldPasswordEntry.Completed += (object sender, EventArgs e) =>
            {
                _vModel.CurrentPassword.Validate();
                PasswordEntry.Focus();
            };

        }
        void PasswordEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

            var isValid = (Regex.IsMatch(e.NewTextValue, passwordRegex));
            ValidationLbl.IsVisible = !isValid;
            if (!isValid)
            {
                bool containEightChar = false;
                bool containdigits = false;
                bool containAlphabets = false;
                bool containSpecialChar = false;
                containEightChar = (Regex.IsMatch(e.NewTextValue, eightCharRegex));
                containdigits = (Regex.IsMatch(e.NewTextValue, numberRegex));
                containSpecialChar = (Regex.IsMatch(e.NewTextValue, specialCharRegex));
                containAlphabets = (Regex.IsMatch(e.NewTextValue, letterRegex));
                eightCharTxt.TextColor = containEightChar ? Color.Green : Color.FromHex("#666666");
                numberTxt.TextColor = containdigits ? Color.Green : Color.FromHex("#666666");
                specialCharTxt.TextColor = containSpecialChar ? Color.Green : Color.FromHex("#666666");
                letterTxt.TextColor = containAlphabets ? Color.Green : Color.FromHex("#666666");
            }
        }
        void SetFocus(string item)
        {
            try
            {
                switch (item)
                {
                    case "CurrentPassword":
                        OldPasswordEntry.Focus();
                        break;
                    case "NewPassword":
                        PasswordEntry.Focus();
                        break;

                    default:
                        break;

                }
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
        void ShowPasswordOld_Tapped(System.Object sender, System.EventArgs e)
        {
            OldPasswordEntry.IsPassword = !OldPasswordEntry.IsPassword;
            ShowPasswordOld.Source = OldPasswordEntry.IsPassword ? "eye_signup" : "eye_hide";

        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
