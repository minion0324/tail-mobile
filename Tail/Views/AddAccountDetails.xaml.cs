using Tail.ViewModels;
namespace Tail.Views
{
    public partial class AddAccountDetails : AppPageBase
    {
        readonly AddAccountDetailsViewModel _vModel;
        public AddAccountDetails()
        {
            _vModel = new AddAccountDetailsViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }
    }
}
