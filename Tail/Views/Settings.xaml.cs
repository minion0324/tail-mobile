using Tail.ViewModels;
namespace Tail.Views
{
   
    public partial class Settings : AppPageBase
    {
        readonly SettingsViewModel _vModel;
        public Settings()
        {
            InitializeComponent();
            _vModel = new SettingsViewModel();
            BindingContext = _vModel;
        }
    }
}
