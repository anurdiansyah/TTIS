using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{ 
    [Table("VMyModule")]
    public partial class MyModule
    {
        [Key]
        public int RoleAccessLitemId { get; set; }
        public int RoleAccessId { get; set; }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public string ModuleDefaultUrl { get; set; }
        public int ModuleIndexOrder { get; set; }

        public int ModuleObjectId { get; set; }
        public string ModuleObjectName { get; set; }
        public string ModuleObjectIcon { get; set; }
        public string ModuleObjectDefaultUrl { get; set; }
        public int ModuleObjectIndexOrder { get; set; }

        public int ModuleObjectMemberId { get; set; }
        public string ModuleObjectMemberName { get; set; }

    }
}
