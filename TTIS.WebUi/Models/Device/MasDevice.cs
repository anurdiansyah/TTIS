using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasDevice
    {
        public int DeviceId { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        public string GadgetType { get; set; }
        public string Series { get; set; }
        public string Manufacturer { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
