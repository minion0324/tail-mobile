using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.ApplicationServices;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class SelectInterest : AppPageBase
    {
        readonly SelectInterestViewModel _vModel;
        public SelectInterest()
        {
            try
            {
                if (Application.Current.MainPage != null)
                    Application.Current.MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);
                InitializeComponent();
                _vModel = new SelectInterestViewModel();
                BindingContext = _vModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
         
        }
        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (CommonSingletonUtility.SharedInstance.IsFromMenu && _vModel.Interests != null && _vModel.Interests.Count > 0)
                {

                        var item = _vModel.Interests.FirstOrDefault(x => x.IsSelected);
                        if (item == null)
                        {
                            try
                            {

                                Device.BeginInvokeOnMainThread(async() =>await DisplayAlert(AppResources.AppName, AppResources.SelectInterestAlertMessage, "OK"));
                                return true;
                               
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                           Task.Run (async () => await _vModel.UpdateIntrest()).Wait();

                            return base.OnBackButtonPressed();
                        }
                }


                }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await DisplayAlert(AppResources.AppName, ex.Message, "OK"));
            }
            return true;
           
        }
        public async Task ShowAlertMessage()
        {
            await DisplayAlert(AppResources.AppName, AppResources.SelectInterestAlertMessage, "OK");
           
        }
    }
}
