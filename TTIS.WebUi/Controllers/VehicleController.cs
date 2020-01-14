using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using TTIS.WebUi.Common;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    [Authorize]
    public class VehicleController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public VehicleController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
            _security = security;
        }


        #endregion

        #region Method

        public async Task<IActionResult> Index()
        {
            httpClient = await _gitternsHttpClient.GetClient();

            getVehicleType();
            getVehicleModel();
            getVehicleStatus();
            getFuel();
            getDocumentPosition();

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.VEH_MAS_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");

        }

        public async Task<IActionResult> VehicleListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/DataTable", await _gitternsHttpClient.GetClient(), dictParam));
                
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
        
        public async Task<IActionResult> GetVehicleAsync(string p_iId)
        {
            MasVehicle oReturn = new MasVehicle();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/Vehicle", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<MasVehicle>(response.JsonData);
                }

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("MasVehicle/DeleteVehicle", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(MasVehicle p_oVehicle)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                foreach (IFormFile oFile in HttpContext.Request.Form.Files)
                {
                    formFiles.Add(oFile);
                }

                List<MasVehicleType> masVehicleType = JsonConvert.DeserializeObject<List<MasVehicleType>>(HttpContext.Session.GetString("VehicleTypes"));
                List<MasVehicleModel> masVehicleModel = JsonConvert.DeserializeObject<List<MasVehicleModel>>(HttpContext.Session.GetString("VehicleModels"));
                
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("MasVehicle/PostVehicle", await _gitternsHttpClient.GetClient(), p_oVehicle, formFiles));
                response = response == null ? new RDCustomResponse() : response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        public void getVehicleType()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/VehicleType", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("VehicleTypes", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getVehicleModel()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/VehicleModel", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("VehicleModels", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult getModelByType(string p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                string sVehicleModels = HttpContext.Session.GetString("VehicleModels");
                bool bIsSuccess = !string.IsNullOrEmpty(sVehicleModels);

                response.JsonData = sVehicleModels;
                response.IsSuccess = bIsSuccess;
                response.Message = bIsSuccess ? string.Empty : "Tidak ditemukan currentEmployee kendaraan untuk tipe yang dipilih";
                response.StatusCode = bIsSuccess ? 200 : 400;
            }
            catch (Exception e)
            {
                response.Message = e.ToString();
            }

            return Json(response);
        }

        public void getVehicleStatus()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/VehicleStatus", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("VehicleStatuses", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getFuel()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicle/Fuel", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("Fuels", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getDocumentPosition()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Sys/DocumentPosition", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("DocumentPositions", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #endregion

    }
}
