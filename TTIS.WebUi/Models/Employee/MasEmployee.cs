using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{ 
    public partial class MasEmployee
    {
        public Guid EmployeeId { get; set; }
        public string TagNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string IdentityAddress { get; set; }
        public string IdentitySubDistrict { get; set; }
        public string IdentityDistrict { get; set; }
        public string IdentityCity { get; set; }
        public string IdentityProvince { get; set; }
        public string IdentityPicture { get; set; }
        public string LivingAddress { get; set; }
        public string LivingSubDistrict { get; set; }
        public string LivingDistrict { get; set; }
        public string LivingCity { get; set; }
        public string LivingProvince { get; set; }
        public string PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public int UnitId { get; set; }
        public int TitleId { get; set; }
        public int EmployeeStatusId { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime ResignDate { get; set; }
        public string ResignReason { get; set; }
        public string Email { get; set; }
        public string PasPhoto { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }
    }
}
