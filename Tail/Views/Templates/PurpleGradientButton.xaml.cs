using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class PurpleGradientButton : ContentView
    {
        public PurpleGradientButton()
        {
            InitializeComponent();
        }
        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }
        public bool ButtonVisible
        {
            get => (bool)GetValue(ButtonVisibleProperty);
            set => SetValue(ButtonVisibleProperty, value);
        }
        public Command Command
        {
            get => (Command)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(propertyName: "ButtonText",
                                                                                                  returnType: typeof(string),
                                                                                                  declaringType: typeof(PurpleGradientButton),
                                                                                                  propertyChanged: OnButtonTextProperty);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(propertyName: "Command",
                                                                returnType: typeof(Command),
                                                                declaringType: typeof(PurpleGradientButton),
                                                                propertyChanged: OnCommandPropertyChanged);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(propertyName: "CommandParameter",
                                                                returnType: typeof(object),
                                                                declaringType: typeof(PurpleGradientButton),
                                                                defaultValue: null,
                                                                propertyChanged: OnCommandParameterPropertyChanged);

        public static readonly BindableProperty ButtonVisibleProperty = BindableProperty.Create(propertyName: "ButtonVisible",
                                                                                                  returnType: typeof(bool),
                                                                                                  declaringType: typeof(PurpleGradientButton),
                                                                                                  propertyChanged: OnButtonVisibleProperty);

        static void OnButtonTextProperty(BindableObject bindable, object oldValue, object newValue)
        {
            PurpleGradientButton _control = bindable as PurpleGradientButton;
            string _buttonText = (string)newValue;
            _control.ButtonLabel.Text = _buttonText;

        }
        static void OnButtonVisibleProperty(BindableObject bindable, object oldValue, object newValue)
        {
            PurpleGradientButton _control = bindable as PurpleGradientButton;
            _control.MainGrid.IsVisible = (bool)newValue;
         

        }
        public static void OnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PurpleGradientButton button = (PurpleGradientButton)bindable;
            var command = newValue as Command;

            button.ButtonFrame.GestureRecognizers.Clear();
            button.ButtonFrame.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = command,
                CommandParameter = button.CommandParameter
            });

            button.ButtonLabel.GestureRecognizers.Clear();
            button.ButtonLabel.Command = command;
            button.ButtonLabel.CommandParameter = button.CommandParameter;
        }
        public static void OnCommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PurpleGradientButton button = (PurpleGradientButton)bindable;

            if (button.ButtonFrame.GestureRecognizers.Count > 0)
            {
                var frameTapGestureRecognizer = button.ButtonFrame.GestureRecognizers[0] as TapGestureRecognizer;
                frameTapGestureRecognizer.CommandParameter = newValue;
            }

           
              
                button.ButtonLabel.CommandParameter = newValue;
            
        }
    }
}
