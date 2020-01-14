using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{
    [Table("VMasUserDeviceDetail")]
    public partial class MasUserDeviceDetail
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        [Key]
        public Guid UserDeviceId { get; set; }
        public Guid AspNetUserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Imei { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
