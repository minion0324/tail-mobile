using System;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class BannerTags : ContentView
    {
        public BannerTags()
        {
            InitializeComponent();
        }
        public TagTypes TagType
        {
            get => (TagTypes)GetValue(TagTypeProperty);
            set => SetValue(TagTypeProperty, value);
        }
        public int PurchaseCount
        {
            get => (int)GetValue(PurchaseCountProperty);
            set => SetValue(PurchaseCountProperty, value);
        }
        public static readonly BindableProperty TagTypeProperty = BindableProperty.Create(propertyName: "TagType",
                                                                                                     returnType: typeof(TagTypes),
                                                                                                     declaringType: typeof(BannerTags),
                                                                                                     propertyChanged: OnTagTypeProperty);

        public static readonly BindableProperty PurchaseCountProperty = BindableProperty.Create(propertyName: "PurchaseCount",
                                                                                                    returnType: typeof(int),
                                                                                                    declaringType: typeof(BannerTags),
                                                                                                    propertyChanged: OnPurchaseCountProperty);


        static void OnTagTypeProperty(BindableObject bindable, object oldValue, object newValue)
        {
           BannerTags TagControl = bindable as BannerTags;
           TagControl.TagFrame.IsVisible = true;
           TagTypes _tagType = (TagTypes)newValue;

            switch (_tagType)
            {
                case TagTypes.Free:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#a3fb88");
                    TagControl.TagLabel.Text = AppResources.FreeText;
                    break;
                case TagTypes.Paid:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#f5f389");
                    TagControl.TagLabel.Text = AppResources.PaidText;
                    break;
                case TagTypes.MoneyLine:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#fed0f7");
                    TagControl.TagLabel.Text = AppResources.MoneyLineText;
                    break;
                case TagTypes.Spread:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#fed0f7");
                    TagControl.TagLabel.Text = AppResources.SpreadText;
                    break;
                case TagTypes.OverUnder:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#fed0f7");
                    TagControl.TagLabel.Text = AppResources.OverUnderText;
                    break;
                case TagTypes.Purchased:
                    TagControl.TagFrame.BackgroundColor = Color.FromHex("#e3e3e3");
                    TagControl.TagLabel.Text = AppResources.PurchasedText;
                    break;
                default:
                    TagControl.TagFrame.IsVisible = false;
                    break;
            }

         

        }
        static void OnPurchaseCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            BannerTags TagControl = bindable as BannerTags;
            int _purchaseCount = Convert.ToInt32(newValue);
            if (_purchaseCount > 0)
            {
                string _purchaseText = (_purchaseCount > 1) ? AppResources.PurchasedText : AppResources.SinglePurchasedText;
                TagControl.TagLabel.Text = _purchaseCount + " " + _purchaseText;
            }
            else
            {
                TagControl.TagFrame.IsVisible = false;
            }

        }

    }
}
