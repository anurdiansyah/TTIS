using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasEmployee
    {
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string LicensePicture { get; set; }
        public int RegisterAs { get; set; }
        public bool RegisterAsUser { get; set; }
        public string Base64PasPhoto { get; set; }
    }
}
