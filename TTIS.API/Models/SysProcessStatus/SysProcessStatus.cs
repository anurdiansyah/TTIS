using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysProcessStatus
    {
        public int ProcessStatusId { get; set; }
        public int ProcessId { get; set; }
        public string ProcessStatusCode { get; set; }
        public string ProcessStatusName { get; set; }
        public string Description { get; set; }
    }
}
