using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class FrameBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? "#CE60B6" : "#F0F0F0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
