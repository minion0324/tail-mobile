using System;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class ProgressView : ContentView
    {
       
        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }
        public bool IsProgressVisible
        {
            get => (bool)GetValue(IsProgressVisibleProperty);
            set => SetValue(IsProgressVisibleProperty, value);
        }
        public string ProgressCount
        {
            get => (string)GetValue(ProgressCountProperty);
            set => SetValue(ProgressCountProperty, value);
        }
        public decimal ProgressPercentageValue
        {
            get => (decimal)GetValue(ProgressPercentageValueProperty);
            set => SetValue(ProgressPercentageValueProperty, value);
        }
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(propertyName: "IsRunning",
                                                                              returnType: typeof(bool),
                                                                              declaringType: typeof(ProgressView),
                                                                              defaultValue: false,
                                                                              propertyChanged: IsRunningPropertyChanged);

        public static readonly BindableProperty IsProgressVisibleProperty = BindableProperty.Create(propertyName: "IsProgressVisible",
                                                                            returnType: typeof(bool),
                                                                            declaringType: typeof(ProgressView),
                                                                            defaultValue: false,
                                                                            propertyChanged: IsProgressVisiblePropertyChanged);

        public static readonly BindableProperty ProgressCountProperty = BindableProperty.Create(propertyName: "ProgressCount",
                                                                           returnType: typeof(string),
                                                                           declaringType: typeof(ProgressView),
                                                                           defaultValue: string.Empty,
                                                                           propertyChanged: ProgressCountPropertyChanged);

  
        public static readonly BindableProperty ProgressPercentageValueProperty = BindableProperty.Create(propertyName: "ProgressPercentageValue",
                                                                       returnType: typeof(decimal),
                                                                       declaringType: typeof(ProgressView),
                                                                       propertyChanged: ProgressPercentageValuePropertyChanged);

        public ProgressView()
        {
            InitializeComponent();
        }
        static void IsRunningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ProgressView)bindable;
            control.activity.IsRunning = (bool)newValue;
        }
        static void IsProgressVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ProgressView)bindable;
            control.ProgressStack.IsVisible = (bool)newValue;
            control.activity.IsVisible= !(bool)newValue;
        }
        static void ProgressCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ProgressView)bindable;
            control.ProgressCountLabel.Text = (string)newValue;
        }
      
        static void ProgressPercentageValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ProgressView)bindable;
            decimal pValue = (decimal)newValue;
            control.ProgressPercentageBar.Progress = Decimal.ToDouble(pValue)/100;
        }
    }
}
