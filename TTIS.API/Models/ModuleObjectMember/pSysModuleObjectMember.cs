using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysModuleObjectMember
    {
        [JsonIgnore]
        public SysModuleObject ModuleObject { get; set; }
    }
}
