using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Models;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class PostPickPickerPopUp : PopupPage
    {
        readonly PostPickPickerPopUpViewModel _vModel;
        public PostPickPickerPopUp(List<NewPickerItem> newPickerItems,string title, Action itemSelectedCallBack)
        {
           
            InitializeComponent();
            _vModel = new PostPickPickerPopUpViewModel();
            _vModel.PickerItemsList = newPickerItems;
            _vModel.TitleText = title;
            _vModel.ItemSelectedCallback = itemSelectedCallBack;
            this.BindingContext = _vModel;
            
        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            StackLayout tappedLayout = (StackLayout)sender;
            var layout = (TapGestureRecognizer)tappedLayout.GestureRecognizers[0];
            var item = layout.CommandParameter as NewPickerItem;
            var selectedItem = _vModel.PickerItemsList.FirstOrDefault(i => i.ItemName == item.ItemName);
            if(selectedItem != null)
            {
                var previousSelectedItem = _vModel.PickerItemsList.FirstOrDefault(i => i.IsSelected);
                if (previousSelectedItem != null)
                    previousSelectedItem.IsSelected = false;
                selectedItem.IsSelected = true;
            }
        }

        async void OutsideTapped(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        void InsideFrame_Tapped(System.Object sender, System.EventArgs e)
        {
            //do nothing
        }
    }
}
