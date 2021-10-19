using Xamarin.Forms;

namespace Tail.Behaviors
{
    public class FieldLengthValidator : Behavior<Entry>
    {
        public int MaxLength
        {
            get;
            set;
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

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length > MaxLength)
            {
                var entryText = entry.Text;
                entry.Text = entryText.Remove(entryText.Length - 1); // remove last char
            }
        }
    }
}
