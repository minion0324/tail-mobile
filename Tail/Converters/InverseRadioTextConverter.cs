using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class InverseRadioTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) == 1)
            {
                return "Select";
            }
            else if (System.Convert.ToInt32(value) == 2)
            {
                return "Selected";
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
