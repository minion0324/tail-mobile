
﻿using System.Threading.Tasks;
using Tail.Common;
﻿using System;
using System.Diagnostics;
using Newtonsoft.Json; 
using Tail.Helpers;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.Services.OnlineServices;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class SignUpViewModel : PageViewModelBase
    {
        #region Private members
        Command _signUpWithEmailCommand;
        Command _signUpWithAppleCommand;
        Command _loginCommand;
        readonly IFacebookService _faceBookService;
      
        readonly IAppleSignInService appleSignInService;
        readonly IKeyChainAccessService keyChainAccessService;
        #endregion

        #region Public members
        public Command SignUpWithEmailCommand => _signUpWithEmailCommand ?? (_signUpWithEmailCommand = new Command(async () => await Handle_SignUpWithEmailCommand()));
        public Command SignUpWithAppleCommand => _signUpWithAppleCommand ?? (_signUpWithAppleCommand = new Command(async () => await Handle_SignUpWithAppleCommand()));
        public Command LoginCommand => _loginCommand ?? (_loginCommand = new Command(async () => await Handle_LoginCommand()));


        #endregion
        #region Constructor
        public SignUpViewModel()
        {
            appleSignInService = DependencyService.Get<IAppleSignInService>();
            keyChainAccessService = DependencyService.Get<IKeyChainAccessService>();
             IFacebookClient _facebookClient;
            _facebookClient = new FacebookClient();
            _faceBookService = new FacebookService(_facebookClient);
        }
        #endregion

        async Task Handle_SignUpWithEmailCommand()
        {
            await NavigationService.NavigateToAsync<NewAccount>();
        }
     
        async Task Handle_SignUpWithAppleCommand()
        {

            if (IsBusy)
                return;
            try
            {
                var account = await appleSignInService.SignInAsync();
                if (account != null)
                {
                    if(account.Email == null)
                    {
                        //already exist
                      var storedAccount = keyChainAccessService.GetStoredAccount("AppleData");
                        if(storedAccount.UserId == account.UserId)
                        {
                            SignUpEventArgs args = new SignUpEventArgs()
                            {
                                ID = storedAccount.UserId,
                                Name = storedAccount.FirstName + account.LastName,
                                Email = storedAccount.Email,
                                ProfileImage = "",
                                Token = storedAccount.Token,
                                LoginType = LoginType.Apple
                            };
                            await NavigationService.NavigateToAsync<NewAccount>(args);
                        }
                        else
                        {
                            SignUpEventArgs args = new SignUpEventArgs()
                            {
                                ID = account.UserId,
                                Name = account.FirstName + account.LastName,
                                Email = account.Email,
                                ProfileImage = "",
                                Token = account.Token,
                                LoginType = LoginType.Apple
                            };
                            await NavigationService.NavigateToAsync<NewAccount>(args);
                        }
                    }
                    else
                    {
                        var saveStatus = keyChainAccessService.SaveDetailsToKeychain(account);
                        Debug.WriteLine(saveStatus);
                        SignUpEventArgs args = new SignUpEventArgs()
                        {
                            ID = account.UserId,
                            Name = account.FirstName + account.LastName,
                            Email = account.Email,
                            ProfileImage = "",
                            Token = account.Token,
                            LoginType = LoginType.Apple
                        };
                        await NavigationService.NavigateToAsync<NewAccount>(args);
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
        async Task Handle_LoginCommand()
        {
            await NavigationService.NavigateToAsync<Login>();
        }
        public async Task GetFbProfile(string accessToken)
        {
            try
            {

                var fbData = await _faceBookService.GetAccountAsync(accessToken);
                if (fbData != null)
                {
                    SettingsService.Instance.FacebookAccessToken = accessToken;
                    fbData.AccessToken = accessToken;
                    FbImageRootData fbPicture = JsonConvert.DeserializeObject<FbImageRootData>(fbData.PictureJson.ToString());
                    fbData.ProfileImage = fbPicture.Picture.Url.ToString();
                   
                    SignUpEventArgs args = new SignUpEventArgs()
                    {
                        ID= fbData.Id,
                        Name = fbData.Name,
                        Email= fbData.Email,
                        ProfileImage=fbData.ProfileImage,
                        Token=fbData.AccessToken,
                        LoginType=LoginType.FB
                    };
                    await NavigationService.NavigateToAsync<NewAccount>(args);
                }

            }
            catch (Exception exe)
            {

                Debug.WriteLine("Fb Image Error=" + exe.InnerException.ToString());
            }
        }

    }
}
