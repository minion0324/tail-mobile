using System;
using Android.Content;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Extractor;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Util;
using Tail.Droid.DataHelpers;
using Tail.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExoPlayer))]
namespace Tail.Droid.DataHelpers
{
    public class ExoPlayer : Tail.Services.Interfaces.IExoPlayer
    {
        public void InitPlayer(string videoUrl)
        {
            Context context = Android.App.Application.Context;
            SimpleExoPlayer _player;
            var mediaUri = Android.Net.Uri.Parse(videoUrl);

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
            //SetNativeControl(relativeLayout);
        }
    }
}
