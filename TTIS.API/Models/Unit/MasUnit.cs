﻿using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasUnit
    {
        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
