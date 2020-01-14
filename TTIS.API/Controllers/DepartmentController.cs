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
    public class MasDepartmentController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.MAS_DEPT;
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public MasDepartmentController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("Departments")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasDepartmentCollection oDepartmentList = new MasDepartmentCollection(_dbContext);
                oDepartmentList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oDepartmentList);
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
        [Route("Department")]
        public ActionResult<string> GetDepartment(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasDepartment oDepartment = new MasDepartment();

            try
            {
                oDepartment = _dbContext.MasDepartment.Where(o => o.DepartmentId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oDepartment);
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
        public ActionResult<IEnumerable<string>> MasDepartment(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasDepartmentCollection oDepartmentList = new MasDepartmentCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oDepartmentList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oDepartmentList.totalRecord,
                    recordsTotal = oDepartmentList.totalRecord,
                    data = oDepartmentList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostDepartment")]
        public IActionResult PostDepartment()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDepartment masDepartment = new MasDepartment();

            try
            {

                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    MasDepartment p_oMasDepartment = new JavaScriptSerializer().Deserialize<MasDepartment>(jsonObject);
                    bIsExist = _dbContext.MasDepartment.Any(o => o.DepartmentId == p_oMasDepartment.DepartmentId);

                    masDepartment = bIsExist ? _dbContext.MasDepartment.Where(o => o.DepartmentId == p_oMasDepartment.DepartmentId).FirstOrDefault() : new MasDepartment();
                    sPrevDetail = JsonConvert.SerializeObject(p_oMasDepartment);

                    masDepartment.Name = p_oMasDepartment.Name;
                    masDepartment.Description = p_oMasDepartment.Description;

                    masDepartment.Version = !bIsExist ? 1 : (masDepartment.Version + 1);
                    masDepartment.CreateDate = !bIsExist ? DateTime.Today : masDepartment.CreateDate;
                    masDepartment.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masDepartment.CreateByUserId;
                    masDepartment.UpdateDate = DateTime.Today;
                    masDepartment.UpdateByUserId = _requestor.IUser().Id;

                    sDetail = JsonConvert.SerializeObject(masDepartment);

                    if (!bIsNeedApproval)
                    {
                        if (!bIsExist) _dbContext.MasDepartment.Add(masDepartment);
                        else _dbContext.MasDepartment.Update(masDepartment);
                        _dbContext.SaveChanges();

                        sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                    }
                    else
                    {
                        int iMemberId = !bIsExist
                                        ? (int)Enumeration.ModuleObjectMember.MAS_DEPT_ADD
                                        : (int)Enumeration.ModuleObjectMember.MAS_DEPT_EDIT;
                        _dbContext.MasDepartment.Where(o => o.DepartmentId == masDepartment.DepartmentId).FirstOrDefault().IsNeedApproval = true;

                        SysApproval sysApproval = new SysApproval();
                        sysApproval.ApprovalId = NewGuid();
                        sysApproval.ModuleObjectId = m_iModuleObjectId;
                        sysApproval.ModuleObjectMemberId = iMemberId;
                        sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                        sysApproval.ReffObj = "Department";
                        sysApproval.ReffId = masDepartment.DepartmentId.ToString();
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
                                    _requestor.IpAddress(), masDepartment.DepartmentId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
        [Route("DeleteDepartment")]
        public ActionResult<string> DeleteDepartment(int id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasDepartment oDepartment = new MasDepartment();

            try
            {
                oDepartment = _dbContext.MasDepartment.Where(o => o.DepartmentId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oDepartment);

                oDepartment.IsNeedApproval = bIsNeedApproval;
                oDepartment.IsDeleted = true;
                oDepartment.UpdateByUserId = _requestor.IUser().Id;
                oDepartment.UpdateDate = DateTime.Now;

                if (!bIsNeedApproval)
                {
                    _dbContext.MasDepartment.Update(oDepartment);
                    _dbContext.SaveChanges();

                    sDetail = JsonConvert.SerializeObject(oDepartment);
                    bIsSuccess = true;
                    sMessage = "Data berhasil dihapus";
                }
                else
                {
                    int iMemberId =  (int)Enumeration.ModuleObjectMember.MAS_DEPT_DELETE;
                    _dbContext.MasDepartment.Where(o => o.DepartmentId == oDepartment.DepartmentId).FirstOrDefault().IsNeedApproval = true;

                    SysApproval sysApproval = new SysApproval();
                    sysApproval.ApprovalId = NewGuid();
                    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    sysApproval.ModuleObjectMemberId = iMemberId;
                    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    sysApproval.ReffObj = "Department";
                    sysApproval.ReffId = oDepartment.DepartmentId.ToString();
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
                                    _requestor.IpAddress(), oDepartment.DepartmentId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
                //                    _requestor.IpAddress(), oDepartment.DepartmentId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
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
