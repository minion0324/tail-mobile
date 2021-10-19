using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Views;

namespace Tail.ViewModels
{
    public class NearByUsersViewModel : PageViewModelBase
    {
     
        public NearByUsersViewModel()
        {
            GetNearByUsers();
          
        }

        private void GetNearByUsers()
        {
            try
            {
                if (IsBusy)
                    return;
                IsBusy = true;
               
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
            
        }
        public async Task Handle_UserDetailsWithoutTabCommand(int userID)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            SettingsService.Instance.IsOtherUserProfile = true;
            await NavigationService.NavigateWithInTabToAsync<MyProfile>(userID);
            IsBusy = false;
        }
    }
}
