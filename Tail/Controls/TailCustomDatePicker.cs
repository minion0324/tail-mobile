using Xamarin.Forms;

namespace Tail.Controls
{
    public class TailCustomDatePicker : DatePicker
    {
        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(TailCustomDatePicker), defaultValue: default(string));
        public string Placeholder { get; set; }
    }
}
