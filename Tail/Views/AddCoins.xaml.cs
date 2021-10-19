using Tail.ViewModels;
namespace Tail.Views
{
    public partial class AddCoins : AppPageBase
    {
        readonly AddCoinsViewModel _vModel;
        public AddCoins()
        {
            _vModel = new AddCoinsViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }

      
    }
}
