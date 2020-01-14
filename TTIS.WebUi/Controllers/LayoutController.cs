using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    public class LayoutController : RDController
    {
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly IAppUser _appUser;

        public LayoutController(IGitternsHttpClient gitternsHttpClient, IAppUser appUser)
        {
            _gitternsHttpClient = gitternsHttpClient;
            _appUser = appUser;

        }

        [HttpGet]
        public async Task<ActionResult> MyModuleAsync()
        {
            bool IsUpdated = false;
            AspNetUsers currentUser = await _appUser.CurrentUser();
            List<SysModule> masModules = new List<SysModule>();
            Dictionary<string, string> dictParam = new Dictionary<string, string>();
            Dictionary<string, string> dictVersions = new Dictionary<string, string>();
            Dictionary<string, string> dictCurrentVersions = new Dictionary<string, string>();
            httpClient = await _gitternsHttpClient.GetClient();

            try
            {
                if(HttpContext.Session.Get("sessIUser") != null)
                {
                    dictParam.Add("id", currentUser.Id);
                    string sMyModules = HttpContext.Session.GetString("sessMyModules");
                    string sRoleVersions = HttpContext.Session.GetString("sessRoleVersions");

                    RDCustomResponse respVersion = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("MasRoleAccess/MyRoleVersions", httpClient, dictParam));
                    dictVersions = JsonConvert.DeserializeObject<Dictionary<string, string>>(respVersion.JsonData);
                    dictCurrentVersions = !string.IsNullOrEmpty(sRoleVersions)
                                            ? JsonConvert.DeserializeObject<Dictionary<string, string>>(sRoleVersions)
                                            : new Dictionary<string, string>();
                    IsUpdated = dictVersions.OrderBy(kvp => kvp.Key).SequenceEqual(dictCurrentVersions.OrderBy(kvp => kvp.Key));
                    HttpContext.Session.SetString("sessRoleVersions", respVersion.JsonData);

                    if (string.IsNullOrEmpty(sMyModules) || !IsUpdated)
                    {
                        RDCustomResponse responseModule = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysModule/MyModule", httpClient, dictParam));
                        if (!string.IsNullOrEmpty(responseModule.JsonData))
                        {
                            HttpContext.Session.SetString("sessMyModules", responseModule.JsonData);
                            masModules = JsonConvert.DeserializeObject<List<SysModule>>(responseModule.JsonData);
                        }
                    }
                    else
                    {
                        masModules = JsonConvert.DeserializeObject<List<SysModule>>(sMyModules);
                    }

                    #region Get Sys Param List

                    var option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(1);
                    if (HttpContext.Request.Cookies["KueSysParam"] == null)
                    {
                        RDCustomResponse respSysParam = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysParam/SysParamList", httpClient, dictParam));
                        Response.Cookies.Append("KueSysParam", respSysParam.JsonData, option);
                    }

                    #endregion
                }
                else
                {
                    RedirectToAction("Logout", "Auth");
                }

            }
            catch (Exception e)
            {
                RedirectToAction("Logout", "Auth");
            }
            finally
            {
                dictParam = null;
                dictVersions = null;
                dictCurrentVersions = null;
            }

            return Json(masModules);
        }
    }
}