using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class RadioTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(System.Convert.ToInt32(value) == 1)
            {
                return "Selected";
            }
            else if (System.Convert.ToInt32(value) == 2)
            {
                return "Select";
            }
            else
            {
                return "Select";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
