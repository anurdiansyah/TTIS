using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTIS.WebUi.Models
{ 
    public partial class MasClient
    {
        [Key]
        public Guid ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int SubDistrictId { get; set; }
        public bool AllowMobileDevice { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
