using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Tail.Common;
using Tail.Models;
using Tail.Services.ApplicationServices;
using Tail.Services.Interfaces;
using Tail.Services.ServiceProviders;
using Tail.Views;
using Xamarin.Forms;

namespace Tail.ViewModels
{
    public class PostYourPickModifiedViewModel : PageViewModelBase
    {
        string _postContent;
        int _sportSelectedIndex;
        int _leagueSelectedIndex;
        int _betTypeSelectedIndex;
        int _setBetTypeSelectedIndex;
        int _currentProgressCount = 1;
        int _WageringUnitValue=1;
        decimal _progressPercentage = 0;
        string _progressCountDisplay;
        string _progressPercentageDisplay;
        bool _isNormalIndicator;
        ObservableCollection<MediaFile> _media;
        ObservableCollection<StepsDetails> _stepsDataList;
        Command _post;
        Command _imageTapCommand;
        Command _discardCommand;
        Command _selectGameCommand;
        Command _selectSportCommand;
        Command _selectLeagueCommand;
        Command _selectBetOptionCommand;
        IList<PickerItem> _sportOptions;
        List<PickerItem> _leagueOptions;
        List<PickerItem> _betOptions;
        List<NewPickerItem> _newSportOptions;
        List<NewPickerItem> _newLeagueOptions;
        List<NewPickerItem> _newBetOptions;
        List<LeagueDetails> _leagueList;
        bool _isGameSelectionEnabled;
        public bool IsGameSelectionEnabled
        {
            get => _isGameSelectionEnabled;
            set => SetProperty(ref _isGameSelectionEnabled, value);
        }
        bool _isMoneyLineVisible;
        public bool IsMoneyLineVisible
        {
            get => _isMoneyLineVisible;
            set => SetProperty(ref _isMoneyLineVisible, value);
        }
        bool _isSpreadVisible;
        public bool IsSpreadVisible
        {
            get => _isSpreadVisible;
            set => SetProperty(ref _isSpreadVisible, value);
        }
        bool _isOverUnderVisible;
        public bool IsOverUnderLineVisible
        {
            get => _isOverUnderVisible;
            set => SetProperty(ref _isOverUnderVisible, value);
        }
        bool _isGameSelected;
        public bool IsGameSelected
        {
            get => _isGameSelected;
            set => SetProperty(ref _isGameSelected, value);
        }
        bool _isMoneyline1Selected;
        public bool IsMoneyline1Selected
        {
            get => _isMoneyline1Selected;
            set => SetProperty(ref _isMoneyline1Selected, value);
        }
        bool _isMoneyline2Selected;
        public bool IsMoneyline2Selected
        {
            get => _isMoneyline2Selected;
            set => SetProperty(ref _isMoneyline2Selected, value);
        }
        bool _isSpread1Selected;
        public bool IsSpread1Selected
        {
            get => _isSpread1Selected;
            set => SetProperty(ref _isSpread1Selected, value);
        }
        bool _isSpread2Selected;
        public bool IsSpread2Selected
        {
            get => _isSpread2Selected;
            set => SetProperty(ref _isSpread2Selected, value);
        }
        bool _isOverUnder1Selected;
        public bool IsOverUnder1Selected
        {
            get => _isOverUnder1Selected;
            set => SetProperty(ref _isOverUnder1Selected, value);
        }
        bool _leagueEnable=false;
        public bool LeagueEnable
        {
            get => _leagueEnable;
            set => SetProperty(ref _leagueEnable, value);
        }
        bool _isOverUnder2Selected;
        public bool IsOverUnder2Selected
        {
            get => _isOverUnder2Selected;
            set => SetProperty(ref _isOverUnder2Selected, value);
        }
        bool _hideCaption;
        public bool HideCaption
        {
            get => _hideCaption;
            set => SetProperty(ref _hideCaption, value);
        }
        bool _isHideCaptionEnabled;
        public bool IsHideCaptionEnabled
        {
            get => _isHideCaptionEnabled;
            set => SetProperty(ref _isHideCaptionEnabled, value);
        }
        string _selectGameText;
        public string SelectGameText
        {
            get => _selectGameText;
            set => SetProperty(ref _selectGameText, value);
        }
        string _plusText;
        public string PlusText
        {
            get => _plusText;
            set => SetProperty(ref _plusText, value);
        }

        public int CurrentProgressCount
        {
            get => _currentProgressCount;
            set => SetProperty(ref _currentProgressCount, value);
        }
        public decimal ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }
        public string ProgressCountDisplay
        {
            get => _progressCountDisplay;
            set => SetProperty(ref _progressCountDisplay, value);
        }
        public string ProgressPercentageDisplay
        {
            get => _progressPercentageDisplay;
            set => SetProperty(ref _progressPercentageDisplay, value);
        }
        public bool IsNormalIndicator
        {
            get => _isNormalIndicator;
            set => SetProperty(ref _isNormalIndicator, value);
        }
        public int SportSelectedIndex
        {
            get => _sportSelectedIndex;
            set => SetProperty(ref _sportSelectedIndex, value);
        }
        public int LeagueSelectedIndex
        {
            get => _leagueSelectedIndex;
            set => SetProperty(ref _leagueSelectedIndex, value);
        }
        public int BetTypeSelectedIndex
        {
            get => _betTypeSelectedIndex;
            set => SetProperty(ref _betTypeSelectedIndex, value);
        }
        public int SetBetTypeSelectedIndex
        {
            get => _setBetTypeSelectedIndex;
            set => SetProperty(ref _setBetTypeSelectedIndex, value);
        }
        public int WageringUnitValue
        {
            get => _WageringUnitValue;
            set => SetProperty(ref _WageringUnitValue, value);
        }
        public IList<PickerItem> SportOptions
        {
            get => _sportOptions;
            set => SetProperty(ref _sportOptions, value);
        }
        public List<PickerItem> LeagueOptions
        {
            get => _leagueOptions;
            set => SetProperty(ref _leagueOptions, value);
        }
        public List<PickerItem> BetOptions
        {
            get => _betOptions;
            set => SetProperty(ref _betOptions, value);
        }
        public List<NewPickerItem> NewSportOptions
        {
            get => _newSportOptions;
            set => SetProperty(ref _newSportOptions, value);
        }
        public List<NewPickerItem> NewLeagueOptions
        {
            get => _newLeagueOptions;
            set => SetProperty(ref _newLeagueOptions, value);
        }
        public List<NewPickerItem> NewBetOptions
        {
            get => _newBetOptions;
            set => SetProperty(ref _newBetOptions, value);
        }
        public ObservableCollection<MediaFile> Media
        {
            get => _media;
            set => SetProperty(ref _media, value);
        }
        public ObservableCollection<StepsDetails> StepsDataList
        {
            get => _stepsDataList;
            set => SetProperty(ref _stepsDataList, value);
        }
        public string PostContent
        {
            get => _postContent;
            set => SetProperty(ref _postContent, value);
        }
        string _selectedSport;
        public string SelectedSport
        {
            get => _selectedSport;
            set => SetProperty(ref _selectedSport, value);
        }
        string _selectedLeague;
        public string SelectedLeague
        {
            get => _selectedLeague;
            set => SetProperty(ref _selectedLeague, value);
        }
        string _selectedBet;
        public string SelectedBet
        {
            get => _selectedBet;
            set => SetProperty(ref _selectedBet, value);
        }
        public List<LeagueDetails> LeagueList
        {
            get => _leagueList;
            set => SetProperty(ref _leagueList, value);
        }
        public Action OnSpotIndexChanged
        {
            get;
            set;
        }
        public Action OnLeagueIndexChanged
        {
            get;
            set;
        }
        public Action OnBetTypeIndexChanged
        {
            get;
            set;
        }
        Command<string> _monyLine1SelectedCommand;
        public Command<string> MonyLine1SelectedCommand
        {
            get => _monyLine1SelectedCommand;
            set => SetProperty(ref _monyLine1SelectedCommand, value);
        }
        Command<string> _monyLine2SelectedCommand;
        public Command<string> MonyLine2SelectedCommand
        {
            get => _monyLine2SelectedCommand;
            set => SetProperty(ref _monyLine2SelectedCommand, value);
        }
        Command<string> _spread1SelectedCommand;
        public Command<string> Spread1SelectedCommand
        {
            get => _spread1SelectedCommand;
            set => SetProperty(ref _spread1SelectedCommand, value);
        }
        Command<string> _spread2SelectedCommand;
        public Command<string> Spread2SelectedCommand
        {
            get => _spread2SelectedCommand;
            set => SetProperty(ref _spread2SelectedCommand, value);
        }

