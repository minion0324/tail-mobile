
namespace Tail.Models
{
    public class OtpRequestInfo
    {
        public string userId { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string OTP { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
    }
    public class OtpResendInfo
    {
        public string userId { get; set; }
        public string phone { get; set; }
        public string country { get; set; }

    }
}
