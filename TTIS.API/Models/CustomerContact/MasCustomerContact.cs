using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasCustomerContact
    {
        public Guid CustomerContactId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CustomerBranchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public MasCustomer Customer { get; set; }
    }
}
