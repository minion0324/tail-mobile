using System;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class MyProfile : AppPageBase
    {
        MyProfileViewModel _vModel;
        public MyProfile()
        {
            try
            {
                InitializeComponent();
                _vModel = new MyProfileViewModel();
                BindingContext = _vModel;
              

            }
            catch (Exception ex)
            {
                Task.Run(async () => await AppNavigationService.GetInstance().ShowAlertAsync(AppResources.AppName, "Error While Open Profile. \nERROR : " + ex.Message));

            }


        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is MyProfileViewModel)
            {
                _vModel = BindingContext as MyProfileViewModel;

                _vModel.OnTabAdded += () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserTabControl.TabViews = _vModel.UserTabViews;
                        UserTabControl.TabItems = _vModel.UserTabItems;
                        if (_vModel.UserTabItems != null)
                        {
                            UserTabControl.SelectedTab = _vModel.UserTabItems[0];
                            if (UserTabControl.SelectedTabIndexChangedCallback == null)
                                UserTabControl.SelectedTabIndexChangedCallback = _vModel.OnUserTabIndexChanged;
                        }
                        predictionTabControl.TabViews = _vModel.PredictionViews;
                        predictionTabControl.TabItems = _vModel.PredictionItems;
                        if (_vModel.PredictionItems != null)
                        {
                            predictionTabControl.SelectedTab = _vModel.PredictionItems[0];
                            predictionTabControl.SelectedTabIndexChangedCallback = _vModel.OnPredictionTabIndexChanged;
                        }
                        if (_vModel.IsOtherProfile)
                        {
                            HeaderControl.BackButtonVisible = true;
                            HeaderControl.Title = AppResources.UserDetails;
                            HeaderControl.LogoVisible = false;
                            HeaderControl.WalletVisible = false;
                            HeaderControl.NotificationVisible = false;
                            HeaderControl.TitleVisible = true;
                          
                        }
                        else
                        {
                            HeaderControl.BackButtonVisible = false;
                            HeaderControl.LogoVisible = true;
                            HeaderControl.WalletVisible = false;
                            HeaderControl.NotificationVisible = true;
                            
                        }

                    });

                };
                _vModel.OnTabRefresh += (int TabIndex) =>
                {
                    UserTabControl.TabViews = _vModel.UserTabViews;
                    UserTabControl.TabItems = _vModel.UserTabItems;
                    if (_vModel.UserTabItems != null)
                    {
                        UserTabControl.SelectedTab = _vModel.UserTabItems[TabIndex];
                        if (UserTabControl.SelectedTabIndexChangedCallback == null)
                            UserTabControl.SelectedTabIndexChangedCallback = _vModel.OnUserTabIndexChanged;
                    }

                    predictionTabControl.TabViews = _vModel.PredictionViews;
                    predictionTabControl.TabItems = _vModel.PredictionItems;
                    if (_vModel.PredictionItems != null)
                    {
                        predictionTabControl.SelectedTab = _vModel.PredictionItems[0];
                        predictionTabControl.SelectedTabIndexChangedCallback = _vModel.OnPredictionTabIndexChanged;
                    }
                };
                _vModel.OnFilterApplied += () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserTabControl.TabViews = _vModel.UserTabViews;
                        if (_vModel.UserTabItems != null)
                            UserTabControl.SelectedTab = _vModel.UserTabItems[1];

                       
                    });

                };
                _vModel.TabSelection += (int tabIndex) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        if (_vModel.UserTabItems != null)
                            UserTabControl.SelectedTab = _vModel.UserTabItems[tabIndex];

                        
                    });

                };
                _vModel.AboutMeUpdate += (bool isVisible) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (isVisible)
                        {
                            mainGrid.RowDefinitions[0].Height = GridLength.Auto;
                        }
                        else
                        {
                            mainGrid.RowDefinitions[0].Height = 0;
                        }
                    });

                };
               
            }
        }
        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (!_vModel.IsOtherProfile)
                {
                    Device.BeginInvokeOnMainThread(async () => await ShowAppExitMessage());
                   
                }
                else
                {
                    return base.OnBackButtonPressed();
                }

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }

            return true;
        }
        public async Task ShowAppExitMessage()
        {
            var res = await AppNavigationService.GetInstance().ShowConfirmAlertAsync(AppResources.AppName, AppResources.AppExitMessage);
            if (res)
            {
                await Task.Delay(500);
                DependencyService.Get<IDeviceHelper>().QuitApp();
            }
        }

        void ScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            if (!(sender is ScrollView scrollView))
                return;

            var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace > e.ScrollY)
                return;
            if (_vModel.ScrollEnd)
                return;
            _vModel.ScrollEnd = true;
            Task.Run(async () => await _vModel.LoadMore());


        }
    }
}
