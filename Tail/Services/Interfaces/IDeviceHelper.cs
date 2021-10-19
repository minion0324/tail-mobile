using Tail.Common;

namespace Tail.Services.Interfaces
{
    public interface IDeviceHelper
    {
        DeviceModel GetDeviceModel();
        string GetDeviceId();
        double GetDeviceHight();
        void QuitApp();
        void UpdateToken();
    }
}
