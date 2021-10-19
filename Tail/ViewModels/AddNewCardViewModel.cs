using System.Threading.Tasks;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class AddNewCardViewModel : PageViewModelBase
    {
        Command _addNewCardCommand;
        public AddNewCardViewModel()
        {
        }
        public Command AddNewCardCommand => _addNewCardCommand ?? (_addNewCardCommand = new Command(async () => await Handle_AddNewCardCommandAsync()));
        private async Task Handle_AddNewCardCommandAsync()
        {
            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);
        }
    }
}
