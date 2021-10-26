
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class AboutUs : AppPageBase
    {
        readonly AboutUsViewModel _vModel;
        public AboutUs()
        {
            InitializeComponent();
            _vModel = new AboutUsViewModel();
            BindingContext = _vModel;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    Maingrid.RowDefinitions[0].Height = 108;

                }

           

            }
        }
    }
}
