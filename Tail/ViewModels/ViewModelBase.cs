using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tail.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        #region HandlePropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, string propertyName = null, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
        
    }
}
