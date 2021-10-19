using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class InterestLabelColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? "#C935A9" : "#333333";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
