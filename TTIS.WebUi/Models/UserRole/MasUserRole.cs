using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasUserRole
    {
        public int UserRoleId { get; set; }
        public int RoleAccessId { get; set; }
        public string AspNetUserId { get; set; }
    }
}
