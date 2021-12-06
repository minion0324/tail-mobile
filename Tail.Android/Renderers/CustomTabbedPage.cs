using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Widget;
using Android.Views;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomTab), typeof(CustomTabbedPage))]
namespace Tail.Droid.Renderers
{
    public class CustomTabbedPage : TabbedPageRenderer, BottomNavigationView.IOnNavigationItemSelectedListener
    {

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            bool _isSelected = false;
            if (e.OldElement == null && e.NewElement != null)
            {
                for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
                {
                    var childView = this.ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {

                        ViewGroup _group = viewGroup;
                        _group.SetClipChildren(false);

                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            CreateLayout(viewGroup, j, _isSelected, e);
                        }
                    }
                }
            }
        }
        private void CreateLayout(ViewGroup viewGroup, int j, bool _isSelected, ElementChangedEventArgs<TabbedPage> e)
        {
            var childRelativeLayoutView = viewGroup.GetChildAt(j);
            if (childRelativeLayoutView is BottomNavigationView)
            {
                BottomNavigationView _navView = (childRelativeLayoutView as BottomNavigationView);
                _navView.SetClipChildren(false);
                float _factor = Android.App.Application.Context.Resources.DisplayMetrics.Density;
                int _pixelValue = System.Convert.ToInt32(80 * _factor);
                _navView.LayoutParameters.Height = _pixelValue;
                _navView.ItemHorizontalTranslationEnabled = false;
                _navView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
                _navView.ItemIconTintList = null;

                Android.Widget.ImageButton _fb = new Android.Widget.ImageButton(Context);
                _fb.SetBackgroundColor(Android.Graphics.Color.Transparent);
                _fb.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.create_post));
                Android.Widget.LinearLayout.LayoutParams paramsss = new Android.Widget.LinearLayout.LayoutParams(130, 130);
                paramsss.Gravity = GravityFlags.CenterHorizontal;
                paramsss.TopMargin = -40;
                var metrics = Resources.DisplayMetrics;
                int _leftPoint = (metrics.WidthPixels / 2) - 65;
                paramsss.LeftMargin = _leftPoint;
                _fb.LayoutParameters = paramsss;
                _navView.AddView(_fb);
                _fb.Click += (sender, args) =>
                {
                    MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, "PlusClick");
                };
                _navView.NavigationItemSelected += (sender, args) =>
                {
                    TabItemSelect(args, _isSelected, e);
                };

            }
        }
        private void TabItemSelect(BottomNavigationView.NavigationItemSelectedEventArgs args, bool _isSelected, ElementChangedEventArgs<TabbedPage> e)
        {
            if (_isSelected)
                return;
            _isSelected = true;
            int ItemId = args.Item.ItemId;
            if (ItemId == 2)
            {
                _isSelected = false;
                return;
            }
            ResetNavigationBar();
            e.NewElement.CurrentPage = e.NewElement.Children[ItemId];
            switch (ItemId)
            {
                case 0:
                    args.Item.SetIcon(Resources.GetDrawable(Resource.Drawable.home_selected));
                    break;
                case 1:
                    args.Item.SetIcon(Resources.GetDrawable(Resource.Drawable.news_feed_selected));
                    break;
                case 2:

                    break;
                case 3:
                    args.Item.SetIcon(Resources.GetDrawable(Resource.Drawable.account_selected));
                    break;
                case 4:
                    args.Item.SetIcon(Resources.GetDrawable(Resource.Drawable.menu_selected));
                    break;
            }

            _isSelected = false;
        }
        private void ResetNavigationBar()
        {

            for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
            {
                var childView = this.ViewGroup.GetChildAt(i);
                if (childView is ViewGroup viewGroup)
                {
                    for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                    {
                        var childRelativeLayoutView = viewGroup.GetChildAt(j);
                        if (childRelativeLayoutView is BottomNavigationView)
                        {
                            IMenu menu = (childRelativeLayoutView as BottomNavigationView).Menu;

                            menu.FindItem(0).SetIcon(Resources.GetDrawable(Resource.Drawable.home));
                            menu.FindItem(1).SetIcon(Resources.GetDrawable(Resource.Drawable.news_Feed));

                            menu.FindItem(3).SetIcon(Resources.GetDrawable(Resource.Drawable.account));
                            menu.FindItem(4).SetIcon(Resources.GetDrawable(Resource.Drawable.menu));
                        }
                    }
                }
            }


        }
    }
}
