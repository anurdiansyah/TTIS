using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.WebUi.Models
{
    public partial class Crews
    {
        //Crew
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string KtpNo { get; set; }
        public string KtpName { get; set; }
        public string KtpAddress { get; set; }
        public string KtpVillage { get; set; }
        public string KtpSubdistrict { get; set; }
        public string KtpCity { get; set; }

        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string ResignReason { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] RowVersion { get; set; }
        public string TagNo { get; set; }
        public Guid PhotoId { get; set; }
        public DateTime InputOn { get; set; }
        public string InputBy { get; set; }
        public DateTime LastUpdateOn { get; set; }
        public string LastUpdateBy { get; set; }
        public Guid CompanyId { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public Int32? StatusSopir { get; set; }

        //Detail
        public string AddressName { get; set; }
        public string AddressVillage { get; set; }
        public string AddressSubdistrict { get; set; }
        public string AddressCity { get; set; }
        public string AddressType { get; set; }
        public bool? AddressIsPrimary { get; set; }
        public string PhoneNo { get; set; }
        public bool? PhoneIsPrimary { get; set; }

        public string LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseCity { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string LicenseImage { get; set; }

        public string IdentityImage { get; set; }


        public string Base64Photo { get; set; }
        public string Base64LicenseImage { get; set; }
        public string Base64IdentityImage { get; set; }
    }
}
