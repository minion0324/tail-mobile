
namespace Tail.Models
{
    public class ForgotPasswordRequestInfo
    {
        public string emailId { get; set; }
    }
    public class UpdatePasswordInfo
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
