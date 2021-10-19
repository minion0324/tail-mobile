using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Helper;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class SelectInterestViewModel : PageViewModelBase
    {
        #region Private members
        private ObservableCollection<FilterSportsDetails> _interests;
        Command _sportItemTapCommand;
        Command _nextCommand;
        bool _isBackVisible;
        #endregion

        #region Public members
        public ObservableCollection<FilterSportsDetails> Interests
        {
            get => _interests;
            set => SetProperty(ref _interests, value);
        }
        public bool IsBackVisible
        {
            get => _isBackVisible;
            set => SetProperty(ref _isBackVisible, value);
        }
        #endregion
        public SelectInterestViewModel()
        {

            IsBackVisible = CommonSingletonUtility.SharedInstance.IsFromMenu;
            Task.Run(async () => await GetInterestsAsync());
        }   
        public async Task GetInterestsAsync()
        {
            IsBusy = true;

            var Response = await TailDataServiceProvider.Instance.GetInterestList();
            if (Response.ResponseData != null)
            {
                Interests = new ObservableCollection<FilterSportsDetails>();
                foreach (var item in Response.ResponseData)
                {
                    FilterSportsDetails _sportInfo = new FilterSportsDetails();
                    _sportInfo.SportID = item.MainSportId;
                    _sportInfo.SportType = (SportType)item.MainSportId;
                    _sportInfo.SportName = item.MsName;
                    _sportInfo.SportImage = TailUtils.GameTypeToImage_Interests((SportType)item.MainSportId);
                    _sportInfo.IsSelected = false;
                    _sportInfo.CheckboxImage = Constants.CHECKBOX_DEFAULT;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Interests.Add(_sportInfo);
                       
                        
                    });
                   
                }
                await GetUserInterestList();
                IsBusy = false;
            }
        }
        public async Task GetUserInterestList()
        {
            var Response = await TailDataServiceProvider.Instance.GetUserInterestList();
            if (Response.ResponseData != null && Response.ResponseData.Count > 0)
            {
                foreach (int id in Response.ResponseData)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (Interests != null)
                        {
                            var item = Interests.FirstOrDefault(x => x.SportID == id);
                            if (item != null)
                                item.IsSelected = !item.IsSelected;
                        }
                    });
                  
                }
            }
        }
        public Command SportItemTapCommand => _sportItemTapCommand ?? (_sportItemTapCommand = new Command((item) =>
       {
           var sportItem = (FilterSportsDetails)item;
           Handle_SportItemTapCommand(sportItem);
       }));
        public Command NextCommad => _nextCommand ?? (_nextCommand = new Command(async () => await Handle_NextCommand()));

        void Handle_SportItemTapCommand(FilterSportsDetails interestsDetails)
        {
            var item = Interests.FirstOrDefault(x => x.SportName == interestsDetails.SportName);
            if (item != null)
                item.IsSelected = !item.IsSelected;
        }
        async Task Handle_NextCommand()
        {
            if (IsBusy)
                return;
              IsBusy = true;
            var item = Interests.FirstOrDefault(x => x.IsSelected);
            if (item != null)
            {
                if (await UpdateIntrest())
                {
                    SettingsService.Instance.IsCompletedSignUpProcess = true;
                    await NavigationService.NavigateToAsync<FollowRecommended>();
                }
            }
            else
            {
                await ShowAlert(AppResources.AppName, AppResources.SelectInterestAlertMessage);
            }

            IsBusy = false;
        }
       
        public override async void Handle_BackCommand()
        {

            if (CommonSingletonUtility.SharedInstance.IsFromMenu)
            {
                if (Interests != null && Interests.Count > 0)
                {
                    var item = Interests.FirstOrDefault(x => x.IsSelected);
                    if (item == null)
                    {
                        try
                        {
                           
                                await ShowAlert(AppResources.AppName, AppResources.SelectInterestAlertMessage);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        await  UpdateIntrest();

                        base.Handle_BackCommand();
                    }
                }
                else
                {
                    base.Handle_BackCommand();
                }
            }
            else
            {
                base.Handle_BackCommand();
            }

        }

           public  async Task<bool> UpdateIntrest()
        {
            bool hasSuccessResponse = false;

            try
            {
                UpdateInterestListRequestInfo updateInterest = new UpdateInterestListRequestInfo();
                List<int> updatedSpotrsIds = new List<int>();
                foreach (var interest in Interests)
                {
                    if (interest.IsSelected)
                        updatedSpotrsIds.Add(interest.SportID);
                }
                updateInterest.sportsId = updatedSpotrsIds;
                var Response = await TailDataServiceProvider.Instance.UpdateInterestList(updateInterest);
                if (Response.ErrorCode == 200)
                {
                    hasSuccessResponse = true;
                   
                }
                else
                {
                   await ShowAlert(AppResources.AppName, Response.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;

        }
        

    }
}
