using System;
using Xamarin.Forms;
namespace Tail.Controls
{
    public class TailPasswordEntry : Entry
    {
        // Need to overwrite default handler because we cant Invoke otherwise
        public new event EventHandler Completed;
        public bool IsLastItem { get; set; }

        public int NextFocusIndex { get; set; }

        public const string ReturnKeyPropertyName = "ReturnKeyType";

        public TailPasswordEntry()
        {
            this.HeightRequest = 44;
        }
        public static readonly BindableProperty BorderColorProperty =
     BindableProperty.Create(nameof(BorderColor),
         typeof(Color), typeof(TailPasswordEntry), Color.Gray);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty HasRightViewProperty =
    BindableProperty.Create(nameof(HasRightView),
        typeof(bool), typeof(TailPasswordEntry), false);
        // Gets or sets Has rightView value  
        public bool HasRightView
        {
            get => (bool)GetValue(HasRightViewProperty);
            set => SetValue(HasRightViewProperty, value);
        }

        public static readonly BindableProperty IsOTPProperty =
    BindableProperty.Create(nameof(IsOTP),
        typeof(bool), typeof(TailPasswordEntry), false);
        // Gets or sets Has rightView value  
        public bool IsOTP
        {
            get => (bool)GetValue(IsOTPProperty);
            set => SetValue(IsOTPProperty, value);
        }
        /// <summary>
        /// The return key type property.
        /// </summary>
        public static readonly BindableProperty ReturnKeyTypeProperty = BindableProperty.Create(
           propertyName: ReturnKeyPropertyName,
           returnType: typeof(ReturnKeyTypes),
           declaringType: typeof(TailPasswordEntry),
           defaultValue: ReturnKeyTypes.Done);

        /// <summary>
        /// Gets or sets the type of the return key.
        /// </summary>
        /// <value>The type of the return key.</value>
        public ReturnKeyTypes ReturnKeyType
        {
            get { return (ReturnKeyTypes)GetValue(ReturnKeyTypeProperty); }
            set { SetValue(ReturnKeyTypeProperty, value); }
        }

        /// <summary>
        /// Invokes the completed.
        /// </summary>
        public void InvokeCompleted()
        {

            if (this.Completed != null)
                this.Completed.Invoke(this, EventArgs.Empty);
        }
    }
  
}

