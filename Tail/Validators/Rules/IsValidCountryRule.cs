using System;
namespace Tail.Validators.Rules
{
    public class IsValidCountryRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(in T value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }

            var str = $"{value }";
            var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
            string _regionCode = phoneNumberUtil.GetRegionCodeForCountryCode(Convert.ToInt32(str));
            return (_regionCode!="ZZ");
        }
    }
}
