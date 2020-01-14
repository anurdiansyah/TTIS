using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysExceptionLog
    {
        public long ExceptionLogId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime LogDate { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }
}
