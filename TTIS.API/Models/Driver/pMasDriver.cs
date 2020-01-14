using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{
    public partial class MasDriver
    {
        [NotMapped]
        public string EmployeeName { get; set; }

        [NotMapped]
        public string Base64LicensePicture { get; set; }
    }
}
