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
    public class MasVehicleController : RDController
    {
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        string sDefaultPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\default\\");
        string sVehiclePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\");
        string sVehicleThumbsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\thumbs\\");

        public MasVehicleController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [Route("Vehicles")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasVehicleCollection oVehicleList = new MasVehicleCollection(_dbContext);
                oVehicleList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oVehicleList);
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("Vehicle")]
        public ActionResult<string> GetVehicle(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleDetail oVehicle = new VehicleDetail();

            try
            {
                oVehicle = _dbContext.VehicleDetail.Where(o => o.VehicleId == id).FirstOrDefault();

                if (oVehicle.DateFrom != null) oVehicle.DateFrom = Convert.ToDateTime(oVehicle.DateFrom).ToUniversalTime();
                if (oVehicle.DateTo != null) oVehicle.DateTo = Convert.ToDateTime(oVehicle.DateTo).ToUniversalTime();
                oVehicle.CreateDate = oVehicle.CreateDate.ToUniversalTime();
                oVehicle.UpdateDate = oVehicle.UpdateDate.ToUniversalTime();
                oVehicle.StnkberlakuHingga = oVehicle.StnkberlakuHingga.ToUniversalTime();
                oVehicle.KirberlakuHingga = oVehicle.KirberlakuHingga.ToUniversalTime();

                string sDefaultBasePhoto = Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sDefaultPhotoPath, "no_image_rectangle.png")));
                oVehicle.VehicleImageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oVehicle.VehicleImage + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oVehicle.VehicleImage + ".jpeg")))
                                                : sDefaultBasePhoto;

                oVehicle.StnkimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oVehicle.Stnkimage + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oVehicle.Stnkimage + ".jpeg")))
                                                : sDefaultBasePhoto;

                oVehicle.BpkbimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oVehicle.Bpkbimage + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oVehicle.Bpkbimage + ".jpeg")))
                                                : sDefaultBasePhoto;

                oVehicle.KirimageBase64 = System.IO.File.Exists(Path.Combine(sVehiclePhotoPath, oVehicle.Kirimage + ".jpeg"))
                                                ? Convert.ToBase64String(RDImageHelper.ImageToByteArray(Path.Combine(sVehiclePhotoPath, oVehicle.Kirimage + ".jpeg")))
                                                : sDefaultBasePhoto;

                jsonData = JsonConvert.SerializeObject(oVehicle);
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("DataTable")]
        public ActionResult<IEnumerable<string>> MasVehicle(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            VehicleDetailCollection oVehicleList = new VehicleDetailCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oVehicleList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oVehicleList.totalRecord,
                    recordsTotal = oVehicleList.totalRecord,
                    data = oVehicleList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostVehicle")]
        public IActionResult PostVehicle()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasVehicle masVehicle = new MasVehicle();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    MasVehicle p_oMasVehicle = new JavaScriptSerializer().Deserialize<MasVehicle>(jsonObject);
                    bIsExist = _dbContext.MasVehicle.Any(o => o.VehicleId == p_oMasVehicle.VehicleId);

                    masVehicle = bIsExist ? _dbContext.MasVehicle.Where(o => o.VehicleId == p_oMasVehicle.VehicleId).FirstOrDefault() : new MasVehicle();
                    sPrevDetail = JsonConvert.SerializeObject(p_oMasVehicle);

                    masVehicle.VehicleId = !bIsExist ? NewGuid() : p_oMasVehicle.VehicleId;
                    masVehicle.NomorRegistrasi = p_oMasVehicle.NomorRegistrasi;
                    string sVehicleCode = _dbContext.MasVehicleType.Where(o => o.VehicleTypeId == p_oMasVehicle.TypeId).FirstOrDefault().Code 
                        + _dbContext.MasVehicleModel.Where(o => o.VehicleModelId == p_oMasVehicle.ModelId).FirstOrDefault().Code
                        + (_dbContext.MasVehicle.Count() + 1).ToString().PadLeft(4, '0');
                    masVehicle.VehicleCode = !bIsExist ? sVehicleCode : string.IsNullOrEmpty(p_oMasVehicle.VehicleCode) ? sVehicleCode : p_oMasVehicle.VehicleCode;
                    masVehicle.Plate = p_oMasVehicle.Plate;
                    masVehicle.Merk = p_oMasVehicle.Merk;
                    masVehicle.Tipe = p_oMasVehicle.Tipe;
                    masVehicle.ModelId = p_oMasVehicle.ModelId;
                    masVehicle.TypeId = p_oMasVehicle.TypeId;
                    masVehicle.NoRangka = p_oMasVehicle.NoRangka;
                    masVehicle.NoMesin = p_oMasVehicle.NoMesin;
                    masVehicle.Warna = p_oMasVehicle.Warna;
                    masVehicle.FuelId = p_oMasVehicle.FuelId;
                    masVehicle.TahunPerakitan = p_oMasVehicle.TahunPerakitan;
                    masVehicle.TahunRegistrasi = p_oMasVehicle.TahunRegistrasi;
                    masVehicle.VehicleStatusId = p_oMasVehicle.VehicleStatusId;
                    masVehicle.VehicleImage = masVehicle.VehicleId + "_img";

                    masVehicle.Bpkbimage = masVehicle.VehicleId + "_bpkb";
                    masVehicle.BpkbpositionId = p_oMasVehicle.BpkbpositionId;
                    masVehicle.BpkbpositionReffId = p_oMasVehicle.BpkbpositionReffId;

                    masVehicle.Stnkimage = masVehicle.VehicleId + "_stnk";
                    masVehicle.StnkberlakuHingga = p_oMasVehicle.StnkberlakuHingga;
                    masVehicle.StnkpositionId = p_oMasVehicle.StnkpositionId;
                    masVehicle.StnkpositionReffId = p_oMasVehicle.StnkpositionReffId;

                    masVehicle.Kirimage = masVehicle.VehicleId + "_kir";
                    masVehicle.KirberlakuHingga = p_oMasVehicle.KirberlakuHingga;
                    masVehicle.KirpositionId = p_oMasVehicle.KirpositionId;
                    masVehicle.KirpositionReffId = p_oMasVehicle.KirpositionReffId;

                    masVehicle.Version = !bIsExist ? 1 : (masVehicle.Version + 1);
                    masVehicle.CreateDate = !bIsExist ? DateTime.Today : masVehicle.CreateDate;
                    masVehicle.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masVehicle.CreateByUserId;
                    masVehicle.UpdateDate = DateTime.Today;
                    masVehicle.UpdateByUserId = _requestor.IUser().Id;

                    IFormFile fileVehicleImage = form.Files["fileVehicleImage"];
                    if (fileVehicleImage != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(sVehiclePhotoPath, masVehicle.VehicleImage + ".jpeg"), FileMode.Create))
                        {
                            fileVehicleImage.CopyTo(fileStream);
                            RDImageHelper.CompressAndSaveImage(Path.Combine(sVehiclePhotoPath, masVehicle.VehicleImage + ".jpeg"), 
                                Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.VehicleImage + ".jpeg"), 80, 64, 0, 256);
                        }
                    }

                    IFormFile fileStnkImage = form.Files["fileStnkImage"];
                    if (fileStnkImage != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(sVehiclePhotoPath, masVehicle.Stnkimage + ".jpeg"), FileMode.Create))
                        {
                            fileStnkImage.CopyTo(fileStream);
                            RDImageHelper.CompressAndSaveImage(Path.Combine(sVehiclePhotoPath, masVehicle.Stnkimage + ".jpeg"), 
                                Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.Stnkimage + ".jpeg"), 80, 64, 0, 256);
                        }
                    }

                    IFormFile fileBpkbImage = form.Files["fileBpkbImage"];
                    if (fileBpkbImage != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(sVehiclePhotoPath, masVehicle.Bpkbimage + ".jpeg"), FileMode.Create))
                        {
                            fileBpkbImage.CopyTo(fileStream);
                            RDImageHelper.CompressAndSaveImage(Path.Combine(sVehiclePhotoPath, masVehicle.Bpkbimage + ".jpeg"), 
                                Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.Bpkbimage + ".jpeg"), 80, 64, 0, 256);
                        }
                    }

                    IFormFile fileKirImage = form.Files["fileKirImage"];
                    if (fileKirImage != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(sVehiclePhotoPath, masVehicle.Kirimage + ".jpeg"), FileMode.Create))
                        {
                            fileKirImage.CopyTo(fileStream);
                            RDImageHelper.CompressAndSaveImage(Path.Combine(sVehiclePhotoPath, masVehicle.Kirimage + ".jpeg"), 
                                Path.Combine(sVehicleThumbsFilePath, "thumb_" + masVehicle.Kirimage + ".jpeg"), 80, 64, 0, 256);
                        }
                    }

                    sDetail = JsonConvert.SerializeObject(masVehicle);
                    if (!bIsExist) _dbContext.MasVehicle.Add(masVehicle);
                    else _dbContext.MasVehicle.Update(masVehicle);
                    _dbContext.SaveChanges();

                    sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
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

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.VEH_MAS_EDIT : (int)Enumeration.ModuleObjectMember.VEH_MAS_ADD;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masVehicle.VehicleId.ToString(), Enumeration.ModuleObject.VEH_MAS.ToString(),
                                    iModuleObjectMember, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                #endregion
            }


            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteVehicle")]
        public ActionResult<string> DeleteVehicle(Guid id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicle oVehicle = new MasVehicle();

            try
            {
                oVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oVehicle);

                oVehicle.IsDeleted = true;
                oVehicle.UpdateByUserId = _requestor.IUser().Id;
                oVehicle.UpdateDate = DateTime.Now;
                _dbContext.MasVehicle.Update(oVehicle);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oVehicle);
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
                                    _requestor.IpAddress(), oVehicle.VehicleId.ToString(), Enumeration.ModuleObject.VEH_MAS.ToString(),
                                    (int)Enumeration.ModuleObjectMember.VEH_MAS_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                #endregion
            }


            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 800,
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
                //                    _requestor.IpAddress(), oVehicle.VehicleId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
                //                    (int)Enumeration.ModuleObjectMember.VEH_MAS_DELETE, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, sMessage);

                #endregion
            }


            return Json(new
            {
                DataTable = string.Empty,
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("VehicleType")]
        public ActionResult<string> GetVehicleType()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicle oVehicle = new MasVehicle();

            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.MasVehicleType.ToList());
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("VehicleModel")]
        public ActionResult<string> GetVehicleModel()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicle oVehicle = new MasVehicle();

            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.MasVehicleModel.ToList());
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("VehicleStatus")]
        public ActionResult<string> GetVehicleStatus()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicle oVehicle = new MasVehicle();

            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.MasVehicleStatus.ToList());
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpGet]
        [Route("Fuel")]
        public ActionResult<string> GetFuel()
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasVehicle oVehicle = new MasVehicle();

            try
            {
                jsonData = JsonConvert.SerializeObject(_dbContext.MasFuel.Where(o => o.IsActive && !o.IsDeleted).ToList());
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
                StatusCode = bIsSuccess ? 200 : 800,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

    }
}
