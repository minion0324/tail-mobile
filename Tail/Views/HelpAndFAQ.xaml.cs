using Tail.Common;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class HelpAndFAQ : AppPageBase
    {
        readonly HelpViewModel _vModel;
  
        public HelpAndFAQ()
        {
            InitializeComponent();
            _vModel = new HelpViewModel();
            BindingContext = _vModel;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    Maingrid.RowDefinitions[0].Height = 108;

                }
                HelpWebViewIOS.Navigating += (sender, e) => {
                    _vModel.IsBusy = true;
                };

                HelpWebViewIOS.Navigated += (sender, e) => {
                    _vModel.IsBusy = false;
                };
                HelpWebView.Navigating += (sender, e) => {
                    _vModel.IsBusy = true;
                };

                HelpWebView.Navigated += (sender, e) => {
                    _vModel.IsBusy = false;
                };

            }
        }
    }
}
