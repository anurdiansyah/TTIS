using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class SysParamGroup
    {
        public int SysParamGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IndexOrder { get; set; }
    }
}
