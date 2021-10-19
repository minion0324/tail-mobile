﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class RadioImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) == 1)
            {
                return "radio_selected.png";
            }
            else if (System.Convert.ToInt32(value) == 2)
            {
                return "radio.png";
            }
            else
            {
                return "radio.png";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
