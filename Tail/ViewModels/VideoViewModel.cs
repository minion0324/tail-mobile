
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class VideoViewModel : PageViewModelBase
    {
        string _videoUrl;
        HtmlWebViewSource _sourceVideoURL;
        public VideoViewModel()
        {
        }
        public string VideoUrl
        {
            get => _videoUrl;
            set => SetProperty(ref _videoUrl, value);
        }
        public HtmlWebViewSource SourceVideoURL
        {
            get => _sourceVideoURL;
            set => SetProperty(ref _sourceVideoURL, value);
        }
       
        public override  void Handle_BackCommand()
        {
            IsBusy = false;
            base.Handle_BackCommand();
        }

        }
    }
