using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tail.Models;
using Tail.Services.Interfaces;

namespace Tail.Services.ApplicationServices
{
    public class CommonSingletonUtility
    {
        private static volatile CommonSingletonUtility _instance;
        IMultiMediaPickerService _multiMediaPicker;
        public CommonSingletonUtility()
        {
        }

        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        /// <value>The shared instance.</value>
        public static CommonSingletonUtility SharedInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommonSingletonUtility();

                }
                return _instance;
            }
        }
       
        public string CroppedImageName
        {
            get;
            set;
        }
        public string OtpPhoneNumber
        {
            get;
            set;
        }
        public string OtpCountryCode
        {
            get;
            set;
        }
        public float DeviceWidth
        {
            get;
            set;
        }
        public string DeviceToken
        {
            get;
            set;
        }
        public float DeviceHeight
        {
            get;
            set;
        }
        public IMultiMediaPickerService MultiMediaPicker
        {
            get => _multiMediaPicker;
            set => SetProperty(ref _multiMediaPicker, value);
        }
        public bool IsFromMenu
        {
            get;
            set;
        }
        public CardDetailsModel SelectedCardDetails
        {
            get;
            set;
        }
        public PostDetails SelectedPostDetails
        {
            get;
            set;
        }
        public int CoinBalance
        {
            get;
            set;
        }
        public bool IsFromPaymentMethods
        {
            get;
            set;
        }
        public bool IsFromEditScreen
        {
            get;
            set;
        }
        public bool IsHandlerAdded
        {
            get;
            set;
        }
        public bool IsNewPostAdded
        {
            get;
            set;
        }
        public string ShareImagePath
        {
            get;
            set;
        }
        public int NotificationCount
        {
            get;
            set;
        }
        public bool AppForegroundStatus
        {
            get;
            set;
        }
        public int MaxPrice
        {
            get;
            set;
        }
        public int MinPrice
        {
            get;
            set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool SetProperty<T>(
        ref T backingStore, T value,
        Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
