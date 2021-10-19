using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.LocalStorage;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class NotificationViewModel : PageViewModelBase
    {
        #region private members
        ObservableCollection<NotificationModel> _notificationList;
        Command _clearCommand;
        bool _infoVisible;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        Command _loadMoreCommand;
        private bool _isLoadMoreVisible;
        private bool _noDataVisible;
        #endregion
        #region public members
        public bool IsLoadMoreVisible
        {
            get => _isLoadMoreVisible;
            set => SetProperty(ref _isLoadMoreVisible, value);
        }
        public bool NoDataVisible
        {
            get => _noDataVisible;
            set => SetProperty(ref _noDataVisible, value);
        }
        public ObservableCollection<NotificationModel> NotificationList
        {
            get => _notificationList;
            set => SetProperty(ref _notificationList, value);
        }
        public bool InfoVisible
        {
            get => _infoVisible;
            set => SetProperty(ref _infoVisible, value);
        }
        public Command ClearCommand => _clearCommand ?? (_clearCommand = new Command(async () => await Handle_ClearCommand()));
        public Command LoadMoreCommand => _loadMoreCommand ?? (_loadMoreCommand = new Command(async () => await Handle_LoadMoreCommand()));

        #endregion
        public NotificationViewModel()
        {
            NotificationList = new ObservableCollection<NotificationModel>();
            InfoVisible = false;
            CommonSingletonUtility.SharedInstance.IsFromEditScreen = true;
            Task.Run(async () => await GetNotificationCache());
        }
        public override async Task InitializeAsync(object parameter = null)
        {
            await GetNotification(0);

        }

        private async Task GetNotification(int pagenumber)
        {
            try
            {
                if (IsBusy)
                    return;
                if (NotificationList.Count == 0)
                    IsBusy = true;
                var notificationResponse = await TailDataServiceProvider.Instance.GetNotifications(pagenumber);
                if (notificationResponse.ErrorCode == 200 && notificationResponse.ResponseData != null && notificationResponse.ResponseData.NotificationData != null)
                {

                    foreach (NotificationInfo notification in notificationResponse.ResponseData.NotificationData)
                    {
                        NotificationModel notificationInfo = new NotificationModel();
                        notificationInfo.IsNotificationRead = true;
                        notificationInfo.NotificationContent = notification.NotifyTxt;
                        notificationInfo.UserId = notification.UserId;
                        notificationInfo.NotificationDateTime = notification.AddOn;

                        if (!string.IsNullOrEmpty(notification.UserImg) && notification.UserImg != "string")
                        {

                            notificationInfo.UserImage = TailUtils.GetThumbProfileImage(notification.UserImg);
                        }
                        else
                        {
                            notificationInfo.UserImage = Constants.DEFAULT_USERIMAGE;
                        }
                        notificationInfo.NotificationSelectCommand = new Command<NotificationModel>(async (item) => await Handle_NotificationSelection(item));
                        notificationInfo.UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID));
                        notificationInfo.PostItem = new PostDetails { PostId = notification.PostId, PType = notification.PostType };
                        notificationInfo.Id = notification.Id;
                        NotificationList.Add(notificationInfo);

                        var _existingItem = NotificationList.FirstOrDefault(p => p.Id == notification.Id);
                        if (_existingItem == null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                NotificationList.Add(notificationInfo);
                                if (pagenumber == 0)
                                    DataStorageService.Instance.SaveNotification(notification);
                            });

                        }


                    }
                    if (notificationResponse.ResponseData.PageInfo != null && notificationResponse.ResponseData.PageInfo.Count != 0)
                    {
                        TotalPages = notificationResponse.ResponseData.PageInfo[0].totalPages;
                        CurrentPage = notificationResponse.ResponseData.PageInfo[0].currentPage;
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (NotificationList.Count > 0)
                            NoDataVisible = false;
                        else
                            NoDataVisible = true;
                        if (TotalPages == CurrentPage)
                             IsLoadMoreVisible = false;
                        else
                            IsLoadMoreVisible = true;
                    });
                    if (pagenumber == 0)
                    {
                        SettingsService.Instance.NotificationCount = 0;
                        await TailDataServiceProvider.Instance.ReadAllNotification();
                    }
                }
                IsBusy = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        async Task Handle_NotificationSelection(NotificationModel notificationItem)
        {
            if (notificationItem.PostItem != null && notificationItem.PostItem.PType > 0)
            {
                PostEventsArgs paramObj = new PostEventsArgs()
                {
                    serviceRequired = true,
                    postObj = notificationItem.PostItem
                };
                await NavigationService.NavigateWithInTabToAsync<Home_Comments>(paramObj);
            }
            else
            {
                await Handle_UserDetailsCommand(notificationItem.UserId);
            }


        }
        async Task Handle_ClearCommand()
        {

            bool confirmation = await NavigationService.ShowConfirmAlertAsync(AppResources.AppName, AppResources.ConfirmClear);
            if (confirmation)
            {
                var clearAllResponse = await TailDataServiceProvider.Instance.ClearAllNotifications();
                if (clearAllResponse.ErrorCode == 200)
                {
                    DataStorageService.Instance.ClearAllNotifications();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        NotificationList = new ObservableCollection<NotificationModel>();
                        NoDataVisible = true;
                        IsLoadMoreVisible = false;

                        SettingsService.Instance.NotificationCount = 0;


                    });
                }
                else
                {
                    await ShowAlert(AppResources.AppName, clearAllResponse.Message);
                }
            }


        }
        private async Task Handle_LoadMoreCommand()
        {

            if (CurrentPage != TotalPages)
            {
                await GetNotification(CurrentPage + 1);

            }
            if (TotalPages == CurrentPage)
                IsLoadMoreVisible = false;
            else
                IsLoadMoreVisible = true;

        }

        async Task<bool> GetNotificationCache()
        {
            bool hasSuccessResponse = false;

            try
            {

                var notificationInfoResponse = DataStorageService.Instance.GetNotification();

                if (notificationInfoResponse.ErrorCode == 200 && notificationInfoResponse.ResponseData != null)
                {
                    foreach (NotificationInfo notification in notificationInfoResponse.ResponseData)
                    {
                        NotificationModel notificationInfo = new NotificationModel();
                        notificationInfo.IsNotificationRead = notification.IsRead;
                        notificationInfo.NotificationContent = notification.NotifyTxt;
                        notificationInfo.UserId = notification.UserId;
                        notificationInfo.NotificationDateTime = notification.AddOn;

                        if (!string.IsNullOrEmpty(notification.UserImg) && notification.UserImg != "string")
                        {
                            notificationInfo.UserImage = TailUtils.GetThumbProfileImage(notification.UserImg);
                        }
                        else
                        {
                            notificationInfo.UserImage = Constants.DEFAULT_USERIMAGE;
                        }
                        notificationInfo.NotificationSelectCommand = new Command<NotificationModel>(async (item) => await Handle_NotificationSelection(item));
                        notificationInfo.UserDetails = new Command<int>(async (userID) => await Handle_UserDetailsCommand(userID));
                        notificationInfo.PostItem = new PostDetails { PostId = notification.PostId };
                        notificationInfo.Id = notification.Id;

                        var _existingItem = NotificationList.FirstOrDefault(p => p.Id == notification.Id);
                        if (_existingItem == null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                NotificationList.Add(notificationInfo);

                            });

                        }


                    }



                }
                else
                {

                    await ShowAlert(AppResources.AppName, notificationInfoResponse.Message);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (NotificationList.Count > 0)
                        NoDataVisible = false;
                    else
                        NoDataVisible = true;

                });
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }


    }
}
