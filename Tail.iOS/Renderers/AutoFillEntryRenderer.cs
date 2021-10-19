using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoFillEntry), typeof(AutoFillEntryRenderer))]
namespace Tail.iOS.Renderers
{
    public class AutoFillEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.TextContentType = UITextContentType.OneTimeCode;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.TintColor = UIColor.Clear;
            }
        }
    }
}
