using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.Services.LocalStorage;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class SettingsViewModel : PageViewModelBase
    {
        bool _appNotifications=true;
        bool _likeDislikelNotifications = true;
        bool _commentNotifications = true;
        bool _picksLikeDislikeNotifications = true;
        bool _picksCommentNotifications = true;
        bool _picksPurchaseNotifications = true;
        bool _newFollowersNotifications = true;

       
        Command _appNotificationsCommand;
        Command _likeDislikelNotificationsCommand;
        Command _commentNotificationsCommand;
        Command _picksLikeDislikeNotificationsCommand;
        Command _picksCommentNotificationsCommand;
        Command _picksPurchaseNotificationsCommand;
        Command _newFollowersNotificationsCommand;
        Command _rightArrowCommand;
        Command _discoverCommand;
        Command _reportCommand;
        Command _rateUsCommand;
        Command _helpCommand;
        GetSettingsResponse _settingsObject;
        public bool AppNotifications
        {
            get => _appNotifications;
            set => SetProperty(ref _appNotifications, value);
        }
        public GetSettingsResponse SettingsObject
        {
            get => _settingsObject;
            set => SetProperty(ref _settingsObject, value);
        }
        public bool LikeDislikelNotifications
        {
            get => _likeDislikelNotifications;
            set => SetProperty(ref _likeDislikelNotifications, value);
        }
        public bool CommentNotifications
        {
            get => _commentNotifications;
            set => SetProperty(ref _commentNotifications, value);
        }
        public bool PicksLikeDislikeNotifications
        {
            get => _picksLikeDislikeNotifications;
            set => SetProperty(ref _picksLikeDislikeNotifications, value);
        }
        public bool PicksCommentNotifications
        {
            get => _picksCommentNotifications;
            set => SetProperty(ref _picksCommentNotifications, value);
        }
        public bool PicksPurchaseNotifications
        {
            get => _picksPurchaseNotifications;
            set => SetProperty(ref _picksPurchaseNotifications, value);
        }
        public bool NewFollowersNotifications
        {
            get => _newFollowersNotifications;
            set => SetProperty(ref _newFollowersNotifications, value);
        }
        public Command AppNotificationsCommand => _appNotificationsCommand ?? (_appNotificationsCommand = new Command( () =>  Handle_AppNotificationsCommand()));
        public Command LikeDislikelNotificationsCommand => _likeDislikelNotificationsCommand ?? (_likeDislikelNotificationsCommand = new Command( () =>  Handle_LikeDislikelNotificationsCommand()));
        public Command CommentNotificationsCommand => _commentNotificationsCommand ?? (_commentNotificationsCommand = new Command( () =>  Handle_CommentNotificationsCommand()));
        public Command PicksLikeDislikeNotificationsCommand => _picksLikeDislikeNotificationsCommand ?? (_picksLikeDislikeNotificationsCommand   = new Command( () =>  Handle_PicksLikeDislikeNotificationsCommand()));
        public Command PicksCommentNotificationsCommand => _picksCommentNotificationsCommand ?? (_picksCommentNotificationsCommand = new Command( () =>  Handle_PicksCommentNotificationsCommand()));
        public Command PicksPurchaseNotificationsCommand => _picksPurchaseNotificationsCommand ?? (_picksPurchaseNotificationsCommand = new Command( () =>  Handle_PicksPurchaseNotificationsCommand()));
        public Command NewFollowersNotificationsCommand => _newFollowersNotificationsCommand ?? (_newFollowersNotificationsCommand = new Command( () =>  Handle_NewFollowersNotificationsCommand()));
        public Command RightArrowCommand => _rightArrowCommand ?? (_rightArrowCommand = new Command(async () => await Handle_RightArrowCommand()));
        public Command DiscoverCommand => _discoverCommand ?? (_discoverCommand = new Command(async () => await Handle_DiscoverCommand()));
        public Command ReportCommand => _reportCommand ?? (_reportCommand = new Command(async () => await Handle_ReportCommand()));
        public Command RateUsCommand => _rateUsCommand ?? (_rateUsCommand = new Command( () =>  Handle_RateUsCommand()));
        public Command HelpCommand => _helpCommand ?? (_helpCommand = new Command(async () => await Handle_HelpCommand()));


        public SettingsViewModel()
        {

           var response= DataStorageService.Instance.GetSettings();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
                AppNotifications = response.ResponseData.IsAppNotify; 
                LikeDislikelNotifications = response.ResponseData.IsPostLikeDislike; 
                CommentNotifications = response.ResponseData.IsPostComment; 
                PicksLikeDislikeNotifications = response.ResponseData.IsPickLikeDislike; 
                PicksCommentNotifications = response.ResponseData.IsPickComment; 
                PicksPurchaseNotifications = response.ResponseData.IsPickPurchase;
                NewFollowersNotifications = response.ResponseData.IsFollowerNew; 
                SettingsObject = response.ResponseData;
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                NotificationCount = SettingsService.Instance.NotificationCount;
            });
        }
        public override void OnPageAppearing()
        {
            base.OnPageAppearing();


            Task.Run(async () => await GetSettingsInfo());
            
        }
        public override void OnPageDisappearing()
        {
           


            Task.Run(async () => {
                await UpdateSettingsInfo();
                base.OnPageDisappearing();
            });

        }
        public async Task GetSettingsInfo()
        {
            var response = await TailDataServiceProvider.Instance.GetSettingsDetails();
            if (response.ErrorCode == 200 && response.ResponseData != null)
            {
              
                AppNotifications = response.ResponseData.IsAppNotify; 
                LikeDislikelNotifications = response.ResponseData.IsPostLikeDislike; 
                CommentNotifications = response.ResponseData.IsPostComment; 
                PicksLikeDislikeNotifications = response.ResponseData.IsPickLikeDislike; 
                PicksCommentNotifications = response.ResponseData.IsPickComment; 
                PicksPurchaseNotifications = response.ResponseData.IsPickPurchase; 
                NewFollowersNotifications = response.ResponseData.IsFollowerNew; 
                SettingsObject = response.ResponseData;
                DataStorageService.Instance.SaveSettings(response.ResponseData);
            }
            else
                await ShowAlert(AppResources.AppName, response.Message);
        }
        public async Task UpdateSettingsInfo()
        {
            SettingsInfo requestObj = new SettingsInfo()
            {
                IsAppNotify = AppNotifications,
                IsFollowerNew= NewFollowersNotifications,
                IsPickComment= PicksCommentNotifications,
                IsPickLikeDislike = PicksLikeDislikeNotifications,
                IsPickPurchase= PicksPurchaseNotifications,
                IsPostComment= CommentNotifications,
                IsPostLikeDislike = LikeDislikelNotifications,
                

            };
            if (SettingsObject != null)
            {
                SettingsObject.IsAppNotify = AppNotifications;
                SettingsObject.IsFollowerNew = NewFollowersNotifications;
                SettingsObject.IsPickComment = PicksCommentNotifications;
                SettingsObject.IsPickLikeDislike = PicksLikeDislikeNotifications;
                SettingsObject.IsPickPurchase = PicksPurchaseNotifications;
                SettingsObject.IsPostComment = CommentNotifications;
                SettingsObject.IsPostLikeDislike = LikeDislikelNotifications;
                DataStorageService.Instance.SaveSettings(SettingsObject);
            }

            
            var updatResponse = await TailDataServiceProvider.Instance.SaveSettings(requestObj);
            if (updatResponse.ErrorCode != 200)
            {

                await ShowAlert(AppResources.AppName, updatResponse.Message);
            }
        }
        void Handle_AppNotificationsCommand()
        {
            AppNotifications = !AppNotifications;
            if (AppNotifications)
            {
                LikeDislikelNotifications = true;
                CommentNotifications = true;
                PicksLikeDislikeNotifications = true;
                PicksCommentNotifications = true;
                PicksPurchaseNotifications = true;
                NewFollowersNotifications = true;
            }
            else
            {
                LikeDislikelNotifications = false;
                CommentNotifications = false;
                PicksLikeDislikeNotifications = false;
                PicksCommentNotifications = false;
                PicksPurchaseNotifications = false;
                NewFollowersNotifications = false;
            }
        }
        private async Task Handle_RightArrowCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<FrequentlyAskedQuestions>();
            IsBusy = false;
        }
        private void Handle_RateUsCommand()
        {
             DependencyService.Get<IAppRating>().RateApp();
        }
        void Handle_LikeDislikelNotificationsCommand()
        {
            LikeDislikelNotifications = !LikeDislikelNotifications;
            DisableAppNotification();
        }
        void Handle_CommentNotificationsCommand()
        {
            CommentNotifications = !CommentNotifications;
            DisableAppNotification();
        }
        void Handle_PicksLikeDislikeNotificationsCommand()
        {
            PicksLikeDislikeNotifications = !PicksLikeDislikeNotifications;
            DisableAppNotification();
        }
        void Handle_PicksCommentNotificationsCommand()
        {
            PicksCommentNotifications = !PicksCommentNotifications;
            DisableAppNotification();
        }
        void Handle_PicksPurchaseNotificationsCommand()
        {
            PicksPurchaseNotifications = !PicksPurchaseNotifications;
            DisableAppNotification();
        }
     
        void Handle_NewFollowersNotificationsCommand()
        {
            NewFollowersNotifications = !NewFollowersNotifications;
            DisableAppNotification();
        }
        void DisableAppNotification()
        {
            if (!LikeDislikelNotifications && !CommentNotifications && !PicksLikeDislikeNotifications && !PicksCommentNotifications && !PicksPurchaseNotifications && !NewFollowersNotifications)
                AppNotifications = false;
            else
                AppNotifications = true;

        }
      
        private async Task Handle_DiscoverCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            SettingsService.Instance.FromSettings = true;
             await NavigationService.NavigateWithInTabToAsync<FollowRecommended>();
            IsBusy = false;
        }
        private async Task Handle_ReportCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<ReportAProblem>();
            IsBusy = false;
        }
        private async Task Handle_HelpCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            await NavigationService.NavigateWithInTabToAsync<HelpAndFAQ>();
            IsBusy = false;
        }
    }
}
 