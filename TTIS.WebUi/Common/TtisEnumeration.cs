using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.WebUi.Common
{
    public class TtisEnumeration
    {
        public enum ApprovalStatus
        {
            New = 1,
            Approved = 2,
            Rejected = 3,
        }

        public enum RegisterEmployeeAs
        { 
            StandardEmployee = 1,
            Driver = 2,
            DriverAssistant = 3,
        }
    }
}
