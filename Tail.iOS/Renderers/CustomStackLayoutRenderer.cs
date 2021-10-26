using System;
using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomStackLayout), typeof(CustomStackLayoutRenderer))]
namespace Tail.iOS.Renderers
{
    public class CustomStackLayoutRenderer : ViewRenderer<StackLayout, UIView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CustomStackLayout layout = e.NewElement as CustomStackLayout;
                layout.OnDrawing += NewElement_OnDrawing;
            }
        }

        private void NewElement_OnDrawing(Action<byte[]> action)
        {
            UIGraphics.BeginImageContext(NativeView.Frame.Size);
            NativeView.DrawViewHierarchy(NativeView.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            if (image != null)
                using (var imageData = image.AsPNG())
                {
                    var bytes = new byte[imageData.Length];
                    System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                    action(bytes);
                }
        }
    }
}
