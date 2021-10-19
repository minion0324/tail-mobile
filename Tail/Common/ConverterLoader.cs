using Xamarin.Forms;
using Tail.Converters;
namespace Tail.Common
{
    public static class ConverterLoader
    {
        public static void LoadConverters()
        {
            Application.Current.Resources.Add("IntToBooleanConverter", new IntToBooleanConverter());
            Application.Current.Resources.Add("InverseBooleanConverter", new InverseBooleanConverter());
            Application.Current.Resources.Add("CheckBoxImageConverter", new CheckBoxImageConverter());
            Application.Current.Resources.Add("InterestBGImageConvertor", new InterestBGImageConvertor());
            Application.Current.Resources.Add("MenuImageConverter", new MenuImageConverter());
            Application.Current.Resources.Add("InterestLabelColorConvertor", new InterestLabelColorConvertor());
            Application.Current.Resources.Add("MenuLabelColorConverter", new MenuLabelColorConverter());
            Application.Current.Resources.Add("PaymentSelectionColorConverter", new PaymentSelectionColorConverter());
            Application.Current.Resources.Add("PaymentSelectionTextConverter", new PaymentSelectionTextConverter());
            Application.Current.Resources.Add("CardImageConverter", new CardImageConverter());
            Application.Current.Resources.Add("TabColorConverter", new TabColorConverter());
            Application.Current.Resources.Add("RadioImageConverter", new RadioImageConverter());
            Application.Current.Resources.Add("RadioTextConverter", new TabColorConverter());
            Application.Current.Resources.Add("InverseRadioImageConverter", new TabColorConverter());
            Application.Current.Resources.Add("InverseRadioTextConverter", new TabColorConverter());
            Application.Current.Resources.Add("TabSelectionColorConverter", new TabSelectionColorConverter());
            Application.Current.Resources.Add("FollowTextConverter", new FollowTextConverter());
            Application.Current.Resources.Add("FollowButtonColorConverter", new FollowButtonColorConverter());
            Application.Current.Resources.Add("FollowFollowingTextConverter", new FollowFollowingTextConverter());
            Application.Current.Resources.Add("SwitchConverter", new SwitchConverter());
            Application.Current.Resources.Add("TabCountVisibilityConverter", new TabCountVisibilityConverter());
            Application.Current.Resources.Add("InverseBooleanConverter1", new InverseBooleanConverter());
            Application.Current.Resources.Add("FirstValidationErrorConverter", new FirstValidationErrorConverter());
            Application.Current.Resources.Add("ImagePathConverter", new ImagePathConverter());
            Application.Current.Resources.Add("PlaceHolderConverter", new PlaceHolderConverter());
            Application.Current.Resources.Add("LogoConverterHome", new LogoConverterHome());
            Application.Current.Resources.Add("FrameBorderColorConverter", new FrameBorderColorConverter());
            Application.Current.Resources.Add("DisableEnableColorConverter", new DisableEnableColorConverter());
            Application.Current.Resources.Add("LogoConverterFeedHome", new LogoConverterFeedHome());
            Application.Current.Resources.Add("LogoConverterFeedAway", new LogoConverterFeedAway());
            Application.Current.Resources.Add("FailedLabelColorConverter", new FailedLabelColorConverter());
            Application.Current.Resources.Add("NotificationBadgeConverter", new NotificationBadgeConverter());

        }
    }
}
