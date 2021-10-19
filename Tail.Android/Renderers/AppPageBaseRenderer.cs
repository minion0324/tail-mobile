using Xamarin.Forms.Platform.Android;
using Android.Content;
using Xamarin.Forms;
using Tail.Views;
using Tail.Droid.Renderers;

[assembly: ExportRenderer(typeof(AppPageBase), typeof(AppPageBaseRenderer))]
namespace Tail.Droid.Renderers
{
    public class AppPageBaseRenderer : PageRenderer
    {
        public AppPageBaseRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Element is AppPageBase page)
            {
                page.OnPageDestroy();
                base.Dispose(disposing);
            }
        }
    }
}
