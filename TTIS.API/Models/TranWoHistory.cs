using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class TranWoHistory
    {
        public Guid WohistoryId { get; set; }
        public Guid WorkOrderId { get; set; }
        public int WoactionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
    }
}
