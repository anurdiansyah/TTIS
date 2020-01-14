using RD.Lib;
using System;
using System.Collections.Generic;
using System.IO;

namespace TTIS.API.Models
{
    public partial class SysExceptionLog
    {
        private readonly LoggingContext _dbContext;

        public SysExceptionLog()
        {
        }

        public SysExceptionLog(LoggingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogException(string sRefNo, string sClassName, string sMethodName, Exception oEx)
        {
            try
            {
                ReferenceNumber = sRefNo;
                LogDate = DateTime.Now;
                Source = string.Format("{0}.{1}", sClassName, sMethodName);
                Description = oEx.ToString();

                _dbContext.SysExceptionLog.Add(this);
                _dbContext.SaveChanges();
            }
            catch
            {
                string sContent = string.Empty;
                try
                {
                    sContent = string.Format("{0:yyyy-MM-dd HH:mm:ss}|{1}|{2}{3}{4}{3}",
                        DateTime.Now,
                        sRefNo,
                        string.Format("{0}.{1}",
                            sClassName,
                            sMethodName),
                        System.Environment.NewLine,
                        oEx.ToString());

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
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

    }
}
