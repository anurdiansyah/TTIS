using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class MasTitle
    {
        public int TitleId { get; set; }
        public int DepartmentId { get; set; }
        public int UnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganizationLevel { get; set; }
    }
}
