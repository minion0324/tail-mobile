using System.Threading.Tasks;
using Xamarin.Forms;
using Tail.Services.Interfaces;
using Tail.Services.ApplicationServices;
using Tail.Common;
using System.Linq;
using Tail.Views;
using System;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Tail.Models;
using Rg.Plugins.Popup.Services;
using Tail.Services.ServiceProviders;
using Tail.Services.LocalStorage;
using Tail.Services.Helper;


namespace Tail.ViewModels
{
    public class PageViewModelBase : ViewModelBase
    {
        bool _isBusy;
        bool _isLoading;
        bool _isFollowing;

        Command _back;
        Command _logout;
        Command _walletCommand;
        Command _notificationCommand;
        Command _skip;
        Command _sesionOut;
        Command _tokenExpire;
        Command<int> _userDetails;

        LocalizedResources _resources;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public bool IsFollowing
        {
            get => _isFollowing;
            set => SetProperty(ref _isFollowing, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public LocalizedResources Resources
        {
            get => _resources;
            private set => SetProperty(ref _resources, value);
        }

        int notificationCount;
        public int NotificationCount
        {
            get { return notificationCount; }
            set
            {
                SetProperty(ref notificationCount, value);
            }
        }
        public IAppNavigationService NavigationService { get; }

        public Command Back => _back ?? (_back = new Command(() => Handle_BackCommand()));
        public Command Logout => _logout ?? (_logout = new Command(async () => await Handle_LogoutCommand()));
        public Command SesionOut => _sesionOut ?? (_sesionOut = new Command(async () => await Handle_SessionoutCommand()));
        public Command TokenExpire => _tokenExpire ?? (_tokenExpire = new Command(async () => await Handle_TokenExpireCommand()));
        public Command WalletCommand => _walletCommand ?? (_walletCommand = new Command(async () => await Handle_WalletCommand()));
        public Command NotificationCommand => _notificationCommand ?? (_notificationCommand = new Command(async () => await Handle_NotificationCommand()));
        public Command Skip => _skip ?? (_skip = new Command(async () => await Handle_SkipCommand()));
        public Command<int> UserDetails => _userDetails ?? (_userDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID)));
        public PageViewModelBase()
        {
            NavigationService = AppNavigationService.GetInstance();
            Resources = new LocalizedResources(typeof(AppResources));
            MessagingCenter.Unsubscribe<NotificationInfo>(this, Tail.Common.Constants.NotificationMessage);

            MessagingCenter.Subscribe<NotificationInfo>(this, Tail.Common.Constants.NotificationMessage, (obj) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    NotificationCount = SettingsService.Instance.NotificationCount;
                });
            });
        }

        public virtual Task InitializeAsync(object parameter = null)
        {
            return Task.CompletedTask;
        }

        public virtual void OnPageAppearing()
        {

        }


        public virtual void OnPageDisappearing()
        {


        }
        public virtual void OnPageDestroy()
        {


        }

        public virtual void Handle_BackCommand()
        {

            string MainPageType = Application.Current.MainPage.Navigation.NavigationStack.Last().GetType().Name;
            if (MainPageType == "Home")
            {
                NavigationService.PopLastTabbedPageAsync();
            }
            else
            {
                NavigationService.PopLastPageAsync();
            }

        }

        async Task Handle_SkipCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            await ShowAlert(AppResources.AppName, AppResources.NotImplemented);

            IsBusy = false;
        }
        async Task Handle_WalletCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            await NavigationService.NavigateWithInTabToAsync<Coins>();
            IsBusy = false;
        }
        async Task Handle_NotificationCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<Notification>();
            IsBusy = false;
        }
        async Task Handle_LogoutCommand()
        {
            bool confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmLogout);
            if (confirmation)
            {

                LogoutRequestInfo requestObj = new LogoutRequestInfo()
                {
                    refreshToken = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.RefreshToken : ""
                };
                var logoutResponse = await TailDataServiceProvider.Instance.Logout(requestObj);
                if (logoutResponse.ErrorCode == 200)
                {
                    SettingsService.Instance.Authenticated = false;
                    SettingsService.Instance.PartialRegstarionUserID = 0;
                    SettingsService.Instance.FromEditProfile = false;
                    SettingsService.Instance.FromSettings = false;
                    CommonSingletonUtility.SharedInstance.IsFromMenu = false;
                    if (SettingsService.Instance.LoggedUserDetails != null && SettingsService.Instance.LoggedUserDetails.Login_type == LoginType.Email)
                    {
                        if (!SettingsService.Instance.IsSavedCredentials)
                        {
                            SettingsService.Instance.LoggedUserDetails = null;
                        }
                    }
                    else
                    {
                        SettingsService.Instance.LoggedUserDetails = null;
                        SettingsService.Instance.IsSavedCredentials = false;
                    }
                    DataStorageService.Instance.ClearAllNotifications();
                    await NavigationService.SetAsMainPageAsync<Login>();
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, logoutResponse.Message);
                }
            }
        }
        async Task Handle_SessionoutCommand()
        {
            if (await LogoutUser())
            {
                await ShowAlert(AppResources.AppName, AppResources.SessionExpireMessage);
            }
        }
        async Task Handle_TokenExpireCommand()
        {

            SettingsService.Instance.Authenticated = false;
            SettingsService.Instance.PartialRegstarionUserID = 0;
            SettingsService.Instance.FromEditProfile = false;
            SettingsService.Instance.FromSettings = false;
            CommonSingletonUtility.SharedInstance.IsFromMenu = false;
            if (SettingsService.Instance.LoggedUserDetails != null && SettingsService.Instance.LoggedUserDetails.Login_type == LoginType.Email)
            {
                if (!SettingsService.Instance.IsSavedCredentials)
                {
                    SettingsService.Instance.LoggedUserDetails = null;
                }
            }
            else
            {
                SettingsService.Instance.LoggedUserDetails = null;
                SettingsService.Instance.IsSavedCredentials = false;
            }
            DataStorageService.Instance.ClearAllNotifications();
            await NavigationService.SetAsMainPageAsync<Login>();
            await ShowAlert(AppResources.AppName, AppResources.SessionExpireMessage);

        }

        public async Task<bool> LogoutUser()
        {
            bool hasSuccessResponse = false;
            LogoutRequestInfo requestObj = new LogoutRequestInfo()
            {
                refreshToken = (SettingsService.Instance.LoggedUserDetails != null) ? SettingsService.Instance.LoggedUserDetails.RefreshToken : ""
            };
            var logoutResponse = await TailDataServiceProvider.Instance.Logout(requestObj);
            if (logoutResponse.ErrorCode == 200)
            {

                SettingsService.Instance.Authenticated = false;
                SettingsService.Instance.PartialRegstarionUserID = 0;
                SettingsService.Instance.FromEditProfile = false;
                SettingsService.Instance.FromSettings = false;
                CommonSingletonUtility.SharedInstance.IsFromMenu = false;
                if (SettingsService.Instance.LoggedUserDetails != null && SettingsService.Instance.LoggedUserDetails.Login_type == LoginType.Email)
                {
                    if (!SettingsService.Instance.IsSavedCredentials)
                    {
                        SettingsService.Instance.LoggedUserDetails = null;
                    }
                }
                else
                {
                    SettingsService.Instance.LoggedUserDetails = null;
                    SettingsService.Instance.IsSavedCredentials = false;
                }
                DataStorageService.Instance.ClearAllNotifications();
                hasSuccessResponse = true;
                await NavigationService.SetAsMainPageAsync<Login>();

            }
            else
            {
                await ShowAlert(AppResources.AppName, logoutResponse.Message);
            }
            return hasSuccessResponse;
        }
        public async Task Handle_UserDetailsCommand(int userID)
        {
            if (IsBusy || userID == 0)
                return;
            IsBusy = true;
            if (userID != SettingsService.Instance.LoggedUserDetails.UserId)
            {
                SettingsService.Instance.IsOtherUserProfile = true;
                await NavigationService.NavigateWithInTabToAsync<MyProfile>(userID);
            }
            else
            {
                var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
                SettingsService.Instance.CurrentTabIndex = 3;
                TabbedPage currentTabbedPage = currentPage as TabbedPage;
                if (currentTabbedPage != null)
                    currentTabbedPage.CurrentPage = currentTabbedPage.Children[3];
            }
            IsBusy = false;
        }
        public async Task ShowAlert(string title, string message)
        {
            await NavigationService.ShowAlertAsync(title, message);
        }

        public async Task<bool> ShowConfirm(string title, string message)
        {
            return await NavigationService.ShowConfirmAlertAsync(title, message);
        }
        public async Task Handle_PickPurchase(PostDetails postItem)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await GetCoinBalance();
            if (CommonSingletonUtility.SharedInstance.CoinBalance >= postItem.PickInfo[0].PickPrice)
            {
                if (postItem.PickInfo[0].PickPrice > 0)
                {
                    CommonSingletonUtility.SharedInstance.SelectedPostDetails = postItem;
                    await PopupNavigation.Instance.PushAsync(new PickPurchasePopup(async () => await Handle_PickPurchasePopUpClosedAsync(), postItem));

                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        postItem.PickInfo[0].IsPickPurchase = true;
                    });
                }
            }
            else
            {
                var response = await ShowConfirm(AppResources.AppName, AppResources.NotEnoughCoinsText);
                if (response)
                {
                    await NavigationService.NavigateWithInTabToAsync<AddCoins>();
                }

            }
            IsBusy = false;

        }
        public async Task GetCoinBalance()
        {
            var response = await TailDataServiceProvider.Instance.GetCoinDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                CommonSingletonUtility.SharedInstance.CoinBalance = response.ResponseData.BalanceCoins;
            }
        }
        async Task Handle_PickPurchasePopUpClosedAsync()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
        public async Task<bool> CheckCameraPermissionsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await ShowAlert(AppResources.AppName, AppResources.CameraPermmission);
                    }
                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;

                }
                else if (status != PermissionStatus.Unknown && Device.RuntimePlatform == Device.iOS)
                {

                    var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                    if (res) { CrossPermissions.Current.OpenAppSettings(); }
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return false;
        }

        public async Task<bool> CheckGalleryPermissionsAsync()
        {
            try
            {
                Plugin.Permissions.Abstractions.PermissionStatus status;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                    if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos) && Device.RuntimePlatform == Device.iOS)
                        {

                            await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.GalleryPermmission, AppResources.OKText);

                        }

                        status = await CrossPermissions.Current.RequestPermissionAsync<PhotosPermission>();
                        if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                        {

                            var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                            if (res) { CrossPermissions.Current.OpenAppSettings(); }

                        }
                    }
                }
                else
                {
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                    if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage) && Device.RuntimePlatform == Device.iOS)
                        {

                            await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.GalleryPermmission, AppResources.OKText);

                        }

                        status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                        if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                        {

                            var res = await Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.EnableFromSettings, AppResources.SettingsText, AppResources.CancelText);
                            if (res) { CrossPermissions.Current.OpenAppSettings(); }

                        }
                    }
                }

                if (status == PermissionStatus.Granted)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return false;
        }
        public async Task<bool> LikeUpdate(string PostId)
        {
            bool hasSuccessResponse = false;

            try
            {
                PostStatusRequest requestObj = new PostStatusRequest()
                {
                    postId = PostId
                };
                var likeResponse = await TailDataServiceProvider.Instance.LikeAPost(requestObj);
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
        public async Task<bool> DisLikeUpdate(string PostId)
        {
            bool hasSuccessResponse = false;

            try
            {
                PostStatusRequest requestObj = new PostStatusRequest()
                {
                    postId = PostId
                };
                var likeResponse = await TailDataServiceProvider.Instance.DisLikeAPost(requestObj);
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
        public async Task<bool> FollowUser(int UserId)
        {
            bool hasSuccessResponse = false;

            try
            {
                FollowRequest reqObj = new FollowRequest()
                {
                    fwgUId = UserId
                };
                var followResponse = await TailDataServiceProvider.Instance.FollowUser(reqObj);
                if (followResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                    IsFollowing = true;
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, followResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public async Task<bool> UnFollowUser(int UserId)
        {
            bool hasSuccessResponse = false;

            try
            {
                FollowRequest reqObj = new FollowRequest()
                {
                    fwgUId = UserId
                };

                var unfollowResponse = await TailDataServiceProvider.Instance.UnFollowUser(reqObj);
                if (unfollowResponse.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                    IsFollowing = false;
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, unfollowResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }

        public void UpdatePickUI(PostDetails postInfo)
        {


            TimeSpan _timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(postInfo.PickInfo[0].GameDateTime));

            if (_timeSpan.Seconds > 0)
            {
                postInfo.PickInfo[0].IsPaidPickPurchaseEnable = false;
                postInfo.PickInfo[0].DisplyPickPrice = AppResources.WaitingForResult;
            }
            else
            {
                postInfo.PickInfo[0].IsPaidPickPurchaseEnable = true;
                postInfo.PickInfo[0].DisplyPickPrice = (postInfo.PickInfo[0].PickPrice > 1) ? string.Format(AppResources.UnlockText, postInfo.PickInfo[0].PickPrice.ToString("0")) : string.Format(AppResources.UnlockTextSingle, postInfo.PickInfo[0].PickPrice.ToString("0"));
            }


            switch (postInfo.PickInfo[0].BetType)
            {
                case 1:
                    postInfo.PickInfo[0].BetPointHome = (postInfo.PickInfo[0].HmoScore > 0) ? "+" + postInfo.PickInfo[0].HmoScore : postInfo.PickInfo[0].HmoScore.ToString();
                    postInfo.PickInfo[0].BetPointAway = (postInfo.PickInfo[0].AmoScore > 0) ? "+" + postInfo.PickInfo[0].AmoScore : postInfo.PickInfo[0].AmoScore.ToString();
                    postInfo.PickInfo[0].BetValueHome = "";
                    postInfo.PickInfo[0].BetValueAway = "";
                    break;
                case 2:
                    postInfo.PickInfo[0].BetPointHome = (postInfo.PickInfo[0].HspScore > 0) ? "+" + postInfo.PickInfo[0].HspScore : postInfo.PickInfo[0].HspScore.ToString();
                    postInfo.PickInfo[0].BetPointAway = (postInfo.PickInfo[0].AspScore > 0) ? "+" + postInfo.PickInfo[0].AspScore : postInfo.PickInfo[0].AspScore.ToString();
                    postInfo.PickInfo[0].BetValueHome = (postInfo.PickInfo[0].HPtSpMoney > 0) ? "+" + postInfo.PickInfo[0].HPtSpMoney : postInfo.PickInfo[0].HPtSpMoney.ToString();
                    postInfo.PickInfo[0].BetValueAway = (postInfo.PickInfo[0].APtSpMoney > 0) ? "+" + postInfo.PickInfo[0].APtSpMoney : postInfo.PickInfo[0].APtSpMoney.ToString();
                    break;
                case 3:
                    postInfo.PickInfo[0].BetPointHome = "O " + postInfo.PickInfo[0].OverScore;
                    postInfo.PickInfo[0].BetPointAway = "U " + postInfo.PickInfo[0].UnderScore;
                    postInfo.PickInfo[0].BetValueHome = (postInfo.PickInfo[0].TlOvMoney > 0) ? "+" + postInfo.PickInfo[0].TlOvMoney : postInfo.PickInfo[0].TlOvMoney.ToString();
                    postInfo.PickInfo[0].BetValueAway = (postInfo.PickInfo[0].TlUnMoney > 0) ? "+" + postInfo.PickInfo[0].TlUnMoney : postInfo.PickInfo[0].TlUnMoney.ToString();
                    break;

            }

            postInfo.PickInfo[0].SelectedSpotName = TailUtils.GameTypeToName(postInfo.PickInfo[0].Sport_Type);
            postInfo.PickInfo[0].SelectedSpotImage = TailUtils.GameTypeToImage_Home(postInfo.PickInfo[0].Sport_Type);
            postInfo.PickInfo[0].AccuracyText = AppResources.AccuracyIn + postInfo.PickInfo[0].SelectedSpotName;
            postInfo.PickInfo[0].SelectedEventImage = (postInfo.PickInfo[0].SportId == 4) ? "NBA.png" : string.Empty;
            postInfo.PickInfo[0].Last10PredText = AppResources.LastPredictions + postInfo.PickInfo[0].SelectedSpotName;
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDateTime))
            {
                DateTime _eventDateTime = Convert.ToDateTime(postInfo.PickInfo[0].GameDateTime);
                string _displayFormate = _eventDateTime.ToString("ddd, MMM d yyyy");
                postInfo.PickInfo[0].GameDate = _displayFormate;
            }
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDateTime))
            {
                DateTime _eventDateTime = Convert.ToDateTime(postInfo.PickInfo[0].GameDateTime);
                string _displayFormate = _eventDateTime.ToString("hh:mm tt");
                postInfo.PickInfo[0].GameTime = _displayFormate;
            }
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].GameDate))
            {
                postInfo.PickInfo[0].DisplayGameDateTime = postInfo.PickInfo[0].GameDate + " at " + postInfo.PickInfo[0].GameTime;
            }
            if (string.IsNullOrEmpty(postInfo.PickInfo[0].HtLogo))
                postInfo.PickInfo[0].FirstTeamImage = "team_placeholder.png";
            else
                postInfo.PickInfo[0].FirstTeamImage = Constants.TEAM_LOGO_PATH + postInfo.PickInfo[0].HtLogo;
            if (string.IsNullOrEmpty(postInfo.PickInfo[0].AtLogo))
                postInfo.PickInfo[0].SecondTeamImage = "team_placeholder.png";
            else
                postInfo.PickInfo[0].SecondTeamImage = Constants.TEAM_LOGO_PATH + postInfo.PickInfo[0].AtLogo;

            postInfo.PickInfo[0].PickPurchase_Type = (postInfo.PickInfo[0].PickPrice == 0) ? PickPurchaseType.Free : PickPurchaseType.Paid;
            switch (postInfo.PickInfo[0].TypeToWin)
            {

                case 2:
                    postInfo.PickInfo[0].PredictionTeam = TeamType.Team1;
                    postInfo.ResultTextAway = "";
                    if (postInfo.Result_Type == ResultType.Won)
                    {
                        postInfo.PickInfo[0].HomeBackground = "#13B080";
                        postInfo.ResultTextHome = AppResources.WonText;

                    }
                    else if (postInfo.Result_Type == ResultType.Lost)
                    {
                        postInfo.PickInfo[0].HomeBackground = "#CE3D3D";
                        postInfo.ResultTextHome = AppResources.LostText;
                    }
                    else if (postInfo.Result_Type == ResultType.Push)
                    {
                        postInfo.PickInfo[0].HomeBackground = "#c4c4c4";
                        postInfo.ResultTextHome = AppResources.PushText;
                    }
                    else
                    {
                        postInfo.PickInfo[0].HomeBackground = "#642364";
                    }

                    postInfo.PickInfo[0].HomeBorderVisible = true;
                    postInfo.PickInfo[0].AwayBackground = "#ffffff";
                    postInfo.PickInfo[0].OpacityHome = 1.0;
                    postInfo.PickInfo[0].OpacityAway = 1.0;
                    postInfo.PickInfo[0].OpacityMoneyTextyHome = 0.5;
                    postInfo.PickInfo[0].OpacityMoneyTextAway = 1.0;
                    postInfo.PickInfo[0].HomeTextColor = "#ffffff";
                    postInfo.PickInfo[0].HomeMoneyTextColor = "#ffffff";
                    postInfo.PickInfo[0].AwayTextColor = "#000000";
                    postInfo.PickInfo[0].AwayMoneyTextColor = "#B5B5B5";
                    postInfo.PickInfo[0].SelectedTextHome = "Selected";
                    postInfo.PickInfo[0].SelectedTextAway = "";
                    break;
                case 1:
                    postInfo.PickInfo[0].PredictionTeam = TeamType.Team2;
                    postInfo.PickInfo[0].HomeBorderVisible = false;
                    postInfo.PickInfo[0].HomeBackground = "#ffffff";
                    postInfo.ResultTextHome = "";
                    if (postInfo.Result_Type == ResultType.Won)
                    {
                        postInfo.PickInfo[0].AwayBackground = "#13B080";
                        postInfo.ResultTextAway = AppResources.WonText;
                    }
                    else if (postInfo.Result_Type == ResultType.Lost)
                    {
                        postInfo.PickInfo[0].AwayBackground = "#CE3D3D";
                        postInfo.ResultTextAway = AppResources.LostText;
                    }
                    else if (postInfo.Result_Type == ResultType.Push)
                    {
                        postInfo.PickInfo[0].AwayBackground = "#c4c4c4";
                        postInfo.ResultTextAway = AppResources.PushText;
                    }
                    else
                    {
                        postInfo.PickInfo[0].AwayBackground = "#642364";
                    }
                    postInfo.PickInfo[0].OpacityHome = 1.0;
                    postInfo.PickInfo[0].OpacityAway = 1.0;
                    postInfo.PickInfo[0].OpacityMoneyTextyHome = 1.0;
                    postInfo.PickInfo[0].OpacityMoneyTextAway = 0.5;
                    postInfo.PickInfo[0].HomeTextColor = "#000000";
                    postInfo.PickInfo[0].HomeMoneyTextColor = "#B5B5B5";
                    postInfo.PickInfo[0].AwayTextColor = "#ffffff";
                    postInfo.PickInfo[0].AwayMoneyTextColor = "#ffffff";
                    postInfo.PickInfo[0].SelectedTextHome = "";
                    postInfo.PickInfo[0].SelectedTextAway = "Selected";
                    break;
                default:
                    postInfo.PickInfo[0].PredictionTeam = TeamType.Team1;
                    postInfo.PickInfo[0].HomeBorderVisible = true;
                    postInfo.ResultTextAway = "";
                    if (postInfo.Result_Type == ResultType.Won)
                    {
                        postInfo.PickInfo[0].HomeBackground = "#13B080";
                        postInfo.ResultTextHome = AppResources.WonText;
                    }
                    else if (postInfo.Result_Type == ResultType.Lost)
                    {
                        postInfo.PickInfo[0].HomeBackground = "#CE3D3D";
                        postInfo.ResultTextHome = AppResources.LostText;
                    }
                    else if (postInfo.Result_Type == ResultType.Push)
                    {
                        postInfo.PickInfo[0].AwayBackground = "#c4c4c4";
                        postInfo.ResultTextHome = AppResources.PushText;
                    }
                    else
                    {
                        postInfo.PickInfo[0].HomeBackground = "#642364";
                    }
                    postInfo.PickInfo[0].AwayBackground = "#ffffff";
                    postInfo.PickInfo[0].OpacityHome = 1.0;
                    postInfo.PickInfo[0].OpacityAway = 1.0;
                    postInfo.PickInfo[0].OpacityMoneyTextyHome = 1.0;
                    postInfo.PickInfo[0].OpacityMoneyTextAway = 0.5;
                    postInfo.PickInfo[0].HomeTextColor = "#ffffff";
                    postInfo.PickInfo[0].HomeMoneyTextColor = "#ffffff";
                    postInfo.PickInfo[0].AwayTextColor = "#000000";
                    postInfo.PickInfo[0].AwayMoneyTextColor = "#B5B5B5";
                    postInfo.PickInfo[0].SelectedTextHome = "";
                    postInfo.PickInfo[0].SelectedTextAway = "";
                    break;

            }

            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].FirstTeamColor) && postInfo.PickInfo[0].FirstTeamColor.Substring(0, 1) != "#")
            {
                postInfo.PickInfo[0].FirstTeamColor = "#" + postInfo.PickInfo[0].FirstTeamColor;
            }
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].FirstTeamSecondaryColor) && postInfo.PickInfo[0].FirstTeamSecondaryColor.Substring(0, 1) != "#")
            {
                postInfo.PickInfo[0].FirstTeamSecondaryColor = "#" + postInfo.PickInfo[0].FirstTeamSecondaryColor;
            }
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].SecondTeamColor) && postInfo.PickInfo[0].SecondTeamColor.Substring(0, 1) != "#")
            {
                postInfo.PickInfo[0].SecondTeamColor = "#" + postInfo.PickInfo[0].SecondTeamColor;
            }
            if (!string.IsNullOrEmpty(postInfo.PickInfo[0].SecondTeamSecondaryColor) && postInfo.PickInfo[0].SecondTeamSecondaryColor.Substring(0, 1) != "#")
            {
                postInfo.PickInfo[0].SecondTeamSecondaryColor = "#" + postInfo.PickInfo[0].SecondTeamSecondaryColor;
            }

            if (postInfo.PickInfo != null && postInfo.PickInfo.Count != 0 && (postInfo.PickInfo[0].PickPurchase_Type == PickPurchaseType.Free || SettingsService.Instance.LoggedUserDetails.UserId == postInfo.UserId))
            {
                postInfo.PickInfo[0].IsPickPurchase = true;
            }
            if (postInfo.IsPurchased)
                postInfo.PickInfo[0].IsPickPurchase = true;

            if (postInfo.Result_Type != ResultType.None)
            {
                postInfo.PickInfo[0].IsPickPurchase = true;
                postInfo.PickInfo[0].IsGameComplete = true;

            }

            if (postInfo.PickInfo[0].HtScore > postInfo.PickInfo[0].AtScore)
            {
                postInfo.PickInfo[0].FinalScoreText = postInfo.PickInfo[0].FirstTeamName + " Defeated " + postInfo.PickInfo[0].SecondTeamName + " " + postInfo.PickInfo[0].HtScore + "-" + postInfo.PickInfo[0].AtScore;
            }
            else if (postInfo.PickInfo[0].HtScore < postInfo.PickInfo[0].AtScore)
            {
                postInfo.PickInfo[0].FinalScoreText = postInfo.PickInfo[0].SecondTeamName + " Defeated " + postInfo.PickInfo[0].FirstTeamName + " " + postInfo.PickInfo[0].AtScore + "-" + postInfo.PickInfo[0].HtScore;
            }
            else
            {
                postInfo.PickInfo[0].FinalScoreText = postInfo.PickInfo[0].FirstTeamName + " Earns Draw with " + postInfo.PickInfo[0].SecondTeamName + " " + postInfo.PickInfo[0].HtScore + "-" + postInfo.PickInfo[0].AtScore;
            }
            postInfo.PickInfo[0].AttachmentHeight = 0;
            postInfo.PickInfo[0].IsAttachmentEnable = true;
            if (!postInfo.PickInfo[0].IsPickPurchase && postInfo.PickInfo[0].IsContentHide)
            {
                postInfo.PickInfo[0].IsAttachmentEnable = false;
            }
            else
            {
                if (postInfo.PostedAttachments != null && postInfo.PostedAttachments.Count != 0)
                {
                    postInfo.PickInfo[0].AttachmentHeight = 280;
                }

            }
        }

    }
}
