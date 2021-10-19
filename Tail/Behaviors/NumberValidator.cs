using Xamarin.Forms;

namespace Tail.Behaviors
{
    public class NumberValidator : Behavior<Entry>
	{	   
        public bool IsValid
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(Entry bindable)
		{
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnTextChanged;			
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;			
		}

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = double.TryParse(e.NewTextValue, out double result);
        }
    }
}

