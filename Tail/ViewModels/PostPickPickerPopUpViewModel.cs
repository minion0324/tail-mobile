using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PostPickPickerPopUpViewModel: PageViewModelBase
    {
        Command _doneCommand;
        Action _itemSelectedCallback;
        public Action ItemSelectedCallback
        {
            get => _itemSelectedCallback;
            set => SetProperty(ref _itemSelectedCallback, value);
        }
        public List<NewPickerItem> PickerItemsList
        {
            get;
            set;
        }
        public string TitleText
        {
            get;
            set;
        }
        public PostPickPickerPopUpViewModel()
        {
        }
        public Command DoneCommand => _doneCommand ?? (_doneCommand = new Command(async () => await Handle_DoneCommand()));

        private async Task Handle_DoneCommand()
        {
            ItemSelectedCallback.Invoke();
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
