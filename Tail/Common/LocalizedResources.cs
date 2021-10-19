using System;
using System.Resources;
using System.Globalization;
using System.ComponentModel;

using Tail.Services.ApplicationServices;

using Xamarin.Forms;

namespace Tail.Common
{
    public class LocalizedResources : INotifyPropertyChanged
    {       
        readonly ResourceManager _resourceManager;

        CultureInfo _currentCultureInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key] => _resourceManager.GetString(key, _currentCultureInfo);

        public LocalizedResources(Type resource)
        {
            _currentCultureInfo = new CultureInfo(SettingsService.Instance.AppLanguage);
            _resourceManager = new ResourceManager(resource);

            MessagingCenter.Subscribe<object>(this, Constants.LANGUAGE_CHANGE_MESSAGE_CONTRACT, OnLanguageChanged);
        }

        public string GetText(string key, CultureInfo cultureInfo)
        {
            return _resourceManager.GetString(key, cultureInfo);
        }

        private void OnLanguageChanged(object sender)
        {
            _currentCultureInfo = new CultureInfo(SettingsService.Instance.AppLanguage);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }        
    }
}
