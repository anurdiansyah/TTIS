using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasUserDevice
    {
        public Guid UserDeviceId { get; set; }
        public Guid AspNetUserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Imei { get; set; }
        public bool IsNeedApproval { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
