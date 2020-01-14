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
    public class SysParamController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public SysParamController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
            _security = security;
        }

        #endregion

        #region Method

        public IActionResult Index()
        {
            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.PRE_PARAM_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> SysParamListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysParam/DataTable", await _gitternsHttpClient.GetClient(), dictParam));

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

        public async Task<IActionResult> GetSysParamAsync(string p_iId)
        {
            SysParam oReturn = new SysParam();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysParam/SysParam", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<SysParam>(response.JsonData);
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
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("SysParam/DeleteSysParam", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(SysParam p_oSysParam, string[] chkMember)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("SysParam/PostSysParam", httpClient, p_oSysParam, chkMember));
                response = response == null ? new RDCustomResponse() : response;

                if (response.IsSuccess)
                {
                    #region Set Sys Param Coockie

                    var option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(1);
                    RDCustomResponse respSysParam = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysParam/SysParamList", httpClient, new Dictionary<string, string>()));
                    Response.Cookies.Append("KueSysParam", respSysParam.JsonData, option);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion
    }
}
