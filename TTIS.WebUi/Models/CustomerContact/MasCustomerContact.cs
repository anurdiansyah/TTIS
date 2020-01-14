using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class MasCustomerContact
    {
        public Guid CustomerContactId { get; set; }
        public Guid CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

    }
}
