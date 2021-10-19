using System;
using Tail.Common;
using Xamarin.Forms;
using System.Security.Cryptography;
using System.IO;
using Tail.Services.ApplicationServices;
using System.Diagnostics;
using System.Text;

namespace Tail.Services.Helper
{
    public static class TailUtils
    {
        public static string GameTypeToName(SportType Sport_Type)
        {
            try
            {
                switch (Sport_Type)
                {
                    case SportType.Baseball:
                        return AppResources.Baseball;
                    case SportType.Basketball:
                        return AppResources.Basketball;
                    case SportType.Football:
                        return AppResources.Football;
                    case SportType.Hocky:
                        return AppResources.Hocky;
                    case SportType.MMA:
                        return AppResources.MMA;
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static string GameTypeToImage_Home(SportType Sport_Type)
        {
            try
            {
                switch (Sport_Type)
                {
                    case SportType.Baseball:
                        return "baseball_home";
                    case SportType.Basketball:
                        return "basketball_home";
                    case SportType.Football:
                        return "football_home";
                    case SportType.Hocky:
                        return "ice_hockey_home";
                    case SportType.MMA:
                        return "mma_home";
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static string GameTypeToImage_Follow(SportType Sport_Type)
        {
            try
            {
                switch (Sport_Type)
                {
                    case SportType.Baseball:
                        return "baseball_follow";
                    case SportType.Basketball:
                        return "basketball_follow";
                    case SportType.Football:
                        return "football_follow";
                    case SportType.Hocky:
                        return "ice_hockey_follow";
                    case SportType.MMA:
                        return "mma_follow";
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static string GameTypeToImage_Interests(SportType Sport_Type)
        {
            try
            {
                switch (Sport_Type)
                {
                    case SportType.Baseball:
                        return "baseball";
                    case SportType.Basketball:
                        return "basketball";
                    case SportType.Football:
                        return "football";
                    case SportType.Hocky:
                        return "ice_hockey";
                    case SportType.MMA:
                        return "mma";
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static string PurchaseTypeToName(PickPurchaseType _type)
        {
            try
            {
                switch (_type)
                {
                    case PickPurchaseType.Free:
                        return AppResources.FreeText;
                    case PickPurchaseType.Paid:
                        return AppResources.PaidText;
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public static void AlertNoInternetConnection()
        {
            Application.Current.MainPage.DisplayAlert(AppResources.AppName, AppResources.NoConnectionMessage, AppResources.OKText);
        }
        public static string ConvertToDateTimeFormate(DateTime inputDate)
        {
            string outputDate = inputDate.ToString();
            try
            {
                outputDate = inputDate.Month.ToString("00") + "/" + inputDate.Day.ToString("00") + "/" + inputDate.Year + " " + inputDate.ToString("HH:mm:ss");

                return outputDate;
            }
            catch
            {
                return outputDate;
            }


        }
        public static string ConvertToDateFormate(DateTime inputDate)
        {
            string outputDate = inputDate.ToString();
            try
            {
                outputDate = inputDate.Month.ToString("00") + "/" + inputDate.Day.ToString("00") + "/" + inputDate.Year;

                return outputDate;
            }
            catch
            {
                return outputDate;
            }


        }
        private static byte[] CreateKey(string password, int keyBytes = 32)
        {
            byte[] salt = new byte[] {80, 70, 60, 50, 40, 30, 20, 10 };
    
            int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyGenerator.GetBytes(keyBytes);
        }
        public static string Encrypt(string clearValue, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = CreateKey(encryptionKey);

                byte[] encrypted = AesEncryptStringToBytes(clearValue, aes.Key, aes.IV);
                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
            }
        }
        private static byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException($"{nameof(plainText)}");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException($"{nameof(key)}");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException($"{nameof(iv)}");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
            return encrypted;
        }

        public static string Decrypt(string encryptedValue, string encryptionKey)
        {
     
            string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
            encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

            return AesDecryptStringFromBytes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey), Convert.FromBase64String(iv));
        }
        private static string AesDecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException($"{nameof(cipherText)}");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException($"{nameof(key)}");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException($"{nameof(iv)}");

            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                    plaintext = streamReader.ReadToEnd();

            }
            return plaintext;
        }

        public static string GetThumbProfileImage(string _imageName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_imageName))
                {
                    var imageArray=_imageName.Split('.');
                    if (imageArray.Length == 2)
                    {
                        return Constants.ProfileImageUrl + imageArray[0] + "_thumb."+ imageArray[1];
                    }
                    else
                    {
                        return Constants.ProfileImageUrl + _imageName;
                    }
                    
                }
                else
                {
                    return "";
                }
               
            }
            catch
            {
                return "";
            }
        }
        public static string GetOrginalProfileImage(string _imageName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_imageName))
                {
                    return Constants.ProfileImageUrl + _imageName;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }

        public static string GetThumbPostImage(string _imageName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_imageName))
                {
                    var imageArray = _imageName.Split('.');
                    if (imageArray.Length == 2)
                    {
                        return Constants.PostImageUrl + imageArray[0] + "_thumb."+ imageArray[1];
                    }
                    else
                    {
                        return Constants.PostImageUrl + _imageName;
                    }

                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }
        public static string GetOrginalPostImage(string _imageName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_imageName))
                {
                    return Constants.PostImageUrl + _imageName;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }
        public static string GetThumbPostVideo(string _videoName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_videoName))
                {
                    var videoArray = _videoName.Split('.');
                    if (videoArray.Length == 2)
                    {
                        return Constants.PostVideoUrl + videoArray[0] + "_thumb.jpg";
                    }
                    else
                    {
                        return Constants.PostVideoUrl + _videoName;
                    }

                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }
        public static string GetOrginalPostVideo(string _videoName)
        {
            try
            {
                if (!string.IsNullOrEmpty(_videoName))
                {
                    return Constants.PostVideoUrl + _videoName;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
        }
        public static void SetpushToken(string pushtoken)
        {
            Debug.WriteLine("Actual pushtoken=" + pushtoken);
            CommonSingletonUtility.SharedInstance.DeviceToken = pushtoken;
          
        }
       public static string FindDisplayTime(DateTime dt)
        {

            try
            {
               
                TimeSpan _timeSpan = DateTime.Now.Subtract(dt);
                if (_timeSpan.TotalMinutes < 1)
                {
                    string _secondValue = (Convert.ToInt32(_timeSpan.TotalSeconds) > 1) ? Convert.ToInt32(_timeSpan.TotalSeconds) + " seconds ago" : Convert.ToInt32(_timeSpan.TotalSeconds) + " second ago";
                    return (Convert.ToInt32(_timeSpan.TotalSeconds) <= 0) ? "1 second ago" : _secondValue;
                }
                else if (_timeSpan.TotalHours < 1)
                {
                    return (Convert.ToInt32(_timeSpan.TotalMinutes) > 1) ? Convert.ToInt32(_timeSpan.TotalMinutes) + " minutes ago" : Convert.ToInt32(_timeSpan.TotalMinutes) + " minute ago";
                }
                else if (_timeSpan.TotalHours < 24)
                {
                    return (Convert.ToInt32(_timeSpan.TotalHours) > 1) ? Convert.ToInt32(_timeSpan.TotalHours) + " hours ago" : Convert.ToInt32(_timeSpan.TotalHours) + " hour ago";
                }
                else if (_timeSpan.TotalDays < 7)
                {
                    return (Convert.ToInt32(Math.Floor(_timeSpan.TotalHours / 24)) > 1) ? Math.Floor(_timeSpan.TotalHours / 24) + " days ago" : Math.Floor(_timeSpan.TotalHours / 24) + " day ago";
                }
                else if (_timeSpan.TotalDays < 31)
                {
                    return (Convert.ToInt32(Math.Floor(_timeSpan.TotalDays / 7)) > 1) ? Math.Floor(_timeSpan.TotalDays / 7) + " weeks ago" : Math.Floor(_timeSpan.TotalDays / 7) + " week ago";
                }
                else if (_timeSpan.TotalDays < 265)
                {
                    return Math.Floor(_timeSpan.TotalDays / 30) + " months ago";
                }
                else
                {
                    return Math.Floor(_timeSpan.TotalDays / 265) + " years ago";
                }

            }
            catch (Exception exe)
            {
                Debug.WriteLine("Error in FindDisplayTime=" + exe.Message);
                return string.Empty;
            }
        }

        public static string EncryptString(string stringvalue)
        {
            Byte[] stringBytes = System.Text.Encoding.UTF8.GetBytes(stringvalue);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length);
            foreach (byte b in stringBytes)
            {
                sbBytes.Append(b);
            }
            return sbBytes.ToString();
        }

    }
}
