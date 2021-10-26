using System.Diagnostics;
using Tail.Common;
using Xamarin.Forms;
namespace Tail.Views.Templates
{
    public partial class UserDetailsHeader : ContentView
    {
        public UserDetailsHeader()
        {
            InitializeComponent();
           
        }

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }
        public string PostedTime
        {
            get => (string)GetValue(PostedTimeProperty);
            set => SetValue(PostedTimeProperty, value);
        }
        public string UserImage
        {
            get => (string)GetValue(UserImageProperty);
            set => SetValue(UserImageProperty, value);
        }
        public PickPurchaseType PickPurchase_Type
        {
            get => (PickPurchaseType)GetValue(PickPurchase_TypeProperty);
            set => SetValue(PickPurchase_TypeProperty, value);
        }
        public PickType Pick_Type
        {
            get => (PickType)GetValue(Pick_TypeProperty);
            set => SetValue(Pick_TypeProperty, value);
        }
        public ResultType Result_Type
        {
            get => (ResultType)GetValue(Result_TypeProperty);
            set => SetValue(Result_TypeProperty, value);
        }
        public int PurchaseCount
        {
            get => (int)GetValue(PurchaseCountProperty);
            set => SetValue(PurchaseCountProperty, value);
        }
        
        public Command MoreOptionCommand
        {
            get => (Command)GetValue(MoreOptionCommandProperty);
            set => SetValue(MoreOptionCommandProperty, value);
        }
        public object MoreOptionCommandParameter
        {
            get => GetValue(MoreOptionCommandParameterProperty);
            set => SetValue(MoreOptionCommandParameterProperty, value);
        }

        public bool IsMoreOptionVisible
        {
            get => (bool)GetValue(IsMoreOptionVisibleProperty);
            set => SetValue(IsMoreOptionVisibleProperty, value);
        }
        public Command UserDetailsCommand
        {
            get => (Command)GetValue(UserDetailsCommandProperty);
            set => SetValue(UserDetailsCommandProperty, value);
        }
        public object UserDetailsCommandParameter
        {
            get => GetValue(UserDetailsCommandParameterProperty);
            set => SetValue(UserDetailsCommandParameterProperty, value);
        }

        public int UnitCount
        {
            get => (int)GetValue(UnitCountProperty);
            set => SetValue(UnitCountProperty, value);
        }
        public bool IsShared
        {
            get => (bool)GetValue(IsSharedProperty);
            set => SetValue(IsSharedProperty, value);
        }
        public static readonly BindableProperty UserNameProperty = BindableProperty.Create(propertyName: "UserName",
                                                                                                    returnType: typeof(string),
                                                                                                    declaringType: typeof(UserDetailsHeader),
                                                                                                    propertyChanged: OnUserNameProperty);
        public static readonly BindableProperty PostedTimeProperty = BindableProperty.Create(propertyName: "PostedTime",
                                                                                                  returnType: typeof(string),
                                                                                                  declaringType: typeof(UserDetailsHeader),
                                                                                                  propertyChanged: OnPostedTimeProperty);
        public static readonly BindableProperty UserImageProperty = BindableProperty.Create(propertyName: "UserImage",
                                                                                                 returnType: typeof(string),
                                                                                                 declaringType: typeof(UserDetailsHeader),
                                                                                                 propertyChanged: OnUserImageProperty);
        public static readonly BindableProperty PickPurchase_TypeProperty = BindableProperty.Create(propertyName: "PickPurchase_Type",
                                                                                               returnType: typeof(PickPurchaseType),
                                                                                               declaringType: typeof(UserDetailsHeader),
                                                                                               propertyChanged: OnPickPurchaseTypeProperty);
        public static readonly BindableProperty Pick_TypeProperty = BindableProperty.Create(propertyName: "Pick_Type",
                                                                                               returnType: typeof(PickType),
                                                                                               declaringType: typeof(UserDetailsHeader),
                                                                                               propertyChanged: OnPickTypeProperty);

        public static readonly BindableProperty Result_TypeProperty = BindableProperty.Create(propertyName: "Result_Type",
                                                                                             returnType: typeof(ResultType),
                                                                                             declaringType: typeof(UserDetailsHeader),
                                                                                             propertyChanged: OnResultTypeProperty);

        public static readonly BindableProperty PurchaseCountProperty = BindableProperty.Create(propertyName: "PurchaseCount",
                                                                                             returnType: typeof(int),
                                                                                             declaringType: typeof(UserDetailsHeader),
                                                                                             propertyChanged: OnPurchaseCountProperty);
      
        
        public static readonly BindableProperty MoreOptionCommandProperty = BindableProperty.Create(propertyName: "MoreOptionCommand",
                                                                                            returnType: typeof(Command),
                                                                                            declaringType: typeof(UserDetailsHeader),
                                                                                            propertyChanged: OnMoreOptionCommandProperty);

        public static readonly BindableProperty MoreOptionCommandParameterProperty = BindableProperty.Create(propertyName: "MoreOptionCommandParameter",
                                                               returnType: typeof(object),
                                                               declaringType: typeof(UserDetailsHeader),
                                                               defaultValue: null,
                                                               propertyChanged: OnMoreOptionCommandParameter);


