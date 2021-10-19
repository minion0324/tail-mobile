using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class MenuImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nameLabel = parameter as Label;
            var nameParam = nameLabel.Text;
            if ((bool)value)
            {
                
                switch(nameParam)
                {
                    case "Home":
                        return "home_menu_selected";
                    case "Interests":
                        return "interest_menu_selected";
                    case "Settings":
                        return "settings_menu_selected";
                    case "Coins":
                        return "coins_details_selected";
                    case "My Profile":
                        return "profile_menu_selected";
                    case "Support":
                        return "support_menu_selected";
                    case "Logout":
                        return "logout_menu_selected";
                    case "About":
                        return "about_menu_selected";
                }
            }
            else
            {
                
                switch (nameParam)
                {
                    case "Home":
                        return "home_menu";
                    case "Interests":
                        return "interest_menu";
                    case "Settings":
                        return "settings_menu";
                    case "Coins":
                        return "coins_details";
                    case "My Profile":
                        return "profile_menu";
                    case "Support":
                        return "support_menu";
                    case "Logout":
                        return "logout_menu";
                    case "About":
                        return "about_menu";
                }
            }
            return "home_menu";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
