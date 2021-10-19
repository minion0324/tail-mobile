using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class NotificationBadgeConverter : IValueConverter
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

			if (value == null || value.ToString() == "0")
				return false;
			else
				return true;
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
