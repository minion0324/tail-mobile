
using Xamarin.Forms;

namespace Tail.Services.Helper
{
    public static class OtpSmsServices
    {
        public static void ListenToSmsRetriever()
        {
            DependencyService.Get<IListenToSmsRetriever>()?.ListenToSmsRetriever();
        }
    }
    public interface IListenToSmsRetriever
    {
        void ListenToSmsRetriever();
    }
}
