using System;
using System.Collections.Generic;
using System.Diagnostics;
using Tail.Common;
using Tail.Services.Interfaces;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Views
{
    public partial class FrequentlyAskedQuestions : AppPageBase
    {
        readonly FrequentlyAskedQuestionsViewModel _vModel;
        public FrequentlyAskedQuestions()
        {
            InitializeComponent();
            _vModel = new FrequentlyAskedQuestionsViewModel();
            BindingContext = _vModel;
            _vModel.IsBusy = true;
            if (string.Compare(Device.RuntimePlatform, Device.iOS) == 0)
            {
               
                DeviceModel deviceModel = DependencyService.Get<IDeviceHelper>().GetDeviceModel();
                if (deviceModel == DeviceModel.iPhoneX || deviceModel == DeviceModel.iPhoneXR || deviceModel == DeviceModel.iPhoneXSMax)
                {
                    Maingrid.RowDefinitions[0].Height = 108;

                }
            }
          
            FaqWebViewiOS.Navigated += (sender, e) =>
            {
                _vModel.IsBusy = false;
            };
            FaqWebView.Navigated += (sender, e) =>
            {
                _vModel.IsBusy = false;
            };
        }
       
    }
}
