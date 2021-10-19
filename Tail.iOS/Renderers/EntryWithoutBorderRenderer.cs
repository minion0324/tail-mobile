using System.Drawing;
using UIKit;
using Tail.Controls;
using Tail.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryWithoutBorder), typeof(EntryWithoutBorderRenderer))]

namespace Tail.iOS.Renderers
{
    public class EntryWithoutBorderRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && Element != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                var view = Element as EntryWithoutBorder;
                SetReturnButton(view);

                if (view != null)
                {
                    Control.TextColor = view.TextColor.ToUIColor();
                    // Check for only Numeric keyboard
                    if ((view.Keyboard == Keyboard.Numeric || view.Keyboard == Keyboard.Telephone) && Device.Idiom != TargetIdiom.Tablet)
                    {
                        SetNumbericKeyboard();
                    }
                }




            }
        }
        private void SetReturnButton(EntryWithoutBorder view)
        {
            if (view != null && view.ButtonType == "Next")
            {
                Control.ReturnKeyType = UIReturnKeyType.Next;
            }
            else
            {
                Control.ReturnKeyType = UIReturnKeyType.Done;
            }

            Control.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            if (view != null && view.ClearButtonDisabled)
            {
                Control.ClearButtonMode = UITextFieldViewMode.Never;
            }
        }
        private void SetNumbericKeyboard()
        {
            UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)Control.Frame.Size.Width, 44.0f));

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
            {
                Control.ResignFirstResponder();

            });

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                doneButton
             };

            Control.InputAccessoryView = toolbar;
        }

    }
}
