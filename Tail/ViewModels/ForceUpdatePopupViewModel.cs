using System;
using Tail.Common;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ForceUpdatePopupViewModel : PageViewModelBase
    {
        Command _updateCommand;
        public ForceUpdatePopupViewModel()
        {
        }
        public Command UpdateCommand => _updateCommand ?? (_updateCommand = new Command( () =>  Handle_UpdateCommand()));

        private void Handle_UpdateCommand()
        {
            if (Device.RuntimePlatform == Device.iOS)
                Launcher.OpenAsync(new Uri(Constants.AppstoreLink));
            else
                Launcher.OpenAsync(new Uri(Constants.PlayStoreLink));
        }
    }
}
