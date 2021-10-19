using System;
using System.Collections;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class ThemePicker : ContentView
    {
        bool _bindableSet;

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public IList ItemSource
        {
            get => (IList)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        public string DisplayMember
        {
            get => (string)GetValue(DisplayMemberProperty);
            set => SetValue(DisplayMemberProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        public int SetSelectedIndex
        {
            get => (int)GetValue(SetSelectedIndexProperty);
            set => SetValue(SetSelectedIndexProperty, value);
        }
        public Action SelectedIndexChangedCallback
        {
            get => (Action)GetValue(SelectedIndexChangedCallbackProperty);
            set => SetValue(SelectedIndexChangedCallbackProperty, value);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: "Title",
            returnType: typeof(string),
            declaringType: typeof(ThemePicker),
            propertyChanged: OnTitlePropertyChanged);

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(propertyName: "ItemSource",
            returnType: typeof(IList),
            declaringType: typeof(ThemePicker),
            propertyChanged: OnItemSourcePropertyChanged);

        public static readonly BindableProperty DisplayMemberProperty = BindableProperty.Create(propertyName: "DisplayMember",
            returnType: typeof(string),
            declaringType: typeof(ThemePicker),
            propertyChanged: OnItemDisplayBindingPropertyChanged);

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(propertyName: "SelectedIndex",
            returnType: typeof(int),
            declaringType: typeof(ThemePicker));

        public static readonly BindableProperty SelectedIndexChangedCallbackProperty = BindableProperty.Create(propertyName: "SelectedIndexChangedCallback",
            returnType: typeof(Action),
            declaringType: typeof(ThemePicker));

        public static readonly BindableProperty SetSelectedIndexProperty = BindableProperty.Create(propertyName: "SetSelectedIndex",
           returnType: typeof(int),
           declaringType: typeof(ThemePicker),
           propertyChanged: OnSetSelectedIndexChanged);


        public ThemePicker()
        {
            InitializeComponent();
        }

        static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ThemePicker picker = (ThemePicker)bindable;
            picker.TemplatePicker.Title = Convert.ToString(newValue);
        }

        static void OnItemSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ThemePicker picker = (ThemePicker)bindable;
            picker.TemplatePicker.ItemsSource = (IList)newValue;
        }

        static void OnItemDisplayBindingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ThemePicker picker = (ThemePicker)bindable;
            picker.TemplatePicker.ItemDisplayBinding = new Binding(newValue.ToString());
        }
        static void OnSetSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ThemePicker picker = (ThemePicker)bindable;
            int indexValue = (int)newValue;
            if (indexValue > -1)
                picker.TemplatePicker.SelectedIndex = indexValue;


        }
      

        protected void TemplatePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bindableSet)
            {
                _bindableSet = false;
                return;
            }

            SelectedIndex = TemplatePicker.SelectedIndex;
            SelectedIndexChangedCallback?.Invoke();
        }

        protected void TemplatePicker_Tapped(object sender, EventArgs e)
        {
            TemplatePicker.Focus();
        }
    }
}
