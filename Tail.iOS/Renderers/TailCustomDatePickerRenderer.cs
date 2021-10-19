using System;
using CoreGraphics;
using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TailCustomDatePicker), typeof(TailCustomDatePickerRenderer))]
namespace Tail.iOS.Renderers
{
    public class TailCustomDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
                return;
            var element = e.NewElement as TailCustomDatePicker;
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                Control.Text = element.Placeholder;
            }
            Control.BorderStyle = UITextBorderStyle.Line;
            Control.Layer.BorderColor = UIColor.FromRGB(216, 216, 216).CGColor;
            Control.Layer.CornerRadius = 0;
            Control.Layer.BorderWidth = 1f;
            Control.AdjustsFontSizeToFitWidth = true;
            Control.LeftView = new UIView(new CGRect(0, 0, 10, 0));
            Control.LeftViewMode = UITextFieldViewMode.Always;

            if (Control.Text == element.Placeholder)
            {
                Control.TextColor = UIColor.FromRGB(166, 166, 166);
                
            }
            else
            {
                Control.TextColor = UIColor.Black;
            }
            Control.ShouldEndEditing += (textField) => {
                var seletedDate = textField;
                var text = seletedDate.Text;
                if (text == element.Placeholder)
                {
                    Control.Text = element.MaximumDate.ToString("MM/dd/yyyy");
                }
                if (Control.Text == element.Placeholder)
                {
                    Control.TextColor = UIColor.FromRGB(166, 166, 166);
                }
                else
                {
                    Control.TextColor = UIColor.Black;
                }
                return true;
            };
        }

    }
}
