using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTIS.WebUi.Models
{
    public partial class SysModuleObject
    {

        [Key]
        public int ModuleObjectId { get; set; }
        public int ModuleId { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDescription { get; set; }
        public int IndexOrder { get; set; }
        public bool IsVisible { get; set; }
        public bool IsNeedApproval { get; set; }
        public string Icon { get; set; }
        public string DefaultUrl { get; set; }
        public string AdditionalStyle { get; set; }

        public ICollection<SysModuleObjectMember> SysModuleObjectMember { get; set; }
        public ICollection<MasRoleAccessLitem> MasRoleAccessLitem { get; set; }
    }
}
