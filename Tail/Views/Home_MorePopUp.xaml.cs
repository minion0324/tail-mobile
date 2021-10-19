using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tail.ViewModels;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Models;

namespace Tail.Views
{
    public partial class Home_MorePopUp : PopupPage
    {

        readonly HomeMorePopUpViewModel _vModel;
        public Home_MorePopUp(PostDetails postItem, Action<PostDetails, bool, int> popUpCloseCallback)
        {
            string username = postItem.UserName;
            int userId = postItem.UserId;
            InitializeComponent();
            _vModel = new HomeMorePopUpViewModel();
            _vModel.PopupCloseCallback = popUpCloseCallback;
            BindingContext = _vModel;
            _vModel.PostItem = postItem;
            if (postItem.Post_Type == PostType.Pick)
            {
                SharePostLabel.Text = AppResources.SharePick;
            }
            else
            {
                SharePostLabel.Text = AppResources.SharePost;
            }
            if (postItem.IsShare && postItem.SharedUserDetails.ShareUserId== SettingsService.Instance.LoggedUserDetails.UserId)
            {
                if (postItem.Post_Type == PostType.Pick)
                {
                    LabelUnfollow.Text = AppResources.DeletePickText;
                }
                else
                {
                    LabelUnfollow.Text = AppResources.DeletePostText;
                }
                MainGrid.RowDefinitions[2].Height = 0;
                MainGrid.RowDefinitions[3].Height = 0;
                MainGrid.RowDefinitions[4].Height = 0;

                return;
            }

            if (userId != SettingsService.Instance.LoggedUserDetails.UserId)
            {
                if (postItem.IsFollowingBack)
                {
                    LabelUnfollow.Text = AppResources.Unfollow + " " + username;
                }
                else
                {
                    LabelUnfollow.Text = AppResources.Follow + " " + username;
                }
                if (postItem.Post_Type == PostType.Pick)
                {
                    LabelReport.Text = AppResources.ReportPick;
                }
                else
                {
                    LabelReport.Text = AppResources.ReportPost;
                }
                if (postItem.Post_Type == PostType.Pick)
                {
                    LabelHide.Text = AppResources.HidePick;
                }
                else
                {
                    LabelHide.Text = AppResources.HidePost;
                }
                if (postItem.IsReport)
                    LabelReport.Text = AppResources.ReportedPost;


                reportPostBox.IsVisible = false;
            }
            else
            {
                if (postItem.Post_Type == PostType.Pick)
                {
                    LabelUnfollow.Text = AppResources.DeletePickText;
                }
                else
                {
                    LabelUnfollow.Text = AppResources.DeletePostText;
                }

                MainGrid.RowDefinitions[2].Height = 0;
                hidePost.IsVisible = false;
                hidePostBox.IsVisible = false;
                reportPost.IsVisible = false;
                reportPostBox.IsVisible = false;
                sharePostBox.IsVisible = false;
                MainGrid.RowDefinitions[4].Height = 0;

            }


        }
        protected async void DismissPopup_Clicked(object sender, EventArgs e)
        {

            await PopupNavigation.Instance.PopAsync();
        }
    }
}
