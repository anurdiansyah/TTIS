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
    public class AspNetUsersController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        #endregion

        #region Constructor

        public AspNetUsersController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
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
            getUserRoleAccess();

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.SCR_USR_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> AspNetUserListAsync(string p_sKeyword, int draw, int start, int length)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                string responseString = GetAsync("AspNetUsers/DataTable", await _gitternsHttpClient.GetClient(), dictParam);
                response = JsonConvert.DeserializeObject<RDCustomResponse>(responseString);

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

        public async Task<IActionResult> GetAspNetUser(string p_iAspNetUsersId)
        {
            RDCustomResponse response = new RDCustomResponse();
            HttpContext.Session.SetString("sessMasRoleAccess", "");
            AspNetUsers oReturn = new AspNetUsers();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iAspNetUsersId);

                response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("AspNetUsers/AspNetUser", await _gitternsHttpClient.GetClient(), dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    oReturn = JsonConvert.DeserializeObject<AspNetUsers>(response.JsonData);
                    HttpContext.Session.SetString("sessMasRoleAccess", JsonConvert.SerializeObject(oReturn.RoleAccess));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(AspNetUsers p_oAspNetUsers)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                string sessMasRoleAccess = HttpContext.Session.GetString("sessMasRoleAccess");
                List<MasRoleAccess> masRoleAccess = !string.IsNullOrEmpty(sessMasRoleAccess)
                                                    ? JsonConvert.DeserializeObject<List<MasRoleAccess>>(sessMasRoleAccess)
                                                    : new List<MasRoleAccess>();
                p_oAspNetUsers.RoleAccess = masRoleAccess;

                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("AspNetUsers/PostAspNetUsers", await _gitternsHttpClient.GetClient(), p_oAspNetUsers));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string p_iAspNetUsersId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iAspNetUsersId);
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("AspNetUsers/DeleteAspNetUsers", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateEmployee(string p_sTagNumber)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sTagNumber", p_sTagNumber);
                response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("AspNetUsers/ValidateUserByTag", await _gitternsHttpClient.GetClient(), dictParam));

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeDetail(string p_sTagNumber)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sTagNumber", p_sTagNumber);
                response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("AspNetUsers/EmployeeDetailByTag", await _gitternsHttpClient.GetClient(), dictParam));

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult UserRoleAccessList(int draw, int start, int length)
        {
            List<MasRoleAccess> masRoleAccess = new List<MasRoleAccess>();
            try
            {
                string sessMasRoleAccess = HttpContext.Session.GetString("sessMasRoleAccess");
                if (!string.IsNullOrEmpty(sessMasRoleAccess)) masRoleAccess = JsonConvert.DeserializeObject<List<MasRoleAccess>>(sessMasRoleAccess);

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = masRoleAccess.Count,
                    recordsTotal = masRoleAccess.Count,
                    data = masRoleAccess
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddRoleAccess(int p_iRoleAccessId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<MasRoleAccess> masRoleAccess = new List<MasRoleAccess>();

            try
            {
                string sessMasRoleAccess = HttpContext.Session.GetString("sessMasRoleAccess");
                if (!string.IsNullOrEmpty(sessMasRoleAccess)) masRoleAccess = JsonConvert.DeserializeObject<List<MasRoleAccess>>(sessMasRoleAccess);

                if (!masRoleAccess.Any(o => o.RoleAccessId == p_iRoleAccessId))
                {
                    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                    dictParam.Add("id", p_iRoleAccessId.ToString());
                    RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasRoleAccess/RoleAccess", await _gitternsHttpClient.GetClient(), dictParam));
                    if (response.IsSuccess && !string.IsNullOrEmpty(response.JsonData)) masRoleAccess.Add(JsonConvert.DeserializeObject<MasRoleAccess>(response.JsonData));

                    HttpContext.Session.SetString("sessMasRoleAccess", JsonConvert.SerializeObject(masRoleAccess));
                    bIsSuccess = true;
                }
                else
                {
                    sMessage = "The Role Access you try to add, is already exist for this user.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new
            {
                IsSuccess = bIsSuccess,
                Message = sMessage
            });
        }

        [HttpPost]
        public ActionResult DeleteRoleAccess(int p_iRoleAccessId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            List<MasRoleAccess> masRoleAccess = new List<MasRoleAccess>();

            try
            {
                string sessMasRoleAccess = HttpContext.Session.GetString("sessMasRoleAccess");
                if (!string.IsNullOrEmpty(sessMasRoleAccess)) masRoleAccess = JsonConvert.DeserializeObject<List<MasRoleAccess>>(sessMasRoleAccess);
                masRoleAccess.Remove(masRoleAccess.Where(o => o.RoleAccessId == p_iRoleAccessId).FirstOrDefault());
                HttpContext.Session.SetString("sessMasRoleAccess", JsonConvert.SerializeObject(masRoleAccess));

                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new
            {
                IsSuccess = bIsSuccess,
                Message = sMessage
            });
        }

        #endregion

        #region Custom Method

        public void getUserRoleAccess()
        {
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasRoleAccess/RoleAccessList", httpClient, dictParam));
                if (!string.IsNullOrEmpty(response.JsonData))
                {
                    HttpContext.Session.SetString("sessUserRoleList", response.JsonData);
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
