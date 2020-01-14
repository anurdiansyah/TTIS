using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysLog
    {
        public int SystemLogId { get; set; }
        public int ProcessId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime LogDate { get; set; }
        public bool IsSuccess { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
    }
}
