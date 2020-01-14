using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TTIS.API.Models;
using TTIS.API.Services;
using TTIS.API.UsersModels;

namespace TTIS.API.Services
{
    public class Requestor : RDController, IRequestor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IS4UsersContext _usersContext;
        private readonly TTISDbContext _ttisContext;
        public AspNetUsers _iUser = new AspNetUsers();

        public Requestor(IHttpContextAccessor httpContextAccessor, IS4UsersContext usersContext, TTISDbContext ttisContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _usersContext = usersContext;
            _ttisContext = ttisContext;
        }

        public string IpAddress()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public AspNetUsers IUser()
        {
            try
            {
                string userId = _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("AspNetUserId")
                                ? userId = _httpContextAccessor.HttpContext.Request.Headers["AspNetUserId"]
                                : userId = _httpContextAccessor.HttpContext.User.Claims.First(o => o.Type == ClaimTypes.NameIdentifier).Value;
                if (_usersContext.AspNetUsers.Any(o => o.Id == userId))
                {
                    _iUser = _usersContext.AspNetUsers.Where(o => o.Id == userId).FirstOrDefault();
                    _iUser.UserDetail = _ttisContext.MasUserDetail.Any(o => o.AspNetUserId == userId)
                                        ? _ttisContext.MasUserDetail.Where(o => o.AspNetUserId == userId).FirstOrDefault()
                                        : new Models.MasUserDetail();
                    _iUser.UserDetail.EmployeeDetail = _ttisContext.EmployeeDetail.Any(o => o.TagNumber == _iUser.UserDetail.TagNumber)
                                                     ? _ttisContext.EmployeeDetail.Where(o => o.TagNumber == _iUser.UserDetail.TagNumber).FirstOrDefault()
                                                     : new EmployeeDetail();
                    _iUser.UserDevice = _ttisContext.MasUserDevice.Any(o => o.AspNetUserId == Guid.Parse(_iUser.Id) && o.IsActive)
                                        ? _ttisContext.MasUserDevice.Where(o => o.AspNetUserId == Guid.Parse(_iUser.Id) && o.IsActive).FirstOrDefault()
                                        : new MasUserDevice();

                    return _iUser;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
