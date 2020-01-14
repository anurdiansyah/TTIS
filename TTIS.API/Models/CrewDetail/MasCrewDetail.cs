using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasCrewDetail
    {
        public int CrewDetailId { get; set; }
        public Guid UserId { get; set; }
        public string TagNo { get; set; }
        public string IdentityImage { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateByUserId { get; set; }
    }
}
