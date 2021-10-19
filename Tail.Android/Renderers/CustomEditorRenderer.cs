using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Tail.Controls;
using Tail.Droid.Renderers;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Tail.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CustomEditor editor = e.NewElement as CustomEditor;
                Control.Hint = editor.Placeholder;
                Control.SetHintTextColor(editor.PlaceholderColor.ToAndroid());
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (string.Compare(e.PropertyName, CustomEditor.PlaceholderProperty.PropertyName) == 0)
            {
                var editor = Element as CustomEditor;
                Control.Hint = editor.Placeholder;
                Control.SetHintTextColor(editor.PlaceholderColor.ToAndroid());
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}
