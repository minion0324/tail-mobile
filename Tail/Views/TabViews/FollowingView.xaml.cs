using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class FollowingView : ContentView
    {
        public FollowingView(MyProfileViewModel _viewModel)
        {

            InitializeComponent();
            this.BindingContext = _viewModel;
          
        }
    }
}
