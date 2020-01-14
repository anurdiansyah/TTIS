using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Common;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;

namespace TTIS.WebUi.Services
{
    public class AppUser : RDController, IAppUser
    {
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetUsers _iUser = new AspNetUsers();

        public AppUser(IGitternsHttpClient gitternsHttpClient, IHttpContextAccessor httpContextAccessor)
        {
            _gitternsHttpClient = gitternsHttpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AspNetUsers> CurrentUser()
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();

            string sessIUser = _httpContextAccessor.HttpContext.Session.GetString("sessIUser");
            string sIUserId = _httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == JwtClaimTypes.Subject).Value;
            if (string.IsNullOrEmpty(sessIUser))
            {
                dictParam = new Dictionary<string, string>();
                dictParam.Add("id", sIUserId);
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("AspNetUsers/AspNetUser", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    _iUser = JsonConvert.DeserializeObject<AspNetUsers>(response.JsonData);
                    _httpContextAccessor.HttpContext.Session.SetString("sessIUser", response.JsonData);
                }
            }
            else
            {
                _iUser = JsonConvert.DeserializeObject<AspNetUsers>(sessIUser);
            }

            return _iUser;
        }
    }
}
