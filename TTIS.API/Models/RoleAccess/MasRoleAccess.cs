using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasRoleAccess
    {
        public MasRoleAccess()
        {
            MasRoleAccessLitem = new HashSet<MasRoleAccessLitem>();
        }

        public int RoleAccessId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsNeedApproval { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public ICollection<MasRoleAccessLitem> MasRoleAccessLitem { get; set; }
    }
}
