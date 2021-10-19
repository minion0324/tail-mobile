using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class PicksView : ContentView
    {

        public PicksView(MyProfileViewModel _viewModel)
        {

            InitializeComponent();
            this.BindingContext = _viewModel;
        }
    }
}
