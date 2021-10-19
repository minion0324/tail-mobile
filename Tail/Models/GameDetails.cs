using Newtonsoft.Json;

namespace Tail.Models
{
   
    public class GameDetails
    {
        [JsonProperty("SportId")]
        public int SportId
        {
            get;
            set;
        }

        [JsonProperty("SportName")]
        public string SportName
        {
            get;
            set;
        }
        [JsonProperty("SportIcon")]
        public string SportIcon
        {
            get;
            set;
        }
        [JsonProperty("Team1Logo")]
        public string Team1Logo
        {
            get;
            set;
        }
        [JsonProperty("Team1Name")]
        public string Team1Name
        {
            get;
            set;
        }
        [JsonProperty("Team2Logo")]
        public string Team2Logo
        {
            get;
            set;
        }
        [JsonProperty("Team2Name")]
        public string Team2Name
        {
            get;
            set;
        }
        [JsonProperty("GameDate")]
        public string GameDate
        {
            get;
            set;
        }
        [JsonProperty("GameTime")]
        public string GameTime
        {
            get;
            set;
        }
        [JsonProperty("PickByPrediction")]
        public string PickByPrediction
        {
            get;
            set;
        }
        [JsonProperty("ActualGameStatus")]
        public string ActualGameStatus
        {
            get;
            set;
        }
    }
}
