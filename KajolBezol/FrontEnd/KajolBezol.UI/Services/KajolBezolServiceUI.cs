using KajolBezol.UI.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KajolBezol.UI.Settings;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace KajolBezol.UI.Services
{
    public class KajolBezolServiceUI
    {

        string _BaseUri;
        JsonSerializerOptions _jsonSerializerOptions;

        private readonly IHttpClientFactory _httpClientFactory;

        UserProfile _userProfile;

        public AppSettings _appSettings;


        public UserProfile UserProfile
        {
            get { return _userProfile; }
            set { _userProfile = value; }
        }



        public KajolBezolServiceUI(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
            this._appSettings = config.GetRequiredSection("AppSettings").Get<AppSettings>();
            _BaseUri = this._appSettings.serviceAPI;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public async Task<UserProfile> PopulateProfile(UserLogin user)
        {

            UserProfile userprofile = null;

            string json = JsonSerializer.Serialize<UserLogin>(user, _jsonSerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = new Uri(this._BaseUri + "/login");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(uri, content);


            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                userprofile = JsonSerializer.Deserialize<UserProfile>(result, this._jsonSerializerOptions);
                userprofile.UserLogin = new UserLogin
                {
                    UserName = user.UserName,
                    PassCode = user.PassCode,
                    DeviceKey = user.DeviceKey
                };

            }

            _userProfile = userprofile;

            return _userProfile;

        }



        public async Task<List<TradeRequest>> InsertTradeTransaction(string transactionType, string amount)
        {

            List<TradeRequest> tradeRequests = new List<TradeRequest>();

            string result = null;

            var uri = new Uri(this._BaseUri + $"/{transactionType}?amount={amount}");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userProfile.Token);

            var response = await client.PostAsync(uri, new StringContent("{}", Encoding.UTF8, "application/json"));


            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                tradeRequests = JsonSerializer.Deserialize<List<TradeRequest>>(result);

                await this.PopulateProfile(this.UserProfile.UserLogin);

            }

            return tradeRequests;

        }


        public async Task<bool> IsTradingOpen()
        {

            bool result = false;

            var uri = new Uri(this._BaseUri + $"/IsTradingOpen");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userProfile.Token);

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    result = bool.Parse(await response.Content.ReadAsStringAsync());
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }






            return result;

        }


        public async Task<int> GetAppVersion()
        {


            string result = string.Empty;

            var uri = new Uri(this._BaseUri + $"/AppVersion");

            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return int.Parse(result);

        }

    }
}
