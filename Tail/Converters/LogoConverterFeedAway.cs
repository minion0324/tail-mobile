using System;
using System.Globalization;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class LogoConverterFeedAway : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PickInfoDetails selectedData = (PickInfoDetails)value;

            if (selectedData != null)
            {
                if (selectedData.SportId == 4)
                {
                    return selectedData.SecondTeamImage;
                }
                else if (selectedData.SportId == 7)
                {
                    return "mma_or_boxing_placeholder.png";
                }
                else
                {
                    return "team_placeholder.png";
                }
            }
            else
            {
                return "team_placeholder.png";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
