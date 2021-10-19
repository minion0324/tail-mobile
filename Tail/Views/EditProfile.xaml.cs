using System;
using System.Diagnostics;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class EditProfile : AppPageBase
    {
        readonly EditProfileViewModel _vModel;

        public EditProfile()
        {
            try
            {
                InitializeComponent();
                _vModel = new EditProfileViewModel
                {
                    SetFocus = SetFocus,
                    StartCropView = InitCropView
                };
                BindingContext = _vModel;
                NameEntry.Completed += (object sender, EventArgs e) =>
                {
                    _vModel.UserName.Validate();
                    EmailEntry.Focus();
                };
                EmailEntry.Completed += (object sender, EventArgs e) =>
                {
                    _vModel.Email.Validate();
                    DOBPicker.Focus();
                };
                CountryCodeEntry.Completed += (object sender, EventArgs e) =>
                {
                    _vModel.CountryCode.Validate();
                    PhoneEntry.Focus();
                };
                PhoneEntry.Completed += (object sender, EventArgs e) =>
                {
                    _vModel.Phone.Validate();
                    AboutMeEntry.Focus();
                };
                DateTime currentDateTime = DateTime.Now;
                var newDatetime = currentDateTime.AddYears(-18);
                DOBPicker.MaximumDate = newDatetime;

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

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
                    

                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, AppResources.OKText));
            }
        }
        void InitCropView(CropArgs item)
        {
            Navigation.PushModalAsync(new CropView(item, CropRefreshCallback));
        }
        void CropRefreshCallback()
        {

            try
            {
                if (CommonSingletonUtility.SharedInstance.CroppedImageName != "")
                {
                    _vModel.UserProfileImage = CommonSingletonUtility.SharedInstance.CroppedImageName;
                    _vModel.UploadedProfileImage = "";
                    _vModel.IsAnyChangeInImage = true;
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
        void PhoneEntry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            _vModel.DOB.Validate();
            _vModel.CountryCode.Validate();

        }
        

    }
}