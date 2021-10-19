using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class PostsView : ContentView
    {
        #region Private Members
        MyProfileViewModel _vModel;
        #endregion

        public PostsView(MyProfileViewModel _viewModel)
        {

            InitializeComponent();
            _vModel = _viewModel;
            this.BindingContext = _viewModel;
          
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is MyProfileViewModel)
            {
                _vModel = BindingContext as MyProfileViewModel;

                _vModel.OnPostRefresh += () =>
                {
                    PostList.Refresh();

                };
            }
               
        }
    }
}
