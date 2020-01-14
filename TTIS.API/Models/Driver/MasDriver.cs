using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasDriver
    {
        public Guid DriverId { get; set; }
        public Guid EmployeeId { get; set; }
        public string TagNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string LicensePicture { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
