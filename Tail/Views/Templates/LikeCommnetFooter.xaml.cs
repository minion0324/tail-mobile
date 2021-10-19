using System;
using Xamarin.Forms;

namespace Tail.Views.Templates
{
    public partial class LikeCommnetFooter : ContentView
    {
        public LikeCommnetFooter()
        {
            InitializeComponent();
        }
        public int LikeCount
        {
            get => (int)GetValue(LikeCountProperty);
            set => SetValue(LikeCountProperty, value);
        }
        public string LikeImage
        {
            get => (string)GetValue(LikeImageProperty);
            set => SetValue(LikeImageProperty, value);
        }
        public int CommentCount
        {
            get => (int)GetValue(CommentCountProperty);
            set => SetValue(CommentCountProperty, value);
        }
        public int DisLikeCount
        {
            get => (int)GetValue(DisLikeCountProperty);
            set => SetValue(DisLikeCountProperty, value);
        }
        public string DisLikeImage
        {
            get => (string)GetValue(DisLikeImageProperty);
            set => SetValue(DisLikeImageProperty, value);
        }
        public Command LikeCommand
        {
            get => (Command)GetValue(LikeCommandProperty);
            set => SetValue(LikeCommandProperty, value);
        }
        public object LikeCommandParameter
        {
            get => GetValue(LikeCommandParameterProperty);
            set => SetValue(LikeCommandParameterProperty, value);
        }
        public Command CommentCommand
        {
            get => (Command)GetValue(CommentCommandProperty);
            set => SetValue(CommentCommandProperty, value);
        }
        public object CommentCommandParameter
        {
            get => GetValue(CommentCommandParameterProperty);
            set => SetValue(CommentCommandParameterProperty, value);
        }
        public Command DisLikeCommand
        {
            get => (Command)GetValue(DisLikeCommandProperty);
            set => SetValue(DisLikeCommandProperty, value);
        }
        public object DisLikeCommandParameter
        {
            get => GetValue(DisLikeCommandParameterProperty);
            set => SetValue(DisLikeCommandParameterProperty, value);
        }
        public bool CommentBoxVisible
        {
            get => (bool)GetValue(CommentBoxVisibleProperty);
            set => SetValue(CommentBoxVisibleProperty, value);
        }
        public static readonly BindableProperty LikeCountProperty = BindableProperty.Create(propertyName: "LikeCount",
                                                                                                   returnType: typeof(int),
                                                                                                   declaringType: typeof(LikeCommnetFooter),
                                                                                                    propertyChanged: OnLikeCountProperty);

        public static readonly BindableProperty LikeImageProperty = BindableProperty.Create(propertyName: "LikeImage",
                                                                                                returnType: typeof(string),
                                                                                                declaringType: typeof(LikeCommnetFooter),
                                                                                                 propertyChanged: OnLikeImageProperty);

        public static readonly BindableProperty CommentCountProperty = BindableProperty.Create(propertyName: "CommentCount",
                                                                                              returnType: typeof(int),
                                                                                              declaringType: typeof(LikeCommnetFooter),
                                                                                               propertyChanged: OnCommentCountProperty);

        public static readonly BindableProperty DisLikeCountProperty = BindableProperty.Create(propertyName: "DisLikeCount",
                                                                                                returnType: typeof(int),
                                                                                                declaringType: typeof(LikeCommnetFooter),
                                                                                                 propertyChanged: OnDisLikeCountProperty);

        public static readonly BindableProperty DisLikeImageProperty = BindableProperty.Create(propertyName: "DisLikeImage",
                                                                                                returnType: typeof(string),
                                                                                                declaringType: typeof(LikeCommnetFooter),
                                                                                                 propertyChanged: OnDisLikeImageProperty);


        public static readonly BindableProperty LikeCommandProperty = BindableProperty.Create(propertyName: "LikeCommand",
                                                                                         returnType: typeof(Command),
                                                                                         declaringType: typeof(LikeCommnetFooter),
                                                                                         propertyChanged: OnLikeCommandProperty);

        public static readonly BindableProperty LikeCommandParameterProperty = BindableProperty.Create(propertyName: "LikeCommandParameter",
                                                               returnType: typeof(object),
                                                               declaringType: typeof(LikeCommnetFooter),
                                                               defaultValue: null,
                                                               propertyChanged: OnLikeCommandParameterProperty);



