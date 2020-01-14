using System;
using System.Collections.Generic;

namespace TTIS.WebUi.Models
{
    public partial class SysApproval
    {
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
    }
}
