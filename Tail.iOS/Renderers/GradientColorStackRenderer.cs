using System.Drawing;

using CoreAnimation;
using CoreGraphics;

using Tail.Controls;
using Tail.iOS.Renderers;
using Tail.Common;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using UIKit;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace Tail.iOS.Renderers
{
    public class GradientColorStackRenderer : VisualElementRenderer<Frame>
    {
        CAGradientLayer gradientLayer = null;

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradientColorStack stack = (GradientColorStack)Element;
            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();

            #region for Set Color Gradient

            switch (stack.GradientDirection)
            {
                case GradientDirection.Right:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(0.0, 0.5),
                        EndPoint = new CGPoint(1.0, 0.5),
                    };
                    break;
                case GradientDirection.Left:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(1.0, 0.5),
                        EndPoint = new CGPoint(0.0, 0.5),
                    };
                    break;
                case GradientDirection.Bottom:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(0.5, 0.0),
                        EndPoint = new CGPoint(0.5, 1.0),

                    };
                    break;
                case GradientDirection.Top:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(0.5, 1.0),
                        EndPoint = new CGPoint(0.5, 0.0),
                    };
                    break;
                case GradientDirection.TopLeftToBottomRight:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(0.0, 0.0),
                        EndPoint = new CGPoint(1.0, 1.0),
                    };
                    break;
                case GradientDirection.TopRightToBottomLeft:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(1.0, 0.0),
                        EndPoint = new CGPoint(0.0, 1.0),

                    };
                    break;
                case GradientDirection.BottomLeftToTopRight:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(0.0, 1.0),
                        EndPoint = new CGPoint(1.0, 0.0),

                    };
                    break;
                default:
                    gradientLayer = new CAGradientLayer()
                    {
                        StartPoint = new CGPoint(1.0, 1.0),
                        EndPoint = new CGPoint(0.0, 0.0),

                    };
                    break;
            }

            #endregion
           
            gradientLayer.Frame = rect;
            gradientLayer.CornerRadius = stack.ButtonRadius;
            gradientLayer.Colors = new CGColor[] { startColor, endColor };

            if(stack.IsShadowVisible)
            {
                gradientLayer.ShadowColor = stack.ShadowColor.ToCGColor();
                gradientLayer.ShadowOpacity = 0.5f;
                gradientLayer.ShadowOffset = new SizeF (0, 5); 
                gradientLayer.MasksToBounds = false;
            }

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
      
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(gradientLayer == null)
            {
                ContentMode = UIViewContentMode.Redraw;
            }
           
        }
    }
}
