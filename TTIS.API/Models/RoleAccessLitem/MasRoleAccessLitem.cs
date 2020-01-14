using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasRoleAccessLitem
    {
        public int RoleAccessLitemId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectId { get; set; }
        public int ModuleObjectMemberId { get; set; }
        public int RoleAccessId { get; set; }

        public MasRoleAccess RoleAccess { get; set; }
    }
}
