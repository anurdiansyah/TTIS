using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class MasCustomer
    {
        public MasCustomer()
        {
            MasCustomerBranch = new HashSet<MasCustomerBranch>();
            MasCustomerContact = new HashSet<MasCustomerContact>();
        }

        public Guid CustomerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public ICollection<MasCustomerBranch> MasCustomerBranch { get; set; }
        public ICollection<MasCustomerContact> MasCustomerContact { get; set; }
    }
}
