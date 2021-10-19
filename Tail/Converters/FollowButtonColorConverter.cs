using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class FollowButtonColorConverter : IValueConverter
    {
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bgColor = string.Empty;
            try
            {

                var isFollow = System.Convert.ToBoolean(value);
                if (isFollow)
                {
                    bgColor = "#ffffff";
                    return bgColor;
                }
                else
                {
                    bgColor = "#672967";
                    return bgColor;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return bgColor;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <returns>The back.</returns>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
