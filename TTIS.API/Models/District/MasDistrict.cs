using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasDistrict
    {
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
    }
}
