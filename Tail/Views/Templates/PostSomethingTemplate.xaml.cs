using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class PostSomethingTemplate : ViewCell
    {
        public PostSomethingTemplate()
        {
            InitializeComponent();
        }
        public bool CreateImage
        {
            get => (bool)GetValue(CreateImageProperty);
            set => SetValue(CreateImageProperty, value);
        }
        public static readonly BindableProperty CreateImageProperty = BindableProperty.Create(propertyName: "CreateImage",
                                                                                                  returnType: typeof(bool),
                                                                                                  declaringType: typeof(PostSomethingTemplate),
                                                                                                  propertyChanged: OnCreateImageProperty);
        static void OnCreateImageProperty(BindableObject bindable, object oldValue, object newValue)
        {
            PostSomethingTemplate _control = bindable as PostSomethingTemplate;
            bool _paramValue = (bool)newValue;
            if (_paramValue)
            {
                _control.OuterStackView.OnDrawing?.Invoke((bytes) =>
                {
                    var thumbPath = DependencyService.Get<IImageHelper>().SaveShareImageToDirectory(bytes);
                    CommonSingletonUtility.SharedInstance.ShareImagePath = thumbPath;
                });
            }
            
        }
    }
}
