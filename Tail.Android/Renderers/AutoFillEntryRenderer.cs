using Android.Content;
using Android.Graphics.Drawables;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AutoFillEntry), typeof(AutoFillEntryRenderer))]
namespace Tail.Droid.Renderers
{
    public class AutoFillEntryRenderer : EntryRenderer
    {
        public AutoFillEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && Element != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
                gd.SetColor(Android.Graphics.Color.Transparent);
                gd.SetStroke(2, Android.Graphics.Color.Transparent);

                this.Control.SetBackgroundDrawable(gd);
            }
        }
    }
}



