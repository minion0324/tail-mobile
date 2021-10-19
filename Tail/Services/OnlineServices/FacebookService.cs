
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tail.Models;
using Tail.Services.Interfaces;

namespace Tail.Services.OnlineServices
{
    public class FacebookService : IFacebookService
    {
        public static string tokenString { get; set; }

        private readonly IFacebookClient _facebookClient;

        public FacebookService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        public async Task<FbAccount> GetAccountAsync(string accessToken)
        {
            var result = await _facebookClient.GetAsync<dynamic>( accessToken, "me", "fields=id,name,email,first_name,last_name,picture.type(large)");
            if (result == null)
            {
                return null;
            }
            else
            {
                var account = new FbAccount
                {
                    AccessToken= accessToken,
                    Id = result.id,
                    Email = result.email,
                    Name = result.name,
                    UserName = result.username,
                    FirstName = result.first_name,
                    LastName = result.last_name,
                    Locale = result.locale,
                    PictureJson = result.picture
                };
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                string picSerialise = JsonConvert.SerializeObject(account.PictureJson);
                PicInfo picDetails = JsonConvert.DeserializeObject<PicInfo>(picSerialise, settings);
                account.ProfileImage = picDetails.data.url;
                return account;
            }
        }

        public async Task PostOnWallAsync(string accessToken, string message)
            => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });

     

    }
}
