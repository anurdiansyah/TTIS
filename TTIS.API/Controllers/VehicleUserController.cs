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
    public class MasVehicleUserController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.VEH_USR;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        string sVehiclePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\");
        string sVehicleThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\thumbs\\");

        public MasVehicleUserController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("VehicleUsers")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasVehicleUserCollection oVehicleUserList = new MasVehicleUserCollection(_dbContext);
                oVehicleUserList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oVehicleUserList);
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
        [Route("VehicleUser")]
        public ActionResult<string> GetVehicleUser(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleDetail oVehicleUser = new VehicleDetail();

            try
            {
                oVehicleUser = _dbContext.VehicleDetail.Where(o => o.VehicleId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oVehicleUser);
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
        public ActionResult<IEnumerable<string>> MasVehicleUser(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleUserDetailCollection oVehicleUserList = new VehicleUserDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oVehicleUserList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oVehicleUserList.totalRecord,
                    recordsTotal = oVehicleUserList.totalRecord,
                    data = oVehicleUserList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("History")]
        public ActionResult<IEnumerable<string>> MasVehicleUserHistory(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleUserHistoryCollection oVehicleUserHistoryList = new VehicleUserHistoryCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oVehicleUserHistoryList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oVehicleUserHistoryList.totalRecord,
                    recordsTotal = oVehicleUserHistoryList.totalRecord,
                    data = oVehicleUserHistoryList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostVehicleUser")]
        public IActionResult PostVehicleUser()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasVehicleUser oldVehicleUser = new MasVehicleUser();
            MasVehicleUser newVehicleUser = new MasVehicleUser();
            MasVehicle masVehicle = new MasVehicle();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    VehicleDetail p_oVehicleDetail = new JavaScriptSerializer().Deserialize<VehicleDetail>(jsonObject);
                    bIsExist = _dbContext.MasVehicleUser.Any(o => o.VehicleId == p_oVehicleDetail.VehicleId && o.IsActive == true && o.IsDeleted == false);

                    if (bIsExist)
                    {
                        sPrevDetail = JsonConvert.SerializeObject(oldVehicleUser);

                        oldVehicleUser = _dbContext.MasVehicleUser.Where(o => o.VehicleId == p_oVehicleDetail.VehicleId && o.IsActive == true && o.IsDeleted == false).FirstOrDefault();
                        oldVehicleUser.IsActive = false;
                        oldVehicleUser.DateTo = DateTime.Now;
                        oldVehicleUser.Version = !bIsExist ? 1 : (oldVehicleUser.Version + 1);
                        oldVehicleUser.CreateDate = !bIsExist ? DateTime.Now : oldVehicleUser.CreateDate;
                        oldVehicleUser.CreateByUserId = !bIsExist ? _requestor.IUser().Id : oldVehicleUser.CreateByUserId;
                        oldVehicleUser.UpdateDate = DateTime.Now;
                        oldVehicleUser.UpdateByUserId = _requestor.IUser().Id;
                    }

                    newVehicleUser.VehicleUserId = NewGuid();
                    newVehicleUser.VehicleId = p_oVehicleDetail.VehicleId;
                    newVehicleUser.EmployeeId = p_oVehicleDetail.VehicleUsageTypeId == (int)Enumeration.VehicleUsageType.TidakDigunakan
                                                ? new Guid()
                                                : (Guid)p_oVehicleDetail.EmployeeId;
                    newVehicleUser.VehicleUsageTypeId = p_oVehicleDetail.VehicleUsageTypeId;
                    newVehicleUser.DateFrom = DateTime.Now;
                    newVehicleUser.DateTo = DateTime.Now;
                    newVehicleUser.IsActive = true;
                    newVehicleUser.Version = !bIsExist ? 1 : (oldVehicleUser.Version + 1);
                    newVehicleUser.CreateDate = !bIsExist ? DateTime.Now : oldVehicleUser.CreateDate;
                    newVehicleUser.CreateByUserId = !bIsExist ? _requestor.IUser().Id : oldVehicleUser.CreateByUserId;
                    newVehicleUser.UpdateDate = DateTime.Now;
                    newVehicleUser.UpdateByUserId = _requestor.IUser().Id;

                    sDetail = JsonConvert.SerializeObject(newVehicleUser);
                    if (bIsExist)
                    {
                        _dbContext.MasVehicleUser.Update(oldVehicleUser);
                        _dbContext.MasVehicleUser.Add(newVehicleUser);
                    }
                    else
                    {
                        _dbContext.MasVehicleUser.Add(newVehicleUser);
                    }

                    masVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == p_oVehicleDetail.VehicleId).FirstOrDefault();
                    masVehicle.VehicleStatusId = p_oVehicleDetail.VehicleUsageTypeId == (int)Enumeration.VehicleUsageType.TidakDigunakan
                                                ? (int)Enumeration.VehicleStatus.Siap_Digunakan
                                                : (int)Enumeration.VehicleStatus.Dalam_Penggunaan;
                    masVehicle.Version += 1;
                    masVehicle.UpdateDate = DateTime.Now;
                    masVehicle.UpdateByUserId = _requestor.IUser().Id;
                    _dbContext.MasVehicle.Update(masVehicle);

                    _dbContext.SaveChanges();
                    sMessage = "Penyimpanan data berhasil";
                    bIsSuccess = true;
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

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.VEH_USR_EDIT : (int)Enumeration.ModuleObjectMember.VEH_USR_ADD;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), oldVehicleUser.VehicleUserId.ToString(), Enumeration.ModuleObject.VEH_USR.ToString(),
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

        [HttpDelete]
        [Route("DeleteVehicleUser")]
        public ActionResult<string> DeleteVehicleUser(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicleUser oVehicleUser = new MasVehicleUser();

            try
            {
                oVehicleUser = _dbContext.MasVehicleUser.Where(o => o.VehicleUserId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oVehicleUser);

                oVehicleUser.IsDeleted = true;
                oVehicleUser.UpdateByUserId = _requestor.IUser().Id;
                oVehicleUser.UpdateDate = DateTime.Now;
                _dbContext.MasVehicleUser.Update(oVehicleUser);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oVehicleUser);
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
                                    _requestor.IpAddress(), oVehicleUser.VehicleUserId.ToString(), Enumeration.ModuleObject.VEH_USR.ToString(),
                                    (int)Enumeration.ModuleObjectMember.VEH_USR_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
                //                    _requestor.IpAddress(), oVehicleUser.VehicleUserId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                //                    (int)Enumeration.ModuleObjectMember.VEH_USR_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
        [Route("VehicleUsageTypes")]
        public ActionResult<string> GetVehicleUsageType()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.SysVehicleUsageType.ToList());
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
        [Route("RequestUsage")]
        public ActionResult<string> RequestUsage(string p_sVehicleCode)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;
            VehicleDetail vehicleDetail = new VehicleDetail();

            try
            {
                if (_dbContext.MasVehicle.Any(o => o.VehicleCode == p_sVehicleCode))
                {
                    vehicleDetail = _dbContext.VehicleDetail.Where(o => o.VehicleCode == p_sVehicleCode).FirstOrDefault();
                    bool bIsAvailable = vehicleDetail.VehicleStatusId == (int)Enumeration.VehicleStatus.Siap_Digunakan;
                    bool bIsOnApprovalProcess = vehicleDetail.IsNeedApproval;

                    if (bIsAvailable && !bIsOnApprovalProcess)
                    {
                        MasVehicleUser newVehicleUser = new MasVehicleUser();
                        newVehicleUser.VehicleUserId = NewGuid();
                        newVehicleUser.VehicleId = vehicleDetail.VehicleId;
                        newVehicleUser.EmployeeId = (Guid)_requestor.IUser().UserDetail.EmployeeDetail.EmployeeId;
                        newVehicleUser.VehicleUsageTypeId = (int)Enumeration.VehicleUsageType.Temporary;
                        newVehicleUser.DateFrom = DateTime.Now;
                        newVehicleUser.DateTo = DateTime.Now;
                        newVehicleUser.IsActive = true;
                        newVehicleUser.Version = 1;
                        newVehicleUser.CreateDate = DateTime.Now;
                        newVehicleUser.CreateByUserId = _requestor.IUser().Id;
                        newVehicleUser.UpdateDate = DateTime.Now;
                        newVehicleUser.UpdateByUserId = _requestor.IUser().Id;

                        if (!bIsNeedApproval)
                        {
                            MasVehicle masVehicle = new MasVehicle();
                            masVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == vehicleDetail.VehicleId).FirstOrDefault();
                            masVehicle.VehicleStatusId = (int)Enumeration.VehicleStatus.Dalam_Penggunaan;
                            masVehicle.UpdateDate = DateTime.Now;
                            masVehicle.UpdateByUserId = _requestor.IUser().Id;

                            _dbContext.MasVehicleUser.Add(newVehicleUser);
                            _dbContext.MasVehicle.Update(masVehicle);
                            _dbContext.SaveChanges();

                            sMessage = "Permintaan penggunaan kendaraan berhasil";
                        }
                        else
                        {
                            MasVehicle requestedVehicle = new MasVehicle();
                            requestedVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == vehicleDetail.VehicleId).FirstOrDefault();
                            requestedVehicle.IsNeedApproval = true;
                            requestedVehicle.Version += 1;
                            requestedVehicle.UpdateDate = DateTime.Now;
                            requestedVehicle.UpdateByUserId = _requestor.IUser().Id;

                            SysApproval sysApproval = new SysApproval();
                            sysApproval.ApprovalId = NewGuid();
                            sysApproval.ApprovalCode = "VRQ/" + DateTime.Today.ToString("ddMMyy")
                                                     + "/" + requestedVehicle.VehicleCode
                                                     + "/" + RDRandom.RandomString(2, false)
                                                     + "-" + (_dbContext.SysApproval.Count() + 1).ToString().PadLeft(3, '0');
                            sysApproval.ModuleObjectId = m_iModuleObjectId;
                            sysApproval.ModuleObjectMemberId = (int)Enumeration.ModuleObjectMember.VEH_USR_ADD;
                            sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                            sysApproval.ReffObj = "Vehicle User";
                            sysApproval.ReffId = requestedVehicle.VehicleId.ToString();
                            sysApproval.Detail = JsonConvert.SerializeObject(newVehicleUser);
                            sysApproval.PreviousDetail = JsonConvert.SerializeObject(new MasVehicleUser());
                            sysApproval.Remark = string.Empty;
                            sysApproval.Version = 1;
                            sysApproval.CreateDate = sysApproval.UpdateDate = DateTime.Now;
                            sysApproval.CreateByUserId = sysApproval.UpdateByUserId = _requestor.IUser().Id;

                            _dbContext.MasVehicle.Update(requestedVehicle);
                            _dbContext.SysApproval.Add(sysApproval);
                            _dbContext.SaveChanges();

                            vehicleDetail.ApprovalCode = sysApproval.ApprovalCode;
                            sMessage = "Permohonan penggunaan kendaraan berhasil" + Environment.NewLine
                                     + "Approval Code " + Environment.NewLine
                                     + sysApproval.ApprovalCode;
                        }

                        jsonData = JsonConvert.SerializeObject(vehicleDetail);
                        bIsSuccess = true;
                    }
                    else
                    {
                        if (!bIsOnApprovalProcess)
                        {
                            sMessage = "Permintaan gagal diproses," + Environment.NewLine
                                     + "Kendaraan " + vehicleDetail.VehicleStatus + Environment.NewLine
                                     + Environment.NewLine
                                     + "Hubungi Tim Operasional untuk konfirmasi";
                        }
                        else
                        {
                            sMessage = "Permintaan gagal diproses," + Environment.NewLine
                                     + "Kendaraan sedang dalam Proses Request lainnya" + Environment.NewLine
                                     + Environment.NewLine
                                     + "Hubungi Tim Operasional untuk konfirmasi";
                        }
                    }
                }
                else
                {
                    sMessage = "Permintaan gagal diproses," + Environment.NewLine
                             + "Kendaraan belum terdaftar" + Environment.NewLine
                             + Environment.NewLine
                             + "Hubungi Tim Operasional untuk konfirmasi";
                }
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
                StatusCode = bIsSuccess
                            ? bIsNeedApproval
                                ? 201 : 200
                            : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("CheckApproval")]
        public ActionResult<string> CheckApproval(string p_sApprovalCode)
        {
            int iStatusCode = 500;
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            SysApproval sysApproval = _dbContext.SysApproval.Where(o => o.ApprovalCode == p_sApprovalCode).FirstOrDefault();

            try
            {
                sMessage = "No Approval Process";
                if (sysApproval != null)
                {
                    if (!string.IsNullOrEmpty(p_sApprovalCode))
                    {
                        if (sysApproval.ApprovalStatusId != (int)Enumeration.ApprovalStatus.New)
                        {
                            if (sysApproval.ApprovalStatusId == (int)Enumeration.ApprovalStatus.Rejected)
                            {
                                iStatusCode = 202;
                                sMessage = "Mohon Maaf, Permohonan Penggunaan Kendaraan ditolak.";
                            }
                            else
                            {
                                iStatusCode = 201;
                                sMessage = "Selamat, Permohonan Penggunaan Kendaraan telah disetujui.";

                                MasVehicleUser masVehicleUser = JsonConvert.DeserializeObject<MasVehicleUser>(sysApproval.Detail);
                                VehicleDetail masVehicle = new VehicleDetail();
                                masVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == masVehicleUser.VehicleId).FirstOrDefault();
                                masVehicle.VehicleImageBase64 = System.IO.File.Exists(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg"))
                                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg")))
                                                                : string.Empty;

                                masVehicle.ApprovalCode = sysApproval.ApprovalCode;
                                if (masVehicle.DateFrom != null) masVehicle.DateFrom = Convert.ToDateTime(masVehicle.DateFrom).ToUniversalTime();
                                if (masVehicle.DateTo != null) masVehicle.DateTo = Convert.ToDateTime(masVehicle.DateTo).ToUniversalTime();
                                masVehicle.StnkberlakuHingga = masVehicle.StnkberlakuHingga.ToUniversalTime();
                                masVehicle.KirberlakuHingga = masVehicle.KirberlakuHingga.ToUniversalTime();
                                masVehicle.CreateDate = masVehicle.CreateDate.ToUniversalTime();
                                masVehicle.UpdateDate = masVehicle.UpdateDate.ToUniversalTime();
                                jsonData = JsonConvert.SerializeObject(masVehicle);
                            }

                            bIsSuccess = true;
                        }
                        else
                        {
                            iStatusCode = 202;
                            if (sysApproval.CreateByUserId == _requestor.IUser().Id)
                            {
                                iStatusCode = 200;
                                sMessage = "Dalam Proses Persetujuan";
                            }
                        }
                    }
                }
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
                StatusCode = iStatusCode,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("CheckUsage")]
        public ActionResult<string> CheckUsage(string p_sVehicleId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            int iStatusCode = 500;
            VehicleDetail masVehicle = new VehicleDetail();

            try
            {
                if (!string.IsNullOrEmpty(p_sVehicleId))
                {
                    if (_dbContext.MasVehicleUser.Any(o => o.VehicleId == Guid.Parse(p_sVehicleId)
                                                    && o.EmployeeId == _requestor.IUser().UserDetail.EmployeeDetail.EmployeeId
                                                    && o.IsActive == true
                                                    && o.IsDeleted == false))
                    {
                        sMessage = _dbContext.MasVehicle.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault().VehicleCode + " Tardaftar sebagai kendaraan yang sedang anda gunakan ";

                        masVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault();
                        masVehicle.VehicleImageBase64 = System.IO.File.Exists(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg")))
                                                        : string.Empty;

                        iStatusCode = 200;
                        bIsSuccess = true;
                    }
                    else
                    {
                        iStatusCode = 202;
                        sMessage = "Anda sudah tidak terdaftar sebagai pengguna "
                                 + _dbContext.MasVehicle.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault().VehicleCode;
                    }
                }
                else
                {
                    if (_dbContext.MasVehicleUser.Any(o => o.EmployeeId == _requestor.IUser().UserDetail.EmployeeDetail.EmployeeId && o.IsActive == true && o.IsDeleted == false))
                    {
                        sMessage = _dbContext.MasVehicle.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault().VehicleCode + " Tardaftar sebagai kendaraan yang sedang anda gunakan ";

                        masVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault();
                        masVehicle.VehicleImageBase64 = System.IO.File.Exists(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg")))
                                                        : string.Empty;
                        if (masVehicle.DateFrom != null) masVehicle.DateFrom = Convert.ToDateTime(masVehicle.DateFrom).ToUniversalTime();
                        if (masVehicle.DateTo != null) masVehicle.DateTo = Convert.ToDateTime(masVehicle.DateTo).ToUniversalTime();

                        iStatusCode = 200;
                        bIsSuccess = true;
                    }
                    else
                    {
                        if(_dbContext.SysApproval.Any(o => o.ApprovalStatusId == (int)Enumeration.ApprovalStatus.New && o.CreateByUserId == _requestor.IUser().Id))
                        {
                            SysApproval sysApproval = _dbContext.SysApproval.Where(o => o.ApprovalStatusId == (int)Enumeration.ApprovalStatus.New && o.CreateByUserId == _requestor.IUser().Id).FirstOrDefault();
                            MasVehicleUser masVehicleUser = JsonConvert.DeserializeObject<MasVehicleUser>(sysApproval.Detail);
                            masVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == masVehicleUser.VehicleId).FirstOrDefault();
                            masVehicle.ApprovalCode = sysApproval.ApprovalCode;

                            iStatusCode = 201;
                            sMessage = "Permohonan Penggunaan ditemukan Approval Code : " + sysApproval.ApprovalCode;
                        }
                        else
                        {
                            iStatusCode = 203;
                            sMessage = "Tidak ada Permohonan Kendaraan maupun Kendaraan yang terdaftar atas nama anda.";
                        }
                    }
                }

                if (masVehicle.DateFrom != null) masVehicle.DateFrom = Convert.ToDateTime(masVehicle.DateFrom).ToUniversalTime();
                if (masVehicle.DateTo != null) masVehicle.DateTo = Convert.ToDateTime(masVehicle.DateTo).ToUniversalTime();
                masVehicle.StnkberlakuHingga = masVehicle.StnkberlakuHingga.ToUniversalTime();
                masVehicle.KirberlakuHingga = masVehicle.KirberlakuHingga.ToUniversalTime();
                masVehicle.CreateDate = masVehicle.CreateDate.ToUniversalTime();
                masVehicle.UpdateDate = masVehicle.UpdateDate.ToUniversalTime();
                jsonData = JsonConvert.SerializeObject(masVehicle);
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
                StatusCode = iStatusCode,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("CheckOutUsage")]
        public ActionResult<string> CheckOutUsage(string p_sVehicleId)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleDetail vehicleDetail = new VehicleDetail();

            try
            {
                Guid EmployeeId = _requestor.IUser().UserDetail.EmployeeDetail.EmployeeId;
                if (_dbContext.MasVehicleUser.Any(o => o.VehicleId == Guid.Parse(p_sVehicleId)
                                                    && o.IsActive == true
                                                    && o.IsDeleted == true
                                                    && o.EmployeeId == EmployeeId))
                {
                    bIsSuccess = true;
                }
                else
                {
                    sMessage = "Anda sudah tidak terdaftar sebagai pengguna "
                             + _dbContext.MasVehicle.Where(o => o.VehicleId == Guid.Parse(p_sVehicleId)).FirstOrDefault().VehicleCode;
                }
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
