
using System.Threading.Tasks;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class AddAccountDetailsViewModel : PageViewModelBase
    {

        Command _addAccountDetailsCommand;
        public AddAccountDetailsViewModel()
        {
        }
        public Command AddAccountDetailsCommand => _addAccountDetailsCommand ?? (_addAccountDetailsCommand = new Command(async () => await Handle_AddAccountDetailsCommandAsync()));
        private async Task Handle_AddAccountDetailsCommandAsync()
        {
            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);
        }
    }
}
