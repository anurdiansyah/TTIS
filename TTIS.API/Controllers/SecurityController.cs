using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTIS.API.Common;
using TTIS.API.Configuration;
using TTIS.API.Models;
using TTIS.API.Services;
using TTIS.API.UsersModels;

namespace TTIS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecurityController : RDController
    {

        string _reffNumber;
        private readonly TTISDbContext _dbContext;
        private readonly IS4UsersContext _usersContext;
        private readonly IRequestor _requestor;
        private readonly LoggingContext _loggingContext;
        AppSettings appSettings;

        public SecurityController(TTISDbContext context, IS4UsersContext usersContext, IRequestor requestor, LoggingContext loggingContext, IConfiguration configuration)
        {
            _reffNumber = ReferenceNumber();
            _dbContext = context;
            _requestor = requestor;
            _usersContext = usersContext;
            _loggingContext = loggingContext;
            appSettings = configuration.Get<AppSettings>();
        }

        [HttpGet]
        [Route("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation(string p_sLat, string p_sLng, string p_sBearing)
        {
            RDCustomResponse customResponse = new RDCustomResponse();

            try
            {
                TranUserLocation tranUserLocation = new TranUserLocation();
                tranUserLocation.UserLocationId = NewGuid();
                tranUserLocation.AspNetUserId = _requestor.IUser().Id;
                tranUserLocation.Latitude = p_sLat;
                tranUserLocation.Longitude = p_sLng;
                tranUserLocation.Bearing = p_sBearing;
                tranUserLocation.CreateByUserId = _requestor.IUser().Id;
                tranUserLocation.CreateDate = DateTime.Now;

                _dbContext.TranUserLocation.Add(tranUserLocation);
                _ = await _dbContext.SaveChangesAsync();

                customResponse.IsSuccess = true;
                customResponse.StatusCode = customResponse.IsSuccess ? 200 : 500;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                customResponse.Message = ex.Message.ToString();
            }

            return Json(customResponse);
        }

        [HttpGet]
        [Route("CheckDevice")]
        public IActionResult CheckDevice(string p_sImei)
        {
            RDCustomResponse customResponse = new RDCustomResponse();

            try
            {
                if (_requestor.IUser() != null)
                {
                    if (_dbContext.MasUserDevice.Any(o => o.Imei == p_sImei && o.IsActive == true))
                    {
                        if (_dbContext.MasUserDevice.Where(o => o.Imei == p_sImei && o.IsActive == true).FirstOrDefault().AspNetUserId == Guid.Parse(_requestor.IUser().Id))
                        {
                            customResponse.IsSuccess = true;
                        }
                        else
                        {
                            customResponse.Message = Environment.NewLine + "Akun ini terdaftar untuk Perangkat Android lain,"
                                                   + Environment.NewLine + "Hubungi Administrator untuk bantuan"
                                                   + Environment.NewLine;
                        }
                    }
                    else
                    {
                        customResponse.Message = Environment.NewLine + "Perangkat Android ini belum terdaftar"
                                               + Environment.NewLine + "Hubungi Administrator untuk bantuan"
                                               + Environment.NewLine;
                    }

                    customResponse.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                customResponse.Message = ex.Message.ToString();
            }
            customResponse.StatusCode = customResponse.IsSuccess ? 200 : 500;
            return Json(customResponse);
        }

        [HttpGet]
        [Authorize]
        [Route("SyncData")]
        public IActionResult SyncData()
        {
            RDCustomResponse customResponse = new RDCustomResponse();

            try
            {
                if (_requestor.IUser() != null)
                {
                    List<string> lstDataSync = new List<string>();
                    lstDataSync.Add(appSettings.API.ClientMinVersion); //Mobile Application Version
                    lstDataSync.Add(string.IsNullOrEmpty(_requestor.IUser().Id.ToString()) ? string.Empty : _requestor.IUser().Id.ToString());
                    lstDataSync.Add(string.IsNullOrEmpty(_requestor.IUser().UserDetail.EmployeeDetail.EmployeeId.ToString()) ? string.Empty : _requestor.IUser().UserDetail.EmployeeDetail.EmployeeId.ToString());
                    lstDataSync.Add(string.IsNullOrEmpty(_requestor.IUser().UserDetail.EmployeeDetail.Version.ToString()) ? string.Empty : _requestor.IUser().UserDetail.EmployeeDetail.Version.ToString());
                    lstDataSync.Add(string.IsNullOrEmpty(_requestor.IUser().UserDevice.Imei) ? string.Empty : _requestor.IUser().UserDevice.Imei);

                    SysApproval sysApproval = _dbContext.SysApproval.Any(o => o.CreateByUserId == lstDataSync[1] && o.ApprovalStatusId == (int)Enumeration.ApprovalStatus.New && o.ModuleObjectMemberId == (int)Enumeration.ModuleObjectMember.VEH_USR_ADD)
                                ? _dbContext.SysApproval.Where(o => o.CreateByUserId == lstDataSync[1] && o.ApprovalStatusId == (int)Enumeration.ApprovalStatus.New && o.ModuleObjectMemberId == (int)Enumeration.ModuleObjectMember.VEH_USR_ADD).FirstOrDefault()
                                : new SysApproval();
                    lstDataSync.Add(string.IsNullOrEmpty(sysApproval.ApprovalCode) ? string.Empty : sysApproval.ApprovalCode);
                    lstDataSync.Add(string.IsNullOrEmpty(sysApproval.ApprovalCode) ? "0" : sysApproval.ApprovalStatusId.ToString());

                    MasVehicleUser masVehicleUser = _dbContext.MasVehicleUser.Where(o => o.EmployeeId == Guid.Parse(lstDataSync[2]) && o.IsActive == true).FirstOrDefault();
                    if(masVehicleUser != null)
                    {
                        MasVehicle masVehicle = _dbContext.MasVehicle.Where(o => o.VehicleId == masVehicleUser.VehicleId).FirstOrDefault();
                        lstDataSync.Add(masVehicle.VehicleId.ToString());
                        lstDataSync.Add(masVehicle.Version.ToString());
                    }

                    StringBuilder sbData = new StringBuilder();
                    for (int i = 0; i < 9; i++)
                    {
                        if (i != 0) sbData.Append("|");
                        if(i< lstDataSync.Count) sbData.Append(lstDataSync[i]);
                    }
                    customResponse.Message = "OK";
                    customResponse.JsonData = sbData.ToString();
                    customResponse.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                customResponse.Message = ex.Message.ToString();
            }

            customResponse.StatusCode = customResponse.IsSuccess ? 200 : 500;
            return Json(customResponse);
        }
    }
}