using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using TTIS.API.Common;

using TTIS.API.Models;
using TTIS.API.Services;
using TTIS.API.UsersModels;

namespace TTIS.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AspNetUsersController : RDController
    {
        private Int32 m_iModuleObjectId = 1001;
        private readonly IS4UsersContext _usersContext;
        private readonly TTISDbContext _ttisContext;
        private readonly IRequestor _requestor;
        private readonly LoggingContext _loggingContext;

        string _reffNumber;

        public AspNetUsersController(IS4UsersContext usersContext, TTISDbContext ttisContext, IRequestor requestor, LoggingContext loggingContext)
        {
            _usersContext = usersContext;
            _ttisContext = ttisContext;
            _requestor = requestor;
            _loggingContext = loggingContext;
            _reffNumber = ReferenceNumber();
        }

        [HttpGet]
        [Route("AspNetUsers")]
        public ActionResult<IEnumerable<string>> List(string p_sKeyword)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            AspNetUsersCollection oAspNetUsersList = new AspNetUsersCollection(_usersContext);

            try
            {
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;
                oAspNetUsersList.List(sKeyword);
                jsonData = JsonConvert.SerializeObject(oAspNetUsersList);

                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                oAspNetUsersList.Clear();
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
        [Route("AspNetUser")]
        public ActionResult<string> GetAspNetUsers(string id)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            AspNetUsers aspNetUsers = new AspNetUsers();
            try
            {
                bool bUserIsExist = _usersContext.AspNetUsers.Any(o => o.Id == id);
                aspNetUsers = !bUserIsExist ? new AspNetUsers() : _usersContext.AspNetUsers.Where(o => o.Id == id).FirstOrDefault();

                aspNetUsers.UserDetail = bUserIsExist && _ttisContext.MasUserDetail.Any(o => o.AspNetUserId == id)
                                            ? _ttisContext.MasUserDetail.Where(o => o.AspNetUserId == id).FirstOrDefault()
                                            : new MasUserDetail();
                aspNetUsers.UserDetail.EmployeeDetail = _ttisContext.EmployeeDetail.Any(o => o.TagNumber == aspNetUsers.UserDetail.TagNumber)
                                                        ? _ttisContext.EmployeeDetail.Where(o => o.TagNumber == aspNetUsers.UserDetail.TagNumber).FirstOrDefault()
                                                        : new EmployeeDetail();
                aspNetUsers.UserDetail.MasUserRole = bUserIsExist
                                                        ? _ttisContext.MasUserRole.Where(o => o.AspNetUserId == aspNetUsers.Id.ToString()).ToList()
                                                        : new List<MasUserRole>();
                List<int> myRoles = new List<int>();
                foreach (var o in aspNetUsers.UserDetail.MasUserRole)
                {
                    myRoles.Add(o.RoleAccessId);
                }
                aspNetUsers.RoleAccess = _ttisContext.MasRoleAccess.Where(o => myRoles.Contains(o.RoleAccessId)).ToList();

                jsonData = JsonConvert.SerializeObject(aspNetUsers);

                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                aspNetUsers = null;
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
        [Route("ValidateUserByTag")]
        public ActionResult<string> ValidateUserByTag(string p_sTagNumber)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            AspNetUsers aspNetUsers = new AspNetUsers();
            try
            {
                bool bUserIsExist = _ttisContext.MasUserDetail.Any(o => o.TagNumber == p_sTagNumber);
                MasUserDetail masUserDetail = _ttisContext.MasUserDetail.Where(obj => obj.TagNumber == p_sTagNumber).FirstOrDefault();
                aspNetUsers = !bUserIsExist ? new AspNetUsers() : _usersContext.AspNetUsers.Where(o => o.Id == masUserDetail.AspNetUserId).FirstOrDefault();

                if (!bUserIsExist)
                {
                    bIsSuccess = true;
                    jsonData = JsonConvert.SerializeObject(_ttisContext.MasEmployee.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault());
                }
                else
                {
                    sMessage = "Nomor tag '" + p_sTagNumber + "' terdaftar sebagai '" + aspNetUsers.UserName + "'";
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                aspNetUsers = null;
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
        [Route("EmployeeDetailByTag")]
        public ActionResult<string> EmployeeDetailByTag(string p_sTagNumber)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            string jsonData = string.Empty;

            AspNetUsers aspNetUsers = new AspNetUsers();
            try
            {
                bool bUserIsExist = _ttisContext.MasUserDetail.Any(o => o.TagNumber == p_sTagNumber);
                MasUserDetail masUserDetail = _ttisContext.MasUserDetail.Where(obj => obj.TagNumber == p_sTagNumber).FirstOrDefault();
                aspNetUsers = !bUserIsExist ? new AspNetUsers() : _usersContext.AspNetUsers.Where(o => o.Id == masUserDetail.AspNetUserId).FirstOrDefault();

                if (_ttisContext.MasEmployee.Any(o => o.TagNumber == p_sTagNumber))
                {
                    bIsSuccess = true;
                    jsonData = JsonConvert.SerializeObject(_ttisContext.MasEmployee.Where(o => o.TagNumber == p_sTagNumber).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                aspNetUsers = null;
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
        public ActionResult<IEnumerable<string>> AspNetUsers(string p_sKeyword, int draw, int start, int length)
        {
            bool bIsSuccess = false;
            string sMessage = string.Empty;
            AspNetUsersCollection oAspNetUsersList = new AspNetUsersCollection(_usersContext);

            try
            {
                int iSkip = (start > 0 && length > 0) ? length * (start / length) : 0;
                int iLength = length > 0 ? length : Int32.MaxValue;
                string sKeyword = string.IsNullOrEmpty(p_sKeyword) ? string.Empty : p_sKeyword;

                oAspNetUsersList.List(sKeyword, iSkip, iLength);
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
                    recordsFiltered = oAspNetUsersList.totalRecord,
                    recordsTotal = oAspNetUsersList.totalRecord,
                    data = oAspNetUsersList,
                },
                JsonData = string.Empty,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("PostAspNetUsers")]
        public IActionResult PostAspNetUsers()
        {
            bool bIsExist = false;
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            AspNetUsers p_oUser = new AspNetUsers();
            bool bIsNeedApproval = _ttisContext.SysModuleObject.Where(o => o.ModuleObjectId == m_iModuleObjectId).FirstOrDefault().IsNeedApproval;

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    p_oUser = JsonConvert.DeserializeObject<AspNetUsers>(jsonObject);
                    bIsExist = _usersContext.AspNetUsers.Any(o => o.Id == p_oUser.Id || o.UserName == p_oUser.UserName);
                    p_oUser.Id = bIsExist ? p_oUser.Id : Guid.NewGuid().ToString();

                    bool bDetailIsExist = _ttisContext.MasUserDetail.Any(o => o.AspNetUserId == p_oUser.Id);
                    MasUserDetail masUserDetail = bDetailIsExist ? _ttisContext.MasUserDetail.Where(o => o.AspNetUserId == p_oUser.Id).FirstOrDefault() : new MasUserDetail();
                    masUserDetail.AspNetUserId = p_oUser.Id;
                    masUserDetail.TagNumber = p_oUser.UserName.Split("@")[0];
                    if (!bDetailIsExist) _ttisContext.MasUserDetail.Add(masUserDetail);

                    List<MasUserRole> masUserRoles = _ttisContext.MasUserRole.Where(o => o.AspNetUserId == p_oUser.Id).ToList();
                    foreach (MasUserRole oUserRole in masUserRoles)
                    {
                        if (!p_oUser.RoleAccess.Any(o => o.RoleAccessId == oUserRole.RoleAccessId))
                        {
                            _ttisContext.MasUserRole.Remove(oUserRole);
                        }
                    }

                    foreach (MasRoleAccess oRole in p_oUser.RoleAccess)
                    {
                        if (!_ttisContext.MasUserRole.Any(o => o.AspNetUserId == p_oUser.Id && o.RoleAccessId == oRole.RoleAccessId))
                        {
                            MasUserRole oUserRole = new MasUserRole();
                            oUserRole.AspNetUserId = p_oUser.Id;
                            oUserRole.RoleAccessId = oRole.RoleAccessId;

                            _ttisContext.MasUserRole.Add(oUserRole);
                        }
                    }

                    AspNetUsers oUser = bIsExist ? _usersContext.AspNetUsers.Where(o => o.Id == p_oUser.Id || o.UserName == p_oUser.UserName).FirstOrDefault() : new AspNetUsers();
                    sPrevDetail = JsonConvert.SerializeObject(oUser);

                    oUser.Id = p_oUser.Id;
                    oUser.UserName = p_oUser.UserName;
                    oUser.NormalizedUserName = p_oUser.NormalizedUserName;
                    oUser.Email = p_oUser.Email;
                    oUser.NormalizedEmail = p_oUser.NormalizedEmail;
                    oUser.PhoneNumber = p_oUser.PhoneNumber;
                    oUser.PhoneNumberConfirmed = !bIsNeedApproval;
                    oUser.PasswordHash = !bIsExist ? HashPassword(p_oUser.UserName.Split("@")[0]) : p_oUser.PasswordHash;
                    oUser.AccessFailedCount = !bIsExist ? 0 : p_oUser.AccessFailedCount;
                    oUser.ConcurrencyStamp = Guid.NewGuid().ToString();
                    oUser.EmailConfirmed = false;
                    oUser.LockoutEnabled = false;
                    oUser.LockoutEnd = null;
                    oUser.PhoneNumberConfirmed = false;
                    oUser.SecurityStamp = Guid.NewGuid().ToString();
                    sDetail = JsonConvert.SerializeObject(oUser);
                    if (!bIsExist) _usersContext.AspNetUsers.Add(oUser);
                    else _usersContext.AspNetUsers.Update(oUser);


                    if (!bIsNeedApproval)
                    {
                        _usersContext.SaveChanges();
                        _ttisContext.SaveChanges();

                        sMessage = bIsExist ? "Perubahan data berhasil disimpan" : "Penambahan data berhasil disimpan";
                    }
                    else
                    {
                        int iMemberId = !bIsExist
                                          ? (int)Enumeration.ModuleObjectMember.SCR_USR_ADD
                                          : (int)Enumeration.ModuleObjectMember.SCR_USR_EDIT;

                        oUser.PhoneNumberConfirmed = false;
                        _usersContext.AspNetUsers.Update(oUser);
                        _usersContext.SaveChanges();

                        SysApproval sysApproval = new SysApproval();
                        sysApproval.ApprovalId = NewGuid();
                        sysApproval.ModuleObjectId = m_iModuleObjectId;
                        sysApproval.ModuleObjectMemberId = iMemberId;
                        sysApproval.ApprovalStatusId = (int)Enumeration.ApprovalStatus.New;
                        sysApproval.ReffObj = "User";
                        sysApproval.ReffId = oUser.Id.ToString();
                        sysApproval.Detail = sDetail;
                        sysApproval.PreviousDetail = sPrevDetail;
                        sysApproval.Remark = string.Empty;
                        sysApproval.Version = 1;
                        sysApproval.CreateDate = sysApproval.UpdateDate = DateTime.Now;
                        sysApproval.CreateByUserId = sysApproval.UpdateByUserId = _requestor.IUser().Id;

                        _ttisContext.SysApproval.Add(sysApproval);
                        _ttisContext.SaveChanges();

                        sMessage = bIsExist ? "Permohonan perubahan data berhasil" : "Permohonan penambahan data berhasil";
                    }

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
                int iMemberId = !bIsExist ? (int)Enumeration.ModuleObjectMember.SCR_USR_ADD : (int)Enumeration.ModuleObjectMember.SCR_USR_EDIT;

                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), p_oUser.Id, Enumeration.ModuleObject.SCR_USR.ToString(),
                                    iMemberId, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, string.Empty);
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = string.Empty,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword()
        {
            string sDetail = string.Empty;
            string sPrevDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;
            ChangePasswordModel p_oModel = new ChangePasswordModel();

            try
            {
                if (Request.HasFormContentType)
                {
                    var form = Request.Form;
                    string jsonObject = form["JsonObject"];
                    p_oModel = JsonConvert.DeserializeObject<ChangePasswordModel>(jsonObject);
                    AspNetUsers oUser = _usersContext.AspNetUsers.Where(o => o.Id == p_oModel.Id).FirstOrDefault();
                    sPrevDetail = JsonConvert.SerializeObject(oUser);

                    oUser.PasswordHash = HashPassword(p_oModel.NewPassword);

                    _usersContext.AspNetUsers.Update(oUser);
                    _usersContext.SaveChanges();

                    sMessage = "Password Successfully Changed";
                    sDetail = JsonConvert.SerializeObject(oUser);
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
                int iMemberId = (int)Enumeration.ModuleObjectMember.SCR_USR_EDIT;

                #region Log User
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), p_oModel.Id, Enumeration.ModuleObject.SCR_USR.ToString(),
                                    iMemberId, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, string.Empty);

                #endregion
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = string.Empty,
                ReferenceNumber = _reffNumber,
                StatusCode = bIsSuccess ? 200 : 500,
                IsSuccess = bIsSuccess.ToString(),
                Message = sMessage
            });
        }

        [HttpDelete]
        [Route("DeleteAspNetUsers")]
        public IActionResult DeleteAspNetUsers(string id)
        {
            string sPrevDetail = string.Empty;
            string sDetail = string.Empty;

            bool bIsSuccess = false;
            string sMessage = string.Empty;

            AspNetUsers aspNetUsers = new AspNetUsers();
            try
            {
                bool bUserIsExist = _usersContext.AspNetUsers.Any(o => o.Id == id);
                aspNetUsers = !bUserIsExist ? new AspNetUsers() : _usersContext.AspNetUsers.Where(o => o.Id == id).FirstOrDefault();

                if (aspNetUsers.Id != null)
                {
                    sPrevDetail = JsonConvert.SerializeObject(aspNetUsers);

                    _usersContext.AspNetUsers.Remove(aspNetUsers);
                    _usersContext.SaveChanges();

                    //_ttisContext.MasUserDetail.RemoveRange(_ttisContext.MasUserDetail.Where(o => o.AspNetUserId == id));
                    _ttisContext.MasUserRole.RemoveRange(_ttisContext.MasUserRole.Where(o => o.AspNetUserId == id));
                    _ttisContext.SaveChanges();

                    sDetail = JsonConvert.SerializeObject(aspNetUsers);
                }

                sMessage = "Data Deleted Successfully";
                bIsSuccess = true;
            }
            catch (Exception ex)
            {
                Logging.LogException(_loggingContext, _reffNumber, GetType().FullName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                sMessage = ex.Message.ToString();
            }
            finally
            {
                int iMemberId = (int)Enumeration.ModuleObjectMember.SCR_USR_DELETE;
                Logging.LogActivity(_loggingContext, _requestor.IUser().Id, _requestor.IUser().UserName,
                                    _requestor.IpAddress(), aspNetUsers.Id, Enumeration.ModuleObject.SCR_USR.ToString(),
                                    iMemberId, ReferenceNumber(), sDetail, sPrevDetail, bIsSuccess, string.Empty);
            }

            return Json(new
            {
                DataTable = string.Empty,
                JsonData = string.Empty,
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
