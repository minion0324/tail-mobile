using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Tail.Common;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.Controls
{
    public partial class CustomTabViewControl : ContentView
    {
        public CustomTabViewControl()
        {
            try
            {
                InitializeComponent();

                BindingContext = this;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
        }

        public ObservableCollection<TabItemsModel> TabItems
        {
            get
            {
                return (ObservableCollection<TabItemsModel>)GetValue(TabItemsProperty);
            }
            set
            {
                SetValue(TabItemsProperty, value);
            }
        }
        public TabItemsModel SelectedTab
        {
            get
            {
                return (TabItemsModel)GetValue(SelectedTabProperty);
            }
            set
            {
                SetValue(SelectedTabProperty, value);
            }
        }
        public ObservableCollection<ContentView> TabViews
        {
            get
            {
                return (ObservableCollection<ContentView>)GetValue(TabViewsProperty);
            }
            set
            {
                SetValue(TabViewsProperty, value);
            }
        }
       
        public Action<TabItemsModel> SelectedTabIndexChangedCallback
        {
            get => (Action<TabItemsModel>)GetValue(SelectedTabIndexChangedCallbackProperty);
            set => SetValue(SelectedTabIndexChangedCallbackProperty, value);
        }
       
        public static readonly BindableProperty SelectedTabProperty = BindableProperty.Create(
                                                                  "SelectedTab", 
                                                                  typeof(TabItemsModel),
                                                                  typeof(CustomTabViewControl), 
                                                                  null); 


        public static readonly BindableProperty TabItemsProperty = BindableProperty.Create(
                                                               "TabItems", 
                                                               typeof(ObservableCollection<TabItemsModel>),
                                                               typeof(CustomTabViewControl),
                                                               new ObservableCollection<TabItemsModel>());

        public static readonly BindableProperty SelectedTabIndexChangedCallbackProperty = BindableProperty.Create(propertyName: "u",
            returnType: typeof(Action<TabItemsModel>),
            declaringType: typeof(CustomTabViewControl));


        public static readonly BindableProperty TabViewsProperty = BindableProperty.Create(
                                                                  "TabViews", 
                                                                  typeof(ObservableCollection<ContentView>),
                                                                  typeof(CustomTabViewControl), 
                                                                  new ObservableCollection<ContentView>()
                                                                ); 


        void CollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection;

            TabItemsModel item = (TabItemsModel)selected[0];

            var index = TabItems.IndexOf(item);
            if (index != -1 && TabViews.Count > 0)
            {
                    SubGrid.Children.Clear();
                    SubGrid.Children.Add(TabViews[index]);
            }
            var item1 = TabItems.FirstOrDefault(i => i.IsSelected);
            if (item1 != null)
            {
                item1.IsSelected = false;
            }
            var q = TabItems.FirstOrDefault(t => t.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
            if (q != null)
            {
                q.IsSelected = true;
                SelectedTab = q;
            }
            SelectedTabIndexChangedCallback?.Invoke(SelectedTab);
        } 
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TabItemsProperty.PropertyName)
            {
                TabCollection.ItemsSource = TabItems;
            }
            else if (propertyName == SelectedTabProperty.PropertyName)
            {
                TabCollection.SelectedItem = SelectedTab;
            }
        }
      
    }
}


