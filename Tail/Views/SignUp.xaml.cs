using System.Diagnostics;
using Tail.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class SignUp : AppPageBase
    {
        readonly SignUpViewModel _vModel;
        public SignUp()
        {
            InitializeComponent();
            _vModel = new SignUpViewModel();
            BindingContext = _vModel;
            App.PostSuccessFacebookAction = async token =>
            {
                await _vModel.GetFbProfile(token);
            };
            if (Device.RuntimePlatform == Device.iOS)
            {
               
                if (DeviceInfo.Version.Major < 13)
                    AppleLoginStack.IsVisible = false;
                else
                    AppleLoginStack.IsVisible = true;
            }
        }
    }
}
