using System;
using System.Diagnostics;
using Foundation;
using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.IO;
using Xam.Plugins.ImageCropper.iOS;
using Tail.Views;
using Tail.Services.ApplicationServices;

[assembly: ExportRenderer(typeof(CropView), typeof(Tail.iOS.Renderers.CropViewRenderer))]
namespace Tail.iOS.Renderers
{
    public class CropViewRenderer : PageRenderer
    {
        CropViewDelegate selector;
        byte[] Image;
        bool IsShown;
        public bool DidCrop { get; set; }
        int ImageHeight = 0;
        int ImageWidth = 0;
       
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var page = base.Element as CropView;
            Image = page.Image;
            DidCrop = page.DidCrop;
            ImageHeight = page.CroppingHight;
            ImageWidth = page.CroppingWidth;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            try
            {
                if (!IsShown)
                {

                    IsShown = true;

                    UIImage image = new UIImage(NSData.FromArray(Image));
                    Image = null;

                    selector = new CropViewDelegate(this);

                    TOCropViewController picker = new TOCropViewController(image);
                    picker.AspectRatioLockEnabled = true;
                    picker.CustomAspectRatio = new System.Drawing.Size(ImageWidth, ImageHeight);
                    picker.AspectRatioPickerButtonHidden = true;
                   
                    picker.Delegate = selector;

                    PresentViewController(picker, false, null);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            try
            {
                var page = base.Element as CropView;
                page.DidCrop = selector.DidCrop;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
    public class CropViewDelegate : TOCropViewControllerDelegate
    {
        readonly UIViewController parent;
        public bool DidCrop { get; set; }

        public CropViewDelegate(UIViewController parent)
        {
            this.parent = parent;
        }

        public override void DidCropToImage(TOCropViewController cropViewController, UIImage image, CoreGraphics.CGRect cropRect, nint angle)
        {
            DidCrop = true;

            try
            {
                CommonSingletonUtility.SharedInstance.CroppedImageName = "";
                if (image != null)
                {

                    int byteImageSize = image.AsJPEG().ToArray().Length;
                    double sizeInMB = (double)byteImageSize / (1000 * 1000);
                   
                   
                    string fileName = "TailTempCrop.jpg";
                    string file= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
                    CommonSingletonUtility.SharedInstance.CroppedImageName =file;
                    if (sizeInMB > 1)
                    {
                        nfloat compressionQuality = (sizeInMB < 3) ? (nfloat)0.6 : (nfloat)0.5;
                        File.WriteAllBytes(file, image.AsJPEG(compressionQuality).ToArray());
                       
                    }
                    else
                    {
                        File.WriteAllBytes(file, image.AsJPEG().ToArray());
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }

            parent.DismissViewController(true, () => { App.Current.MainPage.Navigation.PopModalAsync(); });
        }

        public override void DidFinishCancelled(TOCropViewController cropViewController, bool cancelled)
        {
            parent.DismissViewController(true, () => { App.Current.MainPage.Navigation.PopModalAsync(); });
        }
    }
}

