using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTIS.WebUi.Models
{
    public partial class SysModuleObjectMember
    {
        [Key]
        public int ModuleObjectMemberId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string MemberDescription { get; set; }
        public int IndexOrder { get; set; }
        public bool IsVisible { get; set; }
        public bool IsNeedApproval { get; set; }

        public SysModuleObject ModuleObject { get; set; }
        public ICollection<MasRoleAccessLitem> MasRoleAccessLitem { get; set; }

    }
}
