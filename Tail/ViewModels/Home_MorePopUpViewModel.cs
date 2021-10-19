using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class HomeMorePopUpViewModel : PageViewModelBase
    {
        #region private members
        PostDetails _postItem;

        Command _unfollow;
        Command _hide;
        Command _share;
        Command _report;
        
        #endregion
        #region Public members

        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        Action<PostDetails, bool, int> _popupCloseCallback;
        public Action<PostDetails, bool, int> PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }

        public Command Unfollow => _unfollow ?? (_unfollow = new Command(async () => await Handle_Unfollow()));
        public Command Hide => _hide ?? (_hide = new Command(async () => await Handle_Hide()));
        public Command Share => _share ?? (_share = new Command(async () => await Handle_Share()));
        public Command Report => _report ?? (_report = new Command(async () => await Handle_Report()));

        #endregion

         

        public HomeMorePopUpViewModel()
        {
        }

        async Task Handle_Unfollow()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if ((PostItem.UserId == SettingsService.Instance.LoggedUserDetails.UserId && !PostItem.IsShare) || ( PostItem.IsShare && PostItem.SharedUserDetails.ShareUserId == SettingsService.Instance.LoggedUserDetails.UserId ) )
            {
                bool confirmation = false;
                if (PostItem.IsShare)
                {
                    confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmShareDelete);
                }
                else if (PostItem.Post_Type == PostType.Pick)
                {
                     confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmPickDelete);
                }
                else
                {
                     confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmPostDelete);
                }
               
                if (confirmation)
                {
                   
                    PopupCloseCallback?.Invoke(PostItem, false, 1);
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            else
            {
                if (PostItem.IsFollowingBack)
                {
                    if (!await UnFollowUser(PostItem.UserId))
                    {
                        IsBusy = false;
                        return;
                    }     
                }
                else
                {
                    if (!await FollowUser(PostItem.UserId))
                    {
                        IsBusy = false;
                        return;
                    }
                }
                PostItem.IsFollowingBack = !PostItem.IsFollowingBack;
                await PopupNavigation.Instance.PopAsync();
            }

            IsBusy = false;
        }
        async Task Handle_Hide()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (await HideStatusUpdate(PostItem.PostId))
            {
                PopupCloseCallback?.Invoke(PostItem, false, 2);
                await PopupNavigation.Instance.PopAsync();
            }
            IsBusy = false;
        }
        async Task Handle_Share()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            PopupCloseCallback?.Invoke(PostItem, true, 2);
            await PopupNavigation.Instance.PopAsync();
            await PopupNavigation.Instance.PushAsync(new ShareOptionPopup(PostItem, async () => await Handle_SharePopUpClosed()));
            IsBusy = false;
        }
        async Task Handle_SharePopUpClosed()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        async Task Handle_Report()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (PostItem.IsReport)
            {
                await ShowAlert(AppResources.AppName, AppResources.ReportedPostAlert);
                IsBusy = false;
                return;
            }
                
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateWithInTabToAsync<ReportPost>(PostItem);
            IsBusy = false;
        }
        async Task<bool> HideStatusUpdate(string PostId)
        {
            bool hasSuccessResponse = false;

            try
            {
                PostStatusRequest requestObj = new PostStatusRequest()
                {
                    postId = PostId
                };
                var likeResponse = await TailDataServiceProvider.Instance.HideAPost(requestObj);
                if (likeResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, likeResponse.Message);
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
