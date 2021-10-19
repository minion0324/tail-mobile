using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Tail.Droid.Renderers
{
    public class FullScreenMediaController : MediaController
    {
        private ImageButton fullScreen;
        private readonly Action fullScreenAction;
        public FullScreenMediaController(Context context, Action fullScreenAction) : base(context)
        {
            this.fullScreenAction = fullScreenAction;
        }

        public override void SetAnchorView(View view)
        {
            base.SetAnchorView(view);
            fullScreen = new ImageButton(Context);
            fullScreen.SetBackgroundColor(Color.Transparent);
            var parameters =
                new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                {
                    Gravity = GravityFlags.End,
                    TopMargin = 20,
                    RightMargin = 40
                };

            AddView(fullScreen, parameters);
            SetFullScreenImageSource();
            fullScreen.Click += (e, args) =>
            {
                fullScreenAction?.Invoke();
            };
        }

        public void SetFullScreenImageSource()
        {
            fullScreen.SetImageResource(Resource.Drawable.icon_fullscreen);
        }

        public void SetExitFullScreenImageSource()
        {
            fullScreen.SetImageResource(Resource.Drawable.icon_fullscreen_exit);
        }
    }
}