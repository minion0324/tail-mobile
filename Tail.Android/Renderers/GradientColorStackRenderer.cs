using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace Tail.Droid.Renderers
{
    public class GradientColorStackRenderer : VisualElementRenderer<Frame>
    {
        public GradientColorStackRenderer(Context context) : base(context) { }

        protected override void DispatchDraw(Canvas canvas)
        {
            GradientColorStack stack = (GradientColorStack)Element;

            var startColor = stack.StartColor.ToAndroid();
            var endColor = stack.EndColor.ToAndroid();
            var colors = new int[] { startColor, endColor };
            var drawable = new GradientDrawable(GradientDrawable.Orientation.TrBl, colors);
            if (stack.GradientDirection == Common.GradientDirection.Bottom)
            {
                drawable = new GradientDrawable(GradientDrawable.Orientation.TopBottom, colors);
            }
            
            base.SetBackgroundDrawable(drawable);
        }

       
    }
}
