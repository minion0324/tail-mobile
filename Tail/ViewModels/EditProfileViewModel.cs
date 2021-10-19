using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Tail.Services.LocalStorage;
using Tail.Services.ServiceProviders;
using Tail.Validators;
using Tail.Validators.Rules;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class EditProfileViewModel : PageViewModelBase
    {
        #region Private members

        Command _updateProfileCommand;
        Command _changePasswordCommand;
        Command _removeccountCommand;
        Command _addProfilePhotoCommand;
        string _uploadedProfileImage;
        string _userProfileImage;
        bool _isAnyChangeInImage = false;
        bool _isUserWithEmail = true;
        string _phoneWithoutFormate;
        string _aboutMe;


        #endregion

        #region Public members
        public bool IsAnyChangeInImage
        {
            get => _isAnyChangeInImage;
            set => SetProperty(ref _isAnyChangeInImage, value);
        }
        public DateTime MaximumDate
        {
            get => DateTime.Now;
        }
        public UserInfo ProfileDetails
        {
            get;
            set;

        }
        public LoggedInUser UserLoginInfo
        {
            get;
            set;

        }
        public string Password
        {
            get;
            set;
        }

        public string UploadedProfileImage
        {
            get => _uploadedProfileImage;
            set => SetProperty(ref _uploadedProfileImage, value);
        }
        public string AboutMe
        {
            get => _aboutMe!= null?_aboutMe.Trim(): _aboutMe;
            set => SetProperty(ref _aboutMe, value!=null?value.Trim():value);
        }
        public string UserProfileImage
        {
            get => _userProfileImage;
            set => SetProperty(ref _userProfileImage, value);
        }
        public bool IsUserWithEmail
        {
            get { return _isUserWithEmail; }

            set
            {
                SetProperty(ref _isUserWithEmail, value);
            }
        }
        public string PhoneWithoutFormate
        {
            get => _phoneWithoutFormate;
            set => SetProperty(ref _phoneWithoutFormate, value);
        }
        public ValidatableObject<string> UserName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> DOB { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Phone { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> CountryCode { get; set; } = new ValidatableObject<string>();

        #endregion
        public EditProfileViewModel()
        {
            try
            {
                CommonSingletonUtility.SharedInstance.IsFromEditScreen = true;  
                 UserProfileImage = Constants.DEFAULT_ADD_PROFILE_IMAGE;
            UploadedProfileImage = "";
                Task.Run(async () => await GetProfile()).Wait();
                AddValidationRules();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }

        public Command UpdateProfileCommand => _updateProfileCommand ?? (_updateProfileCommand = new Command(async () => await Handle_UpdateProfileCommand()));
        public Command ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new Command(async () => await Handle_ChangePasswordCommand()));
        public Command RemoveAccountCommand => _removeccountCommand ?? (_removeccountCommand = new Command(async () => await Handle_RemoveccountCommand()));
        public Command AddProfilePhotoCommand => _addProfilePhotoCommand ?? (_addProfilePhotoCommand = new Command(async () => await Handle_AddProfilePhotoCommand()));
        public Action<String> SetFocus { get; set; }
        public Action<CropArgs> StartCropView { get; set; }
        public void AddValidationRules()
        {
            UserName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterName });
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterEmail });
            DOB.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterDOB });
            CountryCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterCountry });
            CountryCode.Validations.Add(new IsValidCountryRule<string> { ValidationMessage = AppResources.EnterValidCountry });

            Phone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterPhone });


        }
        public bool IsValid()
        {

            if (CountryCode.Value.StartsWith("+"))
            {
                CountryCode.Value = CountryCode.Value.Substring(1);
            }
            if (Phone.Value.Contains("-"))
            {
                PhoneWithoutFormate = Phone.Value.Replace("-", string.Empty);
            }

            bool _isNameValid = UserName.Validate();
            bool _isEmailValid = Email.Validate();
            bool _isDOBValid = DOB.Validate();
            bool _isPhoneValid = Phone.Validate();
            bool _isCountryCodeValid = CountryCode.Validate();


            if (!_isNameValid)
            {
                SetFocus("Name");
            }
            else if (!_isEmailValid)
            {
                SetFocus("Email");
            }
            else if (!_isDOBValid)
            {
                SetFocus("DOB");
            }
            else if (!_isCountryCodeValid)
            {
                SetFocus("Country");
            }
            else if (!_isPhoneValid)
            {
                SetFocus("Phone");
            }

            return _isNameValid && _isEmailValid && _isDOBValid && _isPhoneValid;
        }

        async Task Handle_UpdateProfileCommand()
        {

            if (IsBusy)
                return;
            IsBusy = true;

            if (!IsValid())
            {
                IsBusy = false;
                return;
            }

            if (IsAnyChangeInImage)
            {
                await UploadProfileImage(UserProfileImage);

            }
            if (!await VerifyUpdate())
            {
                IsBusy = false;
                return;
            }

            CommonSingletonUtility.SharedInstance.IsFromEditScreen = false;
            IsBusy = false;


        }
       
        async Task<bool> GetProfile()
        {
            bool hasSuccessResponse = false;

            try
            {

                var loginInfo = DataStorageService.Instance.GetLoggedInUser();
                UserInfo _userDetails = SettingsService.Instance.LoggedUserDetails;
                if (loginInfo.ErrorCode == 200 && loginInfo.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    UserLoginInfo = loginInfo.ResponseData;
                    UserName.Value = loginInfo.ResponseData.UserName.Trim();
                    Email.Value = loginInfo.ResponseData.Email;
                    DOB.Value = loginInfo.ResponseData.Dob;
                    Phone.Value = loginInfo.ResponseData.Phone;
                    UserProfileImage = Constants.DEFAULT_USERIMAGE;
                    if (!string.IsNullOrEmpty(loginInfo.ResponseData.UserImage) && loginInfo.ResponseData.UserImage != Constants.DEFAULT_USERIMAGE)
                    {
                        UserProfileImage = TailUtils.GetThumbProfileImage(loginInfo.ResponseData.UserImage);
                        UploadedProfileImage = loginInfo.ResponseData.UserImage;
                    }
                   
                    IsUserWithEmail = true;
                    if (_userDetails.Login_type == LoginType.FB || _userDetails.Login_type == LoginType.Apple)
                        IsUserWithEmail = false;

                    AboutMe = loginInfo.ResponseData.AboutMe!= null? loginInfo.ResponseData.AboutMe.Trim(): loginInfo.ResponseData.AboutMe;
                    if (!string.IsNullOrEmpty(loginInfo.ResponseData.CountryCode))
                    {
                        CountryCode.Value = (loginInfo.ResponseData.CountryCode.Substring(0, 1) == "+") ? loginInfo.ResponseData.CountryCode.Substring(1) : loginInfo.ResponseData.CountryCode;
                    }
                    
                }
                else
                {
                    if (loginInfo.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, loginInfo.Message);
                }

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> UploadProfileImage(string ProfileImage)
        {
            bool hasSuccessResponse = false;

            try
            {
                
                string _keyName = TailUtils.EncryptString(Email.Value);
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";

                if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, ProfileImage, Constants.S3BucketForProfileImage, true))
                {
                    UploadedProfileImage = _orginalImageName;
                    hasSuccessResponse = true;
                }



            }
            catch (Exception ex) 
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        async Task<bool> VerifyUpdate()
        {
            bool hasSuccessResponse = false;

            try
            {

                    UpdateProileRequestInfo requestObj = new UpdateProileRequestInfo()
                    {
                        uName =UserName.Value !=null? UserName.Value.Trim(): UserName.Value,
                        dob = TailUtils.ConvertToDateFormate(Convert.ToDateTime(DOB.Value, CultureInfo.InvariantCulture)),
                        phone = PhoneWithoutFormate,
                        aboutMe = AboutMe,
                        country = "+" + CountryCode.Value
                    };
                    if (!string.IsNullOrEmpty(UploadedProfileImage))
                    {
                        requestObj.userImg = UploadedProfileImage;
                    }
                    var updatResponse = await TailDataServiceProvider.Instance.UpdateProfile(requestObj);
                    if (updatResponse.ErrorCode == 200)
                    {
                    UserInfo _userDetails = SettingsService.Instance.LoggedUserDetails;

                    hasSuccessResponse = true;
                    string _oldphoneNumber = _userDetails.Phone;

                    _userDetails.UserName= UserLoginInfo.UserName = UserName.Value;
                    _userDetails.Email = UserLoginInfo.Email = Email.Value;
                    _userDetails.Dob = UserLoginInfo.Dob = DOB.Value;
                    _userDetails.UserImage = UserLoginInfo.UserImage = UploadedProfileImage;
                    _userDetails.AboutMe = UserLoginInfo.AboutMe = AboutMe;
                    _userDetails.Phone= UserLoginInfo.Phone = PhoneWithoutFormate;
                    _userDetails.CountryCode = UserLoginInfo.CountryCode = "+" + CountryCode.Value;

                    LoggedInUser _LoggedDetails = new LoggedInUser()
                    {
                        UserId = UserLoginInfo.UserId,
                        UserName = UserLoginInfo.UserName,
                        UserImage = (string.IsNullOrEmpty(UserLoginInfo.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : UserLoginInfo.UserImage,
                        AccessToken = UserLoginInfo.AccessToken,
                        RefreshToken = UserLoginInfo.RefreshToken,
                        Email = UserLoginInfo.Email,
                        CountryCode = UserLoginInfo.CountryCode,
                        IsSportsAdded = Convert.ToInt32(UserLoginInfo.IsSportsAdded),
                        IsPhoneVerified = UserLoginInfo.IsPhoneVerified,
                        Id = UserLoginInfo.Id,
                        UserTypeId = UserLoginInfo.UserTypeId,
                        IsAdmin = UserLoginInfo.IsAdmin,
                        AboutMe = UserLoginInfo.AboutMe,
                        Dob = UserLoginInfo.Dob,
                        Phone = UserLoginInfo.Phone,
                        UserStatus = UserLoginInfo.UserStatus

                    };

                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                    DataStorageService.Instance.SaveLoginDetails(_LoggedDetails);
                    if (_oldphoneNumber == PhoneWithoutFormate)
                    {
                        CommonSingletonUtility.SharedInstance.IsFromEditScreen = false;
                        Back.Execute(null);
                    }
                    else
                    {
                        SettingsService.Instance.FromEditProfile = true;
                        CommonSingletonUtility.SharedInstance.OtpPhoneNumber = PhoneWithoutFormate;
                        CommonSingletonUtility.SharedInstance.IsFromEditScreen = false;
                        await NavigationService.NavigateToAsync<OTPVerification>();

                    }
                }
                else
                {
                    if (updatResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, updatResponse.Message);
                }

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task Handle_ChangePasswordCommand()
        {
            await PopupNavigation.Instance.PushAsync(new ChangePasswordPopup(async () => await Handle_PopUpClosed()));
        }
   


        async Task Handle_VerifyPasswordPopUpClosed()
        {
            var removeResponse = await TailDataServiceProvider.Instance.RemoveAccount();
            if (removeResponse.ErrorCode == 200)
            {
                if(IsUserWithEmail)
                    await PopupNavigation.Instance.PopAllAsync();
                await ShowAlert(AppResources.AppName, removeResponse.Message);
                await LogoutUser();
            }
            else
            {
                if (removeResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                    await NavigationService.ShowAlertAsync(AppResources.AppName, removeResponse.Message);
            }
      }
        async Task Handle_RemoveccountCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                bool confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmRemoveAccount);
                if (confirmation)
                {
                    if (IsUserWithEmail)
                    {
                        await PopupNavigation.Instance.PushAsync(new PasswordVerificationPopup(async () => await Handle_VerifyPasswordPopUpClosed()));

                    }
                    else
                    {
                        await Handle_VerifyPasswordPopUpClosed();
                    }
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

        }
        async Task Handle_AddProfilePhotoCommand()
        {
            string actionSheetResult = await AppNavigationService.GetInstance().ShowActionSheetAsync(AppResources.AddPhoto, AppResources.CancelText, null, AppResources.TakePhoto, AppResources.ChooseGallery);

            if (actionSheetResult == AppResources.TakePhoto)
            {
                var hasPermission = await CheckCameraPermissionsAsync();
                if (hasPermission)
                {

                    var pickedMediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = Constants.MediaDirectoryName,
                        Name = Constants.SavedCameraImageName,
                        SaveToAlbum = false,
                        SaveMetaData = false,
                        CompressionQuality = 60
                    });
                    if (pickedMediaFile != null)
                    {
                        var memoryStream = new MemoryStream();
                        pickedMediaFile.GetStream().CopyTo(memoryStream);
                        byte[] imageAsByte = memoryStream.ToArray();
                        CropArgs obj = new CropArgs()
                        {
                            ImageAsByte = imageAsByte,
                            Height = 200,
                            Width = 200,
                        };
                        StartCropView(obj);

                    }
                }

            }
            else if (actionSheetResult == AppResources.ChooseGallery && await CheckGalleryPermissionsAsync())
            {

                    var pickedMediaFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        SaveMetaData = false,
                    });
                    if (pickedMediaFile != null)
                    {
                        var memoryStream = new MemoryStream();
                        pickedMediaFile.GetStream().CopyTo(memoryStream);
                        byte[] imageAsByte = memoryStream.ToArray();
                        CropArgs obj = new CropArgs()
                        {
                            ImageAsByte = imageAsByte,
                            Height = 200,
                            Width = 200,
                        };
                        StartCropView(obj);


                    }
            }
        }
            async Task Handle_PopUpClosed()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

    }

}

