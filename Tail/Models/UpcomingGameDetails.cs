using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.Common;
using Tail.Services.Helper;
using Xamarin.Forms;

namespace Tail.Models
{
    public class StepsDetails : BaseModel
    {

        BettingTypeDetails _bettingDetails;
        public BettingTypeDetails BettingDetails
        {
            get => _bettingDetails;
            set => SetProperty(ref _bettingDetails, value);
        }
        IList<GameSchedule> _upcomingGames;
        public IList<GameSchedule> UpcomingGames
        {
            get => _upcomingGames;
            set => SetProperty(ref _upcomingGames, value);
        }
        IList<PickerItem> _leagueOptions;
        public IList<PickerItem> LeagueOptions
        {
            get => _leagueOptions;
            set => SetProperty(ref _leagueOptions, value);
        }
        IList<PickerItem> _spotOptions;
        public IList<PickerItem> SpotOptions
        {
            get => _spotOptions;
            set => SetProperty(ref _spotOptions, value);
        }
        bool _isStep1;
        public bool IsStep1
        {
            get => _isStep1;
            set => SetProperty(ref _isStep1, value);
        }
        int _spotSelectedIndex;
        public int SpotSelectedIndex
        {
            get => _spotSelectedIndex;
            set => SetProperty(ref _spotSelectedIndex, value);
        }
        int _setSpotSelectedIndex;
        public int SetSpotSelectedIndex
        {
            get => _setSpotSelectedIndex;
            set => SetProperty(ref _setSpotSelectedIndex, value);
        }
        int _leagueSelectedIndex;
        public int LeagueSelectedIndex
        {
            get => _leagueSelectedIndex;
            set => SetProperty(ref _leagueSelectedIndex, value);
        }
        int _setLeagueSelectedIndex;
        public int SetLeagueSelectedIndex
        {
            get => _setLeagueSelectedIndex;
            set => SetProperty(ref _setLeagueSelectedIndex, value);
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
        string _informationText;
        public string InformationText
        {
            get => _informationText;
            set => SetProperty(ref _informationText, value);
        }
        int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }
        bool _isGameAvailable=false;
        public bool IsGameAvailable
        {
            get => _isGameAvailable;
            set => SetProperty(ref _isGameAvailable, value);
        }
    }
   

