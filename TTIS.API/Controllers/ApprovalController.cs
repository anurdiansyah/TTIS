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
using TTIS.API.Services;
using TTIS.API.UsersModels;

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SysApprovalController : RDController
    {
        private readonly IS4UsersContext _usersContext;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public SysApprovalController(IS4UsersContext usersContext, TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _usersContext = usersContext;
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("SysApprovals")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                SysApprovalCollection oSysApprovalList = new SysApprovalCollection(_dbContext);
                oSysApprovalList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oSysApprovalList);
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
        [Route("SysApproval")]
        public ActionResult<string> GetSysApproval(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysApprovalDetail oSysApproval = new SysApprovalDetail();

            try
            {
                oSysApproval = _dbContext.SysApprovalDetail.Where(o => o.ApprovalId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oSysApproval);
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
        public ActionResult<IEnumerable<string>> SysApproval(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysApprovalDetailCollection oSysApprovalList = new SysApprovalDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oSysApprovalList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oSysApprovalList.totalRecord,
                    recordsTotal = oSysApprovalList.totalRecord,
                    data = oSysApprovalList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostSysApproval")]
        public IActionResult PostSysApproval()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            SysApproval sysApproval = new SysApproval();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    SysApprovalDetail sysApprovalDetail = new JavaScriptSerializer().Deserialize<SysApprovalDetail>(jsonObject);
                    sysApproval =_dbContext.SysApproval.Where(o=>o.ApprovalId == sysApprovalDetail.ApprovalId).FirstOrDefault();
                    sPrevDetail = JsonConvert.SerializeObject(sysApprovalDetail);

                    bIsSuccess = ProcessApproval(sysApprovalDetail);

                    if (bIsSuccess)
                    {
                        sysApproval.ApprovalStatusId = sysApprovalDetail.ApprovalStatusId;
                        sysApproval.Version = sysApproval.Version + 1;
                        sysApproval.UpdateDate = DateTime.Today;
                        sysApproval.UpdateByUserId = _requestor.IUser().Id;

                        sDetail = JsonConvert.SerializeObject(sysApproval);
                        _dbContext.SysApproval.Update(sysApproval);
                        _dbContext.SaveChanges();

                        sMessage = sysApprovalDetail.ApprovalStatusId == (int)Enumeration.ApprovalStatus.Approved
                                            ? "Data " + sysApprovalDetail.ActionName + " berhasil di Approve"
                                            : "Data " + sysApprovalDetail.ActionName + " berhasil di Tolak";
                        bIsSuccess = true;
                    }
                };
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                #region Log User

                int iModuleObjectMember = (int)Enumeration.ModuleObjectMember.SCR_APPR_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), sysApproval.ApprovalId.ToString(), Enumeration.ModuleObject.SCR_APPR.ToString(),
                                    iModuleObjectMember, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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

        private bool ProcessApproval(SysApprovalDetail sysApprovalDetail)
        {
            bool bIsExistItem = false;
            bool bIsSuccess = false;
            bool bIsApproved = sysApprovalDetail.ApprovalStatusId == (int)Enumeration.ApprovalStatus.Approved;

            try
            {
                switch (sysApprovalDetail.ModuleObjectId)
                {
                    case (int)Enumeration.ModuleObject.SCR_USR:
                        AspNetUsers aspNetUsers = bIsApproved
                                      ? JsonConvert.DeserializeObject<AspNetUsers>(sysApprovalDetail.Detail)
                                      : JsonConvert.DeserializeObject<AspNetUsers>(sysApprovalDetail.PreviousDetail);
                        bIsExistItem = _usersContext.AspNetUsers.Any(o => o.Id == aspNetUsers.Id);
                        aspNetUsers.PhoneNumberConfirmed = true;

                        if (bIsExistItem) _usersContext.AspNetUsers.Update(aspNetUsers);
                        else if (bIsApproved) _usersContext.AspNetUsers.Add(aspNetUsers);

                        _usersContext.SaveChanges();
                        bIsSuccess = true;
                        break;

                    case (int)Enumeration.ModuleObject.SCR_GRP:
                        MasRoleAccess masRoleAccess = bIsApproved 
                                      ? JsonConvert.DeserializeObject<MasRoleAccess>(sysApprovalDetail.Detail) 
                                      : JsonConvert.DeserializeObject<MasRoleAccess>(sysApprovalDetail.PreviousDetail);
                        bIsExistItem = _dbContext.MasRoleAccess.Any(o => o.RoleAccessId == masRoleAccess.RoleAccessId);
                        masRoleAccess.IsNeedApproval = false;
                        masRoleAccess.Version += 1;
                        masRoleAccess.UpdateDate = DateTime.Now;
                        masRoleAccess.UpdateByUserId = _requestor.IUser().Id;

                        if (bIsExistItem) _dbContext.MasRoleAccess.Update(masRoleAccess);
                        else if (bIsApproved) _dbContext.MasRoleAccess.Add(masRoleAccess);

                        bIsSuccess = true;
                        break;

                    case (int)Enumeration.ModuleObject.PRE_PARAM:
                        SysParam sysParam = bIsApproved
                                      ? JsonConvert.DeserializeObject<SysParam>(sysApprovalDetail.Detail)
                                      : JsonConvert.DeserializeObject<SysParam>(sysApprovalDetail.PreviousDetail);
                        bIsExistItem = _dbContext.SysParam.Any(o => o.Code == sysParam.Code);
                        sysParam.IsNeedApproval = false;

                        if (bIsExistItem) _dbContext.SysParam.Update(sysParam);
                        else if (!bIsExistItem && bIsApproved) _dbContext.SysParam.Add(sysParam);

                        bIsSuccess = true;
                        break;

                    case (int)Enumeration.ModuleObject.VEH_USR:
                        bool bUsedVehicleBefore = false;

                        MasVehicleUser masVehicleUser = JsonConvert.DeserializeObject<MasVehicleUser>(sysApprovalDetail.Detail);
                        MasVehicle masVehicle = new MasVehicle();
                        masVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == masVehicleUser.VehicleId).FirstOrDefault();
                        masVehicle.IsNeedApproval = false;
                        masVehicle.Version += 1;
                        masVehicle.UpdateDate = DateTime.Now;
                        masVehicle.UpdateByUserId = _requestor.IUser().Id;

                        bUsedVehicleBefore = _dbContext.MasVehicleUser.Any(o => o.VehicleId == masVehicleUser.VehicleId && o.IsActive == true);

                        if (bUsedVehicleBefore)
                        {
                            MasVehicleUser oldVehicleUser = _dbContext.MasVehicleUser.Where(o => o.VehicleId == masVehicleUser.VehicleId && o.IsActive == true && o.IsDeleted == false).FirstOrDefault();
                            oldVehicleUser.IsActive = false;
                            oldVehicleUser.DateTo = DateTime.Now;
                            oldVehicleUser.Version += 1;
                            oldVehicleUser.UpdateDate = DateTime.Now;
                            oldVehicleUser.UpdateByUserId = _requestor.IUser().Id;

                            _dbContext.MasVehicleUser.Update(masVehicleUser);
                        }

                        if (bIsApproved)
                        {
                            masVehicle.VehicleStatusId = (int)Enumeration.VehicleStatus.Dalam_Penggunaan;
                            _dbContext.MasVehicleUser.Add(masVehicleUser);
                        }

                        _dbContext.MasVehicle.Update(masVehicle);

                        bIsSuccess = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return bIsSuccess;
        }

        [HttpDelete]
        [Route("DeleteSysApproval")]
        public ActionResult<string> DeleteSysApproval(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysApproval oSysApproval = new SysApproval();

            try
            {
                oSysApproval = _dbContext.SysApproval.Where(o => o.ApprovalId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oSysApproval);

                oSysApproval.IsDeleted = true;
                oSysApproval.UpdateByUserId = _requestor.IUser().Id;
                oSysApproval.UpdateDate = DateTime.Now;
                _dbContext.SysApproval.Update(oSysApproval);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oSysApproval);
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
                                    _requestor.IpAddress(), oSysApproval.ApprovalId.ToString(), Enumeration.ModuleObject.SCR_APPR.ToString(),
                                    (int)Enumeration.ModuleObjectMember.SCR_APPR_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
            finally
            {
                #region Log User

                //Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                //                    _requestor.IpAddress(), oSysApproval.SysApprovalId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                //                    (int)Enumeration.ModuleObjectMember.SCR_APPR_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
    }
}
