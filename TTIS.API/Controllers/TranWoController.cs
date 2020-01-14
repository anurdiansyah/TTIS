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
    public class TranWoController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.MAS_DEPT;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public TranWoController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("TranWos")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                TranWoCollection oTranWoList = new TranWoCollection(_dbContext);
                oTranWoList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oTranWoList);
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
        [Route("TranWo")]
        public ActionResult<string> GetTranWo(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            TranWo oTranWo = new TranWo();

            try
            {
                oTranWo = _dbContext.TranWo.Where(o => o.WorkOrderId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oTranWo);
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
        public ActionResult<IEnumerable<string>> TranWo(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            TranWoCollection oTranWoList = new TranWoCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oTranWoList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oTranWoList.totalRecord,
                    recordsTotal = oTranWoList.totalRecord,
                    data = oTranWoList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostTranWo")]
        public IActionResult PostTranWo()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            TranWo TranWo = new TranWo();

            try
            {

                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    TranWo p_oTranWo = new JavaScriptSerializer().Deserialize<TranWo>(jsonObject);
                    bIsExist = _dbContext.TranWo.Any(o => o.WorkOrderId == p_oTranWo.WorkOrderId);

                    TranWo = bIsExist ? _dbContext.TranWo.Where(o => o.WorkOrderId == p_oTranWo.WorkOrderId).FirstOrDefault() : new TranWo();
                    sPrevDetail = JsonConvert.SerializeObject(p_oTranWo);


                    TranWo.Version = !bIsExist ? 1 : (TranWo.Version + 1);
                    TranWo.CreateDate = !bIsExist ? DateTime.Today : TranWo.CreateDate;
                    TranWo.CreateByUserId = !bIsExist ? _requestor.IUser().Id : TranWo.CreateByUserId;
                    TranWo.UpdateDate = DateTime.Today;
                    TranWo.UpdateByUserId = _requestor.IUser().Id;

                    sDetail = JsonConvert.SerializeObject(TranWo);

                    if (!bIsNeedApproval)
                    {
                        if (!bIsExist) _dbContext.TranWo.Add(TranWo);
                        else _dbContext.TranWo.Update(TranWo);
                        _dbContext.SaveChanges();

                        sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                    }
                    else
                    {
                        int iMemberId = !bIsExist
                                        ? (int)Enumeration.ModuleObjectMember.MAS_DEPT_ADD
                                        : (int)Enumeration.ModuleObjectMember.MAS_DEPT_EDIT;

                        SysApproval sysApproval = new SysApproval();
                        sysApproval.ApprovalId = NewGuid();
                        sysApproval.ModuleObjectId = m_iModuleObjectId;
                        sysApproval.ModuleObjectMemberId = iMemberId;
                        sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                        sysApproval.ReffObj = "TranWo";
                        sysApproval.ReffId = TranWo.WorkOrderId.ToString();
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
                                    _requestor.IpAddress(), TranWo.WorkOrderId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
        [Route("DeleteTranWo")]
        public ActionResult<string> DeleteTranWo(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            TranWo oTranWo = new TranWo();

            try
            {
                oTranWo = _dbContext.TranWo.Where(o => o.WorkOrderId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oTranWo);

                oTranWo.IsDeleted = true;
                oTranWo.UpdateByUserId = _requestor.IUser().Id;
                oTranWo.UpdateDate = DateTime.Now;

                if (!bIsNeedApproval)
                {
                    _dbContext.TranWo.Update(oTranWo);
                    _dbContext.SaveChanges();

                    sDetail = JsonConvert.SerializeObject(oTranWo);
                    bIsSuccess = true;
                    sMessage = "Data berhasil dihapus";
                }
                else
                {
                    int iMemberId =  (int)Enumeration.ModuleObjectMember.MAS_DEPT_DELETE;

                    SysApproval sysApproval = new SysApproval();
                    sysApproval.ApprovalId = NewGuid();
                    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    sysApproval.ModuleObjectMemberId = iMemberId;
                    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    sysApproval.ReffObj = "WorkOrder";
                    sysApproval.ReffId = oTranWo.WorkOrderId.ToString();
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
                                    _requestor.IpAddress(), oTranWo.WorkOrderId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
                //                    _requestor.IpAddress(), oTranWo.TranWoId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
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
