using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class FollowersView : ContentView
    {
   

        public FollowersView(MyProfileViewModel _viewModel)
        {
            InitializeComponent();
            this.BindingContext = _viewModel;
        }
    }
}
