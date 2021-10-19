using System;
using System.Globalization;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class LogoConverterHome : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TeamDetails selectedData = (TeamDetails)value;

            if (selectedData != null)
            {
                if(selectedData.SportID == 4)
                {
                    return selectedData.TeamLogo;
                }
                else if (selectedData.SportID == 7)
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
