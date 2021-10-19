using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ServiceProviders;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ShareOptionPopupViewModel : PageViewModelBase
    {
        #region private members
        Command _shareNowCommand;
        Command _moreOptionsCommand;

        PostDetails _postItem;
        string _shareText;
        #endregion
        #region Public members
        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        public string ShareText
        {
            get => _shareText;
            set => SetProperty(ref _shareText, value);
        }
        public Command ShareNowCommand => _shareNowCommand ?? (_shareNowCommand = new Command(async () => await Handle_ShareNowCommand()));
        public Command MoreOptionsCommand => _moreOptionsCommand ?? (_moreOptionsCommand = new Command(async () => await Handle_MoreOptionsCommand()));


        #endregion
        public ShareOptionPopupViewModel()
        {
        }
        async Task Handle_ShareNowCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            Debug.WriteLine(ShareText);
            if (PostItem != null && await AddShare())
            {
                MessagingCenter.Send<string>("", "SharedAPost");
                await PopupNavigation.Instance.PopAsync();
                IsBusy = false;
            }

        }
        private async Task Handle_MoreOptionsCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await PopupNavigation.Instance.PopAsync();
            await ShareUri();
            IsBusy = false;
        }
        public async Task ShareUri()
        {

            var name = PostItem.UserName;
            var typedShareText = string.Empty;
            if (ShareText != null)
                typedShareText = ShareText.Trim();
            if (PostItem.Post_Type == PostType.Free)
            {
                var postText1 = string.Format(AppResources.SocialMediaSharePostText, name);
                var postText = string.Empty;
                if (typedShareText != string.Empty)
                {
                    postText = typedShareText + "\n\n" + postText1 ;
                }
                else
                {
                    postText = postText1 ;
                }
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = postText
                });
            }
            else
            {
                var postText1 = string.Format(AppResources.SocialMediaSharePickText, name);
                var postText = string.Empty;
                if (typedShareText != string.Empty)
                {
                    postText = typedShareText + "\n\n" + postText1 ;
                }
                else
                {
                    postText = postText1 ;
                }
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = postText
                });
            }

        
        }

        async Task<bool> AddShare()
        {
            bool hasSuccessResponse = false;

            try
            {

                ShareRequest requestObj = new ShareRequest()
                {
                    postId = PostItem.PostId,
                    sDesc = ShareText
                };
                var shareResponse = await TailDataServiceProvider.Instance.AddShare(requestObj);
                if (shareResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, shareResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

    }
}
