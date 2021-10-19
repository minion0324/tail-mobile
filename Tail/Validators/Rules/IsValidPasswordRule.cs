
using System.Text.RegularExpressions;

namespace Tail.Validators.Rules
{
    public class IsValidPasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public Regex RegexPassword { get; set; } = new Regex("(?=.*\\d)(?=.*[A-Z])(?=.*[$@$!%*#?&]).{8,}");

        public bool Check(in T value)
        {
            return (RegexPassword.IsMatch($"{value}"));
        }
    }
}
