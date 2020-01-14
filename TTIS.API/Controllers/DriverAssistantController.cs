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
    public class DriverAssistantController : RDController
    {

        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;


        string sDriverAssistantLicensePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\");
        string sDriverAssistantLicenseThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\thumbs\\");

        public DriverAssistantController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;

            if (!Directory.Exists(sDriverAssistantLicensePhotoPath)) Directory.CreateDirectory(sDriverAssistantLicensePhotoPath);
            if (!Directory.Exists(sDriverAssistantLicenseThumbsFilePath)) Directory.CreateDirectory(sDriverAssistantLicenseThumbsFilePath);
        }

        [HttpGet]
        [Route("DriverAssistants")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword, int draw)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasDriverAssistantCollection oDriverAssistantList = new MasDriverAssistantCollection(_dbContext);
                oDriverAssistantList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oDriverAssistantList);
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
        [Route("DriverAssistant")]
        public ActionResult<string> GetDriverAssistant(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriverAssistant oDriverAssistant = new MasDriverAssistant();

            try
            {
                oDriverAssistant = _dbContext.MasDriverAssistant.Where(o => o.DriverAssistantId == id).FirstOrDefault();
                jsonData = JsonConvert.SerializeObject(oDriverAssistant);
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
        [Route("DriverAssistantByTag")]
        public ActionResult<string> GetDriverAssistant(string p_sTagNumber)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriverAssistant oDriverAssistant = new MasDriverAssistant();

            try
            {
                oDriverAssistant = _dbContext.MasDriverAssistant.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault();
                
                jsonData = JsonConvert.SerializeObject(oDriverAssistant);
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
        [Route("PostDriverAssistant")]
        public IActionResult PostDriverAssistant()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriverAssistant p_oDriverAssistant = new MasDriverAssistant();
            MasDriverAssistant masDriverAssistant = new MasDriverAssistant();

            try
            {
                var form = Request.Form;
                string jsonObject = form["JsonObject"];
                p_oDriverAssistant = new JavaScriptSerializer().Deserialize<MasDriverAssistant>(jsonObject);
                bIsExist = _dbContext.MasDriverAssistant.Any(o => o.DriverAssistantId == p_oDriverAssistant.DriverAssistantId);

                masDriverAssistant = bIsExist ? _dbContext.MasDriverAssistant.Where(o => o.DriverAssistantId == p_oDriverAssistant.DriverAssistantId).FirstOrDefault() : new MasDriverAssistant();
                sPrevDetail = JsonConvert.SerializeObject(masDriverAssistant);

                masDriverAssistant.DriverAssistantId = !bIsExist ? NewGuid() : p_oDriverAssistant.DriverAssistantId;
                masDriverAssistant.TagNumber = p_oDriverAssistant.TagNumber;
                masDriverAssistant.EmployeeId = p_oDriverAssistant.EmployeeId;
                
                masDriverAssistant.IsActive = p_oDriverAssistant.IsActive;
                masDriverAssistant.IsDeleted = p_oDriverAssistant.IsDeleted;
                masDriverAssistant.Version = p_oDriverAssistant.Version;
                masDriverAssistant.CreateByUserId = bIsExist ? masDriverAssistant.CreateByUserId : _requestor.IUser().Id;
                masDriverAssistant.CreateDate = bIsExist ? masDriverAssistant.CreateDate : DateTime.Now;
                masDriverAssistant.UpdateDate = DateTime.Now;
                masDriverAssistant.UpdateByUserId = _requestor.IUser().Id;
                
                sDetail = JsonConvert.SerializeObject(masDriverAssistant);

                if (!bIsExist) _dbContext.MasDriverAssistant.Add(masDriverAssistant);
                else _dbContext.MasDriverAssistant.Update(masDriverAssistant);
                _dbContext.SaveChanges();

                sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";

                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                #region Log User

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.EMP_DRIV_ASTN_ADD : (int)Enumeration.ModuleObjectMember.EMP_DRIV_ASTN_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masDriverAssistant.DriverAssistantId.ToString(), Enumeration.ModuleObject.EMP_DRIV_ASTN.ToString(),
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

        [HttpGet]
        [Route("DataTable")]
        public ActionResult<IEnumerable<string>> DriverAssistant(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriverAssistantWithDetailCollection oDriverAssistantList = new MasDriverAssistantWithDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oDriverAssistantList.List(sKeyword, iSkip, iLength);

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
                    recordsFiltered = oDriverAssistantList.totalRecord,
                    recordsTotal = oDriverAssistantList.totalRecord,
                    data = oDriverAssistantList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteDriverAssistant")]
        public ActionResult<string> DeleteDriverAssistant(string id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasDriverAssistant oDriverAssistant = new MasDriverAssistant();

            try
            {
                oDriverAssistant = _dbContext.MasDriverAssistant.Where(o => o.DriverAssistantId == Guid.Parse(id)).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oDriverAssistant);

                oDriverAssistant.IsDeleted = true;
                oDriverAssistant.UpdateByUserId = _requestor.IUser().Id;
                oDriverAssistant.UpdateDate = DateTime.Now;
                _dbContext.MasDriverAssistant.Update(oDriverAssistant);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oDriverAssistant);
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
                                    _requestor.IpAddress(), oDriverAssistant.DriverAssistantId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
