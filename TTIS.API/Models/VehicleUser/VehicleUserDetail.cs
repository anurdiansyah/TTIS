using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{
    [Table("VVehicleUser")]
    public partial class VehicleUserDetail
    {
        [Key]
        public Guid? VehicleId { get; set; }
        public Guid? VehicleUserId { get; set; }
        public string Plate { get; set; }
        public string VehicleCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid? EmployeeId { get; set; }
        public Int32 VehicleUsageTypeId { get; set; }
        public string UsageType { get; set; }
        public string VehicleUser { get; set; }
        public bool IsNeedApproval { get; set; }

    }
}
