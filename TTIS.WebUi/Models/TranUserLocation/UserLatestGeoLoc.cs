using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.WebUi.Models
{
    public partial class UserLatestGeoLoc
    {
        public string TagNumber { get; set; }
        public string NickName { get; set; }
        public string PasPhoto { get; set; }

        [Key]
        public Guid UserLocationId { get; set; }
        public string AspNetUserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Bearing { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
    }
}
