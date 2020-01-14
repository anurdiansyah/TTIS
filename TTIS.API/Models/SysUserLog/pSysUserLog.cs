using RD.Lib;
using System;
using System.Collections.Generic;
using System.IO;

namespace TTIS.API.Models
{
    public partial class SysUserLog
    {

        private readonly LoggingContext _dbContext;

        public SysUserLog()
        {
        }

        public SysUserLog(LoggingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogUser(string p_sAspNetUserId, string p_sUserName, string p_sIpAddress, string p_sRefId, string p_sRefObject,
                            int p_iModuleObjectMemberId, string p_sReferenceNumber, string p_sDetail, string p_sPreviousDetail,
                            bool p_bIsSuccess, string p_sRemark)
        {
            try
            {
                AspNetUserId = p_sAspNetUserId;
                UserName = p_sUserName;
                IpAddress = p_sIpAddress;
                RefId = p_sRefId;
                RefObject = p_sRefObject;
                ModuleObjectMemberId = p_iModuleObjectMemberId;
                ReferenceNumber = p_sReferenceNumber;
                Detail = p_sDetail;
                PreviousDetail = p_sPreviousDetail;
                IsSuccess = p_bIsSuccess;
                Remark = p_sRemark;
                IsDeleted = false;
                LogDate = DateTime.Now;

                _dbContext.SysUserLog.Add(this);
                _dbContext.SaveChanges();
            }
            catch
            {
                string sContent = string.Empty;
                try
                {
                    sContent = string.Format("{0:yyyy-MM-dd HH:mm:ss}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        DateTime.Now,
                        UserName,
                        IpAddress,
                        RefId,
                        RefObject,
                        ReferenceNumber,
                        Detail,
                        PreviousDetail,
                        IsSuccess.ToString(),
                        Remark);

                    RDLogging.WriteToFile(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"),
                        sContent,
                        true);

                }
                catch
                {
                    try
                    {
                        RDLogging.WriteToEventLog("TTIS.API", sContent);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
