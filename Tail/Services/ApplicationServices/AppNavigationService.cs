using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Tail.Services.Interfaces;
using Tail.Common;
using Tail.ViewModels;
using Tail.Views;
using System.Linq;
using System.Diagnostics;
using Tail.Models;
using Tail.Services.ServiceProviders;

namespace Tail.Services.ApplicationServices
{
    public class AppNavigationService : IAppNavigationService
    {
        static IAppNavigationService _instance;

        public static IAppNavigationService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppNavigationService();
            }

            return _instance;
        }

        public async Task InitializeAsync()
        {
            SettingsService.Instance.CurrentTabIndex = 0;
            SettingsService.Instance.IsOtherUserProfile = false;
            if (SettingsService.Instance.Authenticated)
            {
                if (SettingsService.Instance.IsCompletedSignUpProcess)
                {
                    
                    
                        var appPage = Activator.CreateInstance<Home>() as TabbedPage;
                    Application.Current.MainPage = new NavigationPage(appPage)
                    {
                        BarTextColor = Color.White
                    };
                    Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
                    await (appPage.BindingContext as PageViewModelBase).InitializeAsync(null);
                    if (!string.IsNullOrEmpty(CommonSingletonUtility.SharedInstance.DeviceToken))
                    {
                        UpdateDeviceTokenInfo requestObj = new UpdateDeviceTokenInfo()
                        {
                            RefreshToken = SettingsService.Instance.LoggedUserDetails.RefreshToken,
                            DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken
                        };
                        
                        SettingsService.Instance.DeviceToken = CommonSingletonUtility.SharedInstance.DeviceToken;

                       await TailDataServiceProvider.Instance.UpdateDeviceToken(requestObj);
                    }
                }
                else
                {
                    await NavigateWithViewModelInit<SelectInterest>(null, true);
                }

            }
            else
            {
               
                await NavigateWithViewModelInit<Login>(null, true);
            }



        }

        public Task SetAsMainPageAsync<TAppPage>(object parameter = null) where TAppPage : AppPageBase
        {
            SettingsService.Instance.IsOtherUserProfile = false;
            return NavigateWithViewModelInit<TAppPage>(parameter, true);
        }
        public Task SetTabAsMainPageAsync<TAppPage>(object parameter = null) where TAppPage : TabbedPage
        {
            SettingsService.Instance.IsOtherUserProfile = false;
            var appPage = Activator.CreateInstance<Home>() as TabbedPage;
            Application.Current.MainPage = new NavigationPage(appPage)
            {
                BarTextColor = Color.White
            };
            Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            return (appPage.BindingContext as PageViewModelBase).InitializeAsync(null);
        }
        public Task NavigateToAsync<TAppPage>(object parameter = null, bool withViewModelInit = true) where TAppPage : AppPageBase
        {
           
            if (withViewModelInit)
            {
                return NavigateWithViewModelInit<TAppPage>(parameter, false);
            }
            else
            {
                return NavigateWithViewInit<TAppPage>(parameter, false);
            }
        }
        public Task NavigateWithInTabToAsync<TAppPage>(object parameter = null, bool withViewModelInit = true) where TAppPage : AppPageBase
        {
            return NavigateTabWithViewModelInit<TAppPage>(parameter);

        }
        public Task PopLastTabbedPageAsync(bool animated = true)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                var currentPage = navigationPage.Navigation.NavigationStack.Last();
                TabbedPage currentTabbedPage = currentPage as TabbedPage;
                if (currentTabbedPage == null)
                    return Task.FromResult(animated);
                currentTabbedPage.CurrentPage = currentTabbedPage.Children[SettingsService.Instance.CurrentTabIndex];
                return currentTabbedPage.CurrentPage.Navigation.PopAsync();
            }
            else
            {
                return Task.FromResult(animated);
            }
        }
        public async Task PushModalAsync<TAppPage>(object parameter = null) where TAppPage : AppPageBase
        {
            var appPage = Activator.CreateInstance<TAppPage>() as AppPageBase;

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.Navigation.PushModalAsync(appPage);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(appPage)
                {
                    BarTextColor = Color.White
                };
                Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
            }

            await (appPage.BindingContext as PageViewModelBase).InitializeAsync(parameter);
        }

        public Task PopToMainPageAsync(bool animated = true)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                return navigationPage.Navigation.PopToRootAsync(animated);
            }
            else
            {
                return Task.FromResult(animated);
            }
        }

        public Task PopLastPageAsync(bool animated = true)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {

                return navigationPage.Navigation.PopAsync(animated);
            }
            else
            {
                return Task.FromResult(animated);
            }
        }

        public Task PopLastTwoPageAsync(bool animated = true)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                var currentPage = Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 2];
                navigationPage.Navigation.RemovePage(currentPage);
                return navigationPage.Navigation.PopAsync(animated);
            }
            else
            {
                return Task.FromResult(animated);
            }
        }

        public Task PopModalAsync(bool animated = true)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                return navigationPage.Navigation.PopModalAsync(true);
            }
            else
            {
                return Task.FromResult(true);
            }
        }

        public async Task ShowAlertAsync(string title, string message)
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1];

            if (currentPage != null)
            {
                string okText = AppResources.OKText;
                if (currentPage.BindingContext is PageViewModelBase pageViewModelBase)
                {
                    okText = pageViewModelBase.Resources["OKText"];
                }
                await currentPage.DisplayAlert(title, message, okText);
            }
        }

        public async Task<bool> ShowConfirmAlertAsync(string title, string message)
        {
            bool confirm = false;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1];

            if (currentPage != null)
            {
                string yesText = AppResources.YesText;
                string noText = AppResources.NoText;
                if (currentPage.BindingContext is PageViewModelBase pageViewModelBase)
                {
                    yesText = pageViewModelBase.Resources["YesText"];
                    noText = pageViewModelBase.Resources["NoText"];
                }

                confirm = await currentPage.DisplayAlert(title, message, yesText, noText);
            }
            return confirm;
        }

        public async Task<bool> ShowCustomConfirmAlertAsync(string title, string message, string acceptText, string cancelText)
        {
            bool confirm = false;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1];
            if (currentPage != null)
            {
                confirm = await currentPage.DisplayAlert(title, message, acceptText, cancelText);
            }
            return confirm;
        }

        public async Task<string> ShowActionSheetAsync(string title, string cancel, string destructions, params string[] buttons)
        {
            var action = string.Empty;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1];

            if (currentPage != null)
            {
                action = await currentPage.DisplayActionSheet(title, cancel, destructions, buttons);
            }
            return action;
        }

        async Task NavigateWithViewModelInit<TAppPage>(object parameter = null, bool mainPage = false) where TAppPage : AppPageBase
        {

            try
            {
                var appPage = Activator.CreateInstance<TAppPage>() as AppPageBase;

                if (mainPage)
                {
                    Application.Current.MainPage = new NavigationPage(appPage)
                    {
                        BarTextColor = Color.Black
                    };
                    Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
                }
                else
                {
                    
                    if (Application.Current.MainPage is NavigationPage navigationPage)
                    {
                        for (int i = 0; i < navigationPage.Navigation.NavigationStack.Count; i++)
                        {
                            if (navigationPage.Navigation.NavigationStack[i].GetType() == appPage.GetType())
                            {
                                navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[i]);
                                break;
                            }
                        }

                        await navigationPage.Navigation.PushAsync(appPage);

                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(appPage)
                        {
                            BarTextColor = Color.Black
                        };
                        Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
                    }
                    await (appPage.BindingContext as PageViewModelBase).InitializeAsync(parameter);
                }

               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
        }
        async Task NavigateTabWithViewModelInit<TAppPage>(object parameter = null) where TAppPage : AppPageBase
        {
            
                var appPage = Activator.CreateInstance<TAppPage>() as AppPageBase;
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    var currentPage = navigationPage.Navigation.NavigationStack.Last();
                    TabbedPage currentTabbedPage = currentPage as TabbedPage;
                    if (currentTabbedPage == null)
                        return;
                    currentTabbedPage.CurrentPage = currentTabbedPage.Children[SettingsService.Instance.CurrentTabIndex];
                    await currentTabbedPage.CurrentPage.Navigation.PushAsync(appPage);
                }
                await (appPage.BindingContext as PageViewModelBase).InitializeAsync(parameter);
           
        }
        async Task NavigateWithViewInit<TAppPage>(object parameter = null, bool mainPage = false) where TAppPage : AppPageBase
        {
            var appPage = Activator.CreateInstance(typeof(TAppPage), parameter) as AppPageBase;

            if (mainPage)
            {
                Application.Current.MainPage = new NavigationPage(appPage)
                {
                    BarTextColor = Color.Black
                };
                Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
            }
            else
            {
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    for (int i = 0; i < navigationPage.Navigation.NavigationStack.Count; i++)
                    {
                        if (navigationPage.Navigation.NavigationStack[i].GetType() == appPage.GetType())
                        {
                            navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[i]);
                            break;
                        }
                    }
                    await navigationPage.Navigation.PushAsync(appPage);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(appPage)
                    {
                        BarTextColor = Color.Black
                    };
                    Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
                }
            }
        }
    }
}
