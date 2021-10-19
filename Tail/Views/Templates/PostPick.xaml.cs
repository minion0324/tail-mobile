using System.Diagnostics;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class PostPick : ContentView
    {
        public PostPick()
        {
            InitializeComponent();
        }
        public bool CreateImage
        {
            get => (bool)GetValue(CreateImageProperty);
            set => SetValue(CreateImageProperty, value);
        }
        public bool ContentAvailable
        {
            get => (bool)GetValue(ContentAvailableProperty);
            set => SetValue(ContentAvailableProperty, value);
        }
        public bool IsPlayVisible
        {
            get => (bool)GetValue(IsPlayVisibleProperty);
            set => SetValue(IsPlayVisibleProperty, value);
        }
        public static readonly BindableProperty CreateImageProperty = BindableProperty.Create(propertyName: "CreateImage",
                                                                                                  returnType: typeof(bool),
                                                                                                  declaringType: typeof(PostPick),
                                                                                                  propertyChanged: OnCreateImageProperty);
        public static readonly BindableProperty ContentAvailableProperty = BindableProperty.Create(propertyName: "ContentAvailable",
                                                                                            returnType: typeof(bool),
                                                                                            declaringType: typeof(PostPick),
                                                                                            defaultValue: false,
                                                                                            propertyChanged: OnContentAvailableProperty);

        public static readonly BindableProperty IsPlayVisibleProperty = BindableProperty.Create(propertyName: "IsPlayVisible",
                                                                                         returnType: typeof(bool),
                                                                                         declaringType: typeof(PostPick),
                                                                                         defaultValue: false,
                                                                                         propertyChanged: OnIsPlayVisibleProperty);

        static void OnCreateImageProperty(BindableObject bindable, object oldValue, object newValue)
        {

           
        }
        static void OnContentAvailableProperty(BindableObject bindable, object oldValue, object newValue)
        {
            PostPick _control = bindable as PostPick;
            bool _paramValue = (bool)newValue;
            if (_paramValue)
            {
                _control.LabelContentAvailable.IsVisible = _paramValue;
                _control.AttachmentView.IsVisible = false;
                _control.AttachmentView.HeightRequest = 0;
                _control.indicatorView.IsVisible = false;
            }

        }
        static void OnIsPlayVisibleProperty(BindableObject bindable, object oldValue, object newValue)
        {
            PostPick _control = bindable as PostPick;
            bool _paramValue = (bool)newValue;
            if (_paramValue)
            {
                _control.PlayButton.IsVisible = true;
            }

        }
    }
}
