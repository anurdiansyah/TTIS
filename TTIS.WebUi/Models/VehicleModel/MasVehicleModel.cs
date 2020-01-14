using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class MasVehicleModel
    {
        public int VehicleModelId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
