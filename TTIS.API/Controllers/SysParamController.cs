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
    public class SysParamController : RDController
    {
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public SysParamController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [Route("SysParamList")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;
                SysParamCollection oSysParamList = new SysParamCollection(_dbContext);
                oSysParamList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oSysParamList);
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
        [Route("SysParam")]
        public ActionResult<string> GetSysParam(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysParam oSysParam = new SysParam();

            try
            {
                oSysParam = _dbContext.SysParam.Where(o => o.Code == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oSysParam);
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
        public ActionResult<IEnumerable<string>> SysParam(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysParamCollection oSysParamList = new SysParamCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oSysParamList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oSysParamList.totalRecord,
                    recordsTotal = oSysParamList.totalRecord,
                    data = oSysParamList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [Route("PostSysParam")]
        [HttpPost]
        public IActionResult PostSysParam()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            SysParam SysParam = new SysParam();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    SysParam p_oSysParam = new JavaScriptSerializer().Deserialize<SysParam>(form[RDConstanta.HttpContentType.JsonObject]);
                    bIsExist = _dbContext.SysParam.Any(o => o.Code == p_oSysParam.Code);
                    SysParam = !bIsExist ? new SysParam() : _dbContext.SysParam.Where(o => o.Code == p_oSysParam.Code).FirstOrDefault();
                    sPrevDetail = JsonConvert.SerializeObject(SysParam);

                    SysParam.Value = p_oSysParam.Value;
                    sDetail = JsonConvert.SerializeObject(SysParam);

                    if (!bIsExist) _dbContext.SysParam.Add(SysParam);
                    else _dbContext.SysParam.Update(SysParam);
                    _dbContext.SaveChanges();

                    sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
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

                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), SysParam.Code, Enumeration.ModuleObject.PRE_PARAM.ToString(),
                                    (int)Enumeration.ModuleObjectMember.PRE_PARAM_EDIT, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

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
