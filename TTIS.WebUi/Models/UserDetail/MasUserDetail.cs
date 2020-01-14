using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.WebUi.Models
{ 
    public partial class MasUserDetail
    {

        [Key]
        public int UserDetailId { get; set; }
        public string AspNetUserId { get; set; }
        public string TagNumber { get; set; }

        public ICollection<MasUserRole> MasUserRole { get; set; }

        [NotMapped]
        public EmployeeDetail EmployeeDetail { get; set; }
    }
}