    public class UpcomingGameDetails : BaseModel
    {

      
        string _gameDateTime1;
        public string GameDateTime1
        {
            get => _gameDateTime1;
            set => SetProperty(ref _gameDateTime1, value);
        }
        [JsonIgnore]
        public string GameDate1
        {
            get
            {
                if (!string.IsNullOrEmpty(GameDateTime1))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(GameDateTime1);
                    string _displayFormate = _eventDateTime.ToString("MMMM d");
                    return _displayFormate;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        string _gameDateTime2;
        public string GameDateTime2
        {
            get => _gameDateTime2;
            set => SetProperty(ref _gameDateTime2, value);
        }
        [JsonIgnore]
        public string GameDate2
        {
            get
            {
                if (!string.IsNullOrEmpty(GameDateTime2))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(GameDateTime2);
                    string _displayFormate = _eventDateTime.ToString("MMMM d");
                    return _displayFormate;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        IList<GameDetailsValue> _gameList1;
        public IList<GameDetailsValue> GameList1
        {
            get => _gameList1;
            set => SetProperty(ref _gameList1, value);
        }
        IList<GameDetailsValue> _gameList2;
        public IList<GameDetailsValue> GameList2
        {
            get => _gameList2;
            set => SetProperty(ref _gameList2, value);
        }

        bool _game1Refresh;
        public bool Game1Refresh
        {
            get => _game1Refresh;
            set => SetProperty(ref _game1Refresh, value);
        }
        bool _game2Refresh;
        public bool Game2Refresh
        {
            get => _game2Refresh;
            set => SetProperty(ref _game2Refresh, value);
        }
        bool _isGameAvailable;
        public bool IsGameAvailable
        {
            get => _isGameAvailable;
            set => SetProperty(ref _isGameAvailable, value);
        }

    }
    public class GameDetailsValue : BaseModel
    {
        Command<int> _selectedCommand;
        public Command<int> SelectedCommand
        {
            get => _selectedCommand;
            set => SetProperty(ref _selectedCommand, value);
        }
        GameDetailsItem _gameItem;
        public GameDetailsItem GameItem
        {
            get => _gameItem;
            set => SetProperty(ref _gameItem, value);
        }
    }
    public class GameDetailsItem : BaseModel
    {
        int _gameID;
        public int GameID
        {
            get => _gameID;
            set => SetProperty(ref _gameID, value);
        }
        int _gameTyeID;
        public int GameTypeID
        {
            get => _gameTyeID;
            set => SetProperty(ref _gameTyeID, value);
        }
        int _leagueTypeID;
        public int LeagueTypeID
        {
            get => _leagueTypeID;
            set => SetProperty(ref _leagueTypeID, value);
        }
        string _team1Logo;
        public string Team1Logo
        {
            get => _team1Logo;
            set => SetProperty(ref _team1Logo, value);
        }
        string _team1Name;
        public string Team1Name
        {
            get => _team1Name;
            set => SetProperty(ref _team1Name, value);
        }
        string _team2Logo;
        public string Team2Logo
        {
            get => _team2Logo;
            set => SetProperty(ref _team2Logo, value);
        }
        string _team2Name;
        public string Team2Name
        {
            get => _team2Name;
            set => SetProperty(ref _team2Name, value);
        }
        string _gameDateTime;
        public string GameDateTime
        {
            get => _gameDateTime;
            set => SetProperty(ref _gameDateTime, value);
        }
        [JsonIgnore]
        public string GameTime
        {
            get
            {
                if (!string.IsNullOrEmpty(GameDateTime))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(GameDateTime);
                    string _displayFormate = _eventDateTime.ToString("hh:mm tt");
                    return _displayFormate;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        [JsonIgnore]
        public string GameDate
        {
            get
            {
                if (!string.IsNullOrEmpty(GameDateTime))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(GameDateTime);
                    string _displayFormate = _eventDateTime.ToString("ddd, MMM d yyyy");
                    return _displayFormate;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        bool _selected;
        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
        public string SelectedImage
        {
            get
            {
                return (Selected) ? "radio_selected" : "radio";
            }

        }
        public string SelectedText
        {
            get
            {
                return (Selected) ? AppResources.SelectedText : AppResources.SelectText;
            }
        }

    }

    public class BettingTypeDetails : BaseModel
    {


        GameInfo _selectedGame;
        public GameInfo SelectedGame
        {
            get => _selectedGame;
            set => SetProperty(ref _selectedGame, value);
        }

        SportType _sport_Type;
        public SportType Sport_Type
        {
            get => _sport_Type;
            set => SetProperty(ref _sport_Type, value);
        }
        [JsonIgnore]
        public string SelectedSpotName
        {
            get
            {
                return TailUtils.GameTypeToName(Sport_Type);

            }
        }
        [JsonIgnore]
        public string SelectedSpotImage
        {
            get
            {
                return TailUtils.GameTypeToImage_Home(Sport_Type);
            }
        }

        string _selectedEventImage;
        public string SelectedEventImage
        {
            get => _selectedEventImage;
            set => SetProperty(ref _selectedEventImage, value);
        }
        string _selectedEventName;
        public string SelectedEventName
        {
            get => _selectedEventName;
            set => SetProperty(ref _selectedEventName, value);
        }
        int _selectedEventID;
        public int SelectedEventID
        {
            get => _selectedEventID;
            set => SetProperty(ref _selectedEventID, value);
        }
        Command _editCommand;
        public Command EditCommand
        {
            get => _editCommand;
            set => SetProperty(ref _editCommand, value);
        }
        Command _moneyLineTabSelectedCommand;
        public Command MoneyLineTabSelectedCommand
        {
            get => _moneyLineTabSelectedCommand;
            set => SetProperty(ref _moneyLineTabSelectedCommand, value);
        }
        Command _spreadTabSelectedCommand;
        public Command SpreadTabSelectedCommand
        {
            get => _spreadTabSelectedCommand;
            set => SetProperty(ref _spreadTabSelectedCommand, value);
        }
        Command _overUnderTabSelectedCommand;
        public Command OverUnderTabSelectedCommand
        {
            get => _overUnderTabSelectedCommand;
            set => SetProperty(ref _overUnderTabSelectedCommand, value);
        }
        double _pickPrice=0;
        public double PickPrice
        {
            get => _pickPrice;
            set => SetProperty(ref _pickPrice, value);
        }
        string _pickPriceHint;
        public string PickPriceHint
        {
            get => _pickPriceHint;
            set => SetProperty(ref _pickPriceHint, value);
        }

        int _moneyLineSelection;
        public int MoneyLineSelection
        {
            get => _moneyLineSelection;
            set => SetProperty(ref _moneyLineSelection, value);
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
        int _spreadSelection;
        public int SpreadSelection
        {
            get => _spreadSelection;
            set => SetProperty(ref _spreadSelection, value);
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
        int _overUnderSelection;
        public int OverUnderSelection
        {
            get => _overUnderSelection;
            set => SetProperty(ref _overUnderSelection, value);
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
    }

}
