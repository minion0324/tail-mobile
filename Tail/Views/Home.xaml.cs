using Tail.Controls;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;
namespace Tail.Views
{
    public partial class Home : CustomTab
    {
        readonly HomeViewModel _vModel;
        public Home()
        {

            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                TabHome.IconImageSource = "home_selected";
                DependencyService.Get<IStatusBarStyleManager>().SetDarkTheme();
                TabCreatePost.IconImageSource = "";
                MainTabControl.HeightRequest = 100;
                SettingsService.Instance.IsOtherUserProfile = false;
            }
               
            _vModel = new HomeViewModel();
            BindingContext = _vModel;


        }

        void CustomTab_CurrentPageChanged(System.Object sender, System.EventArgs e)
        {
            if (SettingsService.Instance != null)
                SettingsService.Instance.CurrentTabIndex = this.Children.IndexOf(this.CurrentPage);

            if(SettingsService.Instance != null && SettingsService.Instance.CurrentTabIndex==3)
                SettingsService.Instance.IsOtherUserProfile = false;
            else
                SettingsService.Instance.IsOtherUserProfile = true;

        }
        

    }
}
