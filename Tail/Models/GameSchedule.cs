using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.Common;
using Xamarin.Forms;

namespace Tail.Models
{
    public class GameSchedule : BaseModel
    {
        IList<ScheduleInfo> _gameData;
        [JsonProperty("resultData")]
        public IList<ScheduleInfo> GameData
        {
            get => _gameData;
            set => SetProperty(ref _gameData, value);
        }
        IList<PaginationDetails> _pageInfo;
        [JsonProperty("pageInfo")]
        public IList<PaginationDetails> PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }
      

    }
    public class ScheduleInfo : BaseModel
    {
        string _eventDate;
        [JsonProperty("_id")]
        public string EventDate
        {
            get => _eventDate;
            set => SetProperty(ref _eventDate, value);
        }
     
        [JsonIgnore]
        public string GameDate
        {
            get
            {
                if (!string.IsNullOrEmpty(EventDate))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(EventDate);
                    string _displayFormate = _eventDateTime.ToString("MMMM d");
                    return _displayFormate;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
     
        IList<GameInfo> _games;
        [JsonProperty("games")]
        public IList<GameInfo> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }
        bool _gameRefresh;
        public bool GameRefresh
        {
            get => _gameRefresh;
            set => SetProperty(ref _gameRefresh, value);
        }
        bool _scrollReset;
        public bool ScrollReset
        {
            get => _scrollReset;
            set => SetProperty(ref _scrollReset, value);
        }
    }
    public class GameInfo : BaseModel
    {
        Command<string> _selectedCommand;
        public Command<string> SelectedCommand
        {
            get => _selectedCommand;
            set => SetProperty(ref _selectedCommand, value);
        }
        string _gameDateTime;
        [JsonProperty("eventDate")]
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
        public string GameDateHeader
        {
            get
            {
                if (!string.IsNullOrEmpty(GameDateTime))
                {
                    DateTime _eventDateTime = Convert.ToDateTime(GameDateTime);
                    string _displayFormate = _eventDateTime.ToString("MMMM d");
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
        string _eventID;
        [JsonProperty("eventId")]
        public string EventID
        {
            get => _eventID;
            set => SetProperty(ref _eventID, value);
        }
       
        TeamDetails _homeTeamDetails;
        [JsonProperty("homeTeam")]
        public TeamDetails HomeTeamDetails
        {
            get => _homeTeamDetails;
            set => SetProperty(ref _homeTeamDetails, value);
        }
        TeamDetails _awayTeamDetails;
        [JsonProperty("awayTeam")]
        public TeamDetails AwayTeamDetails
        {
            get => _awayTeamDetails;
            set => SetProperty(ref _awayTeamDetails, value);
        }

        string _overScore;
        [JsonProperty("ovScore")]
        public string OverScore
        {
            get => _overScore;
            set => SetProperty(ref _overScore, value);
        }
        string _ovMoney;
        [JsonProperty("tlOvMoney")]
        public string OvMoney
        {
            get => _ovMoney;
            set => SetProperty(ref _ovMoney, value);
        }
        public string OverScoreDisply
        {
            get
            {
                if (string.IsNullOrEmpty(OverScore))
                    return "O 0";
                else
                    return "O " + Math.Round(Convert.ToDouble(OverScore), 2);
            }

        }
        public string OverScoreMoneyDisply
        {
            get
            {
                if (string.IsNullOrEmpty(OvMoney))
                    return "0";
                else
                    return (Convert.ToDouble(OvMoney) > 0) ? "+" + Math.Round(Convert.ToDouble(OvMoney), 2).ToString() : Math.Round(Convert.ToDouble(OvMoney), 2).ToString();
            }

        }
        string _underScore;
        [JsonProperty("unScore")]
        public string UnderScore
        {
            get => _underScore;
            set => SetProperty(ref _underScore, value);
        }
        string _unMoney;
        [JsonProperty("tlUnMoney")]
        public string UnMoney
        {
            get => _unMoney;
            set => SetProperty(ref _unMoney, value);
        }
        public string UnderScoreDisply
        {
            get
            {
                if (string.IsNullOrEmpty(UnderScore))
                    return "U 0";
                else
                    return "U " + Math.Round(Convert.ToDouble(UnderScore), 2);
            }

        }
        public string UnderScoreMoneyDisply
        {
            get
            {
                if (string.IsNullOrEmpty(UnMoney))
                    return "0";
                else
                    return (Convert.ToDouble(UnMoney) > 0) ? "+" + Math.Round(Convert.ToDouble(UnMoney), 2).ToString() : Math.Round(Convert.ToDouble(UnMoney), 2).ToString();
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
    public class TeamDetails : BaseModel
    {
        int _teamId;
        [JsonProperty("teamId")]
        public int TeamId
        {
            get => _teamId;
            set => SetProperty(ref _teamId, value);
        }
        int _teamNormId;
        [JsonProperty("teamNormId")]
        public int TeamNormId
        {
            get => _teamNormId;
            set => SetProperty(ref _teamNormId, value);
        }
        string _teamName;
        [JsonProperty("tName")]
        public string TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }
        int _sportID;
        public int SportID
        {
            get => _sportID;
            set => SetProperty(ref _sportID, value);
        }
        SportType _sports_Type;
        public SportType Sports_Type
        {
            get => _sports_Type;
            set => SetProperty(ref _sports_Type, value);
        }
        [JsonIgnore]
        public string TeamDefaultLogo
        {
            get
            {
                if (Sports_Type == SportType.Baseball)
                    return "baseball_placeholder_small.png";
                else if (Sports_Type == SportType.Basketball)
                    return "basketball_placeholder_small.png";
                else if (Sports_Type == SportType.Football)
                    return "football_placeholder_small.png";
                else if (Sports_Type == SportType.Hocky)
                    return "hocky_placeholder_small.png";
                else if (Sports_Type == SportType.MMA)
                    return "mma_or_boxing_placeholder_small.png";  
                else
                    return "baseball_placeholder_small.png";

            }
        }
        string _tlogo;
        [JsonProperty("tlogo")]
        public string Tlogo
        {
            get => _tlogo;
            set => SetProperty(ref _tlogo, value);
        }
        public string TeamLogo
        {
            get
            {
                if (string.IsNullOrEmpty(_tlogo))
                    return "team_placeholder.png";
                else
                    return Constants.TEAM_LOGO_PATH+ Tlogo;
            }

        }
        string _teamColor;
        [JsonProperty("homeColorCode")]
        public string TeamColor
        {
            get => _teamColor;
            set => SetProperty(ref _teamColor, value);
        }
        string _teamSecondaryColor ;
        [JsonProperty("awayColorCode")]
        public string TeamSecondaryColor
        {
            get => _teamSecondaryColor;
            set => SetProperty(ref _teamSecondaryColor, value);
        }
        public bool IsDefaultLogo
        {
            get
            {
                if (string.IsNullOrEmpty(TeamColor) || Sports_Type == SportType.MMA)
                    return true;
                else
                    return false;

            }
        }
        string _teamNameShort;
        [JsonProperty("tAbbr")]
        public string TeamNameShort
        {
            get => _teamNameShort;
            set => SetProperty(ref _teamNameShort, value);
        }
        int _score;
        [JsonProperty("score")]
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }
        string _record;
        [JsonProperty("record")]
        public string Record
        {
            get => _record;
            set => SetProperty(ref _record, value);
        }
        bool _isWinner;
        [JsonProperty("isWinner")]
        public bool IsWinner
        {
            get => _isWinner;
            set => SetProperty(ref _isWinner, value);
        }
        string _spread;
        [JsonProperty("spread")]
        public string Spread
        {
            get => _spread;
            set => SetProperty(ref _spread, value);
        }
        string _spMoney;
        [JsonProperty("ptSpMoney")]
        public string SpMoney
        {
            get => _spMoney;
            set => SetProperty(ref _spMoney, value);
        }
        public string SpreadDisply
        {
            get
            {
                if (string.IsNullOrEmpty(Spread))
                    return "0";
                else
                    return(Convert.ToDouble(Spread)>0)? "+" + Math.Round(Convert.ToDouble(Spread), 2).ToString() :  Math.Round(Convert.ToDouble(Spread), 2).ToString();
            }

        }
        public string SpreadMoneyDisply
        {
            get
            {
                if (string.IsNullOrEmpty(SpMoney))
                    return "0";
                else
                    return (Convert.ToDouble(SpMoney) > 0) ? "+" + Math.Round(Convert.ToDouble(SpMoney), 2).ToString() : Math.Round(Convert.ToDouble(SpMoney), 2).ToString();
            }

        }
        string _moneyline;
        [JsonProperty("moneyline")]
        public string Moneyline
        {
            get => _moneyline;
            set => SetProperty(ref _moneyline, value);
        }
        public string MoneylineDisply
        {
            get
            {
                if (string.IsNullOrEmpty(Moneyline))
                    return "0";

                return (Convert.ToDouble(Moneyline) > 0) ? "+" + Math.Round(Convert.ToDouble(Moneyline), 2).ToString() : Math.Round(Convert.ToDouble(Moneyline), 2).ToString();
            }

        }
       
    }
  
}
