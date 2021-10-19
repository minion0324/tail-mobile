using Android.Content;

using Tail.Controls;
using Tail.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PickerWithoutBorder), typeof(PickerWithoutBorderRenderer))]
namespace Tail.Droid.Renderers
{
    public class PickerWithoutBorderRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        public PickerWithoutBorderRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			if (Control != null && Element != null)
			{
                var view = Element as PickerWithoutBorder;
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
				Control.SetHintTextColor(view.TitleColor.ToAndroid());
				Control.SetPadding(0, 5, 0, 5);                          
                Control.TextSize = (float)view.FontSize;
			}
		}
	}
}
