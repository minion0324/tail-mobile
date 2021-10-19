using System;
using System.Diagnostics;
using System.Globalization;
using Tail.Common;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var urlPath = string.Empty;
            try
            {

                string imageName = System.Convert.ToString(value);

                if (!string.IsNullOrEmpty(imageName))
                {
                    urlPath = TailUtils.GetOrginalPostImage(imageName);
                }
                else
                {
                    urlPath="";
                }
               

            }
            catch (Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
            return urlPath;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
