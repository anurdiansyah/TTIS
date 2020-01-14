using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasUserDetail
    {
        public int UserDetailId { get; set; }
        public string AspNetUserId { get; set; }
        public string TagNumber { get; set; }
    }
}
