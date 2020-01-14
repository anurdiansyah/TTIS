using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public class EmployeeStatusController : RDController
    {

        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public EmployeeStatusController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [Route("EmployeeStatusList")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasEmployeeStatusCollection oMasEmployeeStatusList = new MasEmployeeStatusCollection(_dbContext);
                oMasEmployeeStatusList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oMasEmployeeStatusList);
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

                //Logging.LogActivity(_dbContext, _requestor.IUser().Id, _requestor.IUser().UserName,
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
