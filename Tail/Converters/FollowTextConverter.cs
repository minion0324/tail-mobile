using System;
using System.Diagnostics;
using System.Globalization;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class FollowTextConverter : IValueConverter
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
            var buttonText = string.Empty;
            try
            {

                var isFollow = System.Convert.ToBoolean(value);
                if (isFollow)
                {
                    buttonText = AppResources.Unfollow;
                    return buttonText;
                }
                else
                {
                    buttonText = AppResources.Follow;
                    return buttonText;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return buttonText;
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

