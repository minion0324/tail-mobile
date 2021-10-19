using System;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Tail.Controls;
using Tail.Droid.Renderers;
using Tail.Services.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TailCustomDatePicker), typeof(TailCustomDatePickerRenderer))]
namespace Tail.Droid.Renderers
{
    public class TailCustomDatePickerRenderer : DatePickerRenderer
    {
        public TailCustomDatePickerRenderer(Context context) : base(context)
        {
          
        }
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            this.Control.SetPadding(20, 0, 0, 0);
        
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
            gd.SetColor(Android.Graphics.Color.Transparent);
            gd.SetStroke(2, Android.Graphics.Color.Rgb(216, 216, 216));

            this.Control.SetBackgroundDrawable(gd);

            TailCustomDatePicker element = Element as TailCustomDatePicker;
            _element = element;
           
            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                
                Control.Text = element.Placeholder;
            }
            if (Control.Text == element.Placeholder)
            {
                Control.SetTextColor(Android.Graphics.Color.Rgb(166, 166, 166));
            }
            else
            {
                Control.SetTextColor(Android.Graphics.Color.Black);
            }
           


            this.Control.TextChanged += (sender, arg) => {
                var selectedDate = arg.Text.ToString();
                if (selectedDate == element.Placeholder)
                {
                    Control.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }
                if (Control.Text == element.Placeholder)
                {
                    Control.SetTextColor(Android.Graphics.Color.Rgb(166, 166, 166));
                }
                else
                {
                    Control.SetTextColor(Android.Graphics.Color.Black);
                }
               
            };
        }

        protected Xamarin.Forms.DatePicker _element;

        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var dialog = new DatePickerDialog(Context, (o, e) =>
            {
                _element.Date = e.Date;
                
                ((IElementController)_element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
            }, year, month, day);

            dialog.SetButton((int)DialogButtonType.Positive, Context.Resources.GetString(global::Android.Resource.String.Ok), OnOk);
            dialog.SetButton((int)DialogButtonType.Negative, Context.Resources.GetString(global::Android.Resource.String.Cancel), OnCancel);

            return dialog;
        }

        private void OnCancel(object sender, DialogClickEventArgs e)
        {
            _element.Unfocus();
        }
        private void OnOk(object sender, DialogClickEventArgs e)
        {
            Control.Text = ((DatePickerDialog)sender).DatePicker.DateTime.ToString("MM/dd/yyyy");
            string formattedDate = TailUtils.ConvertToDateTimeFormate(((DatePickerDialog)sender).DatePicker.DateTime);
            _element.Date = DateTime.ParseExact(formattedDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            _element.Unfocus();
        }
    }
}
