
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Views.TabViews
{
    public partial class OverUnder : ContentView
    {
        public OverUnder()
        {
            InitializeComponent();
        }
        public int OverUnderSelection
        {
            get => (int)GetValue(OverUnderSelectionProperty);
            set => SetValue(OverUnderSelectionProperty, value);
        }


        public static readonly BindableProperty OverUnderSelectionProperty = BindableProperty.Create(propertyName: "OverUnderSelection",
                                                                                           returnType: typeof(int),
                                                                                           declaringType: typeof(OverUnder),
                                                                                           propertyChanged: OverUnderSelectionPropertyChanged);


        static void OverUnderSelectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (OverUnder)bindable;
            int _moneyLineValue = (int)newValue;
            if (_moneyLineValue == 1)
            {
                stackView.SelectionImage1.Source = "radio_selected";
                stackView.SelectionImage2.Source = "radio";
                stackView.SelectionText1.Text = AppResources.SelectedText;
                stackView.SelectionText2.Text = AppResources.SelectText;

            }
            else if (_moneyLineValue == 2)
            {
                stackView.SelectionImage1.Source = "radio";
                stackView.SelectionImage2.Source = "radio_selected";
                stackView.SelectionText1.Text = AppResources.SelectText;
                stackView.SelectionText2.Text = AppResources.SelectedText;
            }
            else
            {
                stackView.SelectionImage1.Source = "radio";
                stackView.SelectionImage2.Source = "radio";
                stackView.SelectionText1.Text = AppResources.SelectText;
                stackView.SelectionText2.Text = AppResources.SelectText;
            }
        }
    }
}
