using Tail.ViewModels;
namespace Tail.Views
{
    public partial class AddNewCard : AppPageBase
    {

        readonly AddNewCardViewModel _vModel;
        public AddNewCard()
        {
            _vModel = new AddNewCardViewModel();
            BindingContext = _vModel;
            InitializeComponent();
        }
    }
}
