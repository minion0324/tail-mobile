using Tail.ViewModels;
namespace Tail.Views
{
    public partial class PaymentMenthods : AppPageBase
    {
        readonly PaymentMethodsViewModel _vModel;
        public PaymentMenthods()
        {
            _vModel = new PaymentMethodsViewModel();
            BindingContext = _vModel;
            InitializeComponent();
           
        }
    }
}
