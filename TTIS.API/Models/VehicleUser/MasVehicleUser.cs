using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasVehicleUser
    {
        public Guid VehicleUserId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int VehicleUsageTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
