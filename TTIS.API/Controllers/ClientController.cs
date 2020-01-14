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
    public class MasClientController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.MAS_DEPT;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public MasClientController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("Clients")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasClientCollection oClientList = new MasClientCollection(_dbContext);
                oClientList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oClientList);
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
        [Route("Client")]
        public ActionResult<string> GetClient(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasClient oClient = new MasClient();

            try
            {
                oClient = _dbContext.MasClient.Where(o => o.ClientId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oClient);
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
        public ActionResult<IEnumerable<string>> MasClient(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasClientCollection oClientList = new MasClientCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oClientList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oClientList.totalRecord,
                    recordsTotal = oClientList.totalRecord,
                    data = oClientList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostClient")]
        public IActionResult PostClient()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasClient masClient = new MasClient();

            try
            {

                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    MasClient p_oMasClient = new JavaScriptSerializer().Deserialize<MasClient>(jsonObject);
                    bIsExist = _dbContext.MasClient.Any(o => o.ClientId == p_oMasClient.ClientId);

                    masClient = bIsExist ? _dbContext.MasClient.Where(o => o.ClientId == p_oMasClient.ClientId).FirstOrDefault() : new MasClient();
                    sPrevDetail = JsonConvert.SerializeObject(p_oMasClient);



                    masClient.Version = !bIsExist ? 1 : (masClient.Version + 1);
                    masClient.CreateDate = !bIsExist ? DateTime.Today : masClient.CreateDate;
                    masClient.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masClient.CreateByUserId;
                    masClient.UpdateDate = DateTime.Today;
                    masClient.UpdateByUserId = _requestor.IUser().Id;

                    sDetail = JsonConvert.SerializeObject(masClient);

                    //if (!bIsNeedApproval)
                    //{
                    if (!bIsExist) _dbContext.MasClient.Add(masClient);
                    else _dbContext.MasClient.Update(masClient);
                    _dbContext.SaveChanges();

                    sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                    //}
                    //else
                    //{
                    //    int iMemberId = !bIsExist
                    //                    ? (int)Enumeration.ModuleObjectMember.MAS_DEPT_ADD
                    //                    : (int)Enumeration.ModuleObjectMember.MAS_DEPT_EDIT;
                    //    _dbContext.MasClient.Where(o => o.ClientId == masClient.ClientId).FirstOrDefault().IsNeedApproval = true;

                    //    SysApproval sysApproval = new SysApproval();
                    //    sysApproval.ApprovalId = NewGuid();
                    //    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    //    sysApproval.ModuleObjectMemberId = iMemberId;
                    //    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    //    sysApproval.ReffObj = "Client";
                    //    sysApproval.ReffId = masClient.ClientId.ToString();
                    //    sysApproval.Detail = sDetail;
                    //    sysApproval.PreviousDetail = sPrevDetail;
                    //    sysApproval.Remark = string.Empty;
                    //    sysApproval.Version = 1;
                    //    sysApproval.CreateDate = sysApproval.UpdateDate = DateTime.Now;
                    //    sysApproval.CreateByUserId = sysApproval.UpdateByUserId = _requestor.IUser().Id;
                    //    _dbContext.SysApproval.Add(sysApproval);
                    //    _dbContext.SaveChanges();

                    //    sMessage = bIsExist ? "Permohonan perubahan data berhasil" : "Permohonan penambahan data berhasil";
                    //}
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

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.MAS_DEPT_EDIT : (int)Enumeration.ModuleObjectMember.MAS_DEPT_ADD;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masClient.ClientId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
        [Route("DeleteClient")]
        public ActionResult<string> DeleteClient(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasClient oClient = new MasClient();

            try
            {
                oClient = _dbContext.MasClient.Where(o => o.ClientId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oClient);

                oClient.IsDeleted = true;
                oClient.UpdateByUserId = _requestor.IUser().Id;
                oClient.UpdateDate = DateTime.Now;

                if (!bIsNeedApproval)
                {
                    _dbContext.MasClient.Update(oClient);
                    _dbContext.SaveChanges();

                    sDetail = JsonConvert.SerializeObject(oClient);
                    bIsSuccess = true;
                    sMessage = "Data berhasil dihapus";
                }
                else
                {
                    int iMemberId = (int)Enumeration.ModuleObjectMember.MAS_DEPT_DELETE;

                    SysApproval sysApproval = new SysApproval();
                    sysApproval.ApprovalId = NewGuid();
                    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    sysApproval.ModuleObjectMemberId = iMemberId;
                    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    sysApproval.ReffObj = "Client";
                    sysApproval.ReffId = oClient.ClientId.ToString();
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
                                    _requestor.IpAddress(), oClient.ClientId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
                                    (int)Enumeration.ModuleObjectMember.MAS_DEPT_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
                //                    _requestor.IpAddress(), oClient.ClientId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                //                    (int)Enumeration.ModuleObjectMember.MAS_DEPT_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
