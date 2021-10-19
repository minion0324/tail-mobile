using System;
using System.ComponentModel;
using CoreGraphics;
using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TailCustomFrame), typeof(TailCustomFrameRenderer))]
namespace Tail.iOS.Renderers
{
    public class TailCustomFrameRenderer : FrameRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                // If the Frame's position or size changes we need to reset the shadow
                if (e.PropertyName == "X" || e.PropertyName == "Y" || e.PropertyName == "Width" || e.PropertyName == "Height")
                {

                    SetFrameShadow();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public override void Draw(CGRect rect)
        {
            try
            {
                base.Draw(rect);

                SetFrameShadow();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void SetFrameShadow()
        {
            Layer.ShadowRadius = 6;
            Layer.ShadowColor = UIColor.FromRGB(229,229,229).CGColor;
            Layer.ShadowOffset = new CGSize(0, 3);
            Layer.ShadowOpacity = 1.0f;
            Layer.MasksToBounds = false;
            Layer.BorderWidth = 1;
            Layer.CornerRadius = Element.CornerRadius;
            Layer.ShadowPath = UIBezierPath.FromRoundedRect(Layer.Bounds, Element.CornerRadius).CGPath;
            Layer.BorderColor = UIColor.Clear.CGColor;
            
        }
    }
}
