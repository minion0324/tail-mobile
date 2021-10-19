using Tail.ViewModels;
namespace Tail.Views
{
    public partial class AccountDetails : AppPageBase
    {
        readonly AccountDetailsViewModel _vModel;
        public AccountDetails()
        {
            _vModel = new AccountDetailsViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }
    }
}
