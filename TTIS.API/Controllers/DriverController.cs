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
    public class DriverController : RDController
    {

        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        string sDriverLicensePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\");
        string sDriverLicenseThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\thumbs\\");

        public DriverController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;

            if (!Directory.Exists(sDriverLicensePhotoPath)) Directory.CreateDirectory(sDriverLicensePhotoPath);
            if (!Directory.Exists(sDriverLicenseThumbsFilePath)) Directory.CreateDirectory(sDriverLicenseThumbsFilePath);
        }

        [HttpGet]
        [Route("Drivers")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword, int draw)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasDriverCollection oDriverList = new MasDriverCollection(_dbContext);
                oDriverList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oDriverList);
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
        [Route("Driver")]
        public ActionResult<string> GetDriver(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriver oDriver = new MasDriver();

            try
            {
                oDriver = _dbContext.MasDriver.Where(o => o.DriverId == id).FirstOrDefault();
                jsonData = JsonConvert.SerializeObject(oDriver);
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
        [Route("DriverByTag")]
        public ActionResult<string> GetDriver(string p_sTagNumber)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriver oDriver = new MasDriver();

            try
            {
                oDriver = _dbContext.MasDriver.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault();

                oDriver.LicenseExpiryDate = oDriver.LicenseExpiryDate.ToUniversalTime();
                if (System.IO.File.Exists(Path.Combine(sDriverLicensePhotoPath, oDriver.LicensePicture + ".jpeg")))
                {
                    oDriver.Base64LicensePicture = Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sDriverLicensePhotoPath, oDriver.LicensePicture + ".jpeg")));
                }

                jsonData = JsonConvert.SerializeObject(oDriver);
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
        [Route("PostDriver")]
        public IActionResult PostDriver()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriver p_oDriver = new MasDriver();
            MasDriver masDriver = new MasDriver();

            try
            {
                var form = Request.Form;
                string jsonObject = form["JsonObject"];
                p_oDriver = new JavaScriptSerializer().Deserialize<MasDriver>(jsonObject);
                bIsExist = _dbContext.MasDriver.Any(o => o.DriverId == p_oDriver.DriverId);

                masDriver = bIsExist ? _dbContext.MasDriver.Where(o => o.DriverId == p_oDriver.DriverId).FirstOrDefault() : new MasDriver();
                sPrevDetail = JsonConvert.SerializeObject(masDriver);

                masDriver.DriverId = !bIsExist ? NewGuid() : p_oDriver.DriverId;
                masDriver.TagNumber = p_oDriver.TagNumber;
                masDriver.EmployeeId = p_oDriver.EmployeeId;

                masDriver.LicenseNumber = p_oDriver.LicenseNumber;
                masDriver.LicenseType = p_oDriver.LicenseType;
                masDriver.LicenseExpiryDate = p_oDriver.LicenseExpiryDate;
                masDriver.LicensePicture = "lic_" + masDriver.DriverId;

                masDriver.IsActive = p_oDriver.IsActive;
                masDriver.IsDeleted = p_oDriver.IsDeleted;
                masDriver.Version = p_oDriver.Version;
                masDriver.CreateByUserId = bIsExist ? masDriver.CreateByUserId : _requestor.IUser().Id;
                masDriver.CreateDate = bIsExist ? masDriver.CreateDate : DateTime.Now;
                masDriver.UpdateDate = DateTime.Now;
                masDriver.UpdateByUserId = _requestor.IUser().Id;

                foreach (IFormFile files in form.Files)
                {
                    string sFileName = masDriver.LicensePicture + ".jpeg";
                    using (var fileStream = new FileStream(Path.Combine(sDriverLicensePhotoPath, sFileName), FileMode.Create))
                    {
                        files.CopyTo(fileStream);
                        RDImageHelper.CompressAndSaveImage(Path.Combine(sDriverLicensePhotoPath, sFileName), Path.Combine(sDriverLicenseThumbsFilePath, "thumb_" + sFileName), 50, 64, 0, 128);
                    }
                }

                sDetail = JsonConvert.SerializeObject(masDriver);

                if (!bIsExist) _dbContext.MasDriver.Add(masDriver);
                else _dbContext.MasDriver.Update(masDriver);
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

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.EMP_DRIV_ADD : (int)Enumeration.ModuleObjectMember.EMP_DRIV_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masDriver.DriverId.ToString(), Enumeration.ModuleObject.EMP_DRIV.ToString(),
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
        public ActionResult<IEnumerable<string>> Driver(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasDriverWithDetailCollection oDriverList = new MasDriverWithDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oDriverList.List(sKeyword, iSkip, iLength);

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
                    recordsFiltered = oDriverList.totalRecord,
                    recordsTotal = oDriverList.totalRecord,
                    data = oDriverList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteDriver")]
        public ActionResult<string> DeleteDriver(string id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasDriver oDriver = new MasDriver();

            try
            {
                oDriver = _dbContext.MasDriver.Where(o => o.DriverId == Guid.Parse(id)).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oDriver);

                oDriver.IsDeleted = true;
                oDriver.UpdateByUserId = _requestor.IUser().Id;
                oDriver.UpdateDate = DateTime.Now;
                _dbContext.MasDriver.Update(oDriver);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oDriver);
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
                                    _requestor.IpAddress(), oDriver.DriverId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
