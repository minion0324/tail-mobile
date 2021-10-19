using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.ServiceProviders;
using Tail.Validators;
using Tail.Validators.Rules;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ChangePasswordPopupViewModel:PageViewModelBase
    {
        #region private members

       Command _donebtnCommand;


        #endregion
        #region Public members

        Action _popupCloseCallback;
        public Action PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }
        public Command DoneButtonCommand => _donebtnCommand ?? (_donebtnCommand = new Command(async () => await Handle_DoneButtonCommand()));


        #endregion


        public ValidatableObject<string> CurrentPassword { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> NewPassword { get; set; } = new ValidatableObject<string>();
        public ChangePasswordPopupViewModel()
        {
            AddValidationRules();
        }

        public Action<String> SetFocus { get; set; }
        public bool IsValid()
        {

            bool _isCurrentPaswdValid = CurrentPassword.Validate();
            bool _isNewPaswdValid = NewPassword.Validate();
            if (!_isCurrentPaswdValid)
            {
                SetFocus("CurrentPassword");
            }
            else if (!_isNewPaswdValid)
            {
                SetFocus("NewPassword");
            }
            return _isCurrentPaswdValid && _isNewPaswdValid;

        }

        /// <summary>
        /// Handle_DoneButtonCommand
        /// </summary>
        /// <returns></returns>
        async Task Handle_DoneButtonCommand()
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
                    if (await ChangePassword())
                    {
                        PopupCloseCallback?.Invoke();
                    }
                       

                IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    await ShowAlert(AppResources.AppName, ex.Message);
                }
            finally
            {
                IsBusy = false;
            }
            
           
           
        }
        public void AddValidationRules()
        {
            CurrentPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterCurrentPassword });
            NewPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterNewPassword });
            CurrentPassword.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = AppResources.ValidPassword });
            NewPassword.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = AppResources.ValidPassword });
        }

            /// <summary>
            /// ChangePassword
            /// </summary>
            /// <returns></returns>
            async Task<bool> ChangePassword()
        {
            bool hasSuccessResponse = false;

            try
            {
                
                UpdatePasswordInfo requestObj = new UpdatePasswordInfo()
                {
                    currentPassword = CurrentPassword.Value,
                    newPassword =NewPassword.Value
                };
                var updateResponse = await TailDataServiceProvider.Instance.UpdatePassword(requestObj);
                if (updateResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                    await ShowAlert(AppResources.AppName, updateResponse.Message);
                    UserInfo _userDetails = SettingsService.Instance.LoggedUserDetails;
                    _userDetails.Password = TailUtils.Encrypt(requestObj.newPassword, Constants.ENCRPTION_KEY) ;
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                }
                else
                {
                    if (updateResponse.ErrorCode != Constants.REFRESH_TOKEN_ERROR)
                        await NavigationService.ShowAlertAsync(AppResources.AppName, updateResponse.Message);
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
