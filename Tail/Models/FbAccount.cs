
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class FbAccount
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public object PictureJson { get; set; }
        public string ProfileImage { get; set; }

        [JsonIgnore]
        public string AccessToken { get; set; }
    }
    public class PicInfo{
        public PicDetails data { get; set; }
    }
    public class PicDetails
    {
        public string height { get; set; }
        public string url { get; set; }
        public string width { get; set; }
    }


    public class Datum
    {
        public string id { get; set; }
    }
    
    public class RootObject
    {
        public List<Datum> data { get; set; }
       
    }
    public class FacebookInfo
    {
        public string FacebookIds { get; set; }
    }
}
