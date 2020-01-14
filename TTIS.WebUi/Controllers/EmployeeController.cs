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
    public class EmployeeController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public EmployeeController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
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

            getDepartments();
            getUnit();
            getTitle();
            getEmployeeStatusList();

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.EMP_MAS_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> GetEmployeeAsync(string p_sTagNumber)
        {
            httpClient = await _gitternsHttpClient.GetClient();

            MasEmployee employee = new MasEmployee();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sTagNumber", p_sTagNumber);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Employee/EmployeeByTag", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    employee = JsonConvert.DeserializeObject<MasEmployee>(response.JsonData);
                }
                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> EmployeeListAsync(string p_sKeyword, int draw, int start, int length)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Employee/DataTable", httpClient, dictParam));

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
        public async Task<ActionResult> PostAsync(MasEmployee p_oEmployee)
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
                
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("Employee/PostEmployee", httpClient, p_oEmployee, formFiles));
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
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("Employee/DeleteEmployee", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

        #region Custom Method

        public void getDepartments()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasDepartment/Departments", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("Departments", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getUnit()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasUnit/Units", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("Units", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getTitle()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasTitle/Titles", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("Titles", response.JsonData);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void getEmployeeStatusList()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("EmployeeStatus/EmployeeStatusList", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("EmployeeStatusList", response.JsonData);
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
