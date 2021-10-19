using System;
using System.Linq;
using Tail.Services.ApplicationServices;
using Xamarin.Forms;

namespace Tail.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                bool isValid = true;
                if (Convert.ToInt32(args.NewTextValue) != 0)
                {
                     isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)) && Convert.ToInt32(args.NewTextValue) <= CommonSingletonUtility.SharedInstance.MaxPrice && (Convert.ToInt32(args.NewTextValue) >= CommonSingletonUtility.SharedInstance.MinPrice || Convert.ToInt32(args.NewTextValue)==0); //Make sure all characters are numbers

                }
                if (!isValid)
                {
                    ((Entry)sender).Text = args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                }
            }
        }
    }
}
