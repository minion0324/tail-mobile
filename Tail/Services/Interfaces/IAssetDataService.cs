using System.Collections.Generic;
using Tail.Models;
using Tail.Services.Responses;

namespace Tail.Services.Interfaces
{
    public interface IAssetDataService
    {
        bool SaveLoginDetails(LoggedInUser loginResponse);
        ServiceResponse<LoggedInUser> GetLoggedInUser();
        bool SaveNotification(NotificationInfo notificationObj);
        ServiceResponse<List<NotificationInfo>> GetNotification();
        bool UpdateNotificationRead(string notificationId);
        void ClearAllNotifications();
        bool SaveSettings(GetSettingsResponse settingsResponse);
        ServiceResponse<GetSettingsResponse> GetSettings();
    }
}