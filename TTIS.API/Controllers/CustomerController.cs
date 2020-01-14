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
    public class CustomerController : RDController
    {

        private readonly TTISDbContext _dbContext;
        private readonly IRequestor _requestor;
        string _reffNumber;
        private readonly LoggingContext _loggingContext;

        public CustomerController(TTISDbContext context, IRequestor requestor, LoggingContext loggingContext)
        {
            _dbContext = context;
            _requestor = requestor;
            _reffNumber = ReferenceNumber();
            _loggingContext = loggingContext;
        }

        [HttpGet]
        [Route("Customers")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword, int draw)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                MasCustomerCollection oCustomerList = new MasCustomerCollection(_dbContext);
                oCustomerList.List(sKeyword);

                jsonData = JsonConvert.SerializeObject(oCustomerList);
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
        [Route("Customer")]
        public ActionResult<string> GetCustomer(Guid id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasCustomer oCustomer = new MasCustomer();

            try
            {
                oCustomer = _dbContext.MasCustomer.Where(o => o.CustomerId == id).FirstOrDefault();
                oCustomer.MasCustomerContact = _dbContext.MasCustomerContact.Where(o => o.CustomerId == id).ToList();
                jsonData = JsonConvert.SerializeObject(oCustomer);
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
        [Route("PostCustomer")]
        public IActionResult PostCustomer()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;
            bool bIsExist = false;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasCustomer p_oCustomer = new MasCustomer();
            MasCustomer masCustomer = new MasCustomer();

            try
            {
                var form = Request.Form;
                string jsonObject = form["JsonObject"];
                p_oCustomer = new JavaScriptSerializer().Deserialize<MasCustomer>(jsonObject);
                bIsExist = _dbContext.MasCustomer.Any(o => o.CustomerId == p_oCustomer.CustomerId);

                masCustomer = bIsExist ? _dbContext.MasCustomer.Where(o => o.CustomerId == p_oCustomer.CustomerId).FirstOrDefault() : new MasCustomer();
                sPrevDetail = JsonConvert.SerializeObject(masCustomer);

                string p_sName = p_oCustomer.Name.Split(' ')[0].ToUpper();
                string p_sCode = p_sName.Substring(0, 1) + p_sName.Substring((p_sName.Length / 2), 1) + p_sName.Substring((p_sName.Length - 1), 1) + RDRandom.RandomNumber(001, 999);
                masCustomer.Code = !bIsExist ? p_sCode : p_oCustomer.Code;

                masCustomer.CustomerId = !bIsExist ? NewGuid() : p_oCustomer.CustomerId;
                masCustomer.Name = p_oCustomer.Name;
                masCustomer.Address = p_oCustomer.Address;
                masCustomer.PhoneNumber = p_oCustomer.PhoneNumber;
                masCustomer.Email = p_oCustomer.Email;

                masCustomer.IsActive = p_oCustomer.IsActive;
                masCustomer.IsDeleted = p_oCustomer.IsDeleted;
                masCustomer.Version = p_oCustomer.Version;
                masCustomer.CreateByUserId = bIsExist ? masCustomer.CreateByUserId : _requestor.IUser().Id;
                masCustomer.CreateDate = bIsExist ? masCustomer.CreateDate : DateTime.Now;
                masCustomer.UpdateDate = DateTime.Now;
                masCustomer.UpdateByUserId = _requestor.IUser().Id;

                masCustomer.MasCustomerContact = !bIsExist ? new List<MasCustomerContact>() : _dbContext.MasCustomerContact.Where(o => o.CustomerId == p_oCustomer.CustomerId).ToList();
                _dbContext.MasCustomerContact.RemoveRange(masCustomer.MasCustomerContact);
                masCustomer.MasCustomerContact.Clear();

                foreach (MasCustomerContact masCustomerContact in p_oCustomer.MasCustomerContact)
                {
                    masCustomerContact.CustomerId = masCustomer.CustomerId;
                    masCustomerContact.CreateDate = masCustomerContact.Version < 0 ? DateTime.Now : masCustomerContact.CreateDate;
                    masCustomerContact.CreateByUserId = masCustomerContact.Version < 0 ? _requestor.IUser().Id : masCustomerContact.CreateByUserId;
                    masCustomerContact.Version = masCustomerContact.Version < 0 ? 1 : masCustomerContact.Version + 1;
                    masCustomerContact.UpdateDate = DateTime.Now;
                    masCustomerContact.UpdateByUserId = _requestor.IUser().Id;

                    masCustomer.MasCustomerContact.Add(masCustomerContact);
                }

                sDetail = JsonConvert.SerializeObject(masCustomer);

                if (!bIsExist) _dbContext.MasCustomer.Add(masCustomer);
                else _dbContext.MasCustomer.Update(masCustomer);
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

                int iModuleObjectMember = !bIsExist ? (int)Enumeration.ModuleObjectMember.CUST_PRCPL_ADD : (int)Enumeration.ModuleObjectMember.CUST_PRCPL_EDIT;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), masCustomer.CustomerId.ToString(), Enumeration.ModuleObject.CUST_PRCPL.ToString(),
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
        public ActionResult<IEnumerable<string>> Customer(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            MasCustomerCollection oCustomerList = new MasCustomerCollection(_dbContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oCustomerList.List(sKeyword, iSkip, iLength);

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
                    recordsFiltered = oCustomerList.totalRecord,
                    recordsTotal = oCustomerList.totalRecord,
                    data = oCustomerList
                },
                JsonData = jsonData,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteCustomer")]
        public ActionResult<string> DeleteCustomer(string id)
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;
            MasCustomer oCustomer = new MasCustomer();

            try
            {
                oCustomer = _dbContext.MasCustomer.Where(o => o.CustomerId == Guid.Parse(id)).FirstOrDefault();
                sPrevDetail = JsonConvert.SerializeObject(oCustomer);

                oCustomer.IsDeleted = true;
                oCustomer.UpdateByUserId = _requestor.IUser().Id;
                oCustomer.UpdateDate = DateTime.Now;
                _dbContext.MasCustomer.Update(oCustomer);
                _dbContext.SaveChanges();

                sDetail = JsonConvert.SerializeObject(oCustomer);
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
                                    _requestor.IpAddress(), oCustomer.CustomerId.ToString(), Enumeration.ModuleObject.MAS_DEPT.ToString(),
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
