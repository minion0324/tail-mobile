using Tail.Services.Interfaces;
using Tail.Services.LocalStorage.Helpers;
using Tail.Models;
using Tail.Services.Responses;
using System.Collections.Generic;

namespace Tail.Services.LocalStorage
{
    public class DataStorageService : IAssetDataService
    {
        readonly IAssetDataService _tailDatabaseService;

        static DataStorageService _instance;

        public static DataStorageService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStorageService();
                }
                return _instance;
            }
        }

        DataStorageService()
        {
            _tailDatabaseService = new TailDataStore();        
        }

        public bool SaveLoginDetails(LoggedInUser loginResponse)
        {
            return _tailDatabaseService.SaveLoginDetails(loginResponse);
        }
        public ServiceResponse<LoggedInUser> GetLoggedInUser()
        {
            return _tailDatabaseService.GetLoggedInUser();
        }
        public bool SaveNotification(NotificationInfo notificationObj)
        {
            return _tailDatabaseService.SaveNotification(notificationObj);
        }
        public ServiceResponse<List<NotificationInfo>> GetNotification()
        {
            return _tailDatabaseService.GetNotification();
        }
        public bool UpdateNotificationRead(string notificationId)
        {
            return _tailDatabaseService.UpdateNotificationRead(notificationId);
        }
        public void ClearAllNotifications()
        {
            _tailDatabaseService.ClearAllNotifications();
        }
        public bool SaveSettings(GetSettingsResponse settingsResponse)
        {
            return _tailDatabaseService.SaveSettings(settingsResponse);
        }
        public ServiceResponse<GetSettingsResponse> GetSettings()
        {
            return _tailDatabaseService.GetSettings();
        }

    }
}
