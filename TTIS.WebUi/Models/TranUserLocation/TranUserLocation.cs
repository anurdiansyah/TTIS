using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class TranUserLocation
    {
        public Guid UserLocationId { get; set; }
        public string AspNetUserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Bearing { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
    }
}
