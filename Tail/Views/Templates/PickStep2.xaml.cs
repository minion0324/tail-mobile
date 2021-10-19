using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class PickStep2 : ContentView
    {
       

        public PickStep2()
        {
            InitializeComponent();
          
        }
        public int SelectedTabIndex
        {
            get => (int)GetValue(SelectedTabIndexProperty);
            set => SetValue(SelectedTabIndexProperty, value);
        }
        public static readonly BindableProperty SelectedTabIndexProperty = BindableProperty.Create(propertyName: "SelectedTabIndex",
                                                                                         returnType: typeof(int),
                                                                                         declaringType: typeof(PickStep2),
                                                                                         propertyChanged: SelectedTabIndexPropertyChanged);


        static void SelectedTabIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (PickStep2)bindable;
            int _tabIndex = (int)newValue;
            if (_tabIndex == 3)
            {
                stackView.MoneyLineContent.IsVisible = false;
                stackView.SpreadContent.IsVisible = false;
                stackView.OverUnderContent.IsVisible = true;

                stackView.Tab1Label.TextColor = Color.FromHex("#999999");
                stackView.Tab2Label.TextColor = Color.FromHex("#999999");
                stackView.Tab3Label.TextColor = Color.FromHex("#672967");

                stackView.Tab1UnderLine.BackgroundColor = Color.Transparent;
                stackView.Tab2UnderLine.BackgroundColor = Color.Transparent;
                stackView.Tab3UnderLine.BackgroundColor = Color.FromHex("#672967");

            }
            else if (_tabIndex == 2)
            {
                stackView.MoneyLineContent.IsVisible = false;
                stackView.SpreadContent.IsVisible = true;
                stackView.OverUnderContent.IsVisible = false;

                stackView.Tab1Label.TextColor = Color.FromHex("#999999");
                stackView.Tab2Label.TextColor = Color.FromHex("#672967");
                stackView.Tab3Label.TextColor = Color.FromHex("#999999");

                stackView.Tab1UnderLine.BackgroundColor = Color.Transparent;
                stackView.Tab2UnderLine.BackgroundColor = Color.FromHex("#672967");
                stackView.Tab3UnderLine.BackgroundColor = Color.Transparent;
            }
            else
            {
                stackView.MoneyLineContent.IsVisible = true;
                stackView.SpreadContent.IsVisible = false;
                stackView.OverUnderContent.IsVisible = false;


                stackView.Tab1Label.TextColor = Color.FromHex("#672967");
                stackView.Tab2Label.TextColor = Color.FromHex("#999999");
                stackView.Tab3Label.TextColor = Color.FromHex("#999999");

                stackView.Tab1UnderLine.BackgroundColor = Color.FromHex("#672967");
                stackView.Tab2UnderLine.BackgroundColor = Color.Transparent;
                stackView.Tab3UnderLine.BackgroundColor = Color.Transparent;

               
            }
        }

       
    }
}
