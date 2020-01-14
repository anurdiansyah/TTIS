using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTIS.API.Models
{
    public partial class MasRoleAccess
    {
        [NotMapped]
        public ICollection<SysModule> SysModules { get; set; }
    }
}
