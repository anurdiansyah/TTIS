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
using TTIS.API.UsersModels;

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : RDController
    {
        private Int32 m_iModuleObjectId = (int)Enumeration.ModuleObject.EMP_MAS;
        private readonly TTISDbContext _dbContext;
        private readonly IS4UsersContext _usersContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        string sDefaultPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\default\\");

        string sEmployeePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\employee\\");
        string sEmployeePhotoThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\employee\\thumbs\\");

        string sDriverLicensePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\");
        string sDriverLicenseThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\thumbs\\");

        string sVehiclePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\");
        string sVehicleThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\thumbs\\");

        public EmployeeController(TTISDbContext context, IS4UsersContext usersContext, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _usersContext = usersContext;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;

            if (!Directory.Exists(sEmployeePhotoPath)) Directory.CreateDirectory(sEmployeePhotoPath);
            if (!Directory.Exists(sEmployeePhotoThumbsFilePath)) Directory.CreateDirectory(sEmployeePhotoThumbsFilePath);

            if (!Directory.Exists(sDriverLicensePhotoPath)) Directory.CreateDirectory(sDriverLicensePhotoPath);
            if (!Directory.Exists(sDriverLicenseThumbsFilePath)) Directory.CreateDirectory(sDriverLicenseThumbsFilePath);
        }

        [HttpGet]
        [Route("Employees")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword, int draw)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasEmployeeCollection oEmployeeList = new MasEmployeeCollection(_dbContext);
                oEmployeeList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oEmployeeList);
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
        [Route("Employee")]
        public ActionResult<string> GetEmployee(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasEmployee oEmployee = new MasEmployee();

            try
            {
                oEmployee = _dbContext.MasEmployee.Where(o => o.EmployeeId == id).FirstOrDefault();
                jsonData = JsonConvert.SerializeObject(oEmployee);
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
        [Route("EmployeeByTag")]
        public ActionResult<string> GetEmployee(string p_sTagNumber)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasEmployee oEmployee = new MasEmployee();

            try
            {
                if (!string.IsNullOrEmpty(p_sTagNumber))
                {
                    bool bIsExist = _dbContext.MasEmployee.Any(o => o.TagNumber == p_sTagNumber);
                    oEmployee = bIsExist ? _dbContext.MasEmployee.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault() : new MasEmployee();

                    oEmployee.TagNumber = bIsExist ? oEmployee.TagNumber : DateTime.Today.ToString("yyMM") + _dbContext.MasEmployee.Count().ToString().PadLeft(4, '0');
                    oEmployee.DateOfBirth = oEmployee.DateOfBirth.ToUniversalTime();
                    oEmployee.JoinDate = oEmployee.JoinDate.ToUniversalTime();
                    oEmployee.ResignDate = oEmployee.ResignDate.ToUniversalTime();
                    oEmployee.Base64PasPhoto = System.IO.File.Exists(Path.Combine(sEmployeePhotoPath, oEmployee.PasPhoto + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sEmployeePhotoPath, oEmployee.PasPhoto + ".jpeg")))
                                                : string.Empty;
                    oEmployee.Base64IdentityImage = System.IO.File.Exists(Path.Combine(sEmployeePhotoPath, oEmployee.IdentityPicture + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sEmployeePhotoPath, oEmployee.IdentityPicture + ".jpeg")))
                                                        : string.Empty;

                    if (_dbContext.MasDriver.Any(o => o.EmployeeId == oEmployee.EmployeeId))
                    {
                        oEmployee.MasDriver = _dbContext.MasDriver.Where(o => o.EmployeeId == oEmployee.EmployeeId).FirstOrDefault();
                        oEmployee.MasDriver.Base64LicensePicture = System.IO.File.Exists(Path.Combine(sDriverLicensePhotoPath, oEmployee.MasDriver.LicensePicture + ".jpeg"))
                                                                   ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sDriverLicensePhotoPath, oEmployee.MasDriver.LicensePicture + ".jpeg")))
                                                                   : string.Empty;
                    }

                    if (_dbContext.MasDriverAssistant.Any(o => o.EmployeeId == oEmployee.EmployeeId))
                        oEmployee.MasDriverAssistant = _dbContext.MasDriverAssistant.Where(o => o.EmployeeId == oEmployee.EmployeeId).FirstOrDefault();

                    jsonData = JsonConvert.SerializeObject(oEmployee);
                }

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
        [Route("MyEmployeeProfile")]
        public ActionResult<string> MyEmployeeProfile()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            EmployeeDetail oEmployee = new EmployeeDetail();
            string sDefaultBasePhoto = Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sDefaultPhotoPath, "no_image_rectangle.png")));

            try
            {
                if (_dbContext.MasUserDetail.Any(o => o.AspNetUserId == _requestor.IUser().Id))
                {
                    string p_sTagNumber = _dbContext.MasUserDetail.Where(o => o.AspNetUserId == _requestor.IUser().Id).FirstOrDefault().TagNumber;
                    bool bIsExist = _dbContext.MasEmployee.Any(o => o.TagNumber == p_sTagNumber);
                    oEmployee = bIsExist ? _dbContext.EmployeeDetail.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault() : new EmployeeDetail();

                    oEmployee.DateOfBirth = oEmployee.DateOfBirth.ToUniversalTime();
                    oEmployee.JoinDate = oEmployee.JoinDate.ToUniversalTime();
                    oEmployee.ResignDate = oEmployee.ResignDate.ToUniversalTime();
                    oEmployee.CreateDate = oEmployee.CreateDate.ToUniversalTime();
                    oEmployee.UpdateDate = oEmployee.UpdateDate.ToUniversalTime();

                    oEmployee.Base64PasPhoto = System.IO.File.Exists(Path.Combine(sEmployeePhotoPath, oEmployee.PasPhoto + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sEmployeePhotoPath, oEmployee.PasPhoto + ".jpeg")))
                                                : sDefaultBasePhoto;
                    oEmployee.Base64IdentityImage = System.IO.File.Exists(Path.Combine(sEmployeePhotoPath, oEmployee.IdentityPicture + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sEmployeePhotoPath, oEmployee.IdentityPicture + ".jpeg")))
                                                        : sDefaultBasePhoto;

                    if (_dbContext.MasDriver.Any(o => o.EmployeeId == oEmployee.EmployeeId))
                    {
                        oEmployee.MasDriver = _dbContext.MasDriver.Where(o => o.EmployeeId == oEmployee.EmployeeId).FirstOrDefault();
                        oEmployee.MasDriver.LicenseExpiryDate = oEmployee.MasDriver.LicenseExpiryDate.ToUniversalTime();
                        oEmployee.MasDriver.CreateDate = oEmployee.MasDriver.CreateDate.ToUniversalTime();
                        oEmployee.MasDriver.UpdateDate = oEmployee.MasDriver.UpdateDate.ToUniversalTime();
                        oEmployee.MasDriver.Base64LicensePicture = System.IO.File.Exists(Path.Combine(sDriverLicensePhotoPath, oEmployee.MasDriver.LicensePicture + ".jpeg"))
                                                                   ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sDriverLicensePhotoPath, oEmployee.MasDriver.LicensePicture + ".jpeg")))
                                                                   : sDefaultBasePhoto;
                    }

                    if (_dbContext.MasVehicleUser.Any(o => o.EmployeeId == oEmployee.EmployeeId && o.IsActive == true && o.IsDeleted == false))
                    {
                        MasVehicleUser masVehicleUser = new MasVehicleUser();
                        masVehicleUser.DateFrom = masVehicleUser.DateFrom.ToUniversalTime();
                        masVehicleUser.DateTo = masVehicleUser.DateTo.ToUniversalTime();
                        masVehicleUser.CreateDate = masVehicleUser.CreateDate.ToUniversalTime();
                        masVehicleUser.UpdateDate = masVehicleUser.UpdateDate.ToUniversalTime();
                        masVehicleUser = _dbContext.MasVehicleUser.Where(o => o.EmployeeId == oEmployee.EmployeeId && o.IsActive == true && o.IsDeleted == false).FirstOrDefault();

                        oEmployee.MyVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == masVehicleUser.VehicleId).FirstOrDefault();
                        oEmployee.MyVehicle.DateFrom = oEmployee.MyVehicle.DateFrom == null ? new DateTime().ToUniversalTime() : Convert.ToDateTime(oEmployee.MyVehicle.DateFrom).ToUniversalTime();
                        oEmployee.MyVehicle.DateTo = oEmployee.MyVehicle.DateTo == null ? new DateTime().ToUniversalTime() : Convert.ToDateTime(oEmployee.MyVehicle.DateTo).ToUniversalTime();
                        oEmployee.MyVehicle.VehicleImageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.VehicleImage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.VehicleImage + ".jpeg")))
                                                        : sDefaultBasePhoto;

                        oEmployee.MyVehicle.StnkimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Stnkimage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Stnkimage + ".jpeg")))
                                                        : sDefaultBasePhoto;
                        oEmployee.MyVehicle.StnkberlakuHingga = oEmployee.MyVehicle.KirberlakuHingga.ToUniversalTime();

                        oEmployee.MyVehicle.BpkbimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Bpkbimage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Bpkbimage + ".jpeg")))
                                                        : sDefaultBasePhoto;

                        oEmployee.MyVehicle.KirimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Kirimage + ".jpeg"))
                                                        ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oEmployee.MyVehicle.Kirimage + ".jpeg")))
                                                        : sDefaultBasePhoto;
                        oEmployee.MyVehicle.KirberlakuHingga = oEmployee.MyVehicle.KirberlakuHingga.ToUniversalTime();
                        oEmployee.MyVehicle.CreateDate = oEmployee.MyVehicle.CreateDate.ToUniversalTime();
                        oEmployee.MyVehicle.UpdateDate = oEmployee.MyVehicle.UpdateDate.ToUniversalTime();
                    }

                    jsonData = JsonConvert.SerializeObject(oEmployee);
                }

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
        [Route("PostEmployee")]
        public IActionResult PostEmployee()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;
            bool bIsNeedApproval = _dbContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasEmployee p_oEmployee = new MasEmployee();
            MasEmployee masEmployee = new MasEmployee();

            try
            {
                var form = Request.Form;
                string sTagNumber = RDRandom.RandomNumber(000, 999) + _dbContext.MasEmployee.Count().ToString().PadLeft(4, '0');
                string jsonObject = form["JsonObject"];
                p_oEmployee = new JavaScriptSerializer().Deserialize<MasEmployee>(jsonObject);
                bIsExist = _dbContext.MasEmployee.Any(o => o.EmployeeId == p_oEmployee.EmployeeId);

                masEmployee = bIsExist ? _dbContext.MasEmployee.Where(o => o.EmployeeId == p_oEmployee.EmployeeId).FirstOrDefault() : new MasEmployee();
                sPrevDetail = JsonConvert.SerializeObject(masEmployee);

                masEmployee.EmployeeId = !bIsExist ? NewGuid() : p_oEmployee.EmployeeId;
                masEmployee.TagNumber = !bIsExist ? sTagNumber : p_oEmployee.TagNumber;
                masEmployee.FirstName = p_oEmployee.FirstName;
                masEmployee.MiddleName = p_oEmployee.MiddleName;
                masEmployee.LastName = p_oEmployee.LastName;
                masEmployee.NickName = p_oEmployee.NickName;
                masEmployee.PlaceOfBirth = p_oEmployee.PlaceOfBirth;
                masEmployee.DateOfBirth = p_oEmployee.DateOfBirth;
                masEmployee.Gender = p_oEmployee.Gender;

                masEmployee.IdentityNumber = p_oEmployee.IdentityNumber;
                masEmployee.IdentityAddress = p_oEmployee.IdentityAddress;
                masEmployee.IdentitySubDistrict = p_oEmployee.IdentitySubDistrict;
                masEmployee.IdentityDistrict = p_oEmployee.IdentityDistrict;
                masEmployee.IdentityCity = p_oEmployee.IdentityCity;
                masEmployee.IdentityProvince = p_oEmployee.IdentityProvince;
                masEmployee.IdentityPicture = "id_" + masEmployee.EmployeeId;

                masEmployee.LivingAddress = p_oEmployee.LivingAddress;
                masEmployee.LivingSubDistrict = p_oEmployee.LivingSubDistrict;
                masEmployee.LivingDistrict = p_oEmployee.LivingDistrict;
                masEmployee.LivingCity = p_oEmployee.LivingCity;
                masEmployee.LivingProvince = p_oEmployee.LivingProvince;

                masEmployee.PhoneNumber = p_oEmployee.PhoneNumber;
                masEmployee.EmployeeStatusId = p_oEmployee.EmployeeStatusId;
                masEmployee.JoinDate = p_oEmployee.JoinDate;
                masEmployee.ResignDate = bIsExist ? masEmployee.ResignDate : new DateTime(1900, 01, 01);
                masEmployee.ResignReason = p_oEmployee.ResignReason == null ? string.Empty : p_oEmployee.ResignReason;
                masEmployee.DepartmentId = p_oEmployee.DepartmentId;
                masEmployee.UnitId = p_oEmployee.UnitId;
                masEmployee.TitleId = p_oEmployee.TitleId;
                masEmployee.Email = p_oEmployee.Email;
                masEmployee.PasPhoto = "pp_" + masEmployee.EmployeeId;
                masEmployee.IsActive = p_oEmployee.IsActive;
                masEmployee.IsDeleted = p_oEmployee.IsDeleted;
                masEmployee.Version = p_oEmployee.Version;
                masEmployee.CreateByUserId = bIsExist ? masEmployee.CreateByUserId : _requestor.IUser().Id;
                masEmployee.CreateDate = bIsExist ? masEmployee.CreateDate : DateTime.Now;
                masEmployee.UpdateDate = DateTime.Now;
                masEmployee.UpdateByUserId = _requestor.IUser().Id;

                foreach (IFormFile files in form.Files)
                {
                    if (!files.Name.ToLower().Contains("license"))
                    {
                        string sFileName = files.Name.ToLower().Contains("identity")
                                            ? masEmployee.IdentityPicture + ".jpeg"
                                            : masEmployee.PasPhoto + ".jpeg";
                        using (var fileStream = new FileStream(Path.Combine(sEmployeePhotoPath, sFileName), FileMode.Create))
                        {
                            files.CopyTo(fileStream);
                            RDImageHelper.CompressAndSaveImage(Path.Combine(sEmployeePhotoPath, sFileName), Path.Combine(sEmployeePhotoThumbsFilePath, sFileName), 80, 64, 0, 256);
                        }
                    }
                }

                if (p_oEmployee.RegisterAs == (int)Enumeration.RegisterEmployeeAs.Driver)
                {
                    masEmployee.MasDriver = new MasDriver();

                    masEmployee.MasDriver.DriverId = NewGuid();
                    masEmployee.MasDriver.TagNumber = masEmployee.TagNumber;
                    masEmployee.MasDriver.EmployeeId = masEmployee.EmployeeId;

                    masEmployee.MasDriver.LicenseNumber = p_oEmployee.LicenseNumber;
                    masEmployee.MasDriver.LicenseType = p_oEmployee.LicenseType;
                    masEmployee.MasDriver.LicenseExpiryDate = p_oEmployee.LicenseExpiryDate;
                    masEmployee.MasDriver.LicensePicture = masEmployee.MasDriver.DriverId + ".jpeg";

                    masEmployee.MasDriver.IsActive = true;
                    masEmployee.MasDriver.IsDeleted = false;
                    masEmployee.MasDriver.Version = 1;
                    masEmployee.MasDriver.CreateByUserId = bIsExist ? masEmployee.MasDriver.CreateByUserId : _requestor.IUser().Id;
                    masEmployee.MasDriver.CreateDate = bIsExist ? masEmployee.MasDriver.CreateDate : DateTime.Now;
                    masEmployee.MasDriver.UpdateDate = DateTime.Now;
                    masEmployee.MasDriver.UpdateByUserId = _requestor.IUser().Id;

                    using (var fileStream = new FileStream(Path.Combine(sDriverLicensePhotoPath, masEmployee.MasDriver.LicensePicture), FileMode.Create))
                    {
                        form.Files["fileDriverLicense"].CopyTo(fileStream);
                        RDImageHelper.CompressAndSaveImage(Path.Combine(sDriverLicensePhotoPath, masEmployee.MasDriver.LicensePicture), Path.Combine(sDriverLicenseThumbsFilePath, masEmployee.MasDriver.LicensePicture), 80, 64, 0, 256);
                    }

                    _dbContext.MasDriver.Add(masEmployee.MasDriver);
                }

                if (p_oEmployee.RegisterAs == (int)Enumeration.RegisterEmployeeAs.DriverAssistant)
                {
                    masEmployee.MasDriverAssistant = new MasDriverAssistant();

                    masEmployee.MasDriverAssistant.DriverAssistantId = NewGuid();
                    masEmployee.MasDriverAssistant.TagNumber = masEmployee.TagNumber;
                    masEmployee.MasDriverAssistant.EmployeeId = masEmployee.EmployeeId;

                    masEmployee.MasDriverAssistant.IsActive = true;
                    masEmployee.MasDriverAssistant.IsDeleted = false;
                    masEmployee.MasDriverAssistant.Version = 1;
                    masEmployee.MasDriverAssistant.CreateByUserId = bIsExist ? masEmployee.MasDriverAssistant.CreateByUserId : _requestor.IUser().Id;
                    masEmployee.MasDriverAssistant.CreateDate = bIsExist ? masEmployee.MasDriverAssistant.CreateDate : DateTime.Now;
                    masEmployee.MasDriverAssistant.UpdateDate = DateTime.Now;
                    masEmployee.MasDriverAssistant.UpdateByUserId = _requestor.IUser().Id;

                    _dbContext.MasDriverAssistant.Add(masEmployee.MasDriverAssistant);
                }

                if (p_oEmployee.RegisterAsUser)
                {
                    string sRoleCode = _dbContext.SysParam.Where(p => p.Code == "DEFAULT_GROUP_CODE").FirstOrDefault().Value;
                    string sDefaultEmailDomain = _dbContext.SysParam.Where(p => p.Code == "DEFAULT_EMAIL_DOMAIN").FirstOrDefault().Value;
                    bool bRoleIsExist = _dbContext.MasRoleAccess.Any(o => o.RoleCode == sRoleCode);
                    if (bRoleIsExist)
                    {
                        MasRoleAccess masRoleAccess = _dbContext.MasRoleAccess.Where(o => o.RoleCode == sRoleCode).FirstOrDefault();

                        AspNetUsers aspNetUsers = masEmployee.AspNetUser;
                        aspNetUsers = new AspNetUsers();

                        aspNetUsers.Id = NewGuid().ToString();
                        aspNetUsers.UserName = masEmployee.TagNumber + "@" + sDefaultEmailDomain;
                        aspNetUsers.NormalizedUserName = aspNetUsers.UserName.ToUpper();
                        aspNetUsers.Email = aspNetUsers.UserName;
                        aspNetUsers.NormalizedEmail = aspNetUsers.UserName.ToUpper();
                        aspNetUsers.PhoneNumber = string.Empty;
                        aspNetUsers.PhoneNumberConfirmed = true;
                        aspNetUsers.PasswordHash = HashPassword(masEmployee.TagNumber);
                        aspNetUsers.AccessFailedCount = 0;
                        aspNetUsers.ConcurrencyStamp = NewGuid().ToString();
                        aspNetUsers.EmailConfirmed = false;
                        aspNetUsers.LockoutEnabled = false;
                        aspNetUsers.LockoutEnd = null;
                        aspNetUsers.PhoneNumberConfirmed = false;
                        aspNetUsers.SecurityStamp = NewGuid().ToString();

                        aspNetUsers.UserDetail = new MasUserDetail();
                        aspNetUsers.UserDetail.TagNumber = masEmployee.TagNumber;
                        aspNetUsers.UserDetail.AspNetUserId = aspNetUsers.Id;

                        MasUserRole masUserRole = new MasUserRole();
                        masUserRole.AspNetUserId = aspNetUsers.Id;
                        masUserRole.RoleAccessId = masRoleAccess.RoleAccessId;
                        aspNetUsers.UserRoles = new List<MasUserRole>();
                        aspNetUsers.UserRoles.Add(masUserRole);

                        _usersContext.AspNetUsers.Add(aspNetUsers);
                        _dbContext.MasUserDetail.Add(aspNetUsers.UserDetail);
                        _dbContext.MasUserRole.Add(masUserRole);
                    }
                    else
                    {
                        sMessage = "Failed to register Employee as User, Default role not found.";
                    }
                }

                sDetail = JsonConvert.SerializeObject(masEmployee);

                if (!bIsNeedApproval)
                {
                    if (!bIsExist) _dbContext.MasEmployee.Add(masEmployee);
                    else _dbContext.MasEmployee.Update(masEmployee);
                    _dbContext.SaveChanges();
                    _usersContext.SaveChanges();

                    sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                }
                else
                {
                    int iMemberId = !bIsExist
                                        ? (int)Enumeration.ModuleObjectMember.EMP_MAS_ADD
                                        : (int)Enumeration.ModuleObjectMember.EMP_MAS_EDIT;

                    SysApproval sysApproval = new SysApproval();
                    sysApproval.ApprovalId = NewGuid();
                    sysApproval.ModuleObjectId = m_iModuleObjectId;
                    sysApproval.ModuleObjectMemberId = iMemberId;
                    sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                    sysApproval.ReffObj = "Employee Master";
                    sysApproval.ReffId = p_oEmployee.EmployeeId.ToString();
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
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                #region Log User

                int iModuleObjectMember = !bIsExist ? (int)Enumeration.ModuleObjectMember.EMP_MAS_ADD : (int)Enumeration.ModuleObjectMember.EMP_MAS_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masEmployee.EmployeeId.ToString(), Enumeration.ModuleObject.EMP_MAS.ToString(),
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
        public ActionResult<IEnumerable<string>> Employee(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasEmployeeCollection oEmployeeList = new MasEmployeeCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oEmployeeList.List(sKeyword, iSkip, iLength);

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
                    recordsFiltered = oEmployeeList.totalRecord,
                    recordsTotal = oEmployeeList.totalRecord,
                    data = oEmployeeList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteEmployee")]
        public ActionResult<string> DeleteEmployee(string id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasEmployee oEmployee = new MasEmployee();

            try
            {
                oEmployee = _dbContext.MasEmployee.Where(o => o.EmployeeId == Guid.Parse(id)).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oEmployee);

                oEmployee.IsDeleted = true;
                oEmployee.UpdateByUserId = _requestor.IUser().Id;
                oEmployee.UpdateDate = DateTime.Now;
                _dbContext.MasEmployee.Update(oEmployee);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oEmployee);
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
                                    _requestor.IpAddress(), oEmployee.EmployeeId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
