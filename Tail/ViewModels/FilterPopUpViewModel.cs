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
    public class FilterPopUpViewModel : PageViewModelBase
    {
        #region private members
        Command _doneCommand;
        ObservableCollection<FilterSportsMain> _sportsListList;
        List<SportType> _selectedTypes;
        #endregion
        #region Public members
        public ObservableCollection<FilterSportsMain> SportsListList
        {
            get => _sportsListList;
            set => SetProperty(ref _sportsListList, value);
        }
        public List<SportType> SelectedTypes
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
        Action<List<SportType>> _popupCloseCallback;
        public Action<List<SportType>> PopupCloseCallback
        {
            get => _popupCloseCallback;
            set => SetProperty(ref _popupCloseCallback, value);
        }
        #endregion




        public FilterPopUpViewModel()
        {
            SportsListList = new ObservableCollection<FilterSportsMain>();
            SelectedTypes = new List<SportType>();
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
                    SelectedCommand = new Command<FilterSportsDetails>( (item) =>  Handle_SelectedCommand(item)),

                }); 

            }
            FilterSportsDetails _sportInfo1 = new FilterSportsDetails();
            _sportInfo1.SportID = 0;
            _sportInfo1.SportType = SportType.None;
            _sportInfo1.SportName = "Select All";
            _sportInfo1.SportImage = "select_all";
            _sportInfo1.IsSelected = false;
            _sportInfo1.CheckboxImage = Constants.CHECKBOX_DEFAULT;
            SportsListList.Add(new FilterSportsMain
            {
                SportsInfo = _sportInfo1,
                SelectedCommand = new Command<FilterSportsDetails>( (item) =>  Handle_SelectedCommand(item)),


            });
            OnSportFilterUpdated?.Invoke();
        }
        private async Task Handle_DoneCommand()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            for (int i = 0; i < SportsListList.Count-1; i++)
            {
                if (SportsListList[i].SportsInfo.IsSelected)
                {
                    SelectedTypes.Add(SportsListList[i].SportsInfo.SportType);
                }
            }
            
            PopupCloseCallback.Invoke(SelectedTypes);
            IsBusy = false;
            await PopupNavigation.Instance.PopAsync();
        }
        private  void Handle_SelectedCommand(FilterSportsDetails FilterItem)
        {
            if (FilterItem.SportID == 0)
            {
                foreach (var item in SportsListList)
                {
                    item.SportsInfo.IsSelected = !FilterItem.IsSelected;
                    item.SportsInfo.CheckboxImage = (item.SportsInfo.IsSelected) ? Constants.CHECKBOX_SELECTED : Constants.CHECKBOX_DEFAULT;
                }
            }
            else
            {
                FilterItem.IsSelected = !FilterItem.IsSelected;
                FilterItem.CheckboxImage = (FilterItem.IsSelected) ? Constants.CHECKBOX_SELECTED : Constants.CHECKBOX_DEFAULT;
                var _sportTypeArray = Enum.GetValues(typeof(SportType)).Cast<SportType>();
                if (SportsListList[_sportTypeArray.Count()-1].SportsInfo.IsSelected)
                {
                    SportsListList[_sportTypeArray.Count()-1].SportsInfo.CheckboxImage = Constants.CHECKBOX_DEFAULT;
                    SportsListList[_sportTypeArray.Count()-1].SportsInfo.IsSelected = false;
                }
                else
                {
                    var recordCount = SportsListList.Count(a => a.SportsInfo.IsSelected);
                    if (recordCount == 5)
                    {
                        SportsListList[_sportTypeArray.Count() - 1].SportsInfo.CheckboxImage = Constants.CHECKBOX_SELECTED;
                        SportsListList[_sportTypeArray.Count() - 1].SportsInfo.IsSelected = true;
                    }
                }  

            }
         
           
        }
    }
}
