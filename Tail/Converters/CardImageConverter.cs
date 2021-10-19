using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class CardImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cardType = value.ToString();
            switch(cardType)
            {
                case "Visa":
                    return "visacard.png";
                case "Master":
                    return "mastercard.png";
                case "Wallet":
                    return "purchase_wallet.png";
                default:
                    return "purchase_wallet.png";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
