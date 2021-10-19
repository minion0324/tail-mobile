using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Tail.Droid.Renderers;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace Tail.Droid.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.SetPadding(0, 0, 0, 0);
        }
    }
}
