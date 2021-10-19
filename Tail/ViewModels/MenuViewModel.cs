 using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class MenuViewModel : PageViewModelBase
    {
        Command _menuItemTapCommand;
        private ObservableCollection<MenuModel> _menuItems;
        public ObservableCollection<MenuModel> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }
        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<MenuModel>
            {
                new MenuModel { Name = "Home" },
                new MenuModel { Name = "Interests" },
                new MenuModel { Name = "Settings" },
                new MenuModel { Name = "Coins" },
                new MenuModel { Name = "My Profile" },
                new MenuModel { Name = "Support" },
                new MenuModel { Name = "Logout" },
                new MenuModel { Name = "About" }
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                NotificationCount = SettingsService.Instance.NotificationCount;
            });
        }
        public Command MenuItemTapCommand => _menuItemTapCommand ?? (_menuItemTapCommand = new Command(async (item) =>
        {
            try
            {
                var menuItem = (MenuModel)item;
                await Handle_MenuItemTapCommandAsync(menuItem);
            }
            catch(Exception ex)
            {
               await ShowAlert(AppResources.AppName, "MenuItemTapCommand => " + ex.Message);
            }
        }));

        private async Task Handle_MenuItemTapCommandAsync(MenuModel menuitem)
        {
            try
            {
                var alreadySelectedItem = MenuItems.FirstOrDefault(x => x.IsSelected);
                if (alreadySelectedItem != null)
                    alreadySelectedItem.IsSelected = false;
                var item = MenuItems.FirstOrDefault(x => x.Name == menuitem.Name);
                if (item != null)
                    item.IsSelected = true;

                switch (menuitem.Name)
                {
                    case "Home":
                        {
                            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
                           
                            TabbedPage currentTabbedPage = currentPage as TabbedPage;
                            if (currentTabbedPage != null)
                                currentTabbedPage.CurrentPage = currentTabbedPage.Children[0];
                            break;
                        }
                    case "Interests":
                        CommonSingletonUtility.SharedInstance.IsFromMenu = true;
                        await NavigationService.NavigateWithInTabToAsync<SelectInterest>();
                        break;
                    case "Settings":
                        await NavigationService.NavigateWithInTabToAsync<Settings>();
                        break;
                    case "Coins":
                        await NavigationService.NavigateWithInTabToAsync<Coins>();
                        break;
                    case "My Profile":
                        {
                            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
                            SettingsService.Instance.CurrentTabIndex = 3;
                            TabbedPage currentTabbedPage = currentPage as TabbedPage;
                            if (currentTabbedPage != null)
                                currentTabbedPage.CurrentPage = currentTabbedPage.Children[3];
                         
                            break;
                        }
                    case "Support":
                        await NavigationService.NavigateWithInTabToAsync<ContactUs>();
                        break;
                    case "Logout":
                        Logout.Execute(null);
                        break;
                    case "About":
                        await NavigationService.NavigateWithInTabToAsync<AboutUs>();
                        break;
                }
            }
            
            catch(Exception ex)
            {
               await ShowAlert(AppResources.AppName,"Handle_MenuItemTapCommandAsync => " + ex.Message);
            }
}
    }
}
