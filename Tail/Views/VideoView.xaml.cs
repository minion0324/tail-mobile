using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Controls.CustomVideoPlayer;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class VideoView : PopupPage
    {
        readonly VideoViewModel _vModel;
        public VideoView(string videoUrl)
        {
            InitializeComponent();

            _vModel = new VideoViewModel();
            
            BindingContext = _vModel;
            _vModel.VideoUrl = videoUrl;
            videoPlayer.Source = VideoSource.FromUri(videoUrl);
            videoPlayer.Play();
          
            MessagingCenter.Subscribe<object>(this, "PlayInFullScreen",  (val) =>
            {
                TopBar.IsVisible = false;
            });
            MessagingCenter.Subscribe<object>(this, "FullScreenEnded", (val) =>
            {
                TopBar.IsVisible = true;
            });
            MessagingCenter.Subscribe<object>(this, "ShowLoading", (val) =>
            {
                _vModel.IsBusy = true;
              
            });
            MessagingCenter.Subscribe<object>(this, "HideLoading", (val) =>
            {
                _vModel.IsBusy = false;
                
            });
        }
       
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Unsubscribe<object>(this, "HideLoading");
            MessagingCenter.Unsubscribe<object>(this, "ShowLoading");
            MessagingCenter.Unsubscribe<object>(this, "FullScreenEnded");
            MessagingCenter.Unsubscribe<object>(this, "PlayInFullScreen");
            await PopupNavigation.Instance.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<object>(this, "HideLoading");
            MessagingCenter.Unsubscribe<object>(this, "ShowLoading");
            MessagingCenter.Unsubscribe<object>(this, "FullScreenEnded");
            MessagingCenter.Unsubscribe<object>(this, "PlayInFullScreen");
            try
            {
                
                return base.OnBackButtonPressed();
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }

            return false;
        }
    }
}