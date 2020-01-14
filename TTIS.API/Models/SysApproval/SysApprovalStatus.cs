using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysApprovalStatus
    {
        public int ApprovalStatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }
    }
}
