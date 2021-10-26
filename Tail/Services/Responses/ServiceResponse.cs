
using Newtonsoft.Json;

namespace Tail.Services.Responses
{
    public class ServiceResponse<T> : ServiceResponseBase
    {
        [JsonProperty(PropertyName = "Data")]
        public T ResponseData
        {
            get;
            set;
        }
     
    }
  

}
