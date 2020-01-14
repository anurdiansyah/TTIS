using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasCustomerBranch
    {
        public Guid CustomerBranchId { get; set; }
        public Guid CustomerId { get; set; }
        public string BranchCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int SubDistrictId { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public MasCustomer Customer { get; set; }
    }
}
