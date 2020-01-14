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
    public class VehicleUserController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public VehicleUserController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
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

            getEmployees();
            getVehicleStatus();
            getVehicleUsageTypes();
            getDocumentPosition();

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.VEH_USR_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> VehicleUserListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicleUser/DataTable", await _gitternsHttpClient.GetClient(), dictParam));
                
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

        public async Task<IActionResult> VehicleUserHistoryListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicleUser/History", await _gitternsHttpClient.GetClient(), dictParam));

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

        public async Task<IActionResult> GetVehicleUserAsync(string p_iId)
        {
            VehicleDetail oReturn = new VehicleDetail();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicleUser/VehicleUser", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<VehicleDetail>(response.JsonData);
                }

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId.ToString());
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("MasVehicleUser/DeleteVehicleUser", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(VehicleDetail p_oVehicleDetail)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("MasVehicleUser/PostVehicleUser", await _gitternsHttpClient.GetClient(), p_oVehicleDetail));
                response = response == null ? new RDCustomResponse() : response;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public void getEmployees()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Employee/Employees", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("Employees", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getVehicleUsageTypes()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasVehicleUser/VehicleUsageTypes", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("VehicleUsageTypes", response.JsonData);
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
