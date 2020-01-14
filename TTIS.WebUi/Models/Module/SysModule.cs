using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TTIS.WebUi.Models
{
    public partial class SysModule
    {
        public SysModule()
        {
            SysModuleObject = new HashSet<SysModuleObject>();
        }

        [Key]
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public int IndexOrder { get; set; }
        public bool IsVisible { get; set; }
        public string Icon { get; set; }
        public string DefaultUrl { get; set; }
        public string AdditionalStyle { get; set; }

        public ICollection<SysModuleObject> SysModuleObject { get; set; }
    }
}
