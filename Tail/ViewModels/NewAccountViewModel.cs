using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
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
    public class NewAccountViewModel : PageViewModelBase
    {
        #region Private members
        bool _acceptTermsAndCondition;
        bool _isEmailSignUp = true;
        int _isPhoneVerified = 0;
        string _thirdPartyLoginId;
        string _thirdPartyToken;
        string _userProfileImage;
        string _uploadedProfileImage;
        string _phoneWithoutFormate;
        LoginType _signUpType;


        Command _loginCommand;
        Command _termsAndConditionCommand;
        Command _privacyCommand;
        Command _createNewAccountCommand;
        Command _termsAndConditionAgreeCommand;
        Command _addProfilePhotoCommand;


        #endregion

        #region Public members

        public bool AcceptTermsAndCondition
        {
            get => _acceptTermsAndCondition;
            set => SetProperty(ref _acceptTermsAndCondition, value);
        }
        public int IsPhoneVerified
        {
            get => _isPhoneVerified;
            set => SetProperty(ref _isPhoneVerified, value);
        }
        public bool IsEmailSignUp
        {
            get => _isEmailSignUp;
            set => SetProperty(ref _isEmailSignUp, value);
        }
        public string UserProfileImage
        {
            get => _userProfileImage;
            set => SetProperty(ref _userProfileImage, value);
        }
        public string UploadedProfileImage
        {
            get => _uploadedProfileImage;
            set => SetProperty(ref _uploadedProfileImage, value);
        }
        public string ThirdPartyLoginId
        {
            get => _thirdPartyLoginId;
            set => SetProperty(ref _thirdPartyLoginId, value);
        }
        public string ThirdPartyToken
        {
            get => _thirdPartyToken;
            set => SetProperty(ref _thirdPartyToken, value);
        }
        public LoginType SignUpType
        {
            get => _signUpType;
            set => SetProperty(ref _signUpType, value);
        }
        public DateTime MaximumDate
        {
            get => DateTime.Now;
        }
        public string PhoneWithoutFormate
        {
            get => _phoneWithoutFormate;
            set => SetProperty(ref _phoneWithoutFormate, value);
        }
        public ValidatableObject<string> Name { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> DOB { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Phone { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> CountryCode { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<bool> TermsAndCondition { get; set; } = new ValidatableObject<bool>();

        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(async () => await Handle_LoginCommand()));
        public Command TermsAndConditionCommand => _termsAndConditionCommand ?? (_termsAndConditionCommand = new Command(async () => await Handle_TermsAndConditionCommand()));
        public Command PrivacyCommand => _privacyCommand ?? (_privacyCommand = new Command(async () => await Handle_PrivacyCommand()));
        public Command CreateNewAccountCommand => _createNewAccountCommand ?? (_createNewAccountCommand = new Command(async () => await Handle_CreateNewAccountCommandAsync()));
        public Command TermsAndConditionAgreeCommand => _termsAndConditionAgreeCommand ?? (_termsAndConditionAgreeCommand = new Command(() => Handle_TermsAndConditionAgreeCommand()));
        public Command AddProfilePhotoCommand => _addProfilePhotoCommand ?? (_addProfilePhotoCommand = new Command(async () => await Handle_AddProfilePhotoCommand()));

        public Action<String> SetFocus { get; set; }
        public Action<CropArgs> StartCropView { get; set; }
        #endregion



        public NewAccountViewModel()
        {
            UserProfileImage = Constants.DEFAULT_ADD_PROFILE_IMAGE;
            UploadedProfileImage = "";
            SignUpType = LoginType.Email;
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            RegionInfo region = RegionInfo.CurrentRegion;
            int _countryCode = phoneNumberUtil.GetCountryCodeForRegion(region.TwoLetterISORegionName);
            CountryCode.Value = _countryCode.ToString();

            AddValidationRules();
        }
        public override async Task InitializeAsync(object parameter = null)
        {
            try
            {
                if (parameter != null)
                {
                    IsEmailSignUp = false;
                    SignUpEventArgs _prefillData = (SignUpEventArgs)parameter;
                    Name.Value = _prefillData.Name;
                    Email.Value = _prefillData.Email;
                    ThirdPartyLoginId = _prefillData.ID;
                    SignUpType = _prefillData.LoginType;
                    ThirdPartyToken = _prefillData.Token;
                    if (!string.IsNullOrEmpty(_prefillData.ProfileImage))
                    {
                        UserProfileImage = _prefillData.ProfileImage;
                    }
                }
                else
                {
                    Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterPassword });
                    Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = AppResources.ValidPassword });
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Initialize New Account. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void AddValidationRules()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterName });
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterEmail });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = AppResources.ValidEmail });
            DOB.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterDOB });
            CountryCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterCountry });
            CountryCode.Validations.Add(new IsValidCountryRule<string> { ValidationMessage = AppResources.EnterValidCountry });
            Phone.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterPhone });

            TermsAndCondition.Validations.Add(new IsValueTrueRule<bool> { ValidationMessage = AppResources.AcceptTerms });

        }

        async Task Handle_LoginCommand()
        {
            await NavigationService.NavigateToAsync<Login>();
        }
        async Task Handle_TermsAndConditionCommand()
        {
            await NavigationService.NavigateToAsync<TermsAndCondititons>();
        }
        async Task Handle_PrivacyCommand()
        {
            await NavigationService.NavigateToAsync<PrivacyPolicy>();
        }

        void Handle_TermsAndConditionAgreeCommand()
        {
            AcceptTermsAndCondition = !AcceptTermsAndCondition;
            TermsAndCondition.Value = AcceptTermsAndCondition;
        }
        async Task Handle_CreateNewAccountCommandAsync()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                if (!IsValid())
                {
                    IsBusy = false;
                    return;
                }
                if (!TermsAndCondition.Validate())
                {
                    IsBusy = false;
                    await ShowAlert(AppResources.AppName, AppResources.AcceptTerms);
                    return;
                }
                if (UserProfileImage != Constants.DEFAULT_ADD_PROFILE_IMAGE && string.IsNullOrEmpty(UploadedProfileImage) && !await UploadProfileImage(UserProfileImage))
                {
                    IsBusy = false;
                    return;
                }
                if (string.Compare(Device.RuntimePlatform, Device.iOS) != 0 && string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                {
                    DependencyService.Get<IDeviceHelper>().UpdateToken();
                }
                if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                    SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
                if (SignUpType == LoginType.Email)
                {
                    if (!await VerifySignUp())
                    {
                        IsBusy = false;
                        return;
                    }
                    SettingsService.Instance.IsCompletedSignUpProcess = false;
                   
                    if (IsPhoneVerified == 1)
                    {
                        await NavigationService.NavigateToAsync<SelectInterest>();
                    }
                    else
                    {
                        CommonSingletonUtility.SharedInstance.OtpPhoneNumber = PhoneWithoutFormate;
                        CommonSingletonUtility.SharedInstance.OtpCountryCode = "+" + CountryCode.Value;
                        await NavigationService.NavigateToAsync<OTPVerification>(Phone.Value);
                    }

                }
                else if (SignUpType == LoginType.Apple)
                {
                    if (!await VerifyAppleSignUp())
                    {
                        IsBusy = false;
                        return;
                    }
                    SettingsService.Instance.IsCompletedSignUpProcess = false;
                   
                    if (IsPhoneVerified == 1)
                    {
                        await NavigationService.NavigateToAsync<SelectInterest>();
                    }
                    else
                    {
                        CommonSingletonUtility.SharedInstance.OtpPhoneNumber = PhoneWithoutFormate;
                        CommonSingletonUtility.SharedInstance.OtpCountryCode = "+" + CountryCode.Value;
                        await NavigationService.NavigateToAsync<OTPVerification>();
                    }
                }
                else if (SignUpType == LoginType.FB)
                {
                    if (!await VerifyFBSignUp())
                    {
                        IsBusy = false;
                        return;
                    }
                    SettingsService.Instance.IsCompletedSignUpProcess = false;
                  
                    if (IsPhoneVerified == 1)
                    {
                        await NavigationService.NavigateToAsync<SelectInterest>();
                    }
                    else
                    {
                        CommonSingletonUtility.SharedInstance.OtpPhoneNumber = PhoneWithoutFormate;
                        CommonSingletonUtility.SharedInstance.OtpCountryCode = "+" + CountryCode.Value;
                        await NavigationService.NavigateToAsync<OTPVerification>();
                    }
                }
                SettingsService.Instance.IsSavedCredentials = false;
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

        public bool IsValid()
        {

            if (CountryCode.Value.StartsWith("+"))
            {
                CountryCode.Value = CountryCode.Value.Substring(1);
            }
            if (Phone.Value != null && Phone.Value.Contains("-"))
            {
                PhoneWithoutFormate = Phone.Value.Replace("-", string.Empty);
            }


            bool _isNameValid = Name.Validate();
            bool _isEmailValid = Email.Validate();
            bool _isDOBValid = DOB.Validate();
            bool _isPhoneValid = Phone.Validate();
            bool _isCountryCodeValid = CountryCode.Validate();
            bool _isPasswordValid = true;
            if (IsEmailSignUp)
                _isPasswordValid = Password.Validate();

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
            else if (!_isPasswordValid)
            {
                SetFocus("Password");
            }
            return _isNameValid && _isEmailValid && _isDOBValid && _isPhoneValid && _isPasswordValid;
        }

        async Task<bool> UploadProfileImage(string ProfileImage)
        {
            bool hasSuccessResponse = false;

            try
            {
                
                string _keyName = TailUtils.EncryptString(Email.Value);
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";
                if (SignUpType == LoginType.Email)
                {
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, ProfileImage, Constants.S3BucketForProfileImage, true))
                    {
                        UploadedProfileImage = _orginalImageName;
                        hasSuccessResponse = true;
                    }

                }
                else
                {
                    Uri uriResult;
                    bool result = Uri.TryCreate(ProfileImage, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                    if (result)
                    {
                        var _profileImageBytes = await TailDataServiceProvider.Instance.DownloadImageFromUrl(ProfileImage);
                        if (_profileImageBytes != null && await DependencyService.Get<IAwsBucketService>().UploadByteArrayImageFileToAmazonBucketAsync(_profileImageBytes, _orginalImageName, _thumbImageName, Constants.S3BucketForProfileImage, true))
                        {
                                UploadedProfileImage = _orginalImageName;
                                hasSuccessResponse = true;
                        }
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, ProfileImage, Constants.S3BucketForProfileImage, true))
                        {
                            UploadedProfileImage = _orginalImageName;
                            hasSuccessResponse = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        


        async Task<bool> VerifySignUp()
        {
            bool hasSuccessResponse = false;

            try
            {
               
                SignUpRequestInfo requestObj = new SignUpRequestInfo()
                {
                    uName = Name.Value,
                    email = Email.Value,
                    dob = TailUtils.ConvertToDateFormate(Convert.ToDateTime(DOB.Value, CultureInfo.InvariantCulture)),
                    phone = PhoneWithoutFormate,
                    country = "+" + CountryCode.Value,
                    password = Password.Value,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE,


                };
                if (!string.IsNullOrEmpty(UploadedProfileImage))
                {
                    requestObj.userImg = UploadedProfileImage;
                }

                if (SettingsService.Instance.PartialRegstarionUserID != 0)
                {
                    requestObj.userId = SettingsService.Instance.PartialRegstarionUserID.ToString();
                }

                var loginResponse = await TailDataServiceProvider.Instance.VerifySignUp(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        Phone = loginResponse.ResponseData.Phone,
                        CountryCode = "+" + CountryCode.Value,
                        Login_type = LoginType.Email,
                        Password = TailUtils.Encrypt(Password.Value, Constants.ENCRPTION_KEY)
                    };
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                    if (IsPhoneVerified == 0)
                    {
                        SettingsService.Instance.PartialRegstarionUserID = loginResponse.ResponseData.UserId;
                    }
                    else
                    {
                        SettingsService.Instance.Authenticated = true;
                        SettingsService.Instance.PartialRegstarionUserID = 0;

                        LoggedInUser _LoggedDetails = new LoggedInUser()
                        {
                            UserId = loginResponse.ResponseData.UserId,
                            UserName = loginResponse.ResponseData.UserName,
                            UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                            AccessToken = loginResponse.ResponseData.AccessToken,
                            RefreshToken = loginResponse.ResponseData.RefreshToken,
                            Email = loginResponse.ResponseData.Email,
                            CountryCode = loginResponse.ResponseData.CountryCode,
                            IsSportsAdded = Convert.ToInt32(loginResponse.ResponseData.IsSportsAdded),
                            IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified,
                            Id = loginResponse.ResponseData.Id,
                            UserTypeId = loginResponse.ResponseData.UserTypeId,
                            IsAdmin = loginResponse.ResponseData.IsAdmin,
                            AboutMe = loginResponse.ResponseData.AboutMe,
                            Dob = loginResponse.ResponseData.Dob,
                            Phone = loginResponse.ResponseData.Phone,
                            UserStatus = loginResponse.ResponseData.UserStatus

                        };
                        DataStorageService.Instance.SaveLoginDetails(_LoggedDetails);
                    }
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> VerifyFBSignUp()
        {
            bool hasSuccessResponse = false;

            try
            {
                FBSignUpRequestInfo requestObj = new FBSignUpRequestInfo()
                {
                    uName = Name.Value,
                    email = Email.Value,
                    dob = TailUtils.ConvertToDateFormate(Convert.ToDateTime(DOB.Value, CultureInfo.InvariantCulture)),
                    phone = PhoneWithoutFormate,
                    country = "+" + CountryCode.Value,
                    fbToken = ThirdPartyLoginId,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE,

                };
                if (!string.IsNullOrEmpty(UploadedProfileImage))
                {
                    requestObj.userImg = UploadedProfileImage;
                }
                if (SettingsService.Instance.PartialRegstarionUserID != 0)
                {
                    requestObj.userId = SettingsService.Instance.PartialRegstarionUserID.ToString();
                }

                var loginResponse = await TailDataServiceProvider.Instance.VerifyFBSignUp(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        Phone = loginResponse.ResponseData.Phone,
                        Login_type = LoginType.FB,
                        CountryCode = "+" + CountryCode.Value,
                    };
                    SettingsService.Instance.LoggedUserDetails = _userDetails;

                    if (IsPhoneVerified == 0)
                    {
                        SettingsService.Instance.PartialRegstarionUserID = loginResponse.ResponseData.UserId;
                    }
                    else
                    {
                        SettingsService.Instance.Authenticated = true;
                        SettingsService.Instance.PartialRegstarionUserID = 0;
                        LoggedInUser _LoggedDetails = new LoggedInUser()
                        {
                            UserId = loginResponse.ResponseData.UserId,
                            UserName = loginResponse.ResponseData.UserName,
                            UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                            AccessToken = loginResponse.ResponseData.AccessToken,
                            RefreshToken = loginResponse.ResponseData.RefreshToken,
                            Email = loginResponse.ResponseData.Email,
                            CountryCode = loginResponse.ResponseData.CountryCode,
                            IsSportsAdded = Convert.ToInt32(loginResponse.ResponseData.IsSportsAdded),
                            IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified,
                            Id = loginResponse.ResponseData.Id,
                            UserTypeId = loginResponse.ResponseData.UserTypeId,
                            IsAdmin = loginResponse.ResponseData.IsAdmin,
                            AboutMe = loginResponse.ResponseData.AboutMe,
                            Dob = loginResponse.ResponseData.Dob,
                            Phone = loginResponse.ResponseData.Phone,
                            UserStatus = loginResponse.ResponseData.UserStatus

                        };
                        DataStorageService.Instance.SaveLoginDetails(_LoggedDetails);
                    }



                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> VerifyAppleSignUp()
        {
            bool hasSuccessResponse = false;

            try
            {
                AppleSignUpRequestInfo requestObj = new AppleSignUpRequestInfo()
                {
                    uName = Name.Value,
                    email = Email.Value,
                    dob = TailUtils.ConvertToDateFormate(Convert.ToDateTime(DOB.Value, CultureInfo.InvariantCulture)),
                    phone = PhoneWithoutFormate,
                    country = "+" + CountryCode.Value,
                    appleToken = ThirdPartyLoginId,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE,

                };
                if (!string.IsNullOrEmpty(UploadedProfileImage))
                {
                    requestObj.userImg = UploadedProfileImage;
                }
                if (SettingsService.Instance.PartialRegstarionUserID != 0)
                {
                    requestObj.userId = SettingsService.Instance.PartialRegstarionUserID.ToString();
                }

                var loginResponse = await TailDataServiceProvider.Instance.VerifyAppleSignUp(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        Phone = loginResponse.ResponseData.Phone,
                        Login_type = LoginType.Apple,
                        CountryCode = "+" + CountryCode.Value,
                    };
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                    if (IsPhoneVerified == 0)
                    {
                        SettingsService.Instance.PartialRegstarionUserID = loginResponse.ResponseData.UserId;
                    }
                    else
                    {
                        SettingsService.Instance.Authenticated = true;
                        SettingsService.Instance.PartialRegstarionUserID = 0;
                        LoggedInUser _LoggedDetails = new LoggedInUser()
                        {
                            UserId = loginResponse.ResponseData.UserId,
                            UserName = loginResponse.ResponseData.UserName,
                            UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                            AccessToken = loginResponse.ResponseData.AccessToken,
                            RefreshToken = loginResponse.ResponseData.RefreshToken,
                            Email = loginResponse.ResponseData.Email,
                            CountryCode = loginResponse.ResponseData.CountryCode,
                            IsSportsAdded = Convert.ToInt32(loginResponse.ResponseData.IsSportsAdded),
                            IsPhoneVerified = loginResponse.ResponseData.IsPhoneVerified,
                            Id = loginResponse.ResponseData.Id,
                            UserTypeId = loginResponse.ResponseData.UserTypeId,
                            IsAdmin = loginResponse.ResponseData.IsAdmin,
                            AboutMe = loginResponse.ResponseData.AboutMe,
                            Dob = loginResponse.ResponseData.Dob,
                            Phone = loginResponse.ResponseData.Phone,
                            UserStatus = loginResponse.ResponseData.UserStatus

                        };
                        DataStorageService.Instance.SaveLoginDetails(_LoggedDetails);
                    }

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
    }
}
