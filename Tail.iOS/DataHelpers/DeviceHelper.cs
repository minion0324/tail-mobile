using UIKit;
using Xamarin.Forms;
using Tail.iOS.DataHelpers;
using Tail.Services.Interfaces;
using Tail.Common;

[assembly: Dependency(typeof(DeviceHelper))]
namespace Tail.iOS.DataHelpers
{
    public class DeviceHelper : IDeviceHelper
    {
        public float DeviceHeight
        {
            get => (float)(UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale);
        }

        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }

        public double GetDeviceHight()
        {
           return UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale;
        }

        public DeviceModel GetDeviceModel()
        {
            double height = UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale;

            switch (height)
            {
                case 1136:
                    return DeviceModel.iPhone5;

                case 1334:
                    return DeviceModel.iPhone8;

                case 1624:
                    return DeviceModel.iPhoneXR;

                case 2208:
                    return DeviceModel.iPhone8Plus;

                case 1792:
                    return DeviceModel.iPhoneXR;

                case 2436:
                    return DeviceModel.iPhoneX;

                case 2688:
                    return DeviceModel.iPhoneXSMax;

                default:
                    return DeviceModel.iPhoneX;
            }
        }
        public void QuitApp()
        {
            throw new System.NotSupportedException();
        }

        public void UpdateToken()
        {
            throw new System.NotSupportedException();
        }
    }
}
