using System;
using System.Diagnostics;
using Xamarin.Forms;
using SQLite;
using Tail.Services.Interfaces;
using Tail.Models;
using Tail.Services.Responses;
using System.Linq;
using System.Collections.Generic;

namespace Tail.Services.LocalStorage.Helpers
{
    public class TailDataStore : IAssetDataService
    {
        static readonly SQLiteConnection Database;
        static readonly object Locker = new object();

        static TailDataStore()
        {
            try
            {
                Database = DependencyService.Get<ISqLite>().GetConnection();
                Database.CreateTable<LoggedInUser>();
                Database.CreateTable<NotificationInfo>();
                Database.CreateTable<GetSettingsResponse>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in AssetAreaDataStore=" + ex.Message);
            }
        }
        public bool SaveLoginDetails(LoggedInUser loginResponse)
        {
            bool hasSaved = false;

            try
            {
                lock (Locker)
                {
                    int recordsInsertedOrUpdated = 0;

                    LoggedInUser existingRecord = Database.Table<LoggedInUser>().FirstOrDefault();
                    recordsInsertedOrUpdated = existingRecord != null ? Database.Update(loginResponse) : Database.Insert(loginResponse);

                    hasSaved = recordsInsertedOrUpdated == 1;
                }
            }
            catch (Exception exe)
            {
                Debug.WriteLine("Error while saving Login Details : " + exe.InnerException);
            }

            return hasSaved;
        }

        public ServiceResponse<LoggedInUser> GetLoggedInUser()
        {
            var response = new ServiceResponse<LoggedInUser>();


            try
            {
                lock (Locker)
                {
                    var data = Database.Table<LoggedInUser>().FirstOrDefault();

                    response.ResponseData = data;
                    response.ErrorCode = 200;
                }
            }
            catch (Exception e)
            {
                response.ErrorCode = 50001;
                response.Message = e.Message;
            }
            return response;
           
        }
        public bool SaveNotification(NotificationInfo notificationObj)
        {
            bool hasSaved = false;

            try
            {
                lock (Locker)
                {
                    int recordsInsertedOrUpdated = 0;

                    NotificationInfo dp = Database.Table<NotificationInfo>().Where(x => x.Id == notificationObj.Id).SingleOrDefault();
                    recordsInsertedOrUpdated = dp != null ? Database.Update(notificationObj) : Database.Insert(notificationObj);                

                    hasSaved = recordsInsertedOrUpdated == 1;
                }
            }
            catch (Exception exe)
            {
                Debug.WriteLine("Error while saving Login Details : " + exe.InnerException);
            }

            return hasSaved;
        }
        public ServiceResponse<List<NotificationInfo>> GetNotification()
        {
            var response = new ServiceResponse<List<NotificationInfo>>();


            try
            {
                lock (Locker)
                {
                    var data = Database.Table<NotificationInfo>().ToList();
                    response.ResponseData = data.ToList();
                    response.ErrorCode = 200;
                }
            }
            catch (Exception e)
            {
                response.ErrorCode = 50001;
                response.Message = e.Message;
            }
            return response;

        }
        public bool UpdateNotificationRead(string notificationId)
        {
            bool hasSaved = false;

            try
            {
                lock (Locker)
                {

                    String query = "Update NotificationInfo set IsRead=1 Where Id='" + notificationId + "'";
                    Database.Query<NotificationInfo>(query);

                    hasSaved = true;
                }
            }
            catch (Exception exe)
            {
                Debug.WriteLine("Error while saving Login Details : " + exe.InnerException);
            }

            return hasSaved;
        }
        public void ClearAllNotifications()
        {
            try
            {
                Database.Query<NotificationInfo>("DELETE FROM NotificationInfo");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Deleting all Notification : " + ex.InnerException);
            }
        }

        public bool SaveSettings(GetSettingsResponse settingsResponse)
        {
            bool hasSaved = false;

            try
            {
                lock (Locker)
                {
                    int recordsInsertedOrUpdated = 0;

                    GetSettingsResponse existingRecord = Database.Table<GetSettingsResponse>().FirstOrDefault();
                    recordsInsertedOrUpdated = existingRecord != null ? Database.Update(settingsResponse) : Database.Insert(settingsResponse);

                    hasSaved = recordsInsertedOrUpdated == 1;
                }
            }
            catch (Exception exe)
            {
                Debug.WriteLine("Error while saving Login Details : " + exe.InnerException);
            }

            return hasSaved;
        }

        public ServiceResponse<GetSettingsResponse> GetSettings()
        {
            var response = new ServiceResponse<GetSettingsResponse>();


            try
            {
                lock (Locker)
                {
                    var data = Database.Table<GetSettingsResponse>().FirstOrDefault();

                    response.ResponseData = data;
                    response.ErrorCode = 200;
                }
            }
            catch (Exception e)
            {
                response.ErrorCode = 50001;
                response.Message = e.Message;
            }
            return response;

        }


    }
}

