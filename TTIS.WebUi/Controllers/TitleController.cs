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
    public class TitleController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;
        
        #endregion

        #region Constructor

        public TitleController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
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

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.MAS_TITLE_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> TitleListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasTitle/DataTable", await _gitternsHttpClient.GetClient(), dictParam));
                
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
        
        public async Task<IActionResult> GetTitleAsync(string p_iId)
        {
            MasTitle oReturn = new MasTitle();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasTitle/Title", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<MasTitle>(response.JsonData);
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
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("MasTitle/DeleteTitle", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(MasTitle p_oTitle)
        {
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("MasTitle/PostTitle", await _gitternsHttpClient.GetClient(), p_oTitle));
                response = response == null ? new RDCustomResponse() : response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

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

        #endregion

    }
}
