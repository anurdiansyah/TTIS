using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class SysUserLog
    {
        public int UserLogId { get; set; }
        public string AspNetUserId { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public string RefId { get; set; }
        public string RefObject { get; set; }
        public int ModuleObjectMemberId { get; set; }
        public string ReferenceNumber { get; set; }
        public string Detail { get; set; }
        public string PreviousDetail { get; set; }
        public bool IsSuccess { get; set; }
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LogDate { get; set; }

    }
}
