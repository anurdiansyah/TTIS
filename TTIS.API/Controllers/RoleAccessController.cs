using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using RD.Lib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTIS.API.Common;

using TTIS.API.Models;
using TTIS.API.Services;

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MasRoleAccessController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.SCR_GRP;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public MasRoleAccessController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [HttpGet]
        [Route("RoleAccessList")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;
                MasRoleAccessCollection oRoleAccessList = new MasRoleAccessCollection(_dbContext);
                oRoleAccessList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oRoleAccessList);
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
        [Route("RoleAccess")]
        public ActionResult<string> GetRoleAccess(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasRoleAccess oRoleAccess = new MasRoleAccess();

            try
            {
                oRoleAccess = _dbContext.MasRoleAccess.Where(o => o.RoleAccessId == id).FirstOrDefault();

                oRoleAccess.SysModules = oRoleAccess.SysModules == null ? new List<SysModule>() : oRoleAccess.SysModules;
                foreach (MyModule myModule in _dbContext.MyModule.Where(o => o.RoleAccessId == id))
                {
                    SysModule oModule = new SysModule();
                    if (!oRoleAccess.SysModules.Any(o => o.ModuleId == myModule.ModuleId))
                    {
                        oModule.ModuleId = myModule.ModuleId;
                        oModule.ModuleName = myModule.ModuleName;
                        oModule.Icon = myModule.ModuleIcon;
                        oModule.DefaultUrl = myModule.ModuleDefaultUrl;
                        oModule.IndexOrder = myModule.ModuleIndexOrder;
                        oRoleAccess.SysModules.Add(oModule);
                    }
                }

                foreach (SysModule oModule in oRoleAccess.SysModules)
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
                    }
                }

                foreach (SysModule oModule in oRoleAccess.SysModules)
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

                    }
                }

                jsonData = JsonConvert.SerializeObject(oRoleAccess);
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
        [Route("DataTable")]
        public ActionResult<IEnumerable<string>> MasRoleAccess(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasRoleAccessCollection oRoleAccessList = new MasRoleAccessCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oRoleAccessList.List(sKeyword, iSkip, iLength);
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }

            return Json(new
            {
                DataTable = new
                {
                    draw = draw,
                    recordsFiltered = oRoleAccessList.totalRecord,
                    recordsTotal = oRoleAccessList.totalRecord,
                    data = oRoleAccessList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostRoleAccess")]
        public IActionResult PostRoleAccess()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasRoleAccess masRoleAccess = new MasRoleAccess();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    MasRoleAccess p_oMasRoleAccess = new JavaScriptSerializer().Deserialize<MasRoleAccess>(form[RDConstanta.HttpContentType.JsonObject]);
                    bIsExist = _dbContext.MasRoleAccess.Any(o => o.RoleAccessId == p_oMasRoleAccess.RoleAccessId);
                    masRoleAccess = !bIsExist ? new MasRoleAccess() : _dbContext.MasRoleAccess.Where(o => o.RoleAccessId == p_oMasRoleAccess.RoleAccessId).FirstOrDefault();
                    sPrevDetail = JsonConvert.SerializeObject(masRoleAccess);

                    masRoleAccess.RoleCode = p_oMasRoleAccess.RoleCode;
                    masRoleAccess.RoleName = p_oMasRoleAccess.RoleName;
                    masRoleAccess.RoleDescription = p_oMasRoleAccess.RoleDescription;
                    masRoleAccess.IsNeedApproval = bIsNeedApproval;
                    masRoleAccess.IsActive = true;
                    masRoleAccess.Version = !bIsExist ? 1 : (masRoleAccess.Version + 1);
                    masRoleAccess.CreateDate = !bIsExist ? DateTime.Today : masRoleAccess.CreateDate;
                    masRoleAccess.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masRoleAccess.CreateByUserId;
                    masRoleAccess.UpdateDate = DateTime.Today;
                    masRoleAccess.UpdateByUserId = _requestor.IUser().Id;

                    masRoleAccess.MasRoleAccessLitem = !bIsExist ? new List<MasRoleAccessLitem>() : _dbContext.MasRoleAccessLitem.Where(o => o.RoleAccessId == p_oMasRoleAccess.RoleAccessId).ToList();
                    _dbContext.MasRoleAccessLitem.RemoveRange(masRoleAccess.MasRoleAccessLitem);
                    masRoleAccess.MasRoleAccessLitem.Clear();

                    string[] chkMember = new JavaScriptSerializer().Deserialize<string[]>(form[RDConstanta.HttpContentType.JsonStringArray]);
                    foreach (string s in chkMember)
                    {
                        string[] sMemberInfo = s.Split("*");
                        MasRoleAccessLitem oLitem = new MasRoleAccessLitem();
                        oLitem.ModuleId = Convert.ToInt32(sMemberInfo[0]);
                        oLitem.ModuleObjectId = Convert.ToInt32(sMemberInfo[1]);
                        oLitem.ModuleObjectMemberId = Convert.ToInt32(sMemberInfo[2]);

                        masRoleAccess.MasRoleAccessLitem.Add(oLitem);
                    }
                    sDetail = JsonConvert.SerializeObject(masRoleAccess);

                    if (!bIsNeedApproval)
                    {
                        if (!bIsExist) _dbContext.MasRoleAccess.Add(masRoleAccess);
                        else _dbContext.MasRoleAccess.Update(masRoleAccess);
                        _dbContext.SaveChanges();

                        sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                    }
                    else
                    {
                        int iMemberId = !bIsExist 
                                        ? (int)Enumeration.ModuleObjectMember.SCR_GRP_ADD 
                                        : (int)Enumeration.ModuleObjectMember.SCR_GRP_EDIT;

                        SysApproval sysApproval = new SysApproval();
                        sysApproval.ApprovalId = NewGuid();
                        sysApproval.ModuleObjectId = m_iModuleObjectId;
                        sysApproval.ModuleObjectMemberId = iMemberId;
                        sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                        sysApproval.ReffObj = "Role Access";
                        sysApproval.ReffId = p_oMasRoleAccess.RoleAccessId.ToString();
                        sysApproval.Detail = sDetail;
                        sysApproval.PreviousDetail = sPrevDetail;
                        sysApproval.Remark = string.Empty;
                        sysApproval.Version = 1;
                        sysApproval.CreateDate = sysApproval.UpdateDate = DateTime.Now;
                        sysApproval.CreateByUserId = sysApproval.UpdateByUserId = _requestor.IUser().Id;
                        _dbContext.SysApproval.Add(sysApproval);
                        _dbContext.SaveChanges();

                        sMessage = bIsExist ? "Permohonan perubahan data berhasil" : "Permohonan penambahan data berhasil";
                    }
                    bIsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                #region Log User

                int iMemberId = !bIsExist ? (int)Enumeration.ModuleObjectMember.SCR_GRP_ADD : (int)Enumeration.ModuleObjectMember.SCR_GRP_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masRoleAccess.RoleAccessId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                                    iMemberId, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                #endregion
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

        [HttpDelete]
        [Route("DeleteRoleAccess")]
        public ActionResult<string> DeleteRoleAccess(int id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasRoleAccess oRoleAccess = new MasRoleAccess();

            try
            {
                oRoleAccess = _dbContext.MasRoleAccess.Where(o => o.RoleAccessId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oRoleAccess);

                oRoleAccess.IsDeleted = true;
                oRoleAccess.UpdateByUserId = _requestor.IUser().Id;
                oRoleAccess.UpdateDate = DateTime.Now;
                _dbContext.MasRoleAccess.Update(oRoleAccess);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oRoleAccess);
                bIsSuccess = true;
                sMessage = "Data berhasil dihapus";
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                #region Log User

                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), oRoleAccess.RoleAccessId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                                    (int)Enumeration.ModuleObjectMember.SCR_GRP_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                #endregion
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
        [Route("MyRoleAccess")]
        public ActionResult<IEnumerable<string>> MyRoleAccess(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                List<int> myRoles = new List<int>();
                foreach (var o in _dbContext.MasUserRole.Where(o => o.AspNetUserId == id))
                {
                    myRoles.Add(o.RoleAccessId);
                }

                jsonData = JsonConvert.SerializeObject(_dbContext.MasRoleAccess.Where(o => myRoles.Contains(o.RoleAccessId)));
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
        [Route("MyRoleVersions")]
        public ActionResult<IEnumerable<string>> MyRoleVersion(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                Dictionary<string, string> myRoleVersions = new Dictionary<string, string>();
                foreach (var o in _dbContext.MasUserRole.Where(o => o.AspNetUserId == id))
                {
                    myRoleVersions.Add(o.RoleAccessId.ToString(), _dbContext.MasRoleAccess.Where(p => p.RoleAccessId == p.RoleAccessId).FirstOrDefault().Version.ToString());
                }

                jsonData = JsonConvert.SerializeObject(myRoleVersions);
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

        [HttpPost]
        [Route("EmptyAction")]
        public IActionResult EmptyAction()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {

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
