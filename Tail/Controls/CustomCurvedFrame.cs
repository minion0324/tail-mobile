using Xamarin.Forms;

namespace Tail.Controls
{
    public class CustomCurvedFrame: Frame
    {
        
        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CustomCurvedFrame), typeof(CornerRadius), typeof(CustomCurvedFrame));

        public CustomCurvedFrame()
        {
            base.CornerRadius = 0;
        }

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
    }
}
