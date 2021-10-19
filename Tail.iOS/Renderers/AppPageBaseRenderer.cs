using Tail.iOS.Renderers;
using Tail.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(AppPageBase), typeof(AppPageBaseRenderer))]

namespace Tail.iOS.Renderers
{
    public class AppPageBaseRenderer : PageRenderer
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing && Element is AppPageBase page)
            {
                page.OnPageDestroy();
            }
            base.Dispose(disposing);
        }
    }
}
