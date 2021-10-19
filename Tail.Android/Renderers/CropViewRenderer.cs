using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Com.Theartofdev.Edmodo.Cropper;
using Android.Graphics;
using Tail.Views;
using Tail.Services.ApplicationServices;

[assembly: ExportRenderer(typeof(CropView), typeof(Tail.Droid.Renderers.CropViewRenderer))]
namespace Tail.Droid.Renderers
{
    public class CropViewRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
           
            var page = Element as CropView;
            if (page != null)
            {
                var cropImageView = new CropImageView(Context);
                cropImageView.SetAspectRatio(page.CroppingWidth, page.CroppingHight);
                cropImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                Bitmap bitmp = BitmapFactory.DecodeByteArray(page.Image, 0, page.Image.Length);
                cropImageView.SetImageBitmap(bitmp);

                var scrollView = new Xamarin.Forms.ScrollView { Content = cropImageView.ToView() };
                var stackLayout = new StackLayout { Children = { scrollView },HorizontalOptions=LayoutOptions.Center,VerticalOptions=LayoutOptions.Center };

                var rotateButton = new Xamarin.Forms.Button { Text = "Rotate",CornerRadius =22, HeightRequest=44, WidthRequest=120,TextColor=Xamarin.Forms.Color.Black,HorizontalOptions=LayoutOptions.Center };

                rotateButton.Clicked += (sender, ex) =>
                {
                    cropImageView.RotateImage(90);
                };
                stackLayout.Children.Add(rotateButton);

                var finishButton = new Xamarin.Forms.Button { Text = "Finished", CornerRadius = 22, HeightRequest = 44, WidthRequest = 120, TextColor = Xamarin.Forms.Color.Black,HorizontalOptions = LayoutOptions.Center };
                finishButton.Clicked += (sender, ex) =>
                {
                    CommonSingletonUtility.SharedInstance.CroppedImageName = "";
                    Bitmap cropped = cropImageView.CroppedImage;
                    using (MemoryStream memory = new MemoryStream())
                    {
                        cropped.Compress(Bitmap.CompressFormat.Png, 100, memory);

                       
                       Guid guid;
                     
                        guid = Guid.NewGuid();
                        string fileName = guid+".jpg";
                        string file = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
                        CommonSingletonUtility.SharedInstance.CroppedImageName = file;
                        File.WriteAllBytes(file, memory.ToArray());
                    }
                    page.DidCrop = true;
                    page.Navigation.PopModalAsync();
                };

                stackLayout.Children.Add(finishButton);

                var cancelButton = new Xamarin.Forms.Button { Text = "Cancel", CornerRadius = 22, HeightRequest = 44, WidthRequest = 120, TextColor = Xamarin.Forms.Color.Black, HorizontalOptions = LayoutOptions.Center };

                cancelButton.Clicked += (sender, ex) =>
                {
                    CommonSingletonUtility.SharedInstance.CroppedImageName = "";
                    page.Navigation.PopModalAsync();
                };

                stackLayout.Children.Add(cancelButton);

                page.Content = stackLayout;
            }
        }
    }
}
