using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class ViewAllFilterPopUpViewModel : PageViewModelBase
    {
        #region private members
        Command _doneCommand;
        ObservableCollection<FilterSportsMain> _sportsListList;
        ObservableCollection<PickFilterMain> _pickTypeList;
        FilterReturnAgrument _selectedTypes;
        #endregion
        #region Public members
        public ObservableCollection<FilterSportsMain> SportsListList
        {
            get => _sportsListList;
            set => SetProperty(ref _sportsListList, value);
        }
        public ObservableCollection<PickFilterMain> PickTypeList
        {
            get => _pickTypeList;
            set => SetProperty(ref _pickTypeList, value);
        }
        public FilterReturnAgrument SelectedTypes
        {
            get => _selectedTypes;
            set => SetProperty(ref _selectedTypes, value);
        }

        public Command DoneCommand => _doneCommand ?? (_doneCommand = new Command(async () => await Handle_DoneCommand()));

        public Action OnSportFilterUpdated
        {
            get;
            set;
        }
        public Action OnPickTypeFilterUpdated
        {
            get;
            set;
        }
        public Action<FilterReturnAgrument> PopupCloseCallback
        {
            get;
            set;
        }
     

        #endregion
        public ViewAllFilterPopUpViewModel()
        {
            SportsListList = new ObservableCollection<FilterSportsMain>();
            PickTypeList = new ObservableCollection<PickFilterMain>();
            SelectedTypes = new FilterReturnAgrument();
            var _sportTypeArray = Enum.GetValues(typeof(SportType)).Cast<SportType>();
            for (int i = 1; i < _sportTypeArray.Count(); i++)
            {

                FilterSportsDetails _sportInfo = new FilterSportsDetails();

                _sportInfo.SportID = i;
                _sportInfo.SportType = (SportType)i;
                _sportInfo.SportName = TailUtils.GameTypeToName((SportType)i);
                _sportInfo.SportImage = TailUtils.GameTypeToImage_Follow((SportType)i);
                _sportInfo.IsSelected = false;
                _sportInfo.CheckboxImage = Constants.CHECKBOX_DEFAULT;
                SportsListList.Add(new FilterSportsMain
                {
                    SportsInfo = _sportInfo,
                    SelectedCommand = new Command<FilterSportsDetails>((item) => Handle_SelectedCommand(item)),

                }); 

            }
            
            OnSportFilterUpdated?.Invoke();



            var _pickPurchaseArray = Enum.GetValues(typeof(PickPurchaseType)).Cast<PickPurchaseType>();
            for (int i = 1; i < _pickPurchaseArray.Count(); i++)
            {

                PickTypeDetails _pickInfo = new PickTypeDetails();

                _pickInfo.Id = i;
                _pickInfo.Pick_Type = (PickPurchaseType)i;
                _pickInfo.Name = TailUtils.PurchaseTypeToName((PickPurchaseType)i);
                _pickInfo.IsSelected = false;
                _pickInfo.CheckboxImage = Constants.CHECKBOX_DEFAULT;
                PickTypeList.Add(new PickFilterMain
                {
                    PickInfo = _pickInfo,
                    SelectedCommand = new Command<PickTypeDetails>((item) => Handle_PickSelectedCommand(item)),

                }); 

            }
            OnPickTypeFilterUpdated?.Invoke();
        }

        private async Task Handle_DoneCommand()
        {
            SelectedTypes.SelectedSportInfo = new List<SportType>();
            SelectedTypes.SelectedPurchaseInfo = new List<PickPurchaseType>();
            for (int i = 0; i < SportsListList.Count; i++)
            {
                if (SportsListList[i].SportsInfo.IsSelected)
                {
                    SelectedTypes.SelectedSportInfo.Add(SportsListList[i].SportsInfo.SportType);
                }
            }
            for (int i = 0; i < PickTypeList.Count; i++)
            {
                if (PickTypeList[i].PickInfo.IsSelected)
                {
                    SelectedTypes.SelectedPurchaseInfo.Add(PickTypeList[i].PickInfo.Pick_Type);
                }
            }
            PopupCloseCallback.Invoke(SelectedTypes);
            await PopupNavigation.Instance.PopAsync();
        }
        private void Handle_SelectedCommand(FilterSportsDetails FilterItem)
        {
           
            FilterItem.IsSelected = !FilterItem.IsSelected;
            FilterItem.CheckboxImage = (FilterItem.IsSelected) ? Constants.CHECKBOX_SELECTED : Constants.CHECKBOX_DEFAULT;

        }
        private void Handle_PickSelectedCommand(PickTypeDetails FilterItem)
        {

            FilterItem.IsSelected = !FilterItem.IsSelected;
            FilterItem.CheckboxImage = (FilterItem.IsSelected) ? Constants.CHECKBOX_SELECTED : Constants.CHECKBOX_DEFAULT;

        }
    }
}
