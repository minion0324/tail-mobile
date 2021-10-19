using Tail.ViewModels;
namespace Tail.Views
{
    public partial class Coins : AppPageBase
    {
        readonly CoinsViewModel _vModel;
        public Coins()
        {
            _vModel = new CoinsViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }
    }
}
