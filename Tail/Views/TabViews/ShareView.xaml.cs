using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class ShareView : ContentView
    {
        public ShareView(MyProfileViewModel _viewModel)
        {
            InitializeComponent();
            this.BindingContext = _viewModel;
        }
    }
}
