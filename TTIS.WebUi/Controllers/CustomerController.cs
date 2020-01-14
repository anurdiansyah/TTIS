using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TTIS.WebUi.Common;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    [Authorize]
    public class CustomerController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;

        MasCustomer masCustomer { 
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("sessMasCustomer")))
                {
                    return new MasCustomer();
                }
                else
                {
                    return JsonConvert.DeserializeObject<MasCustomer>(HttpContext.Session.GetString("sessMasCustomer"));
                };
            }

            set
            {
                HttpContext.Session.SetString("sessMasCustomer", JsonConvert.SerializeObject(value)); 
            }
        }

        HttpClient httpClient;

        #endregion

        #region Constructor

        public CustomerController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
            _security = security;
        }

        #endregion

        #region Method

        public async Task<IActionResult> Index()
        {
            httpClient = await _gitternsHttpClient.GetClient();

            if (_security.IsHaveAccessRight((int)SecurityEnumeration.ModuleObjectMember.CUST_PRCPL_VIEW)) return View();
            else return RedirectToAction("NoAccess", "Error");
        }

        public async Task<IActionResult> GetCustomerAsync(string p_iId)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                if (!string.IsNullOrEmpty(p_iId))
                {
                    Dictionary<string, string> dictParam = new Dictionary<string, string>();
                    dictParam.Add("id", p_iId);

                    response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Customer/Customer", httpClient, dictParam));
                    if (!string.IsNullOrEmpty(response.JsonData))
                    {
                        masCustomer = JsonConvert.DeserializeObject<MasCustomer>(response.JsonData);
                    }
                    response = response == null ? new RDCustomResponse() : response;
                }
                else
                {
                    masCustomer = new MasCustomer();

                    response.IsSuccess = true;
                    response.StatusCode = 200;
                    response.JsonData = JsonConvert.SerializeObject(masCustomer);
                }
                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> CustomerListAsync(string p_sKeyword, int draw, int start, int length)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                RDCustomResponse response = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("Customer/DataTable", httpClient, dictParam));

                return Json(new
                {
                    draw = response.DataTable.draw,
                    recordsFiltered = response.DataTable.recordsFiltered,
                    recordsTotal = response.DataTable.recordsTotal,
                    data = response.DataTable.data
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(MasCustomer p_oCustomer)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                foreach (IFormFile oFile in HttpContext.Request.Form.Files)
                {
                    formFiles.Add(oFile);
                }

                masCustomer.Code = p_oCustomer.Code;
                masCustomer.Name = p_oCustomer.Name;
                masCustomer.Address = p_oCustomer.Address;
                masCustomer.PhoneNumber = p_oCustomer.PhoneNumber;
                masCustomer.Email = p_oCustomer.Email;

                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("Customer/PostCustomer", httpClient, masCustomer, formFiles));
                response = response == null ? new RDCustomResponse() : response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iId.ToString());
                response = JsonConvert.DeserializeObject<RDCustomResponse>(DeleteAsync("Customer/DeleteCustomer", await _gitternsHttpClient.GetClient(), dictParam));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

        #region Customer Contact

        public async Task<IActionResult> CustomerContactListAsync(string p_sKeyword, int draw, int start, int length)
        {
            httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? "" : p_sKeyword;
                List<MasCustomerContact> dtReturn = masCustomer.MasCustomerContact.Where(o => (o.ContactName.Contains(sKeyword) 
                                                                                               || o.ContactAddress.Contains(sKeyword)
                                                                                               || o.ContactNumber.Contains(sKeyword))
                                                                                               && o.IsDeleted == false).ToList();

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = dtReturn.Count,
                    recordsTotal = dtReturn.Count,
                    data = dtReturn
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult GetCustomerContact(Guid p_iId)
        {
            RDCustomResponse response = new RDCustomResponse();

            try
            {
                MasCustomerContact masCustomerContact = !masCustomer.MasCustomerContact.Any(o => o.CustomerContactId == p_iId)
                                                        ? new MasCustomerContact()
                                                        : masCustomer.MasCustomerContact.Where(o => o.CustomerContactId == p_iId).FirstOrDefault();
                response.JsonData = JsonConvert.SerializeObject(masCustomerContact);
                response.IsSuccess = true;
                response.StatusCode = 200;

                return Json(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IActionResult SaveContact(MasCustomerContact p_oCustomerContact)
        {
            bool bIsSuccess = false;
            RDCustomResponse response = new RDCustomResponse();
            MasCustomer oMasCustomer = JsonConvert.DeserializeObject<MasCustomer>(HttpContext.Session.GetString("sessMasCustomer"));

            try
            {
                p_oCustomerContact.CustomerContactId = NewGuid();
                p_oCustomerContact.Version = -99;
                oMasCustomer.MasCustomerContact.Add(p_oCustomerContact);
                masCustomer = oMasCustomer;

                bIsSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                response.IsSuccess = bIsSuccess;
                response.StatusCode = bIsSuccess ? 200 : 500;
                response.Message = bIsSuccess ? "Contact successfully added" : "Failed to add contact";
            }

            return Json(response);
        }

        public IActionResult DeleteContact(Guid p_iId)
        {
            bool bIsSuccess = false;
            RDCustomResponse response = new RDCustomResponse();
            MasCustomer oMasCustomer = JsonConvert.DeserializeObject<MasCustomer>(HttpContext.Session.GetString("sessMasCustomer"));

            try
            {
                MasCustomerContact p_oCustomerContact = oMasCustomer.MasCustomerContact.Where(o => o.CustomerContactId == p_iId).FirstOrDefault();

                p_oCustomerContact.IsDeleted = true;
                if (p_oCustomerContact.Version < 0) masCustomer.MasCustomerContact.Remove(p_oCustomerContact);
                
                masCustomer = oMasCustomer;

                bIsSuccess = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                response.IsSuccess = bIsSuccess;
                response.StatusCode = bIsSuccess ? 200 : 500;
                response.Message = bIsSuccess ? "Contact successfully deleted" : "Failed to delete contact"; ;
            }

            return Json(response);
        }
        #endregion

    }
}
