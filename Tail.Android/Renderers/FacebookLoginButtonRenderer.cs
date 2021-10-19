using System;
using Android.Content;
using Plugin.Connectivity;
using Tail.Controls;
using Tail.Droid.Helpers;
using Tail.Droid.Renderers;
using Tail.Services.Helper;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace Tail.Droid.Renderers
{
    
	public class FacebookLoginButtonRenderer : ButtonRenderer
	{


		public FacebookLoginButtonRenderer(Context context) : base(context)
		{
		}
		

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				var view = (FacebookLoginButton)Element;
				
				var loginButton = new LoginButton(this.Context);
				loginButton.SetLoginText(view.Text);
				loginButton.SetPadding(0, 12, 0, 0);
                loginButton.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, 0, 0);
				
				Control.SetPadding(0, 0, 0, 0);
				Control.TextAlignment = Android.Views.TextAlignment.Center;
				

				var facebookCallback = new FacebookCallback<LoginResult>
				{
					HandleSuccess = shareResult =>
					{
						loginButton.Text = "Login with Facebook";

						Action<string> local = App.PostSuccessFacebookAction;
						if (local != null)
						{
							local(shareResult.AccessToken.Token);
							LoginManager.Instance.LogOut();
						}
					},
					HandleCancel = () =>
					{
						Console.WriteLine("HelloFacebook: Canceled");
					},
					HandleError = shareError =>
					{
						Console.WriteLine("HelloFacebook: Error: {0}", shareError);
					}
				};
				if (CrossConnectivity.Current.IsConnected)
				{
					loginButton.RegisterCallback(MainActivity.CallbackManager, facebookCallback);
					loginButton.SetReadPermissions("public_profile", "email");
					loginButton.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    
				}
				else
				{
					TailUtils.AlertNoInternetConnection();
				}
				base.SetNativeControl(loginButton);

				if (AccessToken.CurrentAccessToken != null)
				{
					App.PostSuccessFacebookAction(AccessToken.CurrentAccessToken.ToString());
				}
			}
		}
	}
}
