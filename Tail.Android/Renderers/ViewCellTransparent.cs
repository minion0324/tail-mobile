using Android.Content;
using Android.Views;

using Tail.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellTransparent))]
namespace Tail.Droid.Renderers
{
    public class ViewCellTransparent: ViewCellRenderer
    {
        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);
            var listView = parent as global::Android.Widget.ListView;

            if (listView != null)
            {                
                listView.SetSelector(global::Android.Resource.Color.Transparent);
                listView.CacheColorHint = global::Android.Graphics.Color.Transparent;
            }

            return cell;
        }
    }
}
