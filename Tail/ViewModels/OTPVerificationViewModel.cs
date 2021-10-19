using System;
using System.Threading;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.LocalStorage;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class OtpVerificationViewModel : PageViewModelBase
    {
        #region private members
        string _enteredOTP;
        string _firstCharOTP;
        string _secondCharOTP;
        string _thirdCharOTP;
        string _fourthCharOTP;
        string _timerTime;
        string _otpExpireText;
        bool _isTimerVisible;
        bool _isFromSignup = true;
        Command _resendCodeCommand;
        Command _loginCommand;

        CancellationTokenSource _CancellationTokenSource;

        #endregion
        #region public members
        public string EnteredOTP
        {
            get => _enteredOTP;
            set => SetProperty(ref _enteredOTP, value);
        }
        public string FirstCharOTP
        {
            get => _firstCharOTP;
            set => SetProperty(ref _firstCharOTP, value);
        }
        public string SecondCharOTP
        {
            get => _secondCharOTP;
            set => SetProperty(ref _secondCharOTP, value);
        }
        public string ThirdCharOTP
        {
            get => _thirdCharOTP;
            set => SetProperty(ref _thirdCharOTP, value);
        }
        public string FourthCharOTP
        {
            get => _fourthCharOTP;
            set => SetProperty(ref _fourthCharOTP, value);
        }
        public string TimerTime
        {
            get => _timerTime;
            set => SetProperty(ref _timerTime, value);
        }
        public string OTPExpireText
        {
            get => _otpExpireText;
            set => SetProperty(ref _otpExpireText, value);
        }
        public bool IsFromSignup
        {
            get => _isFromSignup;
            set => SetProperty(ref _isFromSignup, value);
        }
        public bool IsTimerVisible
        {
            get => _isTimerVisible;
            set => SetProperty(ref _isTimerVisible, value);
        }
        public bool IsPageDestroyed
        {
            get ;
            set ;
        }
        #endregion
        public OtpVerificationViewModel()
        {

            _CancellationTokenSource = new CancellationTokenSource();
            IsFromSignup = !SettingsService.Instance.FromEditProfile;
            OTPExpireText = AppResources.OTPWillExpireText;
            IsTimerVisible = true;
            OtpSmsServices.ListenToSmsRetriever();
            TimerStart();
        }

        public Command ResendCodeCommand => _resendCodeCommand ?? (_resendCodeCommand = new Command(async () => await Handle_ResendCodeCommand()));
        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(async () => await Handle_LoginCommand()));

        async Task Handle_ResendCodeCommand()
        {
            OtpSmsServices.ListenToSmsRetriever();
            await ResendOTP();
        }
        async Task Handle_LoginCommand()
        {
            await NavigationService.NavigateToAsync<Login>();
        }

        public async Task ValidateOTP()
        {
            if((FirstCharOTP != null && FirstCharOTP.Trim() != string.Empty) && (SecondCharOTP != null && SecondCharOTP.Trim() != string.Empty) && (ThirdCharOTP != null && ThirdCharOTP.Trim() != string.Empty) && (FourthCharOTP != null && FourthCharOTP.Trim() != string.Empty))
            {
                string _fullOTP = FirstCharOTP + SecondCharOTP + ThirdCharOTP + FourthCharOTP;
                CommonSingletonUtility.SharedInstance.IsFromMenu = false;
                if (await VerifyOTP(_fullOTP))
                {
                    if (IsFromSignup)
                    {
                        
                        await NavigationService.NavigateToAsync<SelectInterest>();
                       
                    }
                    else
                    {
                        SettingsService.Instance.FromEditProfile = false;
                       
                        await NavigationService.PopLastPageAsync();
                        await NavigationService.PopLastTabbedPageAsync();
                    }
                    
                }
                   
               
            }
          
        }

        async Task<bool> VerifyOTP(string OTPValue)
        {
            if (IsBusy)
                return false;
            IsBusy = true;
            bool hasSuccessResponse = false;

            try
            {

                OtpRequestInfo requestObj = new OtpRequestInfo()
                {
                    OTP= OTPValue,
                    phone= (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.OtpPhoneNumber))? CommonSingletonUtility.SharedInstance.OtpPhoneNumber : string.Empty,
                    userId =(SettingsService.Instance.LoggedUserDetails != null)? SettingsService.Instance.LoggedUserDetails.UserId.ToString() : string.Empty,
                    deviceToken = SettingsService.Instance.DeviceToken,
                    deviceType = (string.Compare(Device.RuntimePlatform, Device.iOS) == 0) ? Constants.IOS_TYPE : Constants.ANDROID_TYPE,
                    country= (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.OtpCountryCode)) ? CommonSingletonUtility.SharedInstance.OtpCountryCode : string.Empty,

                };
                var loginResponse = await TailDataServiceProvider.Instance.VerifyOTP(requestObj);
                if (loginResponse.ErrorCode == 200 && loginResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    SettingsService.Instance.Authenticated = true;
                    SettingsService.Instance.PartialRegstarionUserID = 0;
                    if (IsFromSignup)
                    {

                        UserInfo _userDetails = new UserInfo()
                        {
                            UserId = loginResponse.ResponseData.UserId,
                            UserName = loginResponse.ResponseData.UserName,
                            UserImage = (string.IsNullOrEmpty(loginResponse.ResponseData.UserImage)) ? Constants.LOGGEDIN_USERIMAGE : loginResponse.ResponseData.UserImage,
                            AccessToken = loginResponse.ResponseData.AccessToken,
                            RefreshToken = loginResponse.ResponseData.RefreshToken,
                            Email = loginResponse.ResponseData.Email,
                            Phone = loginResponse.ResponseData.Phone,
                            CountryCode = loginResponse.ResponseData.CountryCode,
                            Login_type = (SettingsService.Instance.LoggedUserDetails != null)?SettingsService.Instance.LoggedUserDetails.Login_type: LoginType.Email,
                            Password = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.Password : ""
                        };
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
                }
                else
                {
                    FirstCharOTP = string.Empty;
                    SecondCharOTP = string.Empty;
                    ThirdCharOTP = string.Empty;
                    FourthCharOTP = string.Empty;
                    EnteredOTP = string.Empty;
                    await ShowAlert(AppResources.AppName, loginResponse.Message);
                  
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsBusy = false;
            return hasSuccessResponse;

        }

        async Task<bool> ResendOTP()
        {
            if (IsBusy)
                return false;
            IsBusy = true;
            bool hasSuccessResponse = false;

            try
            {

                OtpResendInfo requestObj = new OtpResendInfo()
                {
                    userId = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.UserId.ToString() : string.Empty,
                    phone = (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.OtpPhoneNumber)) ? CommonSingletonUtility.SharedInstance.OtpPhoneNumber : string.Empty,
                    country = (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.OtpCountryCode)) ? CommonSingletonUtility.SharedInstance.OtpCountryCode : string.Empty,
                };
                var loginResponse = await TailDataServiceProvider.Instance.ResendOTP(requestObj);
                if (loginResponse.ErrorCode == 200 )
                {
                    TimerStop();
                    
                    hasSuccessResponse = true;
                    TimerStart();
                    OTPExpireText = AppResources.OTPWillExpireText;
                    IsTimerVisible = true;
                    await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                }
                else
                {
                    await ShowAlert(AppResources.AppName, loginResponse.Message);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsBusy = false;
            return hasSuccessResponse;

        }

        private void TimerStart()
        {
            int Min = 7;
            int Sec = 00;
            int TotalSec = (Min * 60) + Sec;

            CancellationTokenSource CTS = _CancellationTokenSource;

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (CTS.IsCancellationRequested)
                {
                    return false;
                }
                else
                {
                    if (TotalSec == 0)
                    {
                        OTPExpireText = AppResources.OTPExpiredText;
                        IsTimerVisible = false;
                        return false;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        TotalSec = TotalSec - 1;
                        TimeSpan _TimeSpan = TimeSpan.FromSeconds(TotalSec);
                        TimerTime = string.Format("{0:00}:{1:00}", _TimeSpan.Minutes, _TimeSpan.Seconds);
                    });
                    return true;
                }
            });
        }
        private void TimerStop()
        {
            Interlocked.Exchange(ref _CancellationTokenSource, new CancellationTokenSource()).Cancel();
        }
    }
}
