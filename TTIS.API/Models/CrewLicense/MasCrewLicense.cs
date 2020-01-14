using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasCrewLicense
    {
        public int CrewLicenseId { get; set; }
        public Guid CrewId { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public string City { get; set; }
        public string LicenseImage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateByUserId { get; set; }
    }
}
