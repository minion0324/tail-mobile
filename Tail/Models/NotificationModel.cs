using System;
using Tail.Services.Helper;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Models
{
    public class NotificationModel : ViewModelBase
    {
        string _notifyId;
        public string Id
        {
            get => _notifyId;
            set => SetProperty(ref _notifyId, value);
        }
        PostDetails _postItem;
        public PostDetails PostItem
        {
            get => _postItem;
            set => SetProperty(ref _postItem, value);
        }
        int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        string _userImage;
        public string UserImage
        {
            get => _userImage;
            set => SetProperty(ref _userImage, value);
        }
        string _notificationContent;
        public string NotificationContent
        {
            get => _notificationContent;
            set => SetProperty(ref _notificationContent, value);
        }
        bool _isNotificationRead;
        public bool IsNotificationRead
        {
            get => _isNotificationRead;
            set => SetProperty(ref _isNotificationRead, value);
        }
        
        public string NotificationColor
        {
            get
            {
                if (IsNotificationRead)
                {
                    return "#FFFFFF";
                }
                else
                {
                    return "#F2DEFF";
                }

            }
        }
        string _notificationDateTime;
        public string NotificationDateTime
        {
            get => _notificationDateTime;
            set => SetProperty(ref _notificationDateTime, value);
        }

        public string NotificationTime
        {
            get
            {
                if (!string.IsNullOrEmpty(NotificationDateTime))
                {
                    DateTime _postDateTime = Convert.ToDateTime(NotificationDateTime);
                    return TailUtils.FindDisplayTime(_postDateTime);        
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        Command<NotificationModel> _notificationSelectCommand;
        public Command<NotificationModel> NotificationSelectCommand
        {
            get => _notificationSelectCommand;
            set => SetProperty(ref _notificationSelectCommand, value);
        }

        Command<int> _userDetails;
        public Command<int> UserDetails
        {
            get => _userDetails;
            set => SetProperty(ref _userDetails, value);
        }


    }
}
