using System;
using Android.Content;
using Tail.Controls;
using Tail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TailWebview), typeof(TailWebviewRendererAndroid))]

namespace Tail.Droid.Renderers
{
    public class TailWebviewRendererAndroid : WebViewRenderer
    {

        public TailWebviewRendererAndroid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Settings.JavaScriptEnabled = false;
            }
            //TailWebview _webView = (TailWebview)e.NewElement;

            //if (null == e.OldElement)
            //{
            //    Control.SetWebViewClient(new ExtendedWebViewClient(_webView));
                
            //}
        }

    }
    //internal class ExtendedWebViewClient : Android.Webkit.WebViewClient
    //{
    //    private readonly TailWebview _xwebView;

    //    public ExtendedWebViewClient(TailWebview xwebView)
    //    {
    //        _xwebView = xwebView;
    //    }
    //    public override void OnPageFinished(Android.Webkit.WebView view, string url)
    //    {
    //        try
    //        {
    //            if (_xwebView != null)
    //            {
    //                _xwebView.InvokeCompleted();
    //            }
    //            base.OnPageFinished(view, url);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"{ex.Message}");
    //        }
    //    }
    //}
}
