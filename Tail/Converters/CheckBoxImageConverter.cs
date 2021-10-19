using System;
using System.Globalization;

using Xamarin.Forms;
namespace Tail.Converters
{
    public class CheckBoxImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? "checkbox_selected.png" : "checkbox.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

  