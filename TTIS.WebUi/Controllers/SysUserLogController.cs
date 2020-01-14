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
    public class SysUserLogController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public SysUserLogController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
            _security = security;
        }

        #endregion

        #region Method

        public IActionResult Index()
        {
            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.ATRL_USR_LOG_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> SysUserLogListAsync(string p_sKeyword, int draw, int start, int length)
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysUserLog/DataTable", await _gitternsHttpClient.GetClient(), dictParam));

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

        public async Task<IActionResult> GetSysUserLogAsync(string p_iId)
        {
            SysUserLog oReturn = new SysUserLog();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId);

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysUserLog/SysUserLog", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<SysUserLog>(response.JsonData);
                }

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete]
        public async Task<ActionResult> PurgeAsync()
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("SysUserLog/PurgeLog", await _gitternsHttpClient.GetClient(), new Dictionary<string, string>()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(SysUserLog p_oSysUserLog, string[] chkMember)
        {
            try
            {
                return Json(new JavaScriptSerializer().Deserialize<RDCustomResponse>(PostAsync("SysUserLog/PostSysUserLog", await _gitternsHttpClient.GetClient(), p_oSysUserLog, chkMember)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
