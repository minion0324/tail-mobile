using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Helpers;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.Interfaces;
using Tail.Services.LocalStorage;
using Tail.Services.OnlineServices;
using Tail.Services.ServiceProviders;
using Tail.Validators;
using Tail.Validators.Rules;
using Tail.Views;
using Xamarin.Forms;


namespace Tail.ViewModels
{
    public class LoginViewModel : PageViewModelBase
    {
        #region private members
        string _rememberMeCheckBox;
        bool _checkIsSelected;

        Command _login;
        Command _facebookLogin;
        Command _appleLogin;
        Command _forgotPassword;
        Command _signUp;
        Command _rememberMe;
        readonly IAppleSignInService appleSignInService;
        readonly IKeyChainAccessService keyChainAccessService;
        readonly IFacebookService _faceBookService;

        #endregion
        #region Public members

        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();


        public string RememberMeCheckBox
        {
            get => _rememberMeCheckBox;
            set => SetProperty(ref _rememberMeCheckBox, value);
        }

        public bool CheckIsSelected
        {
            get => _checkIsSelected;
            set => SetProperty(ref _checkIsSelected, value);
        }

        public Action<String> SetFocus { get; set; }

        public Command Login => _login ?? (_login = new Command(async () => await Handle_Login()));
        public Command FacebookLogin => _facebookLogin ?? (_facebookLogin = new Command(async () => await Handle_FacebookLogin()));
        public Command AppleLogin => _appleLogin ?? (_appleLogin = new Command(async () => await Handle_AppleLogin()));
        public Command ForgotPassword => _forgotPassword ?? (_forgotPassword = new Command(async () => await Handle_ForgotPassword()));
        public Command SignUp => _signUp ?? (_signUp = new Command(async () => await Handle_SignUp()));
        public Command RememberMe => _rememberMe ?? (_rememberMe = new Command(() => Handle_RememberMe()));

        public event EventHandler SignIn;
        public void InvokeSignInEvent(object sender, EventArgs e)
            => SignIn?.Invoke(sender, e);

        #endregion



