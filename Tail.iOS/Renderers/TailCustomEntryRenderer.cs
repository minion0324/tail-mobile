using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(TailCustomEntry), typeof(TailCustomEntryRenderer))]
namespace Tail.iOS.Renderers
{
    public class TailCustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null && Element != null)
            {
                var view = (TailCustomEntry)Element;
                Control.BackgroundColor = UIKit.UIColor.White;
                Control.TextColor = UIKit.UIColor.Black;

                Control.TintColor = UIKit.UIColor.Gray;
                if(view.BorderColor == Color.White)
                {
                    Control.BorderStyle = UITextBorderStyle.None;
                }
                else
                {
                    Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                    Control.BorderStyle = UITextBorderStyle.Line;
                    Control.Layer.BorderWidth = 1;
                }

                Control.Layer.CornerRadius = 0;
                if (!view.IsOTP)
                {
                    Control.LeftView = new UIView(new CGRect(0, 0, 16, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                }

                if (view.HasRightView)
                {
                    Control.RightView = new UIView(new CGRect(0, 0, 50, 0));
                    Control.RightViewMode = UITextFieldViewMode.Always;
                }
                if ((e.NewElement is TailCustomEntry))
                {
                    SetReturnType((e.NewElement as TailCustomEntry));

                    Control.ShouldReturn += (UITextField tf) =>
                    {
                        (e.NewElement as TailCustomEntry).InvokeCompleted();
                        return true;
                    };
                }
            }
        }
        private void SetReturnType(TailCustomEntry entry)
        {
            ReturnKeyTypes type = entry.ReturnKeyType;

            switch (type)
            {
                case ReturnKeyTypes.Go:
                    Control.ReturnKeyType = UIReturnKeyType.Go;
                    break;
                case ReturnKeyTypes.Next:
                    Control.ReturnKeyType = UIReturnKeyType.Next;
                    break;
                case ReturnKeyTypes.Send:
                    Control.ReturnKeyType = UIReturnKeyType.Send;
                    break;
                case ReturnKeyTypes.Search:
                    Control.ReturnKeyType = UIReturnKeyType.Search;
                    break;
                case ReturnKeyTypes.Done:
                    Control.ReturnKeyType = UIReturnKeyType.Done;
                    break;
                default:
                    Control.ReturnKeyType = UIReturnKeyType.Default;
                    break;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            {
                if(Element != null)
                {
                    var view = (TailCustomEntry)Element;
                    Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                }
                if (e.PropertyName == TailCustomEntry.ReturnKeyPropertyName)
                {
                    Debug.WriteLine($"{(sender as TailCustomEntry).ReturnKeyType.ToString()}");
                    Control.ReturnKeyType = (sender as TailCustomEntry).ReturnKeyType.GetValueFromDescription();
                }
            }
        }
    }
    public static class EnumExtensions
    {
        public static UIReturnKeyType GetValueFromDescription(this ReturnKeyTypes value)
        {
            var type = typeof(UIReturnKeyType);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on iOS: {value}");
        }
    }
}
