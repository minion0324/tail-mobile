using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using Tail.Controls;
using Tail.iOS.Renderers;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(TailWebview), typeof(TailWebviewRendereriOs))]

namespace Tail.iOS.Renderers
{
    public class TailWebviewRendereriOs : ViewRenderer<TailWebview, WKWebView>
    {
        WKWebView wkWebView;

        protected override void OnElementChanged(ElementChangedEventArgs<TailWebview> e)
        {
            base.OnElementChanged(e);

            //if (Control == null)
            //{
            //    WKWebViewConfiguration configuration = new WKWebViewConfiguration();
            //    configuration.AllowsInlineMediaPlayback = true;
            //    wkWebView = new WKWebView(CGRect.Empty, configuration);

            //    if (Element!= null &&(Element).Url != null)
            //    {
            //        wkWebView.LoadRequest(new NSUrlRequest(new NSUrl((Element).Url)));
            //    }
            //    SetNativeControl(wkWebView);
            //}
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (Element != null && (Element).Url != null && e.PropertyName.Equals(nameof(Element.Source)))
            //{
            //    wkWebView.LoadRequest(new NSUrlRequest(new NSUrl((Element).Url)));
            //}
        }

        //public override void Draw(CGRect rect)
        //{
        //    base.Draw(rect);

        //    Control.Frame = rect;
        //}
    }
    
}
