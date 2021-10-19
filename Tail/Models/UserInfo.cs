
using Tail.Common;

namespace Tail.Models
{
    public class UserInfo
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string Dob { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public LoginType Login_type { get; set; }
        public string Password { get; set; }
        public string AboutMe { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsSportsAdded { get; set; }
    }
}
