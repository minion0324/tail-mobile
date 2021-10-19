using Xamarin.Forms;

namespace Tail.Behaviors
{
    public class PhoneFieldFormatter : Behavior<Entry>
    {
      
        const int MAX_DIGITS = 11;

        public string Seperator
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            if (string.IsNullOrEmpty(Seperator))
            {
                Seperator = "-";
            }
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(e.NewTextValue))
            {
                return;
            }

            string formattedPhoneNumber = e.NewTextValue;
            string rawTextEntry = formattedPhoneNumber.Replace(Seperator, "");
            string oldRawTextEntry = e.OldTextValue?.Replace(Seperator, "");
            Entry entryField = (Entry)sender;

            if (rawTextEntry.Length <= oldRawTextEntry?.Length || string.IsNullOrWhiteSpace(formattedPhoneNumber))
            {
                if(!string.IsNullOrWhiteSpace(formattedPhoneNumber) && string.Compare(formattedPhoneNumber[0].ToString(), Seperator) == 0)
                {
                    formattedPhoneNumber = formattedPhoneNumber.Remove(0, 1);
                    entryField.Text = formattedPhoneNumber;
                }                
                return;
            }

            char lastCharacter = formattedPhoneNumber[formattedPhoneNumber.Length - 1];

            if (!int.TryParse(lastCharacter.ToString(), out int digit) || rawTextEntry.Length > MAX_DIGITS)
            {
                formattedPhoneNumber = formattedPhoneNumber.Remove(formattedPhoneNumber.Length - 1);
                entryField.Text = formattedPhoneNumber;                
                return;
            }

            

            switch (rawTextEntry.Length)
            {
                case 4:
                    formattedPhoneNumber = string.Concat(rawTextEntry.Substring(0, 3), Seperator,
                                                    rawTextEntry.Substring(3, 1));
                    break;

                case 7:
                    formattedPhoneNumber = string.Concat(rawTextEntry.Substring(0, 3), Seperator,
                                                    rawTextEntry.Substring(3, 3), Seperator,
                                                    rawTextEntry.Substring(6, 1));
                    break;
                case 10:
                    formattedPhoneNumber = string.Concat(rawTextEntry.Substring(0, 3), Seperator,
                                                    rawTextEntry.Substring(3, 3), Seperator,
                                                    rawTextEntry.Substring(6, 4));
                    break;
                case 11:
                    formattedPhoneNumber = string.Concat(rawTextEntry.Substring(0, 1), Seperator,
                                                    rawTextEntry.Substring(1, 3), Seperator,
                                                    rawTextEntry.Substring(4, 3), Seperator,
                                                    rawTextEntry.Substring(7, 4));
                    break;

                default:

                    break;
            }

            entryField.Text = formattedPhoneNumber;           
        }
    }
}
