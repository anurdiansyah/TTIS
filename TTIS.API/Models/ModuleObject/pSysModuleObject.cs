using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class SysModuleObject
    {
        [JsonIgnore]
        public SysModule Module { get; set; }
    }
}
