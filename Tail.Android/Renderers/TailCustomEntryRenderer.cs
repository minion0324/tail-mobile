using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using Android.Widget;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TailCustomEntry), typeof(TailCustomEntryRenderer))]
namespace Tail.Droid.Renderers
{
    public class TailCustomEntryRenderer : EntryRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BurndyFocus.Droid.Renderers.BurndyEntryRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public TailCustomEntryRenderer(Context context) : base(context)
        {
        }

        /// <summary>
        /// Ons the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && Element != null)
            {
                var view = (TailCustomEntry)Element;
                Control.SetBackgroundColor(Android.Graphics.Color.White);
                Control.SetTextColor(Android.Graphics.Color.Black);

                Control.SetHintTextColor(Android.Graphics.Color.Rgb(166, 166, 166));
                Control.SetPadding(16, 0, 16, 0);
                if(view.HasRightView)
                    Control.SetPadding(16, 0, 50, 0);
                Control.HorizontalScrollBarEnabled = false;
                var entryExt = (e.NewElement as TailCustomEntry);
                Control.ImeOptions = entryExt.ReturnKeyType.GetValueFromDescription();
                Control.SetImeActionLabel(entryExt.ReturnKeyType.ToString(), Control.ImeOptions);
                Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                {
                    if (entryExt.ReturnType != ReturnType.Next)
                        entryExt.Unfocus();
                    entryExt.InvokeCompleted();
                };
                if (view.BorderColor == Xamarin.Forms.Color.White)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    gd.SetStroke(2, Android.Graphics.Color.Transparent);

                    this.Control.SetBackgroundDrawable(gd);
                }
                else
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    gd.SetStroke(2, view.BorderColor.ToAndroid());

                    this.Control.SetBackgroundDrawable(gd);
                   
                }
              
            }
        }

        /// <summary>
        /// Ons the element property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null && Element != null)
            {
                var view = (TailCustomEntry)Element;
                if (view.BorderColor == Xamarin.Forms.Color.White)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    gd.SetStroke(2, Android.Graphics.Color.Transparent);

                    this.Control.SetBackgroundDrawable(gd);
                }
                else
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetCornerRadius(0); //increase or decrease to changes the corner look  
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    gd.SetStroke(2, view.BorderColor.ToAndroid());

                    this.Control.SetBackgroundDrawable(gd);
                    
                }
            }
               
            if (e.PropertyName == TailCustomEntry.ReturnKeyPropertyName)
            {
                var entryExt = (sender as TailCustomEntry);
                Control.ImeOptions = entryExt.ReturnKeyType.GetValueFromDescription();
                // This is hackie ;-) / A Android-only bindable property should be added to the EntryExt class 
                Control.SetImeActionLabel(entryExt.ReturnKeyType.ToString(), Control.ImeOptions);
            }
        }
    }
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the value from description.
        /// </summary>
        /// <returns>The value from description.</returns>
        /// <param name="value">Value.</param>
        public static ImeAction GetValueFromDescription(this ReturnKeyTypes value)
        {
            var type = typeof(ImeAction);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on Android: {value}");
        }
    }
}
