using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    [Authorize]
    public class DriverAssistantController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;

        #endregion

        #region Constructor

        public DriverAssistantController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
        }

        #endregion

        #region Method

        public async Task<IActionResult> Index()
        {
            httpClient = await _gitternsHttpClient.GetClient();

            return View();
        }

        public async Task<IActionResult> GetDriverAssistantAsync(string p_sTagNumber)
        {
            HttpContext.Session.SetString("sessDriverAssistantDocumentList", string.Empty);
            httpClient = await _gitternsHttpClient.GetClient();

            MasDriverAssistant DriverAssistant = new MasDriverAssistant();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sTagNumber", p_sTagNumber);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("DriverAssistant/DriverAssistantByTag", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    DriverAssistant = JsonConvert.DeserializeObject<MasDriverAssistant>(response.JsonData);
                }
                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> DriverAssistantListAsync(string p_sKeyword, int draw, int start, int length)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("DriverAssistant/DataTable", httpClient, dictParam));

                return Json(new
                {
                    draw = response.DataTable.draw,
                    recordsFiltered = response.DataTable.recordsFiltered,
                    recordsTotal = response.DataTable.recordsTotal,
                    data = response.DataTable.data
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(MasDriverAssistant p_oDriverAssistant)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                foreach (IFormFile oFile in HttpContext.Request.Form.Files)
                {
                    formFiles.Add(oFile);
                }
                
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("DriverAssistant/PostDriverAssistant", httpClient, p_oDriverAssistant, formFiles));
                response = response == null ? new RDCustomResponse() : response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId.ToString());
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("DriverAssistant/DeleteDriverAssistant", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

        #region Custom Method

        //public void getDriverAssistantStatusList()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("DriverAssistantStatus/DriverAssistantStatusList", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("DriverAssistantStatusList", response.JsonData);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        #endregion
    }
}
