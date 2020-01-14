using System;
using System.Collections.Generic;

namespace TTIS.API.Models
{
    public partial class MasCityGroupMember
    {
        public Guid CityGroupMemberId { get; set; }
        public Guid CityGroupId { get; set; }
        public int CityId { get; set; }
    }
}
