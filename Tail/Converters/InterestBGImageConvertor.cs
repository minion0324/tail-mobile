using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class InterestBGImageConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? "interest_selected_box.png" : "interest_box.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
