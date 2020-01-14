using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasDriverAssistant
    {
        public Guid DriverAssistantId { get; set; }
        public Guid EmployeeId { get; set; }
        public string TagNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
