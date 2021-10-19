using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tail.Common;
using Tail.Services.Responses;
using Plugin.Connectivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Tail.Services.ApplicationServices;
using System.Net;
using Tail.Models;
using System.Text;
using System.Diagnostics;
using Tail.ViewModels;
using Xamarin.Forms;

namespace Tail.Services.OnlineServices
{
    public class WebserviceBase
    {

        const string kRefreshUri = "token/refreshToken";
        bool IsRefreshBusy = false;
        public WebserviceBase()
        {
            JsonSerializerSettings _serializerSettings;
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(Constants.ApiKey, Constants.ApiKeyValue);
            if (SettingsService.Instance.Authenticated && SettingsService.Instance.LoggedUserDetails != null)
            {

                string accessToken = SettingsService.Instance.LoggedUserDetails.AccessToken;
              
                httpClient.DefaultRequestHeaders.Add(Constants.AuthorizationKey, string.Format("Bearer {0}", accessToken));

            }

            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, object data) where TResult : ServiceResponseBase
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;

                    result.ErrorCode = 1108;
                    result.Message = AppResources.NoConnectionMessage;

                    return (TResult)result;
                }

                HttpClient httpClient = CreateHttpClient();

                string _stringifyData = JsonConvert.SerializeObject(data);

                HttpContent content = new StringContent(_stringifyData, System.Text.Encoding.UTF8, "application/json");

                string apiUrl = uri;
                
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {

                        return await PostAsync<TResult>(uri, data);
                    }
                    return result;
                }
                else
                {

                    string serialized = await response.Content.ReadAsStringAsync();

                   
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {

                        return await PostAsync<TResult>(uri, data);
                    }
                    else if(response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        result.Message = "Invalid API Key.";
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                result.ErrorCode = 1109;
                result.Message = ex.Message;
                return (TResult)result;
            }
        }
        public async Task<TResult> PutAsync<TResult>(string uri, object data) where TResult : ServiceResponseBase
        {
            try
            {

                if (!CrossConnectivity.Current.IsConnected)
                {
                    var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;

                    result.ErrorCode = 1108;
                    result.Message = AppResources.NoConnectionMessage;

                    return (TResult)result;
                }

                HttpClient httpClient = CreateHttpClient();
                string apiUrl = uri;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(apiUrl),

                };
                string _stringifyData = JsonConvert.SerializeObject(data);
                if (data != null)
                {
                    request.Content = new StringContent(_stringifyData, Encoding.UTF8, "application/json");
                }
                
                HttpResponseMessage response = await httpClient.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {

                        return await PutAsync<TResult>(uri, data);
                    }
                    return result;
                }
                else
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                   
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {

                        return await PutAsync<TResult>(uri, data);
                    }
                    return result;


                }
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                result.ErrorCode = 1109;
                result.Message = ex.Message;
                return (TResult)result;
            }
        }
        public async Task<TResult> GetAsync<TResult>(string uri, string param) where TResult : ServiceResponseBase
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                    result.ErrorCode = 1108;
                    result.Message = AppResources.NoConnectionMessage;

                    return (TResult)result;
                }

                HttpClient httpClient = CreateHttpClient();


                string apiUrl = uri;
                if (!string.IsNullOrEmpty(param))
                {
                    apiUrl = apiUrl + param;
                }
                
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                  
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {
                        
                        return await GetAsync<TResult>(uri, param);
                    }

                    return result;
                }
                else
                {

                    string serialized = await response.Content.ReadAsStringAsync();
                  
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                  
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {
                        
                        return await GetAsync<TResult>(uri, param);
                    }
                    return result;

                }
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                result.ErrorCode = 1109;
                result.Message = ex.Message;
                return (TResult)result;
            }
        }
        public async Task<TResult> DeleteAsync<TResult>(string uri, object data) where TResult : ServiceResponseBase
        {
            try
            {

                if (!CrossConnectivity.Current.IsConnected)
                {
                    var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                    result.ErrorCode = 1108;
                    result.Message = AppResources.NoConnectionMessage;

                    return (TResult)result;
                }

                HttpClient httpClient = CreateHttpClient();



                string apiUrl = uri;
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl),

                };
                string _stringifyData = JsonConvert.SerializeObject(data);
                if (data != null)
                {
                    request.Content = new StringContent(_stringifyData, Encoding.UTF8, "application/json");
                }
                
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string serialized = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {
                        return await DeleteAsync<TResult>(uri, data);
                    }

                    return result;
                }
                else
                {

                    string serialized = await response.Content.ReadAsStringAsync();
                  
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    var result = JsonConvert.DeserializeObject<TResult>(serialized, settings);
                    if (result.ErrorCode == Constants.REFRESH_TOKEN_ERROR && await RefreshNewToken())
                    {
                        return await DeleteAsync<TResult>(uri, data);
                    }
                    return result;

                }
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResult>() as ServiceResponseBase;
                result.ErrorCode = 1109;
                result.Message = ex.Message;
                return (TResult)result;
            }
        }
        public async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            try
            {
               
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return null;
                }

                HttpClient _httpClient = CreateHttpClient();
                using (var httpResponse = await _httpClient.GetAsync(imageUrl))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        return await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> RefreshNewToken()
        {
            if (IsRefreshBusy)
                return false;
            IsRefreshBusy = true;
            bool hasSuccessResponse = false;
            try
            {
                if (SettingsService.Instance.LoggedUserDetails == null)
                    return false;

                RefreshTokenRequestInfo requestObj = new RefreshTokenRequestInfo()
                {
                    refreshToken = SettingsService.Instance.LoggedUserDetails.RefreshToken,
                    userId = SettingsService.Instance.LoggedUserDetails.UserId.ToString()
                };

                ServiceResponse<RefreshTokenResponseInfo> refreshResponse = await PostAsync<ServiceResponse<RefreshTokenResponseInfo>>(string.Format(@"{0}/{1}", Constants.BaseUrl, kRefreshUri), requestObj);
                if (refreshResponse.ErrorCode == 200 && refreshResponse.ResponseData != null)
                {
                    hasSuccessResponse = true;
                    UserInfo _userDetails = SettingsService.Instance.LoggedUserDetails;
                    _userDetails.AccessToken = refreshResponse.ResponseData.accessToken;
                    _userDetails.RefreshToken = refreshResponse.ResponseData.refreshToken;
                    SettingsService.Instance.LoggedUserDetails = _userDetails;
                }
                else
                {
                    SessionOut();
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsRefreshBusy = false;
            }
            IsRefreshBusy = false;
            return hasSuccessResponse;
        }

        private void SessionOut()
        {
            if (App.Current != null)
            {
                PageViewModelBase pageViewModelBase = null;
                Xamarin.Forms.TabbedPage appPageBase = null;
                if (App.Current.MainPage is NavigationPage navigationPage)
                {
                    appPageBase = navigationPage.CurrentPage as Xamarin.Forms.TabbedPage;

                    if (appPageBase != null)
                    {
                        pageViewModelBase = appPageBase.BindingContext as PageViewModelBase;
                    }
                }
                if (pageViewModelBase != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        pageViewModelBase.TokenExpire.Execute(null);
                    });

                }
            }
        }
    }
}