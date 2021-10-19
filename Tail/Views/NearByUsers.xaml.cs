using Tail.ViewModels;
namespace Tail.Views
{
    public partial class NearByUsers :AppPageBase
    {
        NearByUsersViewModel _vModel;
        public NearByUsers()
        {
            InitializeComponent();
            _vModel = new NearByUsersViewModel();
            BindingContext = _vModel;
        }
    }
}
