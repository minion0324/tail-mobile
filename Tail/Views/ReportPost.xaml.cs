using Tail.ViewModels;


namespace Tail.Views
{
    public partial class ReportPost : AppPageBase
    {
        ReportPostViewModel _vModel;
        public ReportPost()
        {
            InitializeComponent();
            _vModel = new ReportPostViewModel();
            BindingContext = _vModel;
          
        }
    }
}
