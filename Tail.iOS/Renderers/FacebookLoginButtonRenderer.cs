using System;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using Plugin.Connectivity;
using Tail.Controls;
using Tail.iOS.Renderers;
using Tail.Services.Helper;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]

namespace Tail.iOS.Renderers
{
    public class FacebookLoginButtonRenderer : ButtonRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                UIButton button = Control;
               

                Control.BackgroundColor = UIKit.UIColor.Clear;
                button.TouchUpInside += delegate
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        HandleFacebookLoginClicked();
                    }
                    else
                    {
                        TailUtils.AlertNoInternetConnection();
                    }
                };

            }

            if (AccessToken.CurrentAccessToken != null)
            {
                App.PostSuccessFacebookAction(AccessToken.CurrentAccessToken.ToString());
            }
        }

        private void HandleFacebookLoginClicked()
        {
            if (AccessToken.CurrentAccessToken != null)
            {
                App.PostSuccessFacebookAction(AccessToken.CurrentAccessToken.TokenString);
            }
            else
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                LoginManager manager = new LoginManager();
                manager.Init();
                 manager.LogOut();
          
               
                manager.LogIn(new string[] { "public_profile" ,"email"},
                                                 vc,
                                                 (result, error) =>
                                                 {
                                                     if (error == null && !result.IsCancelled)
                                                     {
                                                         Tail.Helpers.Settings.FaceBookToken = result.Token.TokenString;
                                                         Console.WriteLine("Tail.Helpers.Settings.FaceBookToken = " + result.Token.TokenString);
                                                         App.PostSuccessFacebookAction(result.Token.TokenString);
                                                     }
                                                 });

            }

        }

    }
}