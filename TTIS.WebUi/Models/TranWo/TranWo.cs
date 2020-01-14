using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class TranWo
    {
        public Guid WorkOrderId { get; set; }
        public string Won { get; set; }
        public Guid ClientId { get; set; }
        public Guid CargoId { get; set; }
        public Guid OriginId { get; set; }
        public int Qty { get; set; }
        public int UnitId { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleModelId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid DestinationId { get; set; }
        public int WostatusId { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
