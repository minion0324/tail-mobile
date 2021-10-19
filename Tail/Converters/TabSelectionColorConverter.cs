using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
   
        public class TabSelectionColorConverter : IValueConverter
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
                var selectionColor = string.Empty;
                try
                {

                    var isSelected = System.Convert.ToBoolean(value);
                    if (isSelected)
                    {
                    selectionColor = "#672967";
                        return selectionColor;
                    }
                    else
                    {
                    selectionColor = "#ffffff";
                        return selectionColor;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                return selectionColor;
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
