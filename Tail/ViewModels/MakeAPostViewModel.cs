using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tail.Services.ApplicationServices;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class MakeAPostViewModel : PageViewModelBase
    {
        #region Private members
        Command _postSomethingCommand;
        Command _postYourPickCommand;
        Action _popupCloseCallback;
        #endregion

        public MakeAPostViewModel()
        {

        }

        #region Public members

        
        public Action PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }

        public Command PostSomethingCommand => _postSomethingCommand ?? (_postSomethingCommand = new Command(async () => await Handle_PostSomethingCommand()));
        public Command PostYourPickCommand => _postYourPickCommand ?? (_postYourPickCommand = new Command(async () => await Handle_PostYourPickCommand()));
        #endregion

        async Task Handle_PostSomethingCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PopupCloseCallback?.Invoke();
            SettingsService.Instance.CurrentTabIndex = 0;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            NavigationPage page = currentTabbedPage.CurrentPage as NavigationPage;
            
            Debug.WriteLine(currentTabbedPage.CurrentPage);
            Debug.WriteLine(page.CurrentPage);
            if(page.CurrentPage.ToString() != "Tail.Views.CreateAPost")
            {
                await NavigationService.NavigateWithInTabToAsync<CreateAPost>();
            }
            IsBusy = false;
        }
        async Task Handle_PostYourPickCommand()
        {
            
            if (IsBusy)
                return;
            IsBusy = true;
            PopupCloseCallback?.Invoke();
            SettingsService.Instance.CurrentTabIndex = 2;
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            NavigationPage page = currentTabbedPage.CurrentPage as NavigationPage;

            Debug.WriteLine(currentTabbedPage.CurrentPage);
            Debug.WriteLine(page.CurrentPage);
            if (page.CurrentPage.ToString() != "Tail.Views.PostYourPickModified")
            {
                await NavigationService.NavigateWithInTabToAsync<PostYourPickModified>();
            }
            IsBusy = false;
        }
    }
}
