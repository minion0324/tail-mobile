using System;
using Tail.Common;
using Tail.Services.Interfaces;
using Xamarin.Forms;
namespace Tail.Views.Templates
{
    public partial class HeaderView : ContentView
    {
     
        public HeaderView()
        {
            InitializeComponent();
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    OuterGrid.HeightRequest = 108;

                }

            }
            else
            {
                OuterGrid.HeightRequest = 68;
            }
        }

        public Command WalletCommand
        {
            get => (Command)GetValue(WalletCommandProperty);
            set => SetValue(WalletCommandProperty, value);
        }

        public object WalletCommandParameter
        {
            get => GetValue(WalletCommandParameterProperty);
            set => SetValue(WalletCommandParameterProperty, value);
        }
        public Command NotificationCommand
        {
            get => (Command)GetValue(NotificationCommandProperty);
            set => SetValue(NotificationCommandProperty, value);
        }

        public object NotificationCommandParameter
        {
            get => GetValue(NotificationCommandParameterProperty);
            set => SetValue(NotificationCommandParameterProperty, value);
        }

        public Command BackButtonCommand
        {
            get => (Command)GetValue(BackButtonCommandProperty);
            set => SetValue(BackButtonCommandProperty, value);
        }
        public Command SkipButtonCommand
        {
            get => (Command)GetValue(SkipButtonCommandProperty);
            set => SetValue(SkipButtonCommandProperty, value);
        }
        public bool BackButtonVisible
        {
            get => (bool)GetValue(BackButtonVisibleProperty);
            set => SetValue(BackButtonVisibleProperty, value);
        }
        public bool WalletVisible
        {
            get => (bool)GetValue(WalletVisibleProperty);
            set => SetValue(WalletVisibleProperty, value);
        }
        public bool NotificationVisible
        {
            get => (bool)GetValue(NotificationVisibleProperty);
            set => SetValue(NotificationVisibleProperty, value);
        }
        public bool LogoVisible
        {
            get => (bool)GetValue(LogoVisibleProperty);
            set => SetValue(LogoVisibleProperty, value);
        }
        public bool SkipVisible
        {
            get => (bool)GetValue(SkipVisibleProperty);
            set => SetValue(SkipVisibleProperty, value);
        }
        public bool TitleVisible
        {
            get => (bool)GetValue(TitleVisibleProperty);
            set => SetValue(TitleVisibleProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public bool SearchVisible
        {
            get => (bool)GetValue(SearchVisibleProperty);
            set => SetValue(SearchVisibleProperty, value);
        }
        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }
        public Action SearchTextChangeCallback
        {
            get => (Action)GetValue(SearchTextChangeCallbackProperty);
            set => SetValue(SearchTextChangeCallbackProperty, value);
        }

        public bool ClearVisible
        {
            get => (bool)GetValue(ClearVisibleProperty);
            set => SetValue(ClearVisibleProperty, value);
        }
        public Command ClearButtonCommand
        {
            get => (Command)GetValue(ClearButtonCommandProperty);
            set => SetValue(ClearButtonCommandProperty, value);
        }
        public static readonly BindableProperty WalletCommandProperty = BindableProperty.Create(propertyName: "WalletCommand",
                                                              returnType: typeof(Command),
                                                              declaringType: typeof(HeaderView),
                                                              propertyChanged: OnWalletCommandProperty);

        public static readonly BindableProperty WalletCommandParameterProperty = BindableProperty.Create(propertyName: "WalletCommandParameter",
                                                                returnType: typeof(object),
                                                                declaringType: typeof(HeaderView),
                                                                defaultValue: null,
                                                                propertyChanged: OnWalletCommandParameterProperty);


        public static readonly BindableProperty NotificationCommandProperty = BindableProperty.Create(propertyName: "NotificationCommand",
                                                             returnType: typeof(Command),
                                                             declaringType: typeof(HeaderView),
                                                             propertyChanged: OnNotificationCommandProperty);

        public static readonly BindableProperty NotificationCommandParameterProperty = BindableProperty.Create(propertyName: "NotificationCommandParameter",
                                                                returnType: typeof(object),
                                                                declaringType: typeof(HeaderView),
                                                                defaultValue: null,
                                                                propertyChanged: OnNotificationCommandParameterProperty);


        public static readonly BindableProperty BackButtonCommandProperty = BindableProperty.Create(propertyName: "BackButtonCommand",
                                                                                                      returnType: typeof(Command),
                                                                                                      declaringType: typeof(HeaderView),
                                                                                                      propertyChanged: OnBackButtonCommandPropertyChanged);

        public static readonly BindableProperty SkipButtonCommandProperty = BindableProperty.Create(propertyName: "SkipButtonCommand",
                                                                                                     returnType: typeof(Command),
                                                                                                     declaringType: typeof(HeaderView),
                                                                                                     propertyChanged: OnSkipButtonCommandPropertyChanged);


        public static readonly BindableProperty BackButtonVisibleProperty = BindableProperty.Create(propertyName: "BackButtonVisible",
                                                                                                returnType: typeof(bool),
                                                                                                declaringType: typeof(HeaderView),
                                                                                                defaultValue: false,
                                                                                                propertyChanged: OnBackButtonVisibilityPropertyChanged);

        public static readonly BindableProperty WalletVisibleProperty = BindableProperty.Create(propertyName: "WalletVisible",
                                                                                               returnType: typeof(bool),
                                                                                               declaringType: typeof(HeaderView),
                                                                                               defaultValue: true,
                                                                                               propertyChanged: OnWalletVisiblePropertyChanged);

        public static readonly BindableProperty LogoVisibleProperty = BindableProperty.Create(propertyName: "LogoVisible",
                                                                                              returnType: typeof(bool),
                                                                                              declaringType: typeof(HeaderView),
                                                                                              defaultValue: true,
                                                                                              propertyChanged: OnLogoVisiblePropertyChanged);

        public static readonly BindableProperty SkipVisibleProperty = BindableProperty.Create(propertyName: "SkipVisible",
                                                                                             returnType: typeof(bool),
                                                                                             declaringType: typeof(HeaderView),
                                                                                             defaultValue: false,
                                                                                             propertyChanged: OnSkipVisiblePropertyChanged);

        public static readonly BindableProperty TitleVisibleProperty = BindableProperty.Create(propertyName: "TitleVisible",
                                                                                            returnType: typeof(bool),
                                                                                            declaringType: typeof(HeaderView),
                                                                                            defaultValue: false,
                                                                                            propertyChanged: OnTitleVisiblePropertyChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: "Title",
                                                                                            returnType: typeof(string),
                                                                                            declaringType: typeof(HeaderView),
                                                                                            defaultValue: "",
                                                                                            propertyChanged: OnTitlePropertyChanged);
        public static readonly BindableProperty SearchVisibleProperty = BindableProperty.Create(propertyName: "SearchVisible",
                                                                                           returnType: typeof(bool),
                                                                                           declaringType: typeof(HeaderView),
                                                                                           defaultValue: false,
                                                                                           propertyChanged: OnSearchVisiblePropertyChanged);

        public static readonly BindableProperty NotificationVisibleProperty = BindableProperty.Create(propertyName: "NotificationVisible",
                                                                                          returnType: typeof(bool),
                                                                                          declaringType: typeof(HeaderView),
                                                                                          defaultValue: true,
                                                                                          propertyChanged: OnNotificationVisibleProperty);


        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(propertyName: "SearchText",
            returnType: typeof(string),
            declaringType: typeof(HeaderView));

        public static readonly BindableProperty SearchTextChangeCallbackProperty = BindableProperty.Create(propertyName: "SearchTextChangeCallback",
           returnType: typeof(Action),
           declaringType: typeof(HeaderView));

        public static readonly BindableProperty ClearVisibleProperty = BindableProperty.Create(propertyName: "ClearVisible",
                                                                                            returnType: typeof(bool),
                                                                                            declaringType: typeof(HeaderView),
                                                                                            defaultValue: false,
                                                                                            propertyChanged: OnClearVisiblePropertyChanged);

        public static readonly BindableProperty ClearButtonCommandProperty = BindableProperty.Create(propertyName: "ClearButtonCommand",
                                                                                                  returnType: typeof(Command),
                                                                                                  declaringType: typeof(HeaderView),
                                                                                                  propertyChanged: OnClearButtonCommandPropertyChanged);


        public static void OnWalletCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView walletButton = (HeaderView)bindable;
            var command = newValue as Command;

            walletButton.WalletButton.GestureRecognizers.Clear();
            walletButton.WalletButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = command,
                CommandParameter = walletButton.WalletCommandParameter
            });

        }
        public static void OnWalletCommandParameterProperty(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView walletButton = (HeaderView)bindable;

            if (walletButton.WalletButton.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = walletButton.WalletButton.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }


        public static void OnNotificationCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView notificationButton = (HeaderView)bindable;
            var command = newValue as Command;

            
            notificationButton.NotificationMainGrid.GestureRecognizers.Clear();
            notificationButton.NotificationMainGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = command,
                CommandParameter = notificationButton.NotificationCommandParameter
            });

            

        }

        public static void OnNotificationCommandParameterProperty(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView notificationButton = (HeaderView)bindable;


            if (notificationButton.NotificationMainGrid.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = notificationButton.NotificationMainGrid.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

           

        }
        static void OnBackButtonVisibilityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.BackButton.IsVisible = (bool)newValue;
        }
        static void OnWalletVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
        
            header.WalletButton.IsVisible = (bool)newValue;
        }
        static void OnNotificationVisibleProperty(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.NotificationButton.IsVisible = (bool)newValue;
            header.NotificatioLayOut.IsVisible = (bool)newValue;
        }
        static void OnLogoVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.Logo.IsVisible = (bool)newValue;
        }
        static void OnSkipVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.SkipButton.IsVisible = (bool)newValue;
        }
        static void OnTitleVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.TitleLabel.IsVisible = (bool)newValue;
        }
        static void OnSearchVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.SearchControl.IsVisible = (bool)newValue;
        }
        static void OnBackButtonCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;

            header.BackButton.GestureRecognizers.Clear();
            header.BackButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
            });

        }
        static void OnSkipButtonCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.SkipButton.GestureRecognizers.Clear();
            header.SkipButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
            });

        }
        static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.TitleLabel.Text = (string)newValue;
        }

        void SearchText_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SearchText = e.NewTextValue;
            SearchTextChangeCallback?.Invoke();
        }
        static void OnClearVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.ClearButton.IsVisible = (bool)newValue;
        }
        static void OnClearButtonCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            HeaderView header = bindable as HeaderView;
            header.ClearButton.GestureRecognizers.Clear();
            header.ClearButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
            });

        }
    }
}
