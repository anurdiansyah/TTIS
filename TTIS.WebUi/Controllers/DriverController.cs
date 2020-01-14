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
    public class DriverController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        
        #endregion

        #region Constructor

        public DriverController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration)
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

        public async Task<IActionResult> GetDriverAsync(string p_sTagNumber)
        {
            HttpContext.Session.SetString("sessDriverDocumentList", string.Empty);
            httpClient = await _gitternsHttpClient.GetClient();

            MasDriver Driver = new MasDriver();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sTagNumber", p_sTagNumber);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Driver/DriverByTag", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    Driver = JsonConvert.DeserializeObject<MasDriver>(response.JsonData);
                }
                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> DriverListAsync(string p_sKeyword, int draw, int start, int length)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Driver/DataTable", httpClient, dictParam));

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
        public async Task<ActionResult> PostAsync(MasDriver p_oDriver)
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
                
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("Driver/PostDriver", httpClient, p_oDriver, formFiles));
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
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("Driver/DeleteDriver", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

        #region Custom Method

        //public void getDriverStatusList()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("DriverStatus/DriverStatusList", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("DriverStatusList", response.JsonData);
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
