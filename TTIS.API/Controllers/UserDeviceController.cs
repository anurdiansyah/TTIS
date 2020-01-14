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

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserDeviceController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.SCR_USR_DVC;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public UserDeviceController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("UserDevices")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasUserDeviceCollection oUserDeviceList = new MasUserDeviceCollection(_dbContext);
                oUserDeviceList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oUserDeviceList);
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
        [Route("UserDevice")]
        public ActionResult<string> GetUserDevice(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasUserDevice oUserDevice = new MasUserDevice();

            try
            {
                oUserDevice = _dbContext.MasUserDevice.Where(o => o.UserDeviceId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oUserDevice);
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
        public ActionResult<IEnumerable<string>> MasUserDevice(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasUserDeviceDetailCollection oUserDeviceList = new MasUserDeviceDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oUserDeviceList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oUserDeviceList.totalRecord,
                    recordsTotal = oUserDeviceList.totalRecord,
                    data = oUserDeviceList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostUserDevice")]
        public IActionResult PostUserDevice()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bUserIsFound = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasUserDevice masUserDevice = new MasUserDevice();

            try
            {

                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    MasUserDevice p_oMasUserDevice = new JavaScriptSerializer().Deserialize<MasUserDevice>(jsonObject);
                    MasEmployee masEmployee = _dbContext.MasEmployee.Where(o => o.EmployeeId == p_oMasUserDevice.EmployeeId).FirstOrDefault();
                    bIsExist = _dbContext.MasUserDevice.Any(o => o.Imei == p_oMasUserDevice.Imei && o.IsActive);
                    bUserIsFound = _dbContext.MasUserDetail.Any(o => o.TagNumber == masEmployee.TagNumber);

                    if (bUserIsFound)
                    {

                        masUserDevice = bIsExist ? _dbContext.MasUserDevice.Where(o => o.UserDeviceId == p_oMasUserDevice.UserDeviceId).FirstOrDefault() : new MasUserDevice();
                        sPrevDetail = JsonConvert.SerializeObject(masUserDevice);

                        masUserDevice.UserDeviceId = bIsExist ? masUserDevice.UserDeviceId : NewGuid();
                        masUserDevice.AspNetUserId = Guid.Parse(_dbContext.MasUserDetail.Where(o => o.TagNumber == masEmployee.TagNumber).FirstOrDefault().AspNetUserId);
                        masUserDevice.EmployeeId = p_oMasUserDevice.EmployeeId;
                        masUserDevice.Imei = p_oMasUserDevice.Imei;

                        masUserDevice.IsActive = true;
                        masUserDevice.CreateDate = !bIsExist ? DateTime.Today : masUserDevice.CreateDate;
                        masUserDevice.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masUserDevice.CreateByUserId;
                        masUserDevice.UpdateDate = DateTime.Today;
                        masUserDevice.UpdateByUserId = _requestor.IUser().Id;

                        sDetail = JsonConvert.SerializeObject(masUserDevice);

                        if (!bIsNeedApproval)
                        {
                            if (!bIsExist) _dbContext.MasUserDevice.Add(masUserDevice);
                            else _dbContext.MasUserDevice.Update(masUserDevice);
                            _dbContext.SaveChanges();

                            sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                        }
                        else
                        {
                            int iMemberId = !bIsExist
                                            ? (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_ADD
                                            : (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_EDIT;
                            _dbContext.MasUserDevice.Where(o => o.UserDeviceId == masUserDevice.UserDeviceId).FirstOrDefault().IsNeedApproval = true;

                            SysApproval sysApproval = new SysApproval();
                            sysApproval.ApprovalId = NewGuid();
                            sysApproval.ModuleObjectId = m_iModuleObjectId;
                            sysApproval.ModuleObjectMemberId = iMemberId;
                            sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                            sysApproval.ReffObj = "UserDevice";
                            sysApproval.ReffId = masUserDevice.UserDeviceId.ToString();
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
                    else
                    {
                        sMessage = "Karyawan Dengan Nama : '" + masEmployee.FirstName + " " + masEmployee.MiddleName + " " + masEmployee.LastName + "' \n Tidak terdaftar sebagai Pengguna GITTerns";
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
                if (bUserIsFound)
                {
                    #region Log User

                    int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_EDIT : (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_ADD;
                    Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                        _requestor.IpAddress(), masUserDevice.UserDeviceId.ToString(), Enumeration.ModuleObject.SCR_USR_DVC.ToString(),
                                        iModuleObjectMember, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                    #endregion
                }
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
        [Route("DeleteUserDevice")]
        public ActionResult<string> DeleteUserDevice(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasUserDevice oUserDevice = new MasUserDevice();

            try
            {
                oUserDevice = _dbContext.MasUserDevice.Where(o => o.UserDeviceId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oUserDevice);

                oUserDevice.IsNeedApproval = bIsNeedApproval;
                oUserDevice.IsActive = false;
                oUserDevice.UpdateByUserId = _requestor.IUser().Id;
                oUserDevice.UpdateDate = DateTime.Now;

                if (!bIsNeedApproval)
                {
                    _dbContext.MasUserDevice.Update(oUserDevice);
                    _dbContext.SaveChanges();

                    sDetail = JsonConvert.SerializeObject(oUserDevice);
                    bIsSuccess = true;
                    sMessage = "Data berhasil dihapus";
                }
                else
                {
                    int iMemberId =  (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_DELETE;
                    _dbContext.MasUserDevice.Where(o => o.UserDeviceId == oUserDevice.UserDeviceId).FirstOrDefault().IsNeedApproval = true;

                    SysApproval sysApproval = new SysApproval();
                    sysApproval.ApprovalId = NewGuid();
                    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    sysApproval.ModuleObjectMemberId = iMemberId;
                    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    sysApproval.ReffObj = "UserDevice";
                    sysApproval.ReffId = oUserDevice.UserDeviceId.ToString();
                    sysApproval.Detail = sDetail;
                    sysApproval.PreviousDetail = sPrevDetail;
                    sysApproval.Remark = string.Empty;
                    sysApproval.Version = 1;
                    sysApproval.CreateDate = sysApproval.UpdateDate = DateTime.Now;
                    sysApproval.CreateByUserId = sysApproval.UpdateByUserId = _requestor.IUser().Id;
                    _dbContext.SysApproval.Add(sysApproval);
                    _dbContext.SaveChanges();

                    sMessage = "Permohonan penghapusan data berhasil";
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

                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), oUserDevice.UserDeviceId.ToString(), Enumeration.ModuleObject.SCR_USR_DVC.ToString(),
                                    (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
        [Route("Positions")]
        public IActionResult Positions()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                List<UserLatestGeoLoc> userLatestGeoLoc = _dbContext.UserLatestGeoLoc.ToList();

                jsonData = JsonConvert.SerializeObject(userLatestGeoLoc);
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
            finally
            {
                #region Log User

                //Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                //                    _requestor.IpAddress(), oUserDevice.UserDeviceId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                //                    (int)Enumeration.ModuleObjectMember.SCR_USR_DVC_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
