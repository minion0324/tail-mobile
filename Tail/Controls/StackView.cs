using System.Collections;
using Tail.Models;
using Tail.Views.Templates;
using Xamarin.Forms;

namespace Tail.Controls
{
    public class StackView : StackLayout
    {
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public IList ItemSource
        {
            get => (IList)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }
        public IList AddItems
        {
            get => (IList)GetValue(AddItemsProperty);
            set => SetValue(AddItemsProperty, value);
        }
        public bool IsRefreshing
        {
            get => (bool)GetValue(IsRefreshingProperty);
            set => SetValue(IsRefreshingProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(propertyName: "ItemTemplate",
                                                                                                 returnType: typeof(DataTemplate),
                                                                                                 declaringType: typeof(StackView));

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(propertyName: "ItemSource",
                                                                                                 returnType: typeof(IList),
                                                                                                 declaringType: typeof(StackView),
                                                                                                 propertyChanged: ItemSourcePropertyChanged);

        public static readonly BindableProperty AddItemsProperty = BindableProperty.Create(propertyName: "AddItems",
                                                                                                returnType: typeof(IList),
                                                                                                declaringType: typeof(StackView),
                                                                                                propertyChanged: AddItemsPropertyChanged);


        public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(propertyName: "IsRefreshing",
            returnType: typeof(bool),
            declaringType: typeof(StackView),
            defaultValue: false,
            propertyChanged: IsRefreshingPropertyChanged);

        static void ItemSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (StackView)bindable;
            stackView.IsRefreshing = true;
        }
        static void AddItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (StackView)bindable;
            IList _newAddItems = (IList)newValue;
            stackView.AddNewItem(_newAddItems);

        }
        static void IsRefreshingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var stackView = (StackView)bindable;
            bool shouldRefresh = (bool)newValue;

            if (shouldRefresh)
            {
                stackView.Refresh();
            }
        }

        public void Refresh()
        {
            Children.Clear();

            if (ItemSource != null)
            {
                foreach (object item in ItemSource)
                {
                    if (ItemTemplate == null && item is PostDetailsMainModel)
                    {

                        PostDetailsMainModel _item = item as PostDetailsMainModel;
                        if (_item.PostItem.Post_Type == Common.PostType.Free)
                            ItemTemplate = new DataTemplate(typeof(PostSomething));
                        else
                            ItemTemplate = new DataTemplate(typeof(PostPick));

                    }

                    var view = (View)ItemTemplate.CreateContent();
                    view.BindingContext = item;
                    Children.Add(view);
                }
            }

            IsRefreshing = false;
        }
        public void AddNewItem(IList NewAddItems)
        {


            foreach (object item in NewAddItems)
            {
                if (ItemTemplate == null && item is PostDetailsMainModel)
                {

                    PostDetailsMainModel _item = item as PostDetailsMainModel;
                    if (_item.PostItem.Post_Type == Common.PostType.Free)
                        ItemTemplate = new DataTemplate(typeof(PostSomething));
                    else
                        ItemTemplate = new DataTemplate(typeof(PostPick));

                }
                var view = (View)ItemTemplate.CreateContent();
                view.BindingContext = item;
                Children.Add(view);

            }
        }
    }
}
