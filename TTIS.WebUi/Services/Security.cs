using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Common;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;

namespace TTIS.WebUi.Services
{
    public class Security : RDController, ISecurity
    {
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AspNetUsers _iUser = new AspNetUsers();

        public IConfiguration Configuration { get; }

        public Security(IGitternsHttpClient gitternsHttpClient, IHttpContextAccessor httpContextAccessor)
        {
            _gitternsHttpClient = gitternsHttpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsHaveAccessRight(int moduleObjectMemberId)
        {
            bool bIsHaveAccess = false;
            string sMyModules = _httpContextAccessor.HttpContext.Session.GetString("sessMyModules");
            SysModule masModule = new SysModule();
            SysModuleObject masModuleObject = new SysModuleObject();

            try
            {
                if (!string.IsNullOrEmpty(sMyModules))
                {
                    List<SysModule> masModules = JsonConvert.DeserializeObject<List<SysModule>>(sMyModules);
                    masModule = masModules.Any(o => o.ModuleId == Convert.ToInt32(Convert.ToString(moduleObjectMemberId).Substring(0, 2)))
                                ? masModules.Where(o => o.ModuleId == Convert.ToInt32(Convert.ToString(moduleObjectMemberId).Substring(0, 2))).FirstOrDefault()
                                : new SysModule();
                    masModuleObject = masModule.SysModuleObject.Count > 0 && masModule.SysModuleObject != null
                                    ? masModule.SysModuleObject.Where(o => o.ModuleObjectId == Convert.ToInt32(Convert.ToString(moduleObjectMemberId).Substring(0, 4))).FirstOrDefault()
                                    : new SysModuleObject();

                    bIsHaveAccess = masModuleObject != null
                                    ? masModuleObject.SysModuleObjectMember.Any(o => o.ModuleObjectMemberId == moduleObjectMemberId)
                                    : false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return bIsHaveAccess;
        }
    }
}
