using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class PlaceHolderConverter : IValueConverter
    {
       
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                     return (System.Convert.ToInt32(value) == 7) ? "mma_or_boxing_placeholder.png" : "team_placeholder.png";
                 else
                    return "team_placeholder.png";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

    }
}
