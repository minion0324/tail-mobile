using System.Threading.Tasks;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.Services.Interfaces
{
    public interface IAppNavigationService
    {
        Task InitializeAsync();

        Task SetAsMainPageAsync<TAppPage>(object parameter = null) where TAppPage : AppPageBase;
        Task NavigateToAsync<TAppPage>(object parameter = null, bool withViewModelInit = true) where TAppPage : AppPageBase;
        Task PushModalAsync<TAppPage>(object parameter = null) where TAppPage : AppPageBase;
        Task NavigateWithInTabToAsync<TAppPage>(object parameter = null, bool withViewModelInit = true) where TAppPage : AppPageBase;
        Task SetTabAsMainPageAsync<TAppPage>(object parameter = null) where TAppPage : TabbedPage;


        Task PopToMainPageAsync(bool animated = true);
        Task PopLastPageAsync(bool animated = true);
        Task PopModalAsync(bool animated = true);
        Task PopLastTwoPageAsync(bool animated = true);
        Task PopLastTabbedPageAsync(bool animated = true);

        Task ShowAlertAsync(string title, string message);
        Task<bool> ShowConfirmAlertAsync(string title, string message);
        Task<bool> ShowCustomConfirmAlertAsync(string title, string message, string acceptText, string cancelText);
        Task<string> ShowActionSheetAsync(string title, string cancel, string destructions, params string[] buttons);
    }
}

