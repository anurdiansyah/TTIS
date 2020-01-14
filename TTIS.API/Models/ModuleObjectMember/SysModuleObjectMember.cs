using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysModuleObjectMember
    {
        public int ModuleObjectMemberId { get; set; }
        public int ModuleId { get; set; }
        public int ModuleObjectId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string MemberDescription { get; set; }
        public int IndexOrder { get; set; }
        public bool IsVisible { get; set; }
        public bool IsNeedApproval { get; set; }
    }
}
