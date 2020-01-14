using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TTIS.API.UsersModels;

namespace TTIS.API.Models
{
    public partial class MasEmployee
    {

        [NotMapped]
        public string LicenseNumber { get; set; }

        [NotMapped]
        public string LicenseType { get; set; }

        [NotMapped]
        public DateTime LicenseExpiryDate { get; set; }

        [NotMapped]
        public string LicensePicture { get; set; }


        [NotMapped]
        public int RegisterAs { get; set; }

        [NotMapped]
        public bool RegisterAsUser { get; set; }

        [NotMapped]
        public string Base64PasPhoto { get; set; }

        [NotMapped]
        public string Base64IdentityImage { get; set; }

        [NotMapped]
        public MasDriver MasDriver { get; set; }
        [NotMapped]
        public MasDriverAssistant MasDriverAssistant { get; set; }

        [NotMapped]
        public AspNetUsers AspNetUser { get; set; }

    }
}
