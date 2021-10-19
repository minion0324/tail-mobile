using System;
using System.Text.RegularExpressions;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class NewAccount : AppPageBase
    {
         NewAccountViewModel _vModel;
        const string passwordRegex = @"(?=.*\d)(?=.*[A-Z])(?=.*[$@$!%*#?&]).{8,}";
        const string letterRegex = @"(?=.*[A-Z])";
        const string numberRegex = @"(?=.*\d)";
        const string specialCharRegex = @"(?=.*[$@$!%*#?&])";
        const string eightCharRegex = @".{8,}";
        public NewAccount()
        {
            InitializeComponent();
          
            _vModel = new NewAccountViewModel
            {
                SetFocus = SetFocus,
                StartCropView= InitCropView
            };
            BindingContext = _vModel;
            NameEntry.Completed += (object sender, EventArgs e) => {
                _vModel.Name.Validate();
                EmailEntry.Focus();
            };
            EmailEntry.Completed += (object sender, EventArgs e) => {
               _vModel.Email.Validate();
                DOBPicker.Focus();
            };
            CountryCodeEntry.Completed += (object sender, EventArgs e) => {
                _vModel.CountryCode.Validate();
                PhoneEntry.Focus();
            };
            PhoneEntry.Completed += (object sender, EventArgs e) => {
                _vModel.Phone.Validate();
                if(_vModel.IsEmailSignUp)
                    PasswordEntry.Focus();
            };
            PasswordEntry.Completed += (object sender, EventArgs e) => {
                _vModel.Password.Validate();   
            };
            PasswordEntry.TextChanged += (object sender, TextChangedEventArgs e) => {
                if (string.IsNullOrEmpty(e.NewTextValue))
                    return;
                if (string.IsNullOrEmpty(e.OldTextValue) && e.NewTextValue.Length > 1)
                {
                    PasswordEntry.Text = "";
                }
                else if (!string.IsNullOrEmpty(e.OldTextValue) && e.NewTextValue.Length - e.OldTextValue.Length > 1)
                {
                    PasswordEntry.Text = e.OldTextValue;
                }

            };

            DateTime currentDateTime = DateTime.Now;
           var newDatetime = currentDateTime.AddYears(-18);
            DOBPicker.MaximumDate = newDatetime;
        }
        void InitCropView(CropArgs item)
        {
             Navigation.PushModalAsync(new CropView(item,CropRefreshCallback));
        }
        void CropRefreshCallback()
        {
           
            try
            {
                if (CommonSingletonUtility.SharedInstance.CroppedImageName != "")
                {
                    _vModel.UserProfileImage = "";
                    _vModel.UserProfileImage = CommonSingletonUtility.SharedInstance.CroppedImageName;
                    _vModel.UploadedProfileImage = "";
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, AppResources.OKText));
            }
        }
        void CalenderIcon_Tapped(System.Object sender, System.EventArgs e)
        {
            if (!DOBPicker.IsFocused)
                DOBPicker.Focus();
        }

        void PasswordEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            bool IsValid;
            IsValid = (Regex.IsMatch(e.NewTextValue, passwordRegex));
            ValidationLbl.IsVisible = !IsValid;
            if(!IsValid)
            {
                bool containEightChar ;
                bool containdigits ;
                bool containAlphabets ;
                bool containSpecialChar;
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

        void ShowPassword_Tapped(System.Object sender, System.EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
            ShowPassword.Source = PasswordEntry.IsPassword ? "eye_signup" : "eye_hide";
        }

        void SetFocus(string item)
        {
            try
            {
                switch (item)
                {
                    case "Name":
                        NameEntry.Focus();
                        break;
                    case "Email":
                        EmailEntry.Focus();
                        break;
                    case "DOB":
                        DOBPicker.Focus();
                        break;
                    case "Country":
                        CountryCodeEntry.Focus();
                        break;
                    case "Phone":
                        PhoneEntry.Focus();
                        break;
                    case "Password":
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

        void PhoneEntry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _vModel.DOB.Validate();
            _vModel.CountryCode.Validate();
            
        }

        void PasswordEntry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _vModel.CountryCode.Validate();
            _vModel.Phone.Validate();
        }
    }
}
