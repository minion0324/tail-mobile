using System;
using System.Diagnostics;
using System.Globalization;
using Tail.Common;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class NoImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "no_Image.png";

            var imageUri = value.ToString();
            if (imageUri.Contains("http"))
            {
                return new Uri(imageUri);
            }
            else
            {
                return "no_Image.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
