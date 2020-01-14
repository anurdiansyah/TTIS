using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TTIS.API.Models
{
    public partial class MasUserDetail
    {
        [NotMapped]
        public EmployeeDetail EmployeeDetail { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<MasUserRole> MasUserRole { get; set; }
    }
}
