using Tail.ViewModels;
namespace Tail.Views
{
    public partial class SearchResultViewAll : AppPageBase
    {
        SearchResultViewAllViewModel _vModel;
        public SearchResultViewAll()
        {
            InitializeComponent();
            _vModel = new SearchResultViewAllViewModel();
            BindingContext = _vModel;
        }
    }
}
