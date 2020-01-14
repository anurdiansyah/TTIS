using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using TTIS.API.Common;

using TTIS.API.Models;

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SysModuleController : RDController
    {
        private readonly TTISDbContext _dbContext;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public SysModuleController(TTISDbContext context, LoggingContext loggingContext)
        {
            _dbContext = context;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [Route("Modules")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(int? p_iModuleId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                SysModuleCollection oModuleList = new SysModuleCollection(_dbContext);
                oModuleList.List(p_iModuleId);

                jsonData = JsonConvert.SerializeObject(oModuleList);
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("Module")]
        public ActionResult<string> GetModule(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysModule oModule = new SysModule();

            try
            {
                oModule = _dbContext.SysModule.Where(o => o.ModuleId == id).FirstOrDefault();
                oModule.SysModuleObject = _dbContext.SysModuleObject.Where(o => o.ModuleId == id).ToList();
                foreach (SysModuleObject sysModuleObject in oModule.SysModuleObject)
                {
                    sysModuleObject.SysModuleObjectMember = _dbContext.SysModuleObjectMember.Where(o => o.ModuleObjectId == sysModuleObject.ModuleObjectId).OrderBy(o => o.IndexOrder).ToList();
                }

                jsonData = JsonConvert.SerializeObject(oModule);
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("MyModule")]
        public IActionResult GetMyModule(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            List<SysModule> sysModules = new List<SysModule>();

            try
            {
                List<int> myRoles = new List<int>();
                foreach (var o in _dbContext.MasUserRole.Where(o => o.AspNetUserId == id))
                {
                    myRoles.Add(o.RoleAccessId);
                }

                foreach (MyModule myModule in _dbContext.MyModule.Where(o => myRoles.Contains(o.RoleAccessId)))
                {
                    SysModule oModule = new SysModule();
                    if (!sysModules.Any(o => o.ModuleId == myModule.ModuleId))
                    {
                        oModule.ModuleId = myModule.ModuleId;
                        oModule.ModuleName = myModule.ModuleName;
                        oModule.Icon = myModule.ModuleIcon;
                        oModule.DefaultUrl = myModule.ModuleDefaultUrl;
                        oModule.IndexOrder = myModule.ModuleIndexOrder;
                        sysModules.Add(oModule);
                    }
                }
                sysModules = sysModules.OrderBy(o => o.IndexOrder).ToList();

                foreach (SysModule oModule in sysModules)
                {
                    oModule.SysModuleObject = oModule.SysModuleObject == null ? new List<SysModuleObject>() : oModule.SysModuleObject;
                    foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.ModuleId == oModule.ModuleId && myRoles.Contains(o.RoleAccessId)).ToList())
                    {
                        if (!oModule.SysModuleObject.Any(o => o.ModuleObjectId == myModule.ModuleObjectId))
                        {
                            SysModuleObject oModuleObject = new SysModuleObject();

                            oModuleObject.ModuleId = myModule.ModuleId;
                            oModuleObject.ModuleObjectId = myModule.ModuleObjectId;
                            oModuleObject.ObjectName = myModule.ModuleObjectName;
                            oModuleObject.Icon = myModule.ModuleObjectIcon;
                            oModuleObject.DefaultUrl = myModule.ModuleObjectDefaultUrl;
                            oModuleObject.IndexOrder = myModule.ModuleObjectIndexOrder;

                            oModule.SysModuleObject.Add(oModuleObject);
                        }
                    }
                    oModule.SysModuleObject = oModule.SysModuleObject.OrderBy(o => o.IndexOrder).ToList();
                }

                foreach (SysModule oModule in sysModules)
                {
                    foreach (SysModuleObject oModuleObject in oModule.SysModuleObject)
                    {
                        oModuleObject.SysModuleObjectMember = oModuleObject.SysModuleObjectMember == null ? new List<SysModuleObjectMember>() : oModuleObject.SysModuleObjectMember;
                        foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.ModuleId == oModule.ModuleId && o.ModuleObjectId == oModuleObject.ModuleObjectId && myRoles.Contains(o.RoleAccessId)).ToList())
                        {
                            if (!oModuleObject.SysModuleObjectMember.Any(o => o.ModuleObjectMemberId == myModule.ModuleObjectMemberId))
                            {
                                SysModuleObjectMember oModuleObjectMember = new SysModuleObjectMember();

                                oModuleObjectMember.ModuleObjectMemberId = myModule.ModuleObjectMemberId;
                                oModuleObjectMember.ModuleId = myModule.ModuleId;
                                oModuleObjectMember.ModuleObjectId = myModule.ModuleObjectId;
                                oModuleObjectMember.MemberName = myModule.ModuleObjectMemberName;

                                oModuleObject.SysModuleObjectMember.Add(oModuleObjectMember);
                            }
                        }
                        oModuleObject.SysModuleObjectMember = oModuleObject.SysModuleObjectMember.OrderBy(o => o.IndexOrder).ToList();
                    }
                }

                jsonData = JsonConvert.SerializeObject(sysModules.OrderBy(o => o.IndexOrder).ToList());
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("RoleModule")]
        public ActionResult<string> GetRoleModule(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            List<SysModule> sysModules = new List<SysModule>();
            try
            {
                foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.RoleAccessId == id))
                {
                    SysModule oModule = new SysModule();
                    if (!sysModules.Any(o => o.ModuleId == myModule.ModuleId))
                    {
                        oModule.ModuleId = myModule.ModuleId;
                        oModule.ModuleName = myModule.ModuleName;
                        oModule.Icon = myModule.ModuleIcon;
                        oModule.DefaultUrl = myModule.ModuleDefaultUrl;
                        oModule.IndexOrder = myModule.ModuleIndexOrder;
                        sysModules.Add(oModule);
                    }
                }
                sysModules = sysModules.OrderBy(o => o.IndexOrder).ToList();

                foreach (SysModule oModule in sysModules)
                {
                    oModule.SysModuleObject = oModule.SysModuleObject == null ? new List<SysModuleObject>() : oModule.SysModuleObject;
                    foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.ModuleId == oModule.ModuleId && o.RoleAccessId == id).ToList())
                    {
                        if (!oModule.SysModuleObject.Any(o => o.ModuleObjectId == myModule.ModuleObjectId))
                        {
                            SysModuleObject oModuleObject = new SysModuleObject();

                            oModuleObject.ModuleId = myModule.ModuleId;
                            oModuleObject.ModuleObjectId = myModule.ModuleObjectId;
                            oModuleObject.ObjectName = myModule.ModuleObjectName;
                            oModuleObject.Icon = myModule.ModuleObjectIcon;
                            oModuleObject.DefaultUrl = myModule.ModuleObjectDefaultUrl;
                            oModuleObject.IndexOrder = myModule.ModuleObjectIndexOrder;

                            oModule.SysModuleObject.Add(oModuleObject);
                        }
                        oModule.SysModuleObject = oModule.SysModuleObject.OrderBy(o => o.IndexOrder).ToList();
                    }
                }

                foreach (SysModule oModule in sysModules)
                {
                    foreach (SysModuleObject oModuleObject in oModule.SysModuleObject)
                    {
                        oModuleObject.SysModuleObjectMember = oModuleObject.SysModuleObjectMember == null ? new List<SysModuleObjectMember>() : oModuleObject.SysModuleObjectMember;
                        foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.ModuleId == oModule.ModuleId && o.ModuleObjectId == oModuleObject.ModuleObjectId && o.RoleAccessId == id).ToList())
                        {
                            if (!oModuleObject.SysModuleObjectMember.Any(o => o.ModuleObjectMemberId == myModule.ModuleObjectMemberId))
                            {
                                SysModuleObjectMember oModuleObjectMember = new SysModuleObjectMember();

                                oModuleObjectMember.ModuleObjectMemberId = myModule.ModuleObjectMemberId;
                                oModuleObjectMember.ModuleId = myModule.ModuleId;
                                oModuleObjectMember.ModuleObjectId = myModule.ModuleObjectId;
                                oModuleObjectMember.MemberName = myModule.ModuleObjectMemberName;

                                oModuleObject.SysModuleObjectMember.Add(oModuleObjectMember);
                            }
                        }
                        oModuleObject.SysModuleObjectMember = oModuleObject.SysModuleObjectMember.OrderBy(o => o.IndexOrder).ToList();

                    }
                }

                jsonData = JsonConvert.SerializeObject(sysModules.OrderBy(o => o.IndexOrder).ToList());
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("ApprovalModules")]
        public IActionResult ApprovalModule(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            
            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.SysModuleObject.Where(o=>o.IsNeedApproval == true).ToList());
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }


    }
}
