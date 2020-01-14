using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TTIS.WebUi.Configuration;
using TTIS.WebUi.Models;
using TTIS.WebUi.Models.Helper;

namespace TTIS.WebUi.Services
{
    public class GitternsHttpClient : RDController, IGitternsHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient = new HttpClient();
        public IConfiguration Configuration { get; }
        public AppSettings appSettings { get; set; }

        public GitternsHttpClient(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            appSettings = configuration.Get<AppSettings>();
        }

        public GitternsHttpClient()
        {
        }

        public async Task<HttpClient> GetClient()
        {
            JwtToken jwtToken = new JwtToken();

            try
            {
                _httpClient.BaseAddress = new Uri(appSettings.TtsiApi.ApiUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("AspNetUserId", CurrentUserId(_httpContextAccessor.HttpContext).ToString());

                string sessJwtToken = _httpContextAccessor.HttpContext.Session.GetString("sessJwtToken");
                if (string.IsNullOrEmpty(sessJwtToken))
                {
                    jwtToken = await RefreshTokenAsync();
                }
                else
                {
                    jwtToken = JsonConvert.DeserializeObject<JwtToken>(sessJwtToken);
                    if (DateTime.Now > jwtToken.ExpiredTime)
                    {
                        jwtToken = await RefreshTokenAsync();
                    }
                }

                _httpContextAccessor.HttpContext.Session.SetString("sessJwtToken", JsonConvert.SerializeObject(jwtToken));
                _httpClient.SetBearerToken(jwtToken.AccessToken);

            }
            catch (Exception e)
            {
                throw e;
            }

            return this._httpClient;
        }

        public async Task<JwtToken> RefreshTokenAsync()
        {
            TokenResponse jwtResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = appSettings.TtsiApi.AuthUrl + "/connect/token",

                ClientId = appSettings.TtsiApi.ClientId,
                ClientSecret = appSettings.TtsiApi.ClientSecret,
                Scope = appSettings.TtsiApi.Scope
            });

            return new JwtToken(jwtResponse.AccessToken, jwtResponse.ExpiresIn, DateTime.Now.AddSeconds(Convert.ToInt32(jwtResponse.ExpiresIn))); ;
        }
    }
}
