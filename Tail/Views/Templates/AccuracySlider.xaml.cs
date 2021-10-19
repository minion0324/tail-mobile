using System;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class AccuracySlider : ContentView
    {
        public AccuracySlider()
        {
            InitializeComponent();
        }
        public double? SliderRange
        {
            get => (double?)GetValue(SliderRangeProperty);
            set => SetValue(SliderRangeProperty, value);
        }

        public static readonly BindableProperty SliderRangeProperty = BindableProperty.Create(propertyName: "SliderRange",
                                                                                                     returnType: typeof(double?),
                                                                                                     declaringType: typeof(AccuracySlider),
                                                                                                     propertyChanged: OnSliderRangePropertyProperty);


        static void OnSliderRangePropertyProperty(BindableObject bindable, object oldValue, object newValue)
        {
            AccuracySlider slider = bindable as AccuracySlider;
            if (newValue != null)
            {
                double _sliderValue = Convert.ToDouble(newValue);
                Color _sliderStartColor = (_sliderValue > 6) ? Color.FromHex("#41A33A") : Color.FromHex("#921616");
                Color _sliderEndColor = (_sliderValue > 6) ? Color.FromHex("#669514") : Color.FromHex("#DB5F00");

                slider.RangeGrid.ColumnDefinitions[0].Width = new GridLength(_sliderValue, GridUnitType.Star);
                slider.RangeGrid.ColumnDefinitions[1].Width = new GridLength((10 - _sliderValue), GridUnitType.Star);
                slider.RangeFrame.StartColor = _sliderStartColor;
                slider.RangeFrame.EndColor = _sliderEndColor;
                slider.RangeLabel.Text = (_sliderValue * 10) + "%";
                slider.RangeLabel.TextColor = _sliderEndColor;
            }
            else
            {
                int _sliderValue = 0;
                Color _sliderStartColor = (_sliderValue > 6) ? Color.FromHex("#41A33A") : Color.FromHex("#921616");
                Color _sliderEndColor = (_sliderValue > 6) ? Color.FromHex("#669514") : Color.FromHex("#DB5F00");

                slider.RangeGrid.ColumnDefinitions[0].Width = new GridLength(_sliderValue, GridUnitType.Star);
                slider.RangeGrid.ColumnDefinitions[1].Width = new GridLength((10 - _sliderValue), GridUnitType.Star);
                slider.RangeFrame.StartColor = _sliderStartColor;
                slider.RangeFrame.EndColor = _sliderEndColor;
                slider.RangeLabel.Text = "NA";
                slider.RangeLabel.TextColor = _sliderEndColor;
            }
        }

    }
}
