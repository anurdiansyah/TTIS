using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTIS.WebUi.Models
{
    public partial class MasRoleAccessLitem
    {
        [Key]
        public int RoleAccessLitemId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectId { get; set; }
        public int ModuleObjectMemberId { get; set; }
        public int RoleAccessId { get; set; }

        public SysModule Module { get; set; }
        public SysModuleObject ModuleObject { get; set; }
        public SysModuleObjectMember ModuleObjectMember { get; set; }
        public MasRoleAccess RoleAccess { get; set; }
    }
}
