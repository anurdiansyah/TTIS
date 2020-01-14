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
    public class MasTitleController : RDController
    {
        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public MasTitleController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [Route("Titles")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasTitleCollection oTitleList = new MasTitleCollection(_dbContext);
                oTitleList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oTitleList);
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
        [Route("Title")]
        public ActionResult<string> GetTitle(int id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasTitle oTitle = new MasTitle();

            try
            {
                oTitle = _dbContext.MasTitle.Where(o => o.TitleId == id).FirstOrDefault();

                jsonData = JsonConvert.SerializeObject(oTitle);
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
        public ActionResult<IEnumerable<string>> MasTitle(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasTitleCollection oTitleList = new MasTitleCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oTitleList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oTitleList.totalRecord,
                    recordsTotal = oTitleList.totalRecord,
                    data = oTitleList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostTitle")]
        public IActionResult PostTitle()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasTitle masTitle = new MasTitle();

            try
            {

                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    MasTitle p_oMasTitle = new JavaScriptSerializer().Deserialize<MasTitle>(jsonObject);
                    bIsExist = _dbContext.MasTitle.Any(o => o.TitleId == p_oMasTitle.TitleId);

                    masTitle = bIsExist ? _dbContext.MasTitle.Where(o => o.TitleId == p_oMasTitle.TitleId).FirstOrDefault() : new MasTitle();
                    sPrevDetail = JsonConvert.SerializeObject(p_oMasTitle);

                    masTitle.Name = p_oMasTitle.Name;
                    masTitle.Description = p_oMasTitle.Description;
                    masTitle.Version = !bIsExist ? 1 : (masTitle.Version + 1);
                    masTitle.CreateDate = !bIsExist ? DateTime.Today : masTitle.CreateDate;
                    masTitle.CreateByUserId = !bIsExist ? _requestor.IUser().Id : masTitle.CreateByUserId;
                    masTitle.UpdateDate = DateTime.Today;
                    masTitle.UpdateByUserId = _requestor.IUser().Id;

                    sDetail = JsonConvert.SerializeObject(masTitle);
                    if (!bIsExist) _dbContext.MasTitle.Add(masTitle);
                    else _dbContext.MasTitle.Update(masTitle);
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

                int iModuleObjectMember = bIsExist ? (int)Enumeration.ModuleObjectMember.MAS_TITLE_EDIT : (int)Enumeration.ModuleObjectMember.MAS_TITLE_ADD;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masTitle.TitleId.ToString(), Enumeration.ModuleObject.MAS_TITLE.ToString(),
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
        [Route("DeleteTitle")]
        public ActionResult<string> DeleteTitle(int id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasTitle oTitle = new MasTitle();

            try
            {
                oTitle = _dbContext.MasTitle.Where(o => o.TitleId == id).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oTitle);

                oTitle.IsDeleted = true;
                oTitle.UpdateByUserId = _requestor.IUser().Id;
                oTitle.UpdateDate = DateTime.Now;
                _dbContext.MasTitle.Update(oTitle);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oTitle);
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
                                    _requestor.IpAddress(), oTitle.TitleId.ToString(), Enumeration.ModuleObject.MAS_TITLE.ToString(),
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
                //                    _requestor.IpAddress(), oTitle.TitleId.ToString(), Enumeration.ModuleObject.SCR_GRP.ToString(),
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
