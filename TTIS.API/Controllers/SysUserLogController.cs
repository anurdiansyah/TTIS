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
    public class SysUserLogController : RDController
    {
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public SysUserLogController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [Route("SysUserLogList")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;
                SysUserLogCollection oSysUserLogList = new SysUserLogCollection(_dbContext);
                oSysUserLogList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oSysUserLogList);
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
        [Route("SysUserLog")]
        public ActionResult<string> GetSysUserLog(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysUserLogDetail oSysUserLog = new SysUserLogDetail();

            try
            {
                oSysUserLog = _dbContext.VSysUserLogDetail.Where(o => o.UserLogId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oSysUserLog);
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

        [Route("DataTable")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> SysUserLog(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysUserLogDetailCollection oSysUserLogList = new SysUserLogDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oSysUserLogList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oSysUserLogList.totalRecord,
                    recordsTotal = oSysUserLogList.totalRecord,
                    data = oSysUserLogList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [Route("PurgeLog")]
        [HttpDelete]
        public ActionResult<IEnumerable<string>> PurgeLog(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                _dbContext.SysUserLog
                  .ToList()
                  .ForEach(o => o.IsDeleted = true);
                _dbContext.SaveChanges();

                bIsSuccess = true;
                sMessage = "User log purged successfully";
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
                                    _requestor.IpAddress(), string.Empty, Enumeration.ModuleObject.ATRL_USR_LOG.ToString(),
                                    (int)Enumeration.ModuleObjectMember.ATRL_USR_LOG_PURGE, ReferenceNumber(), 
                                    string.Empty, string.Empty, bIsSuccess, "User log purged successfully");

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
