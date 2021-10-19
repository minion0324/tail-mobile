using Android.Widget;
using Android.Content;

using Tail.Controls;
using Tail.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryWithoutBorder), typeof(EntryWithoutBorderRenderer))]
namespace Tail.Droid.Renderers
{
    public class EntryWithoutBorderRenderer : EntryRenderer
	{
        public EntryWithoutBorderRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null && Element != null)
			{
                EntryWithoutBorder entry = (EntryWithoutBorder)Element;
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
                Control.SetHintTextColor(entry.PlaceholderColor.ToAndroid());
                Control.SetPadding(0, 0, 0, 0);
               
                if (entry.ButtonType == "Next")
                {
                    Control.ImeOptions = global::Android.Views.InputMethods.ImeAction.Next;
                    Control.SetImeActionLabel("Next", global::Android.Views.InputMethods.ImeAction.Next);
         
                }
			}
		}
      
    }
}
