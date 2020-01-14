using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class TranDestination
    {
        public Guid DestinationId { get; set; }
        public string ClientId { get; set; }
        public string Pic { get; set; }
        public string Picnumber { get; set; }
        public string Address { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
