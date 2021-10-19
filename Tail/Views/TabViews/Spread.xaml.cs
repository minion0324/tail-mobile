using Tail.Common;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class Spread : ContentView
    {
        public Spread()
        {
            InitializeComponent();
        }
        public int SpreadSelection
        {
            get => (int)GetValue(SpreadSelectionProperty);
            set => SetValue(SpreadSelectionProperty, value);
        }


        public static readonly BindableProperty SpreadSelectionProperty = BindableProperty.Create(propertyName: "SpreadSelection",
                                                                                           returnType: typeof(int),
                                                                                           declaringType: typeof(Spread),
                                                                                           propertyChanged: SpreadSelectionPropertyChanged);


        static void SpreadSelectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (Spread)bindable;
            int _moneyLineValue = (int)newValue;
            if (_moneyLineValue == 1)
            {
                stackView.SelectionImage1.Source = "radio_selected";
                stackView.SelectionText1.Text = AppResources.SelectedText;
                stackView.SelectionImage2.Source = "radio";
                stackView.SelectionText2.Text = AppResources.SelectText;

            }
            else if (_moneyLineValue == 2)
            {
                stackView.SelectionImage2.Source = "radio_selected";
                stackView.SelectionText2.Text = AppResources.SelectedText;
                stackView.SelectionImage1.Source = "radio";
                stackView.SelectionText1.Text = AppResources.SelectText;
            }
            else
            {
                stackView.SelectionImage1.Source = "radio";
                stackView.SelectionText1.Text = AppResources.SelectText;
                stackView.SelectionImage2.Source = "radio";
                stackView.SelectionText2.Text = AppResources.SelectText;
            }
        }
    }
}
