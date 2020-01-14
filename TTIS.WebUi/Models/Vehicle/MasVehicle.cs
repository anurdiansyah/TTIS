using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasVehicle
    {
        public Guid VehicleId { get; set; }
        public string NomorRegistrasi { get; set; }
        public string VehicleCode { get; set; }
        public string Plate { get; set; }
        public string Merk { get; set; }
        public string Tipe { get; set; }
        public int TypeId { get; set; }
        public int ModelId { get; set; }
        public string NoRangka { get; set; }
        public string NoMesin { get; set; }
        public string Warna { get; set; }
        public Guid FuelId { get; set; }
        public int TahunPerakitan { get; set; }
        public int TahunRegistrasi { get; set; }
        public int VehicleStatusId { get; set; }
        public string VehicleImage { get; set; }
        public string Stnkimage { get; set; }
        public int StnkpositionId { get; set; }
        public string StnkpositionReffId { get; set; }
        public DateTime StnkberlakuHingga { get; set; }
        public string Bpkbimage { get; set; }
        public int BpkbpositionId { get; set; }
        public string BpkbpositionReffId { get; set; }
        public string Kirimage { get; set; }
        public DateTime KirberlakuHingga { get; set; }
        public int KirpositionId { get; set; }
        public string KirpositionReffId { get; set; }
        public bool IsNeedApproval { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
