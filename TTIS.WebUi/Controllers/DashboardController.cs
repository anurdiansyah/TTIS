using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RD.Lib;
using TTIS.WebUi.Common;

namespace TTIS.WebUi.Controllers
{
    [Authorize]
    public class DashboardController : RDController
    {
        private readonly IGitternsHttpClient _gitternsHttpClient;

        public DashboardController(IGitternsHttpClient gitternsHttpClient)
        {
            _gitternsHttpClient = gitternsHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            httpClient = await _gitternsHttpClient.GetClient();

            string sTest = GetSysParamValue("GENERAL_ERROR_MESSAGE");
            return View();
        }

        public async Task<IActionResult> GetPositionsAsync()
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();

            List<UserLatestGeoLoc> userLatestGeoLoc = new List<UserLatestGeoLoc>();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("UserDevice/Positions", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData)) userLatestGeoLoc = JsonConvert.DeserializeObject<List<UserLatestGeoLoc>>(response.JsonData);

                return Json(userLatestGeoLoc);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
