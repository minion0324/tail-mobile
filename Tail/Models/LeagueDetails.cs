using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tail.ViewModels;

namespace Tail.Models
{
    public class LeagueDetails : ViewModelBase
    {



        int _leagueID;
        [JsonProperty("sportId")]
        public int LeagueID
        {
            get => _leagueID;
            set => SetProperty(ref _leagueID, value);
        }
        string _leagueName;
        [JsonProperty("sName")]
        public string LeagueName
        {
            get => _leagueName;
            set => SetProperty(ref _leagueName, value);
        }


        public string LeagueImage
        {
            get
            {
                if (LeagueID == 4)
                {
                    return "NBA.png";
                }
                else
                {
                    return string.Empty;
                }

            }
        }

    }

    public class LeagueDetailsResponse : ViewModelBase
    {


        List<LeagueDetails> _sportsDetails;
        [JsonProperty("sportsDetails")]
        public List<LeagueDetails> SportsDetails
        {
            get => _sportsDetails;
            set => SetProperty(ref _sportsDetails, value);
        }
        List<PriceRange> _price;
        [JsonProperty("priceRange")]
        public List<PriceRange> Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
    }
    public class PriceRange : ViewModelBase
    {
        int _minPrice;
        [JsonProperty("msMinPrice")]
        public int MinPrice
        {
            get => _minPrice;
            set => SetProperty(ref _minPrice, value);
        }
        int _maxPrice;
        [JsonProperty("msMaxPrice")]
        public int MaxPrice
        {
            get => _maxPrice;
            set => SetProperty(ref _maxPrice, value);
        }
    }

}
