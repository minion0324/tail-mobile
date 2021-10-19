
using Tail.Common;

namespace Tail.Models
{
    public class SignUpRequestInfo
    {
        public string userId { get; set; }
        public string uName { get; set; }
        public string email { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string password { get; set; }
        public string userImg { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }

    }
    public class AppleSignUpRequestInfo
    {
        public string userId { get; set; }
        public string uName { get; set; }
        public string appleToken { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string dob { get; set; }
        public string userImg { get; set; }
        public string deviceToken { get; set; }
        public string deviceType { get; set; }
    }
    public class FBSignUpRequestInfo
    {
        public string userId { get; set; }
        public string uName { get; set; }
        public string fbToken { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string dob { get; set; }
        public string deviceToken { get; set; }
        public string userImg { get; set; }
        public string deviceType { get; set; }
    }

    public class SignUpEventArgs
    {
        public string ID { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public LoginType LoginType { get; set; }

    }
}
