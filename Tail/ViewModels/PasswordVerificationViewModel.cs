using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Validators;
using Tail.Validators.Rules;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PasswordVerificationViewModel : PageViewModelBase
    {
        #region private members

        Command _donebtnCommand;


        #endregion
        #region Public members
        public Action PopupCloseCallback
        {
            get;
            set;
        }


       
        public Command DoneButtonCommand => _donebtnCommand ?? (_donebtnCommand = new Command(async () => await Handle_DoneButtonCommand()));


        #endregion

        
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public PasswordVerificationViewModel()
        {
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterPassword });
          
        }
        public Action SetFocus { get; set; }
        public bool IsValid()
        {

            bool _isPaswdValid = Password.Validate();
          
            if (!_isPaswdValid)
            {
                SetFocus();
            }
           
            return _isPaswdValid;

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
                if (await CheckPassword())
                    PopupCloseCallback?.Invoke();
                else
                {
                    await ShowAlert(AppResources.AppName, AppResources.InValidPassword);
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }



        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <returns></returns>
        async Task<bool> CheckPassword()
        {
            bool hasSuccessResponse = false;

            try
            {
                UserInfo loginInfo = SettingsService.Instance.LoggedUserDetails;
                if (!string.IsNullOrEmpty(loginInfo.Password ))
                {
                    var userPassword = TailUtils.Decrypt(loginInfo.Password, Constants.ENCRPTION_KEY);

                    if (Password.Value == userPassword)

                        return true;
                    else
                        return false;
                }
                else
                {
                    return true;
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

