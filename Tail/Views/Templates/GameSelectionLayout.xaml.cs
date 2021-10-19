using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class GameSelectionLayout : ViewCell
    {

        public GameSelectionLayout()
        {
            InitializeComponent();
        }
       
        public bool IsRefresh
        {
            get => (bool)GetValue(IsRefreshProperty);
            set => SetValue(IsRefreshProperty, value);
        }
        public static readonly BindableProperty IsRefreshProperty = BindableProperty.Create(propertyName: "IsRefresh",
                                                                                               returnType: typeof(bool),
                                                                                               declaringType: typeof(GameSelectionLayout),
                                                                                               propertyChanged: IsRefreshPropertyChanged);


       
        static void IsRefreshPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (GameSelectionLayout)bindable;
            if ((bool)newValue)
                stackView.GameStack.IsRefreshing = (bool)newValue;
        }
      

    }
}