        public static readonly BindableProperty CommentCommandProperty = BindableProperty.Create(propertyName: "CommentCommand",
                                                                                       returnType: typeof(Command),
                                                                                       declaringType: typeof(LikeCommnetFooter),
                                                                                       propertyChanged: OnCommentCommandProperty);

        public static readonly BindableProperty CommentCommandParameterProperty = BindableProperty.Create(propertyName: "CommentCommandParameter",
                                                               returnType: typeof(object),
                                                               declaringType: typeof(LikeCommnetFooter),
                                                               defaultValue: null,
                                                               propertyChanged: OnCommentCommandParameterProperty);


        public static readonly BindableProperty DisLikeCommandProperty = BindableProperty.Create(propertyName: "DisLikeCommand",
                                                                                   returnType: typeof(Command),
                                                                                   declaringType: typeof(LikeCommnetFooter),
                                                                                   propertyChanged: OnDisLikeCommandProperty);

        public static readonly BindableProperty DisLikeCommandParameterProperty = BindableProperty.Create(propertyName: "DisLikeCommandParameter",
                                                               returnType: typeof(object),
                                                               declaringType: typeof(LikeCommnetFooter),
                                                               defaultValue: null,
                                                               propertyChanged: OnDisLikeCommandParameterProperty);


        public static readonly BindableProperty CommentBoxVisibleProperty = BindableProperty.Create(propertyName: "CommentBoxVisible",
                                                                                               returnType: typeof(bool),
                                                                                               declaringType: typeof(LikeCommnetFooter),
                                                                                               defaultValue: true,
                                                                                               propertyChanged: OnCommentBoxVisibleProperty);




        static void OnLikeCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            int _count = (int)newValue;
            if (_count > 0)
            {
                string _countString = (_count < 10) ? "0" + _count : Convert.ToString(_count);
                _control.LabelLikeCount.Text = _countString;
                _control.LabelLikeCount.IsVisible = true;
            }
            else
            {
                _control.LabelLikeCount.IsVisible = false;
            }
            
        }
        static void OnLikeImageProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            _control.ImageLike.Source = (string)newValue;


        }
        static void OnCommentCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            int _count = (int)newValue;
            if (_count > 0)
            {
                string _countString = (_count < 10) ? "0" + _count : Convert.ToString(_count);
                _control.LabelCommentCount.Text = _countString;
                _control.LabelCommentCount.IsVisible = true;
            }
            else
            {
                _control.LabelCommentCount.IsVisible = false;
            } 
        }
        static void OnCommentBoxVisibleProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            bool _commentBoxVisible = (bool)newValue;
            _control.CommentButton.IsVisible = _commentBoxVisible;
        }
        static void OnDisLikeCountProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            int _count = (int)newValue;
            if (_count > 0)
            {
                string _countString = (_count < 10) ? "0" + _count : Convert.ToString(_count);
                _control.LabelDisLikeCount.Text = _countString;
                _control.LabelDisLikeCount.IsVisible = true;
            }
            else
            {
                _control.LabelDisLikeCount.IsVisible = false;
            }
            
        }
        static void OnDisLikeImageProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            _control.ImageDisLike.Source = (string)newValue;
        }
        static void OnLikeCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            _control.LikeButton.GestureRecognizers.Clear();
            _control.LikeButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.LikeCommandParameter
            });

        }
        public static void OnLikeCommandParameterProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = (LikeCommnetFooter)bindable;

            if (_control.LikeButton.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.LikeButton.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }
        static void OnCommentCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            _control.CommentButton.GestureRecognizers.Clear();
            _control.CommentButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.CommentCommandParameter
            });

        }
        public static void OnCommentCommandParameterProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = (LikeCommnetFooter)bindable;

            if (_control.CommentButton.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.CommentButton.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }
        static void OnDisLikeCommandProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = bindable as LikeCommnetFooter;
            _control.DisLikeButton.GestureRecognizers.Clear();
            _control.DisLikeButton.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = _control.DisLikeCommandParameter
            });

        }
        public static void OnDisLikeCommandParameterProperty(BindableObject bindable, object oldValue, object newValue)
        {
            LikeCommnetFooter _control = (LikeCommnetFooter)bindable;

            if (_control.DisLikeButton.GestureRecognizers.Count > 0)
            {
                var imageTapGestureRecognizer = _control.DisLikeButton.GestureRecognizers[0] as TapGestureRecognizer;
                imageTapGestureRecognizer.CommandParameter = newValue;
            }

        }

    }
}
