using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{
    [Table("VApprovalDetail")]
    public partial class SysApprovalDetail
    {
        [Key]
        public Guid ApprovalId { get; set; }
        public string ApprovalCode { get; set; }
        public int ModuleObjectId { get; set; }
        public int ModuleObjectMemberId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ReffId { get; set; }
        public string ReffObj { get; set; }
        public string Detail { get; set; }
        public string PreviousDetail { get; set; }
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateByUserId { get; set; }

        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ActionName { get; set; }
        public string ApprovalStatusName { get; set; }
    }
}
