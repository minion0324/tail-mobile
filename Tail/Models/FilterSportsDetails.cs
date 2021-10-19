using System.Collections.Generic;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Models
{
    public class FilterSportsMain : BaseModel
    {

        FilterSportsDetails _sportsInfo;
        public FilterSportsDetails SportsInfo
        {
            get => _sportsInfo;
            set => SetProperty(ref _sportsInfo, value);
        }
      

        Command<FilterSportsDetails> _selectedCommand;
        public Command<FilterSportsDetails> SelectedCommand
        {
            get => _selectedCommand;
            set => SetProperty(ref _selectedCommand, value);
        }

    }
    public class PickFilterMain : BaseModel
    {

     
        PickTypeDetails _pickInfo;
        public PickTypeDetails PickInfo
        {
            get => _pickInfo;
            set => SetProperty(ref _pickInfo, value);
        }

        Command<PickTypeDetails> _selectedCommand;
        public Command<PickTypeDetails> SelectedCommand
        {
            get => _selectedCommand;
            set => SetProperty(ref _selectedCommand, value);
        }

    }

    public class FilterSportsDetails : BaseModel
    {

        int _sportID;
        public int SportID
        {
            get => _sportID;
            set => SetProperty(ref _sportID, value);
        }
        SportType _sportType;
        public SportType SportType
        {
            get => _sportType;
            set => SetProperty(ref _sportType, value);
        }
        string _sportName;
        public string SportName
        {
            get => _sportName;
            set => SetProperty(ref _sportName, value);
        }
        string _sportImage;
        public string SportImage
        {
            get => _sportImage;
            set => SetProperty(ref _sportImage, value);
        }
        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        string _checkboxImage;
        public string CheckboxImage
        {
            get => _checkboxImage;
            set => SetProperty(ref _checkboxImage, value);
        }
    }


    public class PickTypeDetails : BaseModel
    {

        int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        PickPurchaseType _pick_Type;
        public PickPurchaseType Pick_Type
        {
            get => _pick_Type;
            set => SetProperty(ref _pick_Type, value);
        }
        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        string _checkboxImage;
        public string CheckboxImage
        {
            get => _checkboxImage;
            set => SetProperty(ref _checkboxImage, value);
        }



    }
    public class FilterReturnAgrument : BaseModel
    {


        List<SportType> _selectedSportInfo;
        public List<SportType> SelectedSportInfo
        {
            get => _selectedSportInfo;
            set => SetProperty(ref _selectedSportInfo, value);
        }

        List<PickPurchaseType> _selectedPurchaseInfo;
        public List<PickPurchaseType> SelectedPurchaseInfo
        {
            get => _selectedPurchaseInfo;
            set => SetProperty(ref _selectedPurchaseInfo, value);
        }

    }
}
