using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class PublishedDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Unknown Time";

            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                var duration = (DateTime.Now - date).TotalDays;

                if (duration <= 1)
                {
                    return "Today";
                }
                else if (duration >= 1 && duration < 2)
                {
                    return "1 day ago";
                }
                else if (duration >= 2 && duration < 3)
                {
                    return "2 days ago";
                }
                else if (duration >= 3 && duration < 4)
                {
                    return "3 days ago";
                }
                else if (duration >= 4 && duration < 5)
                {
                    return "4 days ago";
                }
                else if (duration >= 5 && duration < 6)
                {
                    return "5 days ago";
                }
                else if (duration >= 6 && duration < 7)
                {
                    return "6 days ago";
                }
                else if (duration >= 7 && duration < 14)
                {
                    return "1 week ago";
                }
                else if (duration >= 14 && duration < 21)
                {
                    return "2 weeks ago";
                }
                else if (duration >= 21 && duration < 28)
                {
                    return "3 weeks ago";
                }
                else if (duration >= 28 && duration < 35)
                {
                    return "4 weeks ago";
                }
                else
                {
                    return "Month(s) ago";
                }
            }
            else
            {
                return "Unknown Time";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
