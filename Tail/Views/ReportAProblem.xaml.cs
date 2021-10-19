using System.IO;
using Tail.ViewModels;
using Xamarin.Forms;
using Tail.Models;
using System.Linq;

namespace Tail.Views
{
    public partial class ReportAProblem : AppPageBase
    {
        ReportAProblemViewModel _vModel;
        public ReportAProblem()
        {
            InitializeComponent();
            _vModel = new ReportAProblemViewModel
            {
                SetFocus = SetFocus
            }; 
            _vModel.OnSelectedImagesUpdated = () => BindableLayout.SetItemsSource(InterestList, _vModel.SelectedMedia);
            BindingContext = _vModel;
        }
        void RemoveIcon_Tapped(System.Object sender, System.EventArgs e)
        {
            Image imgClicked = (Image)sender;
            var img = (TapGestureRecognizer)imgClicked.GestureRecognizers[0];
            var item = img.CommandParameter as MediaFile;
            _vModel.SelectedMedia.Remove(_vModel.SelectedMedia.FirstOrDefault(i => i.PreviewPath == item.PreviewPath));
            if (_vModel.SelectedMedia.Count > 0)
                _vModel.AttachmentVisible = true;
            else
                _vModel.AttachmentVisible = false;
            double size = 0;
            if (_vModel.SelectedMedia.Count > 0)
            {
                foreach (var file in _vModel.SelectedMedia)
                {
                    FileInfo imageInfo = new FileInfo(file.Path);
                    double sizeInMB = (double)imageInfo.Length / (1000 * 1000);
                    size += sizeInMB;
                }
                _vModel.TotalSelectedSize = size;
            }
            if (_vModel.SelectedMedia.Count == 0)
                _vModel.TotalSelectedSize = 0;
        }
        void SetFocus()
        {

            ContentEntry.Focus();
            ContentEntry.Text = string.Empty;


        }

    }
}