        Command<string> _overUnder1SelectedCommand;
        public Command<string> OverUnder1SelectedCommand
        {
            get => _overUnder1SelectedCommand;
            set => SetProperty(ref _overUnder1SelectedCommand, value);
        }
        Command<string> _overUnder2SelectedCommand;
        public Command<string> OverUnder2SelectedCommand
        {
            get => _overUnder2SelectedCommand;
            set => SetProperty(ref _overUnder2SelectedCommand, value);
        }

        public PostYourPickModifiedViewModel()
        {
            Media = new ObservableCollection<MediaFile>();

            OnSpotIndexChanged += SportDropDown_Changed;
            OnLeagueIndexChanged += LeagueDropdown_Change;
            OnBetTypeIndexChanged += BetTypeDropdown_Change;
            MonyLine1SelectedCommand = new Command<string>((item) => Handle_MoneyLine1Selection(item));
            MonyLine2SelectedCommand = new Command<string>((item) => Handle_MoneyLine2Selection(item));

            Spread1SelectedCommand = new Command<string>((item) => Handle_Spread1Selection(item));
            Spread2SelectedCommand = new Command<string>((item) => Handle_Spread2Selection(item));

            OverUnder1SelectedCommand = new Command<string>((item) => Handle_OverUnder1Selection(item));
            OverUnder2SelectedCommand = new Command<string>((item) => Handle_OverUnder2Selection(item));
            BetOptions = new List<PickerItem> { new PickerItem { ItemName = "Moneyline" }, new PickerItem { ItemName = "Spread" }, new PickerItem { ItemName = "Over/Under" } };
            NewBetOptions = new List<NewPickerItem> { new NewPickerItem { ItemName = "Moneyline" }, new NewPickerItem { ItemName = "Spread" }, new NewPickerItem { ItemName = "Over/Under" } };
            NewSportOptions = new List<NewPickerItem>();
            NewLeagueOptions = new List<NewPickerItem>();
            DependencyService.Get<IAwsBucketService>().OnProgress += (s, e) =>
            {
                UploadProgressArgs evArgs = (UploadProgressArgs)e;
                TransferUtilityUploadRequest uploadObj = (TransferUtilityUploadRequest)s;
                var _existingItem = Media.FirstOrDefault(p => p.Path == uploadObj.FilePath);
                if (_existingItem != null)
                {
                    int indexValue = Media.IndexOf(_existingItem);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Media[indexValue].Progress = Math.Round(Decimal.Divide(evArgs.TransferredBytes, evArgs.TotalBytes) * 100, 2);
                    });

                }
                decimal _currentProgress = Media.Sum(item => item.Progress);
                decimal _totalProgress = Media.Count * 100;
                ProgressPercentage = Math.Round(Decimal.Divide(_currentProgress, _totalProgress) * 100, 2);
                ProgressPercentageDisplay = ProgressPercentage + "%";
            };
            Task.Run(async () => await GetGameDetails());
            PlusText = "+";
            SelectGameText = "Select the game";
            SelectedSport = AppResources.SelectYourSpot;
            SelectedLeague = AppResources.SelectYourLeague;
        }

        private void Handle_OverUnder2Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.OverUnderSelection = 2;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            IsOverUnder2Selected = true;
            IsOverUnder1Selected = false;
            IsSpread1Selected = false;
            IsSpread2Selected = false;
            IsMoneyline1Selected = false;
            IsMoneyline2Selected = false;
        }

        private void Handle_OverUnder1Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.OverUnderSelection = 1;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            IsOverUnder2Selected = false;
            IsOverUnder1Selected = true;
            IsSpread1Selected = false;
            IsSpread2Selected = false;
            IsMoneyline1Selected = false;
            IsMoneyline2Selected = false;
        }

        private void Handle_Spread2Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.SpreadSelection = 2;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;
            IsOverUnder2Selected = false;
            IsOverUnder1Selected = false;
            IsSpread1Selected = false;
            IsSpread2Selected = true;
            IsMoneyline1Selected = false;
            IsMoneyline2Selected = false;
        }

        private void Handle_Spread1Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.SpreadSelection = 1;
            StepsDataList[1].BettingDetails.MoneyLineSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;
            IsOverUnder2Selected = false;
            IsOverUnder1Selected = false;
            IsSpread1Selected = true;
            IsSpread2Selected = false;
            IsMoneyline1Selected = false;
            IsMoneyline2Selected = false;
        }

        private void Handle_MoneyLine2Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.MoneyLineSelection = 2;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;
            IsOverUnder2Selected = false;
            IsOverUnder1Selected = false;
            IsSpread1Selected = false;
            IsSpread2Selected = false;
            IsMoneyline1Selected = false;
            IsMoneyline2Selected = true;
        }

        private void Handle_MoneyLine1Selection(string item)
        {
            Debug.WriteLine(item);
            StepsDataList[1].BettingDetails.MoneyLineSelection = 1;
            StepsDataList[1].BettingDetails.SpreadSelection = 0;
            StepsDataList[1].BettingDetails.OverUnderSelection = 0;
            IsOverUnder2Selected = true;
            IsOverUnder1Selected = false;
            IsSpread1Selected = false;
            IsSpread2Selected = false;
            IsMoneyline1Selected = true;
            IsMoneyline2Selected = false;
        }

        private void BetTypeDropdown_Change()
        {
            StepsDataList[1].SelectedTabIndex = BetTypeSelectedIndex + 1;
            if (BetTypeSelectedIndex == 0)
            {
                IsMoneyLineVisible = true;
                IsOverUnderLineVisible = false;
                IsSpreadVisible = false;
            }
            else if (BetTypeSelectedIndex == 1)
            {
                IsMoneyLineVisible = false;
                IsOverUnderLineVisible = false;
                IsSpreadVisible = true;
            }
            else if (BetTypeSelectedIndex == 2)
            {
                IsMoneyLineVisible = false;
                IsOverUnderLineVisible = true;
                IsSpreadVisible = false;
            }
        }

        private async Task GetGameDetails()
        {
            try
            {

                if (IsBusy)
                    return;
                IsBusy = true;
                var dateSpotsResponse = await TailDataServiceProvider.Instance.GetGameDetails();
                if (dateSpotsResponse.ErrorCode == 0 && dateSpotsResponse.ResponseData != null)
                {
                    StepsDataList = new ObservableCollection<StepsDetails>(dateSpotsResponse.ResponseData);
                    if (StepsDataList != null && StepsDataList.Count > 0)
                    {
                        StepsDataList[0].InformationText = AppResources.SelectYourSpotInfo;
                        SportOptions = StepsDataList[0].SpotOptions;
                        foreach (var item in SportOptions)
                        {
                            NewPickerItem pickerItem = new NewPickerItem();
                            pickerItem.ItemName = item.ItemName;
                            pickerItem.IsSelected = false;
                            NewSportOptions.Add(pickerItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, "Error While Get Game Details. \nERROR : " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }

        }
        ///<summary>
        ///Spot change event.
        ///</summary>
        async void SportDropDown_Changed()
        {
            IsGameSelected = false;
           
            if (StepsDataList[0] != null && StepsDataList[0].SpotSelectedIndex != -1)
            {
                LeagueEnable = false;
                StepsDataList[0].IsGameAvailable = false;
                StepsDataList[0].InformationText = AppResources.SelectYourLeagueInfo;
                int _typeID = SportSelectedIndex + 1;

                SportType _SportType = (SportType)_typeID;
                StepsDataList[1].BettingDetails.Sport_Type = _SportType;
                if (await GetLeagueDetails(_typeID))
                {
                    List<LeagueDetails> _leagueOption = LeagueList;
                    if (_leagueOption != null && _leagueOption.Count > 0)
                    {
                        StepsDataList[0].LeagueOptions = new List<PickerItem>();
                        List<PickerItem> _listPicker = new List<PickerItem>();
                        foreach (var item in _leagueOption)
                        {
                            _listPicker.Add(new PickerItem
                            {
                                ItemName = item.LeagueName
                            });

                        }
                        StepsDataList[0].LeagueOptions = new List<PickerItem>(_listPicker);
                        LeagueOptions = new List<PickerItem>(_listPicker);
                        LeagueEnable = true;

                        foreach (var item in LeagueOptions)
                        {
                            NewPickerItem pickerItem = new NewPickerItem();
                            pickerItem.ItemName = item.ItemName;
                            pickerItem.IsSelected = false;
                            NewLeagueOptions.Add(pickerItem);
                        }
                    }
                }
                else
                {
                    StepsDataList[0].LeagueOptions = new List<PickerItem>();
                    LeagueOptions = new List<PickerItem>();
                }
            }
        }
        ///<summary>
        ///League change event.
        ///</summary>
        async void LeagueDropdown_Change()
        {
            IsGameSelected = false;
            try
            {


                if (StepsDataList[0].LeagueSelectedIndex != -1)
                {
                    SelectedLeague = NewLeagueOptions[StepsDataList[0].LeagueSelectedIndex].ItemName;
                    IsNormalIndicator = true;

                    LeagueDetails _leagueItem = LeagueList[StepsDataList[0].LeagueSelectedIndex];
                    if (_leagueItem != null)
                    {
                        StepsDataList[1].BettingDetails.SelectedEventID = _leagueItem.LeagueID;
                        StepsDataList[1].BettingDetails.SelectedEventName = _leagueItem.LeagueName;
                        StepsDataList[1].BettingDetails.SelectedEventImage = _leagueItem.LeagueImage;
                    }

                    if (_leagueItem != null && await GetGameSchedules(_leagueItem.LeagueID, "0"))
                    {
                        if (StepsDataList[0].UpcomingGames[0].GameData.Count >= 2)
                        {
                            StepsDataList[0].IsGameAvailable = true;
                            IsGameSelectionEnabled = true;
                            PlusText = "+";
                            SelectGameText = AppResources.GameSelectionWarning;
                            StepsDataList[0].InformationText = AppResources.GameSelectionWarning;
                        }
                        else if (StepsDataList[0].UpcomingGames[0].GameData.Count == 1)
                        {
                            StepsDataList[0].IsGameAvailable = true;
                            PlusText = "+";
                            SelectGameText = AppResources.GameSelectionWarning;
                            IsGameSelectionEnabled = true;
                            StepsDataList[0].InformationText = AppResources.GameSelectionWarning;
                        }
                        else if (StepsDataList[0].UpcomingGames[0].GameData.Count == 0)
                        {
                            StepsDataList[0].IsGameAvailable = false;
                            IsGameSelectionEnabled = false;
                            PlusText = "";
                            SelectGameText = AppResources.GameInfo;
                            StepsDataList[0].InformationText = AppResources.GameInfo;
                        }
                        

                        if (StepsDataList[1].BettingDetails.Sport_Type == SportType.MMA)
                        {
                            foreach (var gamesData in StepsDataList[0].UpcomingGames[0].GameData)
                            {
                                foreach (var item in gamesData.Games)
                                {
                                    if (item.HomeTeamDetails != null && item.AwayTeamDetails != null)
                                    {
                                        item.HomeTeamDetails.SportID = 7;
                                        item.AwayTeamDetails.SportID = 7;
                                    }
                                }
                            }
                        }
                        else if (StepsDataList[1].BettingDetails.Sport_Type == SportType.Basketball && StepsDataList[0].UpcomingGames[0].GameData != null && StepsDataList[0].UpcomingGames[0].GameData.Count != 0 && _leagueItem.LeagueID == 4)
                        {
                            foreach (var gamesData in StepsDataList[0].UpcomingGames[0].GameData)
                            {
                                foreach (var item in gamesData.Games)
                                {
                                    if (item.HomeTeamDetails != null && item.AwayTeamDetails != null)
                                    {
                                        item.HomeTeamDetails.SportID = 4;
                                        item.AwayTeamDetails.SportID = 4;
                                    }
                                }
                                var AwayTeamitemsToRemove = gamesData.Games.Where(x => x.AwayTeamDetails == null).ToList();
                                if (AwayTeamitemsToRemove != null)
                                {
                                    foreach (var itemToRemove in AwayTeamitemsToRemove)
                                        gamesData.Games.Remove(itemToRemove);
                                }
                                
                                var HomeTeamitemsToRemove = gamesData.Games.Where(x => x.HomeTeamDetails == null).ToList();
                                if (HomeTeamitemsToRemove != null)
                                {
                                    foreach (var itemToRemove in HomeTeamitemsToRemove)
                                        gamesData.Games.Remove(itemToRemove);
                                }


                            }
                        }

                        foreach (var gamesData in StepsDataList[0].UpcomingGames[0].GameData)
                        {
                            foreach (var item in gamesData.Games)
                            {
                                if (item.HomeTeamDetails != null && item.AwayTeamDetails != null)
                                {
                                    item.HomeTeamDetails.Sports_Type = StepsDataList[1].BettingDetails.Sport_Type;
                                    item.AwayTeamDetails.Sports_Type = StepsDataList[1].BettingDetails.Sport_Type;
                                }
                            }
                        }

                            if (IsGameSelectionEnabled)
                        {
                            await PopupNavigation.Instance.PushAsync(new SelectGamePopup(StepsDataList[0].UpcomingGames, () => Handle_SelectGamePopupClosed()));
                        }

                    }
                    else
                    {
                        StepsDataList[0].IsGameAvailable = false;
                        StepsDataList[0].InformationText = AppResources.GameInfo;
                    }

                    IsNormalIndicator = false;
                }
            }
            catch
            {
                IsNormalIndicator = false;
            }
        }
        private async Task<bool> GetGameSchedules(int LeagueId, string EventDate)
        {
            bool hasSuccessResponse = false;
            try
            {

                var gameResponse = await TailDataServiceProvider.Instance.GetGameSchedules(LeagueId, EventDate);
                if (gameResponse.ErrorCode == 200 && gameResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    StepsDataList[0].UpcomingGames = gameResponse.ResponseData;
                    List<ScheduleInfo> _tempGameData = new List<ScheduleInfo>();
                    List<GameInfo> _pendingGames = new List<GameInfo>();
                    foreach (ScheduleInfo gameInfo in StepsDataList[0].UpcomingGames[0].GameData)
                    {
                       
                        List<GameInfo> _tempGames = new List<GameInfo>();
                        if (_pendingGames.Count != 0)
                        {
                            GameInfo last = _pendingGames.Last();
                            foreach (var PendingItem in _pendingGames)
                            {
                                if (_tempGames.Count != 0)
                                {
                                    if (_tempGames[_tempGames.Count - 1].GameDate == PendingItem.GameDate)
                                    {
                                        _tempGames.Add(PendingItem);
                                    }
                                }
                                else
                                {
                                    _tempGames.Add(PendingItem);
                                }
                                if (PendingItem.Equals(last))
                                {
                                    _pendingGames = new List<GameInfo>();
                                }

                            }
                        }

                        foreach (var gameItem in gameInfo.Games)
                        {
                            if (!string.IsNullOrEmpty(gameItem.HomeTeamDetails.TeamColor) && gameItem.HomeTeamDetails.TeamColor.Substring(0, 1) != "#")
                            {
                                gameItem.HomeTeamDetails.TeamColor = "#" + gameItem.HomeTeamDetails.TeamColor;
                            }
                     
                            if (!string.IsNullOrEmpty(gameItem.AwayTeamDetails.TeamColor) && gameItem.AwayTeamDetails.TeamColor.Substring(0, 1) != "#")
                            {
                                gameItem.AwayTeamDetails.TeamColor = "#" + gameItem.AwayTeamDetails.TeamColor;
                            }
                            if (!string.IsNullOrEmpty(gameItem.HomeTeamDetails.TeamSecondaryColor) && gameItem.HomeTeamDetails.TeamSecondaryColor.Substring(0, 1) != "#")
                            {
                                gameItem.HomeTeamDetails.TeamSecondaryColor = "#" + gameItem.HomeTeamDetails.TeamSecondaryColor;
                            }
                            if (!string.IsNullOrEmpty(gameItem.AwayTeamDetails.TeamSecondaryColor) && gameItem.AwayTeamDetails.TeamSecondaryColor.Substring(0, 1) != "#")
                            {
                                gameItem.AwayTeamDetails.TeamSecondaryColor = "#" + gameItem.AwayTeamDetails.TeamSecondaryColor;
                            }

                            if (_tempGameData.Count != 0 && _tempGameData[_tempGameData.Count - 1].Games[0].GameDate == gameItem.GameDate)
                            {
                                    gameItem.SelectedCommand = new Command<string>((item) => Handle_GameSelected(item));
                                    _tempGameData[_tempGameData.Count - 1].Games.Add(gameItem);
                                    continue;
                            }
                            gameItem.SelectedCommand = new Command<string>((item) => Handle_GameSelected(item));
                            if (_tempGames.Count != 0)
                            {
                                if (_tempGames[_tempGames.Count - 1].GameDate == gameItem.GameDate)
                                {
                                    _tempGames.Add(gameItem);
                                }
                                else
                                {
                                    _pendingGames.Add(gameItem);

                                }
                            }
                            else
                            {
                                _tempGames.Add(gameItem);
                            }
                        }
                        if (_tempGames.Count > 0)
                        {
                            ScheduleInfo _schInfo = new ScheduleInfo();
                            _schInfo.EventDate = gameInfo.EventDate;
                            _schInfo.GameRefresh = gameInfo.GameRefresh;
                            _schInfo.ScrollReset = gameInfo.ScrollReset;
                            _schInfo.Games = _tempGames;
                            _tempGameData.Add(_schInfo);
                        }
                    }
                    StepsDataList[0].UpcomingGames[0].GameData = _tempGameData;

                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, gameResponse.Message);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(AppResources.AppName, ex.Message);
            }

            return hasSuccessResponse;
        }
        public void Handle_GameSelected(string GameID)
        {

            ChangeGameSelection(GameID);
        }
        ///<summary>
        ///Select one game from available list.
        ///</summary>
        void ChangeGameSelection(string GameID)
        {

            foreach (ScheduleInfo gameInfo in StepsDataList[0].UpcomingGames[0].GameData)
            {
                gameInfo.GameRefresh = false;
                var _existingItem = gameInfo.Games.FirstOrDefault(p => p.Selected);
                if (_existingItem != null)
                    _existingItem.Selected = false;
                var _updateItem = gameInfo.Games.FirstOrDefault(p => p.EventID == GameID);
                if (_updateItem != null)
                {
                    _updateItem.Selected = true;
                    StepsDataList[1].BettingDetails.SelectedGame = _updateItem;
                    BetTypeSelectedIndex = 0;
                    BetTypeDropdown_Change();
                    SelectedBet = NewBetOptions[0].ItemName;
                    var alreadySelectedItem = NewBetOptions.FirstOrDefault(i => i.IsSelected);
                    if (alreadySelectedItem != null)
                        alreadySelectedItem.IsSelected = false;
                    NewBetOptions[0].IsSelected = true;

                    IsGameSelected = true;

                }
                gameInfo.GameRefresh = true;
            }
            StepsDataList[0].InformationText = "";

            Device.BeginInvokeOnMainThread(async () =>
            {
                await PopupNavigation.Instance.PopAllAsync();
            });
        }
        ///<summary>
        ///Getting Details of available league matches.
        ///</summary>
        private async Task<bool> GetLeagueDetails(int SportID)
        {
            IsNormalIndicator = true;
            bool hasSuccessResponse = false;
            try
            {
                CommonSingletonUtility.SharedInstance.MinPrice = 0;
                CommonSingletonUtility.SharedInstance.MaxPrice = 20;
                StepsDataList[1].BettingDetails.PickPriceHint = string.Format(AppResources.PriceHintText, CommonSingletonUtility.SharedInstance.MaxPrice);
                LeagueList = new List<LeagueDetails>();
                var leagueResponse = await TailDataServiceProvider.Instance.GetLeagueDetails(SportID);
                if (leagueResponse.ErrorCode == 200 && leagueResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    LeagueList = new List<LeagueDetails>(leagueResponse.ResponseData.SportsDetails);
                    if(leagueResponse.ResponseData.Price!=null && leagueResponse.ResponseData.Price.Count != 0)
                    {
                        CommonSingletonUtility.SharedInstance.MaxPrice = leagueResponse.ResponseData.Price[0].MaxPrice;
                        CommonSingletonUtility.SharedInstance.MinPrice = leagueResponse.ResponseData.Price[0].MinPrice;
                        StepsDataList[1].BettingDetails.PickPriceHint = string.Format(AppResources.PriceHintText, CommonSingletonUtility.SharedInstance.MaxPrice);
                    }
                    
                }
                else
                {
                    await NavigationService.ShowAlertAsync(AppResources.AppName, leagueResponse.Message);
                }
            }
            catch (Exception ex)
            {
                IsNormalIndicator = false;
                await ShowAlert(AppResources.AppName, ex.Message);
            }
            IsNormalIndicator = false;
            return hasSuccessResponse;
        }
        public Command Post => _post ?? (_post = new Command(async () => await Handle_Post()));
        public Command DiscardCommand => _discardCommand ?? (_discardCommand = new Command(() => Handle_DiscardCommand()));
        public Command ImageTapCommand => _imageTapCommand ?? (_imageTapCommand = new Command(async (item) => await Handle_ImageTapCommandCommandAsync(item)));
        public Command SelectGameCommand => _selectGameCommand ?? (_selectGameCommand = new Command(async () => await Handle_SelectGameCommand()));
        public Command SelectSportCommand => _selectSportCommand ?? (_selectSportCommand = new Command(async () => await Handle_SelectSportCommand()));
        public Command SelectLeagueCommand => _selectLeagueCommand ?? (_selectLeagueCommand = new Command(async () => await Handle_SelectLeagueCommand()));
        public Command SelectBetOptionCommand => _selectBetOptionCommand ?? (_selectBetOptionCommand = new Command(async () => await Handle_SelectBetOptionCommand()));

        private async Task Handle_SelectBetOptionCommand()
        {
            if (NewBetOptions.Count > 0)
            {
                
                    var item = NewBetOptions.FirstOrDefault(i => i.ItemName == SelectedBet);
                    var previousSelectedItem = NewBetOptions.FirstOrDefault(i => i.IsSelected);
                    if (previousSelectedItem != null)
                        previousSelectedItem.IsSelected = false;
                    item.IsSelected = true;
                
                await PopupNavigation.Instance.PushAsync(new PostPickPickerPopUp(NewBetOptions, AppResources.SelectBetTypeText, () => Handle_BetSelected()));
            }
        }

        private void Handle_BetSelected()
        {
            int index = NewBetOptions.FindIndex(i => i.IsSelected);
            SelectedBet = NewBetOptions[index].ItemName;
            BetTypeSelectedIndex = index;
            BetTypeDropdown_Change();
        }

        private async Task Handle_SelectLeagueCommand()
        {
            if (NewLeagueOptions.Count > 0)
            {
                if (SelectedLeague == AppResources.SelectYourLeague)
                {
                    var item = NewLeagueOptions.FirstOrDefault(i => i.IsSelected);
                    if (item != null)
                        item.IsSelected = false;
                }
                else
                {
                    var item = NewLeagueOptions.FirstOrDefault(i => i.ItemName == SelectedLeague);
                    var previousSelectedItem = NewLeagueOptions.FirstOrDefault(i => i.IsSelected);
                    if (previousSelectedItem != null)
                        previousSelectedItem.IsSelected = false;
                    item.IsSelected = true;
                }
                await PopupNavigation.Instance.PushAsync(new PostPickPickerPopUp(NewLeagueOptions, AppResources.SelectYourLeague, () => Handle_LeagueSelected()));
            }
        }

        private void Handle_LeagueSelected()
        {
            int index = NewLeagueOptions.FindIndex(i => i.IsSelected);
            StepsDataList[0].LeagueSelectedIndex = index;
            LeagueDropdown_Change();
        }

        private async Task Handle_SelectSportCommand()
        {
            if (SelectedSport == AppResources.SelectYourSpot)
            {
                var item = NewSportOptions.FirstOrDefault(i => i.IsSelected);
                if (item != null)
                    item.IsSelected = false;
            }
            else
            {
                var item = NewSportOptions.FirstOrDefault(i => i.ItemName == SelectedSport);
                var previousSelectedItem = NewSportOptions.FirstOrDefault(i => i.IsSelected);
                if (previousSelectedItem != null)
                    previousSelectedItem.IsSelected = false;
                item.IsSelected = true;
            }
               
            await PopupNavigation.Instance.PushAsync(new PostPickPickerPopUp(NewSportOptions,AppResources.SelectYourSpot,async () => await Handle_SportSelected()));

        }
        private async Task Handle_SportSelected()
        {
            IsGameSelectionEnabled = false;
            Debug.WriteLine(NewSportOptions);

            int index = NewSportOptions.FindIndex(i => i.IsSelected);
            StepsDataList[0].SpotSelectedIndex = index;

            IsGameSelected = false;
            if (StepsDataList[0] != null && StepsDataList[0].SpotSelectedIndex != -1)
            {
                SelectedLeague = AppResources.SelectYourLeague;
                SelectedSport = NewSportOptions[index].ItemName;
                StepsDataList[0].IsGameAvailable = false;
                StepsDataList[0].InformationText = AppResources.SelectYourLeagueInfo;
                int _typeID = StepsDataList[0].SpotSelectedIndex + 1;

                SportType _SportType = (SportType)_typeID;
                StepsDataList[1].BettingDetails.Sport_Type = _SportType;
                if (await GetLeagueDetails(_typeID))
                {
                    List<LeagueDetails> _leagueOption = LeagueList;
                    if (_leagueOption != null && _leagueOption.Count > 0)
                    {
                        StepsDataList[0].LeagueOptions = new List<PickerItem>();
                        List<PickerItem> _listPicker = new List<PickerItem>();
                        foreach (var item in _leagueOption)
                        {
                            _listPicker.Add(new PickerItem
                            {
                                ItemName = item.LeagueName
                            });

                        }
                        StepsDataList[0].LeagueOptions = new List<PickerItem>(_listPicker);
                        LeagueOptions = new List<PickerItem>(_listPicker);
                        NewLeagueOptions = new List<NewPickerItem>();
                        foreach (var item in LeagueOptions)
                        {
                            NewPickerItem pickerItem = new NewPickerItem();
                            pickerItem.ItemName = item.ItemName;
                            pickerItem.IsSelected = false;
                            NewLeagueOptions.Add(pickerItem);
                        }
                    }
                }
                else
                {
                    StepsDataList[0].LeagueOptions = new List<PickerItem>();
                    LeagueOptions = new List<PickerItem>();
                }
               

            }
        }

        private void Handle_DiscardCommand()
        {
            Back.Execute(null);
           
        }

        private async Task Handle_Post()
        {
                if (IsBusy)
                    return;
                if (StepsDataList[1].BettingDetails.Sport_Type == SportType.None)
                {
                    await ShowAlert(AppResources.AppName, StepsDataList[0].InformationText);
                    IsBusy = false;
                    return;
                }
                if (StepsDataList[1].BettingDetails.SelectedEventName == null)
                {
                    await ShowAlert(AppResources.AppName, StepsDataList[0].InformationText);
                    IsBusy = false;
                    return;
                }
                if (StepsDataList[1].BettingDetails.SelectedGame == null)
                {
                    await ShowAlert(AppResources.AppName, StepsDataList[0].InformationText);
                    IsBusy = false;
                    return;
                }

                TimeSpan _timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(StepsDataList[1].BettingDetails.SelectedGame.GameDateTime));
                if (_timeSpan.Seconds > 0)
                {
                    await ShowAlert(AppResources.AppName, AppResources.GameStarted);
                    IsBusy = false;
                    return;
                }


                AddPickRequestInfo addPickRequestInfo = new AddPickRequestInfo();
                addPickRequestInfo.imageUrl = new List<ImageData>();
                addPickRequestInfo.videoUrl = new List<VideoData>();
                addPickRequestInfo.postContent = PostContent;

                addPickRequestInfo.userId = SettingsService.Instance.LoggedUserDetails.UserId;
                addPickRequestInfo.mainSportId = (int)StepsDataList[1].BettingDetails.Sport_Type;
                addPickRequestInfo.sportId = StepsDataList[1].BettingDetails.SelectedEventID;
                addPickRequestInfo.sName = StepsDataList[1].BettingDetails.SelectedEventName;
                addPickRequestInfo.eventId = StepsDataList[1].BettingDetails.SelectedGame.EventID;
                addPickRequestInfo.eventDate = StepsDataList[1].BettingDetails.SelectedGame.GameDateTime;
                addPickRequestInfo.htName = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName;
                addPickRequestInfo.htAbbr = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamNameShort;
                addPickRequestInfo.htLogo = (string.IsNullOrEmpty(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Tlogo)) ? "No Image" : StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Tlogo;
                addPickRequestInfo.atName = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;
                addPickRequestInfo.atAbbr = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamNameShort;
                addPickRequestInfo.atLogo = (string.IsNullOrEmpty(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Tlogo)) ? "No Image" : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Tlogo;
                addPickRequestInfo.pickFee = StepsDataList[1].BettingDetails.PickPrice;
                addPickRequestInfo.betType = StepsDataList[1].SelectedTabIndex;
                addPickRequestInfo.wagerUnit = WageringUnitValue;
                addPickRequestInfo.isContent = HideCaption;
                addPickRequestInfo.htHomeColorCode = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamColor;
                addPickRequestInfo.htAwayColorCode = StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamSecondaryColor;

                addPickRequestInfo.atHomeColorCode = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamColor;
                addPickRequestInfo.atAwayColorCode = StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamSecondaryColor;
            if (StepsDataList[1].SelectedTabIndex == 1)
                {
                    if (StepsDataList[1].BettingDetails.MoneyLineSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }

                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.MoneylineDisply);
                    addPickRequestInfo.aMoScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.MoneylineDisply);
                    addPickRequestInfo.hSpScore = 0;
                    addPickRequestInfo.aSpScore = 0;
                    addPickRequestInfo.oScore = 0;
                    addPickRequestInfo.uScore = 0;

                    addPickRequestInfo.hPtSpMoney = 0;
                    addPickRequestInfo.aPtSpMoney = 0;
                    addPickRequestInfo.tlOvMoney = 0;
                    addPickRequestInfo.tlUnMoney = 0;

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.MoneyLineSelection == 1) ? 2 : 1;

                }
                else if (StepsDataList[1].SelectedTabIndex == 2)
                {
                    if (StepsDataList[1].BettingDetails.SpreadSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }
                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = 0;
                    addPickRequestInfo.aMoScore = 0;
                    addPickRequestInfo.hSpScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.Spread);
                    addPickRequestInfo.aSpScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.Spread);
                    addPickRequestInfo.oScore = 0;
                    addPickRequestInfo.uScore = 0;

                    addPickRequestInfo.hPtSpMoney = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.SpMoney);
                    addPickRequestInfo.aPtSpMoney = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.SpMoney);
                    addPickRequestInfo.tlOvMoney = 0;
                    addPickRequestInfo.tlUnMoney = 0;

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.SpreadSelection == 1) ? 2 : 1;
                }
                else if (StepsDataList[1].SelectedTabIndex == 3)
                {
                    if (StepsDataList[1].BettingDetails.OverUnderSelection == 0)
                    {
                        await ShowAlert(AppResources.AppName, AppResources.SelectABet);
                        IsBusy = false;
                        return;
                    }
                    addPickRequestInfo.teamIdToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamId : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamId;
                    addPickRequestInfo.tNameToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? StepsDataList[1].BettingDetails.SelectedGame.HomeTeamDetails.TeamName : StepsDataList[1].BettingDetails.SelectedGame.AwayTeamDetails.TeamName;

                    addPickRequestInfo.hMoScore = 0;
                    addPickRequestInfo.aMoScore = 0;
                    addPickRequestInfo.hSpScore = 0;
                    addPickRequestInfo.aSpScore = 0;
                    addPickRequestInfo.oScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.OverScore);
                    addPickRequestInfo.uScore = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.UnderScore);

                    addPickRequestInfo.hPtSpMoney = 0;
                    addPickRequestInfo.aPtSpMoney = 0;
                    addPickRequestInfo.tlOvMoney = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.OvMoney);
                    addPickRequestInfo.tlUnMoney = Convert.ToDouble(StepsDataList[1].BettingDetails.SelectedGame.UnMoney);

                    addPickRequestInfo.tTypeToWin = (StepsDataList[1].BettingDetails.OverUnderSelection == 1) ? 2 : 1;
                }

                if (Media?.Count > 0)
                {
                    IsBusy = true;
                    ProgressPercentageDisplay = "0%";
                    if (await UploadImages())
                    {
                        var imagesArray = Media.Where(x => x.Type == MediaFileType.Image).ToList();
                        var videosArray = Media.Where(x => x.Type == MediaFileType.Video).ToList();

                        if (imagesArray.Count > 0)
                        {
                            foreach (var imgItem in imagesArray)
                            {
                                ImageData imgData = new ImageData();
                                imgData.fileUrl = imgItem.UploadedName;
                                imgData.fileText = string.Empty;
                                addPickRequestInfo.imageUrl.Add(imgData);
                            }
                        }
                        if (videosArray.Count > 0)
                        {
                            foreach (var vidItem in videosArray)
                            {
                                VideoData vidData = new VideoData();
                                vidData.fileUrl = vidItem.UploadedName;
                                vidData.fileText = string.Empty;
                                addPickRequestInfo.videoUrl.Add(vidData);
                            }
                        }
                        // Call final Post Webservice from here
                        var Response = await TailDataServiceProvider.Instance.AddPick(addPickRequestInfo);
                        if (Response.ErrorCode != 200)
                            await ShowAlert(AppResources.AppName, Response.Message);
                        else
                        {
                            CommonSingletonUtility.SharedInstance.IsNewPostAdded = true;
                            Handle_BackCommand();

                        }

                        IsBusy = false;
                    }
                    else
                    {
                        IsBusy = false;

                        // Show error message here 
                        await ShowAlert(AppResources.AppName, AppResources.UploadFailedText);
                        foreach (MediaFile image in Media)
                        {
                            image.IsUploading = false;
                        }
                    }
                }
                else
                {
                    IsNormalIndicator = true;
                    var Response = await TailDataServiceProvider.Instance.AddPick(addPickRequestInfo);
                    IsBusy = false;
                    IsNormalIndicator = false;
                    if (Response.ErrorCode != 200)
                        await ShowAlert(AppResources.AppName, Response.Message);
                    else
                    {
                        CommonSingletonUtility.SharedInstance.IsNewPostAdded = true;
                        Handle_BackCommand();

                    }



                }
                IsBusy = false;
        }
        void InitUpload()
        {
            foreach (MediaFile image in Media)
            {
                image.IsUploading = true;
            }

        }
        async Task<bool> UploadImages()
        {
            int numberOfBatch = Constants.IMAGE_UPLOAD_BATCH_COUNT;
           
            int numberOfItemsInBatch = Media.Count / numberOfBatch;
            List<Task<bool>> uploadTasks = new List<Task<bool>>();

            if (numberOfItemsInBatch > 0)
            {
                List<List<MediaFile>> uploadBatchImages = new List<List<MediaFile>>();
                int batchStartIndex = 0;
                for (int i = 0; i < numberOfBatch; i++)
                {
                    uploadBatchImages.Add(new List<MediaFile>());
                    for (int k = batchStartIndex; k < batchStartIndex + numberOfItemsInBatch; k++)
                    {
                        uploadBatchImages[i].Add(Media[k]);
                    }
                    batchStartIndex += numberOfItemsInBatch;
                }

                int remainingItems = Media.Count % numberOfBatch;

                for (int j = 0; j < remainingItems; j++)
                {
                    uploadBatchImages[j].Add(Media[batchStartIndex]);
                    batchStartIndex++;
                }

                uploadTasks.Add(UploadImageBatchOne(uploadBatchImages[0], true));
                uploadTasks.Add(UploadImageBatchTwo(uploadBatchImages[1], true));
                uploadTasks.Add(UploadImageBatchThree(uploadBatchImages[2], true));
            }
            else
            {
                numberOfBatch = Media.Count % numberOfBatch;

                switch (numberOfBatch)
                {
                    case 1:
                        uploadTasks.Add(UploadImageOne(Media[0]));
                        break;

                    case 2:
                        uploadTasks.Add(UploadImageOne(Media[0]));
                        uploadTasks.Add(UploadImageTwo(Media[1]));
                        break;

                    default:
                        break;
                }
            }
            InitUpload();
            bool[] uploadTaskStatusList = await Task.WhenAll(uploadTasks);
            CurrentProgressCount = Media.Count;
            ProgressPercentage = (CurrentProgressCount / Media.Count) * 100;
            ProgressPercentageDisplay = ProgressPercentage + "%";
            foreach (bool uploadTaskStatus in uploadTaskStatusList)
            {
                if (!uploadTaskStatus)
                {
                    return false;
                }
            }

            return true;
        }

        async Task<bool> UploadImageBatchOne(List<MediaFile> images, bool batchStatus)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                IsBusy = false;
                return false;
            }
            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                        
                    }
                    else
                        batchStatus = false;


                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                           

                        }
                        else
                            batchStatus = false;
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                           

                        }
                        else
                            batchStatus = false;

                    }




                    UpdateUploadDone(images[i].ImageID);
                }

            }
          

            return batchStatus;
        }
        async Task<bool> UploadImageBatchTwo(List<MediaFile> images, bool batchStatus)
        {
            Debug.WriteLine("UploadImageBatchTwo");
            if (!CrossConnectivity.Current.IsConnected)
            {
                IsBusy = false;
                return false;
            }
            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                      
                    }
                    else
                        batchStatus = false;


                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                        

                        }
                        else
                            batchStatus = false;
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                          

                        }
                        else
                            batchStatus = false;

                    }




                    UpdateUploadDone(images[i].ImageID);
                }

            }
           

            return batchStatus;
        }
        async Task<bool> UploadImageBatchThree(List<MediaFile> images, bool batchStatus)
        {
            Debug.WriteLine("UploadImageBatchThree");
            if (!CrossConnectivity.Current.IsConnected)
            {
                IsBusy = false;
                return false;
            }
            for (int i = 0; i < images.Count; i++)
            {

                if (images[i].Type == MediaFileType.Image)
                {

                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalImageName = _keyName + ".jpg";
                    Debug.WriteLine(_orginalImageName);
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, images[i].Path, Constants.S3BucketForPostImage, false))
                    {
                        images[i].UploadedName = _orginalImageName;
                      
                    }
                    else
                        batchStatus = false;


                    UpdateUploadDone(images[i].ImageID);
                }
                else if (images[i].Type == MediaFileType.Video)
                {
                    string _keyName = Guid.NewGuid().ToString();
                    string _orginalVideoName = _keyName + ".mp4";
                    string _thumbImageName = _keyName + "_thumb.jpg";
                    Debug.WriteLine(_orginalVideoName);
                    FileInfo fi = new FileInfo(images[i].Path);
                    double sizeInMB = (double)fi.Length / (1000 * 1000);
                    if (sizeInMB > 12)
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadLargeSizeVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                           

                        }
                        else
                            batchStatus = false;
                    }
                    else
                    {
                        if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, images[i].Path, images[i].PreviewPath, Constants.S3BucketForPostVideo))
                        {
                            images[i].UploadedName = _orginalVideoName;
                          
                        }
                        else
                            batchStatus = false;

                    }




                    UpdateUploadDone(images[i].ImageID);
                }

            }
           

            return batchStatus;
        }

     

        async Task<bool> UploadImageOne(MediaFile image)
        {
            bool uploadStatus = false;
            if (image.Type == MediaFileType.Image)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalImageName);
                if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, image.Path, Constants.S3BucketForPostImage, false))
                {
                    image.UploadedName = _orginalImageName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            else if (image.Type == MediaFileType.Video)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalVideoName = _keyName + ".mp4";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalVideoName);
                if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, image.Path, image.PreviewPath, Constants.S3BucketForPostVideo))
                {
                    image.UploadedName = _orginalVideoName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            return uploadStatus;
        }

        async Task<bool> UploadImageTwo(MediaFile image)
        {
            bool uploadStatus = false;
            if (image.Type == MediaFileType.Image)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalImageName = _keyName + ".jpg";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_thumbImageName);
                Debug.WriteLine(_orginalImageName);
                if (await DependencyService.Get<IAwsBucketService>().UploadImageFromFileToAmazonBucketAsync(_orginalImageName, _thumbImageName, image.Path, Constants.S3BucketForPostImage, false))
                {
                    image.UploadedName = _orginalImageName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            else if (image.Type == MediaFileType.Video)
            {
                string _keyName = Guid.NewGuid().ToString();
                string _orginalVideoName = _keyName + ".mp4";
                string _thumbImageName = _keyName + "_thumb.jpg";
                Debug.WriteLine(_orginalVideoName);
                if (await DependencyService.Get<IAwsBucketService>().UploadVideoFromFileToAmazonBucketAsync(_orginalVideoName, _thumbImageName, image.Path, image.PreviewPath, Constants.S3BucketForPostVideo))
                {
                    image.UploadedName = _orginalVideoName;
                    uploadStatus = true;
                }
                else
                    uploadStatus = false;

                UpdateUploadDone(image.ImageID);
            }
            return uploadStatus;
        }

        void UpdateUploadDone(int imageID)
        {
            for (int i = 0; i < Media.Count; i++)
            {
                if (Media[i].ImageID == imageID)
                {
                    Media[i].IsUploading = false;
                    break;
                }
            }

        }
        private async Task Handle_ImageTapCommandCommandAsync(object item)
        {
            var tappedItem = item as MediaFile;
            if (tappedItem.Type == MediaFileType.Image)
                await PopupNavigation.Instance.PushAsync(new PostImagePopup(tappedItem.Path));
            else if (tappedItem.Type == MediaFileType.Video)
                await PopupNavigation.Instance.PushAsync(new PostVideoPopup(tappedItem.Path));

        }
        private async Task Handle_SelectGameCommand()
        {
            if (IsNormalIndicator)
                return;
            IsNormalIndicator = true;
            if (IsGameSelectionEnabled)
                await PopupNavigation.Instance.PushAsync(new SelectGamePopup(StepsDataList[0].UpcomingGames, () => Handle_SelectGamePopupClosed()));
            IsNormalIndicator = false;
        }

        private void Handle_SelectGamePopupClosed()
        {
            Debug.WriteLine("Game Popup Close");
        }

        ///<summary>
        ///Override back button to correct tab navigation.
        ///</summary>
        public override void Handle_BackCommand()
        {
            var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
            SettingsService.Instance.CurrentTabIndex = 0;
            TabbedPage currentTabbedPage = currentPage as TabbedPage;
            if (currentTabbedPage != null)
                currentTabbedPage.CurrentPage = currentTabbedPage.Children[0];
            base.Handle_BackCommand();
        }
    }
}