        public static readonly BindableProperty IsMoreOptionVisibleProperty = BindableProperty.Create(propertyName: "IsMoreOptionVisible",
                                                                                             returnType: typeof(bool),
                                                                                             declaringType: typeof(UserDetailsHeader),
                                                                                             defaultValue: true,
                                                                                             propertyChanged: OnIsMoreOptionVisibleProperty);


        public static readonly BindableProperty UserDetailsCommandProperty = BindableProperty.Create(propertyName: "UserDetailsCommand",
                                                                                          returnType: typeof(Command),
                                                                                          declaringType: typeof(UserDetailsHeader),
                                                                                          propertyChanged: OnUserDetailsCommandProperty);


        public static readonly BindableProperty UserDetailsCommandParameterProperty = BindableProperty.Create(propertyName: "UserDetailsCommandParameter",
                                                            returnType: typeof(object),
                                                            declaringType: typeof(UserDetailsHeader),
                                                            defaultValue: null,
                                                            propertyChanged: OnUserDetailsCommandParameter);

        public static readonly BindableProperty UnitCountProperty = BindableProperty.Create(propertyName: "UnitCount",
                                                                                           returnType: typeof(int),
                                                                                           declaringType: typeof(UserDetailsHeader),
                                                                                           propertyChanged: OnUnitCountProperty);

        public static readonly BindableProperty IsSharedProperty = BindableProperty.Create(propertyName: "IsShared",
                                                                                             returnType: typeof(bool),
                                                                                             declaringType: typeof(UserDetailsHeader),
                                                                                             defaultValue:false,
                                                                                             propertyChanged: OnIsSharedProperty);

        static void OnUserNameProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            string _fullName = (string)newValue;
            if (!string.IsNullOrEmpty(_fullName))
            {
                string _trimmedName = (_fullName.Length > 20) ? _fullName.Substring(0, 20) + "..." : _fullName;
                _control.UserNameLabel.Text = _trimmedName;
            }
        }
        static void OnPostedTimeProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            _control.TimeLabel.Text = (string)newValue;
        }
        static void OnUserImageProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
                _control.UserImageView.Source = (string)newValue;
            }
            else
            {
                _control.UserImageViewDroid.Source = (string)newValue;
            }

        }
        static void OnPickPurchaseTypeProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            PickPurchaseType _purchaseType = (PickPurchaseType)newValue;
            TagTypes _tagType;
            if (_purchaseType != PickPurchaseType.None)
            {
                _tagType = (_purchaseType == PickPurchaseType.Free) ? TagTypes.Free : TagTypes.Paid;
                BannerTags _banners = new BannerTags
                {
                    TagType = _tagType
                };
                _control.BannerLayout.Children.Add(_banners);
            }
        }
        static void OnPickTypeProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            PickType _pickType = (PickType)newValue;
            TagTypes _tagType;
            if (_pickType != PickType.None)
            {
                switch (_pickType)
                {
                    case PickType.MoneyLine:
                        _tagType = TagTypes.MoneyLine;
                        break;
                    case PickType.OverUnder:
                        _tagType = TagTypes.OverUnder;
                        break;
                    case PickType.Spread:
                        _tagType = TagTypes.Spread;
                        break;
                    default:
                        _tagType = TagTypes.None;
                        break;
                }
                BannerTags _banners = new BannerTags
                {
                    TagType = _tagType,
                };
                _control.BannerLayout.Children.Add(_banners);
            }
        }
        static void OnResultTypeProperty(BindableObject bindable, object oldValue, object newValue)
        {
           
        }
        

        static void OnPurchaseCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            int _purchaseCount = (int)newValue;
            if (_purchaseCount !=0)
            {
             
                    BannerTags _banners = new BannerTags
                    {
                        TagType = TagTypes.Purchased,
                        PurchaseCount = _purchaseCount
                    };
                    _control.BannerLayout.Children.Add(_banners);
            }
        }
        static void OnMoreOptionCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            _control.MoreButton.GestureRecognizers.Clear();
            _control.MoreButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.MoreOptionCommandParameter
            });

        }
        public static void OnMoreOptionCommandParameter(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = (UserDetailsHeader)bindable;

            if (_control.MoreButton.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.MoreButton.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }
        static void OnIsMoreOptionVisibleProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            _control.MoreButton.IsVisible = (bool)newValue;
        }


        static void OnUserDetailsCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            _control.OuterGrid.GestureRecognizers.Clear();
            _control.OuterGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.UserDetailsCommandParameter
            });

        }
        public static void OnUserDetailsCommandParameter(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = (UserDetailsHeader)bindable;

            if (_control.OuterGrid.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.OuterGrid.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }
        static void OnUnitCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            UserDetailsHeader _control = bindable as UserDetailsHeader;
            int _purchaseCount = (int)newValue;
            if (_purchaseCount != 0)
            {
                _control.UnitLabel.Text = _purchaseCount + " U";
                _control.UnitFrame.IsVisible = true;
            }
            else
            {
                _control.UnitFrame.IsVisible = false;
            }
        }

        static void OnIsSharedProperty(BindableObject bindable, object oldValue, object newValue)
        {
            
           
        }

    }
}
