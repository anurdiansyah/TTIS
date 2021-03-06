﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    public class DevicesController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;

        #endregion

        #region Constructor

        public DevicesController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
        }

        #endregion

        #region Method

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeviceListAsync(string p_sKeyword, int draw, int start, int length)
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            RDDatatable dtReturn = new RDDatatable();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                string jsonResults = GetAsync("Devices/DataTable", httpClient, dictParam);
                if (!string.IsNullOrEmpty(jsonResults))
                {
                    dtReturn = JsonConvert.DeserializeObject<RDDatatable>(jsonResults);
                }

                return Json(new
                {
                    draw = dtReturn.draw,
                    recordsFiltered = dtReturn.recordsFiltered,
                    recordsTotal = dtReturn.recordsTotal,
                    data = dtReturn.data
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> GetDevice(string p_iDevicesId)
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            MasDevice oReturn = new MasDevice();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iDevicesId);

                string jsonResults = GetAsync("Devices/Device", httpClient, dictParam);
                if (!string.IsNullOrEmpty(jsonResults))
                {
                    oReturn = JsonConvert.DeserializeObject<MasDevice>(jsonResults);
                }

                return Json(oReturn);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> PutAsync(MasDevice p_oDevices)
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                RDCustomResponse response = new JavaScriptSerializer().Deserialize<RDCustomResponse>(PutAsync("Devices/PutAll", httpClient, p_oDevices, new List<IFormFile>()));

                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = response.Message
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