        public LoginViewModel()
        {
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            keyChainAccessService = DependencyService.Get<IKeyChainAccessService>();
            try
            {
                RememberMeCheckBox = Constants.CHECKBOX_DEFAULT;
                CheckIsSelected = SettingsService.Instance.IsSavedCredentials;
                AddValidationRules();
                if (CheckIsSelected && SettingsService.Instance.LoggedUserDetails.Login_type == LoginType.Email && SettingsService.Instance.LoggedUserDetails != null)
                {
                    Email.Value = SettingsService.Instance.LoggedUserDetails.Email;
                    if (!string.IsNullOrEmpty(SettingsService.Instance.LoggedUserDetails.Password))
                        Password.Value = TailUtils.Decrypt(SettingsService.Instance.LoggedUserDetails.Password, Constants.ENCRPTION_KEY);
                    RememberMeCheckBox = Constants.CHECKBOX_SELECTED;
                }
                IFacebookClient _facebookClient;
                _facebookClient = new FacebookClient();
                _faceBookService = new FacebookService(_facebookClient);
            }
            catch (Exception ex)
            {
                Task.Run(async () => await ShowAlert(AppResources.AppName, "Error While Open Login. \nERROR : " + ex.Message));
            }
            finally
            {
                IsBusy = false;
            }
            Task.Run(async () => await GetAppversionAsync());
        }
        public async Task GetAppversionAsync()
        {
            var response = await TailDataServiceProvider.Instance.GetAppMinimumVersion();
            if (response.ResponseData > Constants.CurrentAppVersionNumber)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await PopupNavigation.Instance.PushAsync(new ForceUpdatePopup());
                });
            }
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterEmail });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = AppResources.ValidEmail });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterPassword });

        }


        async Task Handle_Login()
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
                if (await VerifyLogin())
                {
                    var userDetails = SettingsService.Instance.LoggedUserDetails;
                    if (userDetails.IsSportsAdded)
                        await NavigationService.SetTabAsMainPageAsync<Home>();
                    else
                        await NavigationService.NavigateToAsync<SelectInterest>();

                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }

        }
        private async Task Handle_AppleLogin()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
                var account = await appleSignInService.SignInAsync();
                if (account != null)
                {
                    if (account.Email == null)
                    {
                        var storedAccount = keyChainAccessService.GetStoredAccount("AppleData");
                        if (storedAccount.UserId == account.UserId)
                        {

                            if (await VerifyAppleLogin(account.UserId, storedAccount))
                            {
                                var userDetails = SettingsService.Instance.LoggedUserDetails;
                                if (userDetails.IsSportsAdded)
                                    await NavigationService.SetTabAsMainPageAsync<Home>();
                                else
                                    await NavigationService.NavigateToAsync<SelectInterest>();
                            }
                        }
                        else
                        {
                            if (await VerifyAppleLogin(account.UserId, account))
                            {
                                var userDetails = SettingsService.Instance.LoggedUserDetails;
                                if (userDetails.IsSportsAdded)
                                    await NavigationService.SetTabAsMainPageAsync<Home>();
                                else
                                    await NavigationService.NavigateToAsync<SelectInterest>();
                            }


                        }
                    }
                    else
                    {
                        var saveStatus = keyChainAccessService.SaveDetailsToKeychain(account);
                        Debug.WriteLine(saveStatus);

                        if (await VerifyAppleLogin(account.UserId, account))
                            await NavigationService.SetTabAsMainPageAsync<Home>();
                    }


                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsBusy = false;
        }
        async Task Handle_FacebookLogin()
        {
            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);
        }

        async Task Handle_ForgotPassword()
        {
            await NavigationService.NavigateToAsync<ForgotPassword>();
        }
        async Task Handle_SignUp()
        {
            await NavigationService.NavigateToAsync<NewAccount>();
        }
        void Handle_RememberMe()
        {
            CheckIsSelected = !CheckIsSelected;

            RememberMeCheckBox = (CheckIsSelected) ? Constants.CHECKBOX_SELECTED : Constants.CHECKBOX_DEFAULT;
        }

        public bool IsValid()
        {
            bool _isEmailValid = Email.Validate();
            bool _isPasswordValid = Password.Validate();
            if (!_isEmailValid)
            {
                SetFocus("Email");
            }
            else if (!_isPasswordValid)
            {
                SetFocus("Password");
            }
            return _isEmailValid && _isPasswordValid;
        }

        async Task<bool> VerifyLogin()
        {
            bool hasSuccessResponse = false;
          
            if (string.Compare(Device.RuntimePlatform, Device.iOS) != 0 && string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
            {
                DependencyService.Get<IDeviceHelper>().UpdateToken();
            }
            if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
            try
            {
                LoginRequestInfo requestObj = new LoginRequestInfo()
                {
                    emailId = Email.Value,
                    password = Password.Value,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE
                };
                var loginResponse = await TailDataServiceProvider.Instance.VerifyLogin(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    SettingsService.Instance.IsSavedCredentials = CheckIsSelected;
                    SettingsService.Instance.Authenticated = true;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        Login_type = LoginType.Email,
                        CountryCode = loginResponse.ResponseData.CountryCode,
                        Password = TailUtils.Encrypt(Password.Value, Constants.ENCRPTION_KEY),
                        IsSportsAdded = Convert.ToBoolean(loginResponse.ResponseData.IsSportsAdded),

                    };
                    SettingsService.Instance.NotificationCount = loginResponse.ResponseData.UnreadMsgCount;
                    SettingsService.Instance.LoggedUserDetails = _userDetails;

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
                else
                {
                    Password.Value = "";
                    await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        async Task<bool> VerifyAppleLogin(string _appleToken, AppleAccount account)
        {
            bool hasSuccessResponse = false;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) != 0 && string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
            {
                DependencyService.Get<IDeviceHelper>().UpdateToken();
            }
            if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
            try
            {
                AppleLoginRequestInfo requestObj = new AppleLoginRequestInfo()
                {
                    appleToken = _appleToken,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    email = account.Email,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE
                };
                var loginResponse = await TailDataServiceProvider.Instance.VerifyAppleLogin(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    SettingsService.Instance.Authenticated = true;
                    SettingsService.Instance.IsSavedCredentials = false;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        CountryCode = loginResponse.ResponseData.CountryCode,
                        Login_type = LoginType.Apple,
                        IsSportsAdded = Convert.ToBoolean(loginResponse.ResponseData.IsSportsAdded),

                    };
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                    SettingsService.Instance.NotificationCount = loginResponse.ResponseData.UnreadMsgCount;

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
                else
                {
                    if (loginResponse.ErrorCode == Constants.INVALID_LOGIN_ERROR)
                    {
                        SignUpEventArgs args = new SignUpEventArgs()
                        {
                            ID = account.UserId,
                            Name = account.FirstName + " " + account.LastName,
                            Email = account.Email,
                            ProfileImage = "",
                            Token = account.Token,
                            LoginType = LoginType.Apple
                        };
                        await NavigationService.NavigateToAsync<NewAccount>(args);
                    }
                    else
                        await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public async Task GetFbProfile(string accessToken)
        {
            try
            {

                var fbData = await _faceBookService.GetAccountAsync(accessToken);
                if (fbData != null)
                {
                    SettingsService.Instance.FacebookAccessToken = accessToken;
                    if (IsBusy)
                        return;
                    IsBusy = true;
                    if (await VerifyFBLogin(fbData.Id, fbData))
                    {
                        var userDetails = SettingsService.Instance.LoggedUserDetails;
                        if (userDetails.IsSportsAdded)
                            await NavigationService.SetTabAsMainPageAsync<Home>();
                        else
                            await NavigationService.NavigateToAsync<SelectInterest>();
                    }


                    IsBusy = false;
                }

            }
            catch (Exception exe)
            {
                IsBusy = false;
                Debug.WriteLine("Fb  Error=" + exe.InnerException.ToString());
            }
        }
        async Task<bool> VerifyFBLogin(string _fbToken, FbAccount account)
        {
            bool hasSuccessResponse = false;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) != 0 && string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
            {
                DependencyService.Get<IDeviceHelper>().UpdateToken();
            }
            if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;
            try
            {
                FBLoginRequestInfo requestObj = new FBLoginRequestInfo()
                {
                    fbToken = _fbToken,
                    email = account.Email,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE
                };
                var loginResponse = await TailDataServiceProvider.Instance.VerifyFBLogin(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    SettingsService.Instance.Authenticated = true;
                    SettingsService.Instance.IsSavedCredentials = false;
                    UserInfo _userDetails = new UserInfo()
                    {
                        UserId = loginResponse.ResponseData.UserId,
                        UserName = loginResponse.ResponseData.UserName,
                        UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                        AccessToken = loginResponse.ResponseData.AccessToken,
                        RefreshToken = loginResponse.ResponseData.RefreshToken,
                        Email = loginResponse.ResponseData.Email,
                        CountryCode = loginResponse.ResponseData.CountryCode,
                        Login_type = LoginType.FB,
                        IsSportsAdded = Convert.ToBoolean(loginResponse.ResponseData.IsSportsAdded),

                    };
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                    SettingsService.Instance.NotificationCount = loginResponse.ResponseData.UnreadMsgCount;
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
                else
                {
                    if (loginResponse.ErrorCode == Constants.INVALID_LOGIN_ERROR)
                    {

                        SignUpEventArgs args = new SignUpEventArgs()
                        {
                            ID = account.Id,
                            Name = account.Name,
                            Email = account.Email,
                            ProfileImage = account.ProfileImage,
                            Token = account.AccessToken,
                            LoginType = LoginType.FB
                        };
                        await NavigationService.NavigateToAsync<NewAccount>(args);
                    }
                    else
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
