using Xamarin.Forms;
using Tail.ViewModels;
using System.Linq;
using Tail.Services.Interfaces;
using Tail.Common;
using System;
using System.Diagnostics;

namespace Tail.Views
{
    public partial class Home_Comments : AppPageBase
    {
        HomeCommentsViewModel _vModel;
        double _screenHight = 0;
        double _gridHight = 0;
        public Home_Comments()
        {
            try
            {
                InitializeComponent();
                _vModel = new HomeCommentsViewModel();
                BindingContext = _vModel;
                _screenHight = Application.Current.MainPage.Height;
                _gridHight = _screenHight - 160;
                if (!IsAndroid)
                {
                    CommentEntry.Focused += (o, s) => KeyBoradVisibleHightAdjustments();
                    CommentEntry.Unfocused += (o, s) => KeyBoradHideHightAdjustments();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
        protected override void OnBindingContextChanged()
        {
            try
            {


                base.OnBindingContextChanged();

                if (BindingContext is HomeCommentsViewModel)
                {
                    _vModel = BindingContext as HomeCommentsViewModel;

                    _vModel.OnCommentUpdated += () =>
                    {
                        CommentStack.Refresh();
                        var firstChild = CommentStack.Children.FirstOrDefault();
                        if (firstChild != null)
                            ContentScroll.ScrollToAsync(firstChild, ScrollToPosition.MakeVisible, false);

                    };
                    _vModel.OnLoadMoreUpdated += () =>
                    {
                        CommentStack.Refresh();
                    };
                    _vModel.AddNewCommentUpdated += () =>
                    {

                        CommentStack.Refresh();
                        var lastChild = CommentStack.Children.LastOrDefault();
                        if (lastChild != null)
                            ContentScroll.ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, false);

                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        void KeyBoradVisibleHightAdjustments()
        {

            if (Device.RuntimePlatform == Device.iOS)
            {
                Maingrid.RowDefinitions[1].Height = _gridHight - 350;
                switch (DependencyService.Get<IDeviceHelper>().GetDeviceModel())
                {
                    case DeviceModel.iPhone5:
                    case DeviceModel.iPhone8:
                    case DeviceModel.iPhone8Plus:
                        Maingrid.RowDefinitions[1].Height = _gridHight - 250;
                        break;
                }
            }
            else
            {
                Maingrid.RowDefinitions[1].Height = _gridHight - 240;

            }
            var lastChild = CommentStack.Children.LastOrDefault();
            if (lastChild != null)
                ContentScroll.ScrollToAsync(lastChild, ScrollToPosition.MakeVisible, false);

        }
        void KeyBoradHideHightAdjustments()
        {
            Maingrid.RowDefinitions[1].Height = GridLength.Star;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            CommentEntry.Focus();
        }
    }
}
