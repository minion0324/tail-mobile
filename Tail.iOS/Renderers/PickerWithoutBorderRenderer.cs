using UIKit;

using Tail.Controls;
using Tail.iOS.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PickerWithoutBorder), typeof(PickerWithoutBorderRenderer))]
namespace Tail.iOS.Renderers
{
    public class PickerWithoutBorderRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                if (Control.InputAssistantItem != null)
                {
                    Control.InputAssistantItem.LeadingBarButtonGroups = null;
                    Control.InputAssistantItem.TrailingBarButtonGroups = null;
                }
            }
        }
    }
}
