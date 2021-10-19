using System;
using System.Globalization;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class PaymentSelectionTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? AppResources.ActiveText : AppResources.ActivateNowText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
