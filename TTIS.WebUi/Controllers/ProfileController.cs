using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using TTIS.WebUi.Common;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    [Authorize]
    public class ProfileController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;
        private readonly ISecurity _security;
        private readonly IAppUser _appUser;

        #endregion

        #region Constructor

        public ProfileController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration, ISecurity security, IAppUser appUser)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
            _security = security;
            _appUser = appUser;
        }

        #endregion

        #region Method

        public async Task<IActionResult> Index()
        {
            AspNetUsers currentUser = await _appUser.CurrentUser();
            currentUser = currentUser == null ? new AspNetUsers() : currentUser;
            EmployeeDetail currentEmployee = currentUser.UserDetail.EmployeeDetail == null ? new EmployeeDetail() : currentUser.UserDetail.EmployeeDetail;

            return View(currentEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel p_oModel)
        {
            RDCustomResponse response = new RDCustomResponse();
            try
            {
                if(p_oModel.NewPassword != p_oModel.ConfirmNewPassword)
                {
                    response.IsSuccess = true;
                    response.Message = "New Password not match..!";
                    return Json(response);
                }

                AspNetUsers user = await _appUser.CurrentUser();
                p_oModel.Id = user.Id;
                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("AspNetUsers/ChangePassword", await _gitternsHttpClient.GetClient(), p_oModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(EmployeeDetail p_oEmployee)
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

                response = JsonConvert.DeserializeObject<RDCustomResponse>(PostAsync("Employee/PostEmployee", httpClient, p_oEmployee, formFiles));
                response = response == null ? new RDCustomResponse() : response;

                if (response.IsSuccess)
                {
                    HttpContext.Session.SetString("sessIUser", string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(response);
        }

        #endregion

    }
}
