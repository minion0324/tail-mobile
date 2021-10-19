
using Newtonsoft.Json;

namespace Tail.Models
{
    public class Predictions
    {
        [JsonProperty("sportId")]
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
        [JsonProperty("SportImage")]
        public string SportImage
        {
            get;
            set;
        }
        [JsonProperty("accPerc")]
        public string PredictionPercentage
        {
            get;
            set;
        }
    }
}
