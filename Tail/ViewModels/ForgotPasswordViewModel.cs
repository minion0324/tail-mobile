using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ServiceProviders;
using Tail.Validators;
using Tail.Validators.Rules;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ForgotPasswordViewModel : PageViewModelBase
    {

        Command _loginCommand;
        Command _signUpCommand;
        Command _sendCommand;

        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(async () => await Handle_LoginCommand()));
        public Command SignUpCommand => _signUpCommand ?? (_signUpCommand = new Command(async () => await Handle_SignUpCommand()));
        public Command SendCommand => _sendCommand ?? (_sendCommand = new Command(async () => await Handle_SendCommand()));

        public Action OnSendMailError
        {
            get;
            set;
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = AppResources.EnterEmail });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = AppResources.ValidEmail });

        }
        public ForgotPasswordViewModel()
        {
            AddValidationRules();
        }
        
        async Task Handle_LoginCommand()
        {
            await NavigationService.NavigateToAsync<Login>();
        }
        async Task Handle_SignUpCommand()
        {
            await NavigationService.NavigateToAsync<NewAccount>();

        }
        async Task Handle_SendCommand()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;

                if (!Email.Validate())
                {
                    IsBusy = false;
                    return;
                }
                if(await SendMail())
                    await NavigationService.NavigateToAsync<Login>();

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
        }
       
        async Task<bool> SendMail()
        {
            bool hasSuccessResponse = false;

            try
            {
                ForgotPasswordRequestInfo requestObj = new ForgotPasswordRequestInfo()
                {
                    emailId = Email.Value,
                };
                var loginResponse = await TailDataServiceProvider.Instance.ForgotPassword(requestObj);
                if (loginResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                    await ShowAlert(AppResources.AppName, loginResponse.Message);
                }
                else
                {
                    
                        await NavigationService.ShowAlertAsync(AppResources.AppName, loginResponse.Message);
                        OnSendMailError?.Invoke();

                       
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
