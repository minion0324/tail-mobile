using Tail.ViewModels;
using Xamarin.Forms;
namespace Tail.Views.TabViews
{
    public partial class PurchaseView : ContentView
    {

        public PurchaseView(MyProfileViewModel _viewModel)
        {

            InitializeComponent();
            this.BindingContext = _viewModel;
        }
    }
}
