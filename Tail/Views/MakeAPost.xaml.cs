using System;
using System.Diagnostics;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class MakeAPost : PopupPage
    {
        readonly MakeAPostViewModel _vModel;
        public MakeAPost(Action popUpCloseCallback)
        {
            try
            {
                _vModel = new MakeAPostViewModel();
                BindingContext = _vModel;
                InitializeComponent();

                if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
                {
                    DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                    if (deviceModel == DeviceModel.iPhone5 || deviceModel == DeviceModel.iPhone8 || deviceModel == DeviceModel.iPhone8Plus)
                    {
                        InnerGrid.Margin = new Thickness(0, 0, 0, 50);
                    }

                }
                else
                {
                    InnerGrid.Margin = new Thickness(0, 0, 0, 65);
                }

                _vModel.PopupCloseCallback = popUpCloseCallback;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }
        protected async void DismissNotification_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }


    }
}
