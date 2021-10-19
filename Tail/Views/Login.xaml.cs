using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class Login : AppPageBase
    {
        LoginViewModel _vModel;
        public Login()
        {

            InitializeComponent();
            if (IsAndroid)
                DependencyService.Get<IStatusBarStyleManager>().SetLightTheme();
            _vModel = new LoginViewModel
            {
                SetFocus = SetFocus,
            };
            BindingContext = _vModel;
            App.PostSuccessFacebookAction = async token =>
            {
                await _vModel.GetFbProfile(token);
            };
            EntryEmail.Completed += (object sender, EventArgs e) => {
                _vModel.Email.Validate();
                EntryPassword.Focus();
            };
            EntryPassword.Completed += (object sender, EventArgs e) => {
               
               if(_vModel.Password.Validate() && _vModel.Email.Validate())
                {
                    _vModel.Login.Execute(null);
                }
            };
            EntryPassword.TextChanged += (object sender, TextChangedEventArgs e) => {
                if (string.IsNullOrEmpty(e.NewTextValue))
                    return;
                if (string.IsNullOrEmpty(e.OldTextValue) && e.NewTextValue.Length > 1)
                {
                    EntryPassword.Text = "";
                }
                else if (!string.IsNullOrEmpty(e.OldTextValue) && e.NewTextValue.Length - e.OldTextValue.Length > 1)
                {
                    EntryPassword.Text = e.OldTextValue;
                }
               
            };
            if (Device.RuntimePlatform == Device.iOS)
            {
               
                if (DeviceInfo.Version.Major < 13)
                    AppleLoginFrame.IsVisible = false;
                else
                    AppleLoginFrame.IsVisible = true;
            }
            else
            {
                AppleLoginFrame.IsVisible = false;
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

        void SetFocus(string item)
        {
            try
            {
                switch (item)
                {
                    case "Email":
                        EntryEmail.Focus();
                        break;

                    case "Password":
                        EntryPassword.Focus();
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
          

            EntryPassword.IsPassword = !EntryPassword.IsPassword;
            ShowPassword.Source = EntryPassword.IsPassword ? "eye_signup" : "eye_hide";
        }

    }
}
