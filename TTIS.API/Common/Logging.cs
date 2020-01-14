using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTIS.API.Models;

namespace TTIS.API.Common
{
    public class Logging
    {
        public static void LogException(LoggingContext dbContext, string sRefNo, string sClassName, string sMethodName, Exception oEx)
        {
            SysExceptionLog exceptionLog = new SysExceptionLog(dbContext);

            exceptionLog.ReferenceNumber = sRefNo;
            exceptionLog.LogDate = DateTime.Today;
            exceptionLog.Source = string.Format("{0}.{1}", sClassName, sMethodName);
            exceptionLog.Description = oEx.ToString();

            exceptionLog.LogException(sRefNo, sClassName, sMethodName, oEx);
        }

        public static void LogActivity(LoggingContext dbContext, string p_sAspNetUserId, string p_sUserName, string p_sIpAddress, string p_sRefId, string p_sRefObject,
                                        int p_iModuleObjectMemberId, string p_sReferenceNumber, string p_sDetail, string p_sPreviousDetail,
                                        bool p_bIsSuccess, string p_sRemark)
        {
            SysUserLog sysUserLog = new SysUserLog(dbContext);
            sysUserLog.LogUser(p_sAspNetUserId, p_sUserName, p_sIpAddress, p_sRefId, p_sRefObject, 
                               p_iModuleObjectMemberId, p_sReferenceNumber, p_sDetail, p_sPreviousDetail, 
                               p_bIsSuccess, p_sRemark);
        }
    }
}
