using Xamarin.Forms;
using Tail.Common;

namespace Tail.Views.TabViews
{
    public partial class MoneyLine : ContentView
    {
  
        public MoneyLine()
        {
            InitializeComponent();
    
        }
        public int MoneyLineSelection
        {
            get => (int)GetValue(MoneyLineSelectionProperty);
            set => SetValue(MoneyLineSelectionProperty, value);
        }

  

        
        public static readonly BindableProperty MoneyLineSelectionProperty = BindableProperty.Create(propertyName: "MoneyLineSelection",
                                                                                           returnType: typeof(int),
                                                                                           declaringType: typeof(MoneyLine),
                                                                                           propertyChanged: MoneyLineSelectionPropertyChanged);

     
        static void MoneyLineSelectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (MoneyLine)bindable;
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
