
namespace Tail.Models
{
    public class RefreshTokenRequestInfo
    {
        public string refreshToken { get; set; }
        public string userId { get; set; }
    }
    public class RefreshTokenResponseInfo
    {
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
    }
}
