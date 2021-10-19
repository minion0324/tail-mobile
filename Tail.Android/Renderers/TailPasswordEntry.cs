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
using Android.Views;
[assembly: ExportRenderer(typeof(TailPasswordEntry), typeof(TailPasswordEntryRenderer))]
namespace Tail.Droid.Renderers
{
    public class TailPasswordEntryRenderer : EntryRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BurndyFocus.Droid.Renderers.BurndyEntryRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public TailPasswordEntryRenderer(Context context) : base(context)
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && Element != null)
            {
                var view = (TailPasswordEntry)Element;
                Control.SetBackgroundColor(Android.Graphics.Color.White);
                Control.SetTextColor(Android.Graphics.Color.Black);

                Control.SetHintTextColor(Android.Graphics.Color.Rgb(166, 166, 166));
                Control.SetPadding(16, 0, 16, 0);
                if (view.HasRightView)
                    Control.SetPadding(16, 0, 50, 0);
                Control.HorizontalScrollBarEnabled = false;
                var entryExt = (e.NewElement as TailPasswordEntry);
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
                Control.CustomSelectionActionModeCallback = new Callback();
                Control.LongClickable = false;
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
                var view = (TailPasswordEntry)Element;
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
                Control.CustomSelectionActionModeCallback = new Callback();
                Control.LongClickable = false;
            }

            if (e.PropertyName == TailPasswordEntry.ReturnKeyPropertyName)
            {
                var entryExt = (sender as TailPasswordEntry);
                Control.ImeOptions = entryExt.ReturnKeyType.GetValueFromDescription();
                // This is hackie ;-) / A Android-only bindable property should be added to the EntryExt class 
                Control.SetImeActionLabel(entryExt.ReturnKeyType.ToString(), Control.ImeOptions);
            }
        }


       

        public class Callback : Java.Lang.Object, ActionMode.ICallback
        {

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                return false;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {

            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }
        }
    }
   

}
