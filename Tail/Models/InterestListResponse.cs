
using Newtonsoft.Json;

namespace Tail.Models
{
    public class InterestListResponseItems : BaseModel
    {
        int _mainSportId;
        string _msName;

        [JsonProperty("mainSportId")]
        public int MainSportId
        {
            get => _mainSportId;
            set => SetProperty(ref _mainSportId, value);
        }

        [JsonProperty("msName")]
        public string MsName
        {
            get => _msName;
            set => SetProperty(ref _msName, value);
        }
    }
}
