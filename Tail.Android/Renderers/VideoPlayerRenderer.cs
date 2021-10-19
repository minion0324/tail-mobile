using System;
using System.ComponentModel;
using System.IO;

using Android.Content;
using Android.Widget;


using Android.App;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Xamarin.Forms.Application;
using RelativeLayout = Android.Widget.RelativeLayout;
using Tail.Controls.CustomVideoPlayer;
using Tail.Droid.Renderers;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Util;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.Extractor;

[assembly: ExportRenderer(typeof(VideoPlayer),
                          typeof(VideoPlayerRenderer))]

namespace Tail.Droid.Renderers
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, RelativeLayout>
    {


        private PlayerView _playerView;
        private SimpleExoPlayer _player;
        public VideoPlayerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);

            if (_player == null)
                InitializePlayer();

        }

        private void InitializePlayer()
        {

            Context context = Android.App.Application.Context;
            string uri = (Element.Source as UriVideoSource).Uri;
            var mediaUri = Android.Net.Uri.Parse(uri);

            var userAgent = Util.GetUserAgent(context, "ExoPlayerDemo");
            var defaultHttpDataSourceFactory = new DefaultHttpDataSourceFactory(userAgent);
            var defaultDataSourceFactory = new DefaultDataSourceFactory(context, null, defaultHttpDataSourceFactory);
            var extractorMediaSource = new ExtractorMediaSource(mediaUri, defaultDataSourceFactory, new DefaultExtractorsFactory(), null, null);
            var defaultBandwidthMeter = new DefaultBandwidthMeter();
            var adaptiveTrackSelectionFactory = new AdaptiveTrackSelection.Factory(defaultBandwidthMeter);
            var defaultTrackSelector = new DefaultTrackSelector(adaptiveTrackSelectionFactory);


            _player = ExoPlayerFactory.NewSimpleInstance(context, defaultTrackSelector);
            _player.Prepare(extractorMediaSource);
            _player.PlayWhenReady = true;

            PlayerView _playView = new PlayerView(context) { Player = _player };

            Android.Widget.RelativeLayout relativeLayout = new Android.Widget.RelativeLayout(context);
            relativeLayout.SetBackgroundColor(Android.Graphics.Color.Transparent);
            relativeLayout.AddView(_playView);


            // Center the VideoView in the RelativeLayout
            Android.Widget.RelativeLayout.LayoutParams layoutParams =
                new Android.Widget.RelativeLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.MatchParent);
            layoutParams.AddRule(Android.Widget.LayoutRules.CenterInParent);
            _playView.LayoutParameters = layoutParams;
            _playView.SetBackgroundColor(Android.Graphics.Color.ParseColor("#000000"));

            
            SetNativeControl(relativeLayout);
        }
            
    }
   
}