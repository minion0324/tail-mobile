using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomStackLayout), typeof(CustomStackLayoutRenderer))]
namespace Tail.Droid.Renderers
{
    public class CustomStackLayoutRenderer : ViewRenderer<CustomStackLayout, Android.Views.View>
    {
        public CustomStackLayoutRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomStackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CustomStackLayout layout = e.NewElement;
                layout.OnDrawing += NewElement_OnDrawing;
            }
        }

        private void NewElement_OnDrawing(Action<byte[]> action)
        {
            if (this.ViewGroup != null)
            {
                int width = ViewGroup.Width;
                int height = ViewGroup.Height;

                //create and draw the bitmap
                Bitmap bmp = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                Canvas c = new Canvas(bmp);
                ViewGroup.Draw(c);

                MemoryStream stream = new MemoryStream();
                bmp.Compress(Bitmap.CompressFormat.Png, 100, stream);
                byte[] byteArray = stream.ToArray();
                action(byteArray);
            }
        }
    }
}
