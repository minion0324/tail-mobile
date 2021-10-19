using System;
using System.Diagnostics;

using Tail.Controls;
using Tail.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace Tail.iOS.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    InsertRowsAnimation = UITableViewRowAnimation.None;
                    DeleteRowsAnimation = UITableViewRowAnimation.None;
                    ReloadRowsAnimation = UITableViewRowAnimation.None;
                    var view = Element as CustomListView;

                    if (view != null && Control != null)
                    {
                        Control.AllowsSelection = view.IsAllowSelection;
                        
                    }

                    if (Control != null && view != null)
                    {
                        Control.TintColor = Color.Transparent.ToUIColor();
                        if(!view.ScrollEnabled)
                            Control.ScrollEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in OnElementChangedListView: " + ex.Message);
            }
        }
    }
}
