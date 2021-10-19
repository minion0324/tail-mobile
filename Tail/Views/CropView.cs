using System;
using Tail.Models;
namespace Tail.Views
{
    public class CropView : AppPageBase
    {
        public byte[] Image { get; set; }
        public Action RefreshAction { get; set; }
        public bool DidCrop { get; set; }
        public int CroppingHight { get; set; }
        public int CroppingWidth { get; set; }
        public CropArgs CropArguments { get; set; }
        public CropView(object parameter, Action CropRefreshCallback)
        {
            CropArguments = (CropArgs)parameter;
            Image = CropArguments.ImageAsByte;
            CroppingHight =CropArguments.Height;
            CroppingWidth = CropArguments.Width;
            RefreshAction = CropRefreshCallback;

        }
       
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if(DidCrop)
                RefreshAction.Invoke();
        }
    }
}

