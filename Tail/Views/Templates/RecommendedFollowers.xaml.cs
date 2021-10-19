using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class RecommendedFollowers : ViewCell
    {
        private IAppNavigationService NavigationService;
        public RecommendedFollowers()
        {

            InitializeComponent();
            NavigationService = AppNavigationService.GetInstance();
        }
        public Command UserDetailsCommand
        {
            get => (Command)GetValue(UserDetailsCommandProperty);
            set => SetValue(UserDetailsCommandProperty, value);
        }
        public object UserDetailsCommandParameter
        {
            get => GetValue(UserDetailsCommandParameterProperty);
            set => SetValue(UserDetailsCommandParameterProperty, value);
        }
        void FollowButton_Clicked(System.Object sender, System.EventArgs e)
        {
            NavigationService.ShowAlertAsync(AppResources.AppName, AppResources.NotImplemented);
        }
        public static readonly BindableProperty UserDetailsCommandProperty = BindableProperty.Create(propertyName: "UserDetailsCommand",
                                                                                        returnType: typeof(Command),
                                                                                        declaringType: typeof(RecommendedFollowers),
                                                                                        propertyChanged: OnUserDetailsCommandProperty);


        public static readonly BindableProperty UserDetailsCommandParameterProperty = BindableProperty.Create(propertyName: "UserDetailsCommandParameter",
                                                            returnType: typeof(object),
                                                            declaringType: typeof(RecommendedFollowers),
                                                            defaultValue: null,
                                    propertyChanged: OnUserDetailsCommandParameter);
        static void OnUserDetailsCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            RecommendedFollowers _control = bindable as RecommendedFollowers;
            _control.OuterGrid.GestureRecognizers.Clear();
            _control.OuterGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.UserDetailsCommandParameter
            });

        }
        public static void OnUserDetailsCommandParameter(BindableObject bindable, object oldValue, object newValue)
        {
            RecommendedFollowers _control = (RecommendedFollowers)bindable;

            if (_control.OuterGrid.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.OuterGrid.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }
    }
}
