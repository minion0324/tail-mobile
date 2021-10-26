using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Tail.Validators
{
    public class ValidatableObject<T> : IValidatable<T>
    {
       
        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        List<string> _errors;
        public List<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }
       

        public bool CleanOnChange { get; set; } = true;

        T _value;
        public T Value
        {
            get=>_value;
            set
            {
                _value = value;

                if (CleanOnChange)
                    IsValid = true;
            }
        }

        bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }


        public virtual bool Validate()
        {
            Errors = new List<string>();
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<Tt>(ref Tt field, Tt value, string propertyName = null, Action onChanged = null)
        {
            if (EqualityComparer<Tt>.Default.Equals(field, value)) return false;
            field = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}