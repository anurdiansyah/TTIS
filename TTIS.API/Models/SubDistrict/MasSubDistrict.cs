using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasSubDistrict
    {
        public int SubDistrictId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
    }
}
