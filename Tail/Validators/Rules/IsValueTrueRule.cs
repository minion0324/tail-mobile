
namespace Tail.Validators.Rules
{
    public class IsValueTrueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(in T value)
        {
            return bool.Parse($"{value}");
        }
    }
}


