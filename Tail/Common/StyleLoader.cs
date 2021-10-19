using Xamarin.Forms;
namespace Tail.Common
{
    public static class StyleLoader
    {
        public static void LoadStyles()
        {
            Application.Current.Resources.Add("GrayLabelRegular10", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 10 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#9E9E9E") }
                }
            });
            Application.Current.Resources.Add("BlackLabelLight11", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Light") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 11 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#666666") }
                }
            });
            Application.Current.Resources.Add("BlackLabelMedium12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#666666") }
                }
            });
            Application.Current.Resources.Add("DarkBlackLabelMedium12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#333333") }
                }
            });

            Application.Current.Resources.Add("PurpleLabelMedium18", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });

            Application.Current.Resources.Add("PurpleLabelPoppinsMedium13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#C3AFD1") }
                }
            });

            Application.Current.Resources.Add("BlackLabelRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("BlackLabelRobotoRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelPoppinsMedium14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("GreenLabelRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#138D0A") }
                }
            });
            Application.Current.Resources.Add("GreenLabelBold12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#138D0A") }
                }
            });
            Application.Current.Resources.Add("OrangeLabelRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#CF7F06") }
                }
            });
            Application.Current.Resources.Add("OrangeLabelBold12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#CF7F06") }
                }
            });
            Application.Current.Resources.Add("RedLabelRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#CE3030") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelRegular12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("RedLabelBold12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#CE3030") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelSemiBold12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
         
            Application.Current.Resources.Add("BlackLabelSemiBold12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("GrayLabelMedium12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#999999") }
                }
            });
            Application.Current.Resources.Add("BlackLabelBold13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelSemiBold13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelSemiBold13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });
            Application.Current.Resources.Add("LightBlackLabelRegular13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#444444") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelSemiBold13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("LightGrayLabelRegular13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#B5B5B5") }
                }
            });
            Application.Current.Resources.Add("LightBlackLabelMedium13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("RedLabelLight13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Light") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FF0000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelMedium13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("DarkGrayLabelRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#474747") }
                }
            });
         
            Application.Current.Resources.Add("DarkBlackLabelRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#333333") }
                }
            });
            Application.Current.Resources.Add("GreyLabelRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#777777") }
                }
            });
            Application.Current.Resources.Add("BlackLabelMedium14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelMedium14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("DarkBlackLabelMedium14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelSemiBold14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelMedium16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("GreenLabelBold14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#2F8A2F") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelBold14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelBold14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelSemiBold15", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 15 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelRegular16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });
            Application.Current.Resources.Add("WhiteButtonRegular16", new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter{ Property = Button.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Button.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Button.TextColorProperty, Value = Color.FromHex("#FFFFFF") },
                }
            });
            Application.Current.Resources.Add("WhiteLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });
           
            Application.Current.Resources.Add("BlackRobotoLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackRobotoLabelBold32", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 32 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("PurpleRobotoLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("PurplePoppinsLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelMedium16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });

            Application.Current.Resources.Add("WhiteLabelMedium22", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 22 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#C935A9") }
                }
            });

            Application.Current.Resources.Add("BlackLabelLight18", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Light") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelMedium18", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#FFFFFF") }
 }
            });

            Application.Current.Resources.Add("GreyLabelRegular16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("GreyLabelRegular18", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("EntryRegular14", new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter{ Property = Entry.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Entry.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Entry.TextColorProperty, Value = Color.FromHex("#000000") },
                    new Setter{ Property = Entry.PlaceholderColorProperty,Value = Color.FromRgba(0,0,0,0.35)}
                }
            });
            Application.Current.Resources.Add("EntryRegular12", new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter{ Property = Entry.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Entry.FontSizeProperty, Value = 12 },
                    new Setter{ Property = Entry.TextColorProperty, Value = Color.FromHex("#000000") },
                    new Setter{ Property = Entry.PlaceholderColorProperty,Value = Color.FromRgba(0,0,0,0.35)}
                }
            });
            Application.Current.Resources.Add("EditorRegular14", new Style(typeof(Editor))
            {
                Setters =
                {
                    new Setter{ Property = Entry.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Entry.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Entry.TextColorProperty, Value = Color.FromHex("#000000") },
                    new Setter{ Property = Entry.PlaceholderColorProperty,Value = Color.FromRgba(0,0,0,0.35)}
                }
            });
            Application.Current.Resources.Add("SearchEntry", new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter{ Property = Entry.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Entry.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Entry.TextColorProperty, Value = Color.FromHex("#CCB2CA") },
                    new Setter{ Property = Entry.PlaceholderColorProperty,Value =  Color.FromHex("#CCB2CA") }
                    
                }
            });
            Application.Current.Resources.Add("GreyLabelMedium14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("GreyLabelBold14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") }
                }
            });
            Application.Current.Resources.Add("BlackLabelRegular17", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 17 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#333333") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("VioletLabelBold16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("TabLabelMedium12", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 12 },
                }
            });
            Application.Current.Resources.Add("BlackPickerMedium14", new Style(typeof(Picker))
            {
                Setters =
                {
                    new Setter{ Property = Entry.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Entry.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Entry.TextColorProperty, Value = Color.FromHex("#000000") },
                }
            });
            Application.Current.Resources.Add("BlackLabelRegular13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("LightGrayLabelRegular11", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 11 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#8F8F8F") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelSemiBold18", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-SemiBold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 18 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#ffffff") }
                }
            });
            Application.Current.Resources.Add("WhiteLabelMedium13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#ffffff") }
                }
            });
            Application.Current.Resources.Add("WhiteButtonWithPurpleBorder", new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter{ Property = Button.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Button.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Button.TextColorProperty, Value = Color.FromHex("#672967") },
                    new Setter{ Property = Button.BorderColorProperty, Value = Color.FromHex("#672967") },
                    new Setter{ Property = Button.BackgroundColorProperty, Value = Color.FromHex("#ffffff") },
                    new Setter{ Property = Button.BorderWidthProperty, Value = 1 },
                    new Setter{ Property = Button.WidthRequestProperty, Value = 88 },
                    new Setter{ Property = Button.HeightRequestProperty, Value = 26 },
                    new Setter{ Property = Button.CornerRadiusProperty, Value = 0 }
                }
            });
            Application.Current.Resources.Add("PurpleButton", new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter{ Property = Button.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Button.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Button.TextColorProperty, Value = Color.FromHex("#ffffff") },
                     new Setter{ Property = Button.BackgroundColorProperty, Value = Color.FromHex("#672967") },
            new Setter{ Property = Button.WidthRequestProperty, Value = 88 },
            new Setter{ Property = Button.HeightRequestProperty, Value = 26 },
                 new Setter{ Property = Button.CornerRadiusProperty, Value = 0 }
                }
            });
            Application.Current.Resources.Add("Medium13DisabledLabel", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#555555") },
                    
                }
            });
            Application.Current.Resources.Add("BlackLabelMedium16", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Medium") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 16 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelPopinsRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelPopinsRegular13", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Poppins-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 13 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("BlackLabelRegular15", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 15 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });
            Application.Current.Resources.Add("PurpleLabelRegular15", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Bold") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 15 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#672967") }
                }
            });
            Application.Current.Resources.Add("BlackLabelRobotoRegular14", new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter{ Property = Label.FontFamilyProperty, Value = GetCustomFont("Roboto-Regular") },
                    new Setter{ Property = Label.FontSizeProperty, Value = 14 },
                    new Setter{ Property = Label.TextColorProperty, Value = Color.FromHex("#000000") }
                }
            });

        }

        public static string GetCustomFont(string fontName)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                fontName = string.Format("{0}.ttf#{0}", fontName);
            }
            return fontName;
        }

    }
}
