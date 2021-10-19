using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class MenuLabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? "#FFFFFF" : "#672967";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
