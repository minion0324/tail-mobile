using System.Threading.Tasks;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class ForgotPassword : AppPageBase
    {
        readonly ForgotPasswordViewModel _vModel;
        public ForgotPassword()
        {
            InitializeComponent();
            _vModel = new ForgotPasswordViewModel();
            BindingContext = _vModel;
            _vModel.OnSendMailError = () => OnSendMailError();
            SetEntryFocused();

        }
        async void SetEntryFocused()
        {
            await Task.Delay(300);
            EmailEntry.Focus();
        }
        void OnSendMailError()
        {
            _vModel.Email.Value = string.Empty;
            EmailEntry.Focus();
        }

    }
}
