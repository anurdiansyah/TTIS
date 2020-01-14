using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.API.Common
{
    public class Enumeration
    {

        public enum ModuleObject
        {
            SCR_USR = 1001,
            SCR_USR_DVC = 1004,
            SCR_GRP = 1002,
            SCR_APPR = 1003,
            PRE_PARAM = 2001,
            ATRL_USR_LOG = 3001,
            MAS_PROV = 4001,
            MAS_CITY = 4002,
            MAS_DSTR = 4003,
            MAS_SUB_DSTR = 4004,
            MAS_DEPT = 4005,
            MAS_UNIT = 4006,
            MAS_TITLE = 4007,
            EMP_MAS = 5001,
            EMP_DRIV = 5002,
            EMP_DRIV_ASTN = 5003,
            CUST_PRCPL = 6001,
            CUST_BRN = 6002,
            CUST_ORD_LOC = 6003,
            VEH_MAS = 7001,
            VEH_TYPE = 7002,
            VEH_MDL = 7003,
            VEH_USR = 7004,
        }

        public enum ModuleObjectMember
        {
            SCR_USR_VIEW = 100101,
            SCR_USR_ADD = 100102,
            SCR_USR_EDIT = 100103,
            SCR_USR_DELETE = 100104,

            SCR_USR_DVC_VIEW = 100401,
            SCR_USR_DVC_ADD = 100402,
            SCR_USR_DVC_EDIT = 100403,
            SCR_USR_DVC_DELETE = 100404,
            SCR_USR_DVC_APPROVE = 100405,
            SCR_USR_DVC_REJECT = 100406,

            SCR_GRP_VIEW = 100201,
            SCR_GRP_ADD = 100202,
            SCR_GRP_EDIT = 100203,
            SCR_GRP_DELETE = 100204,

            SCR_APPR_VIEW = 100301,
            SCR_APPR_ADD = 100302,
            SCR_APPR_EDIT = 100303,
            SCR_APPR_DELETE = 100304,
            SCR_APPR_APPROVE = 100305,

            PRE_PARAM_VIEW = 200101,
            PRE_PARAM_EDIT = 200103,

            ATRL_USR_LOG_VIEW = 300101,
            ATRL_USR_LOG_PURGE = 300102,

            MAS_PROV_VIEW = 400101,
            MAS_PROV_ADD = 400102,
            MAS_PROV_EDIT = 400103,
            MAS_PROV_DELETE = 400104,

            MAS_CITY_VIEW = 400201,
            MAS_CITY_ADD = 400202,
            MAS_CITY_EDIT = 400203,
            MAS_CITY_DELETE = 400204,

            MAS_DSTR_VIEW = 400301,
            MAS_DSTR_ADD = 400302,
            MAS_DSTR_EDIT = 400303,
            MAS_DSTR_DELETE = 400304,

            MAS_SUB_DSTR_VIEW = 400401,
            MAS_SUB_DSTR_ADD = 400402,
            MAS_SUB_DSTR_EDIT = 400403,
            MAS_SUB_DSTR_DELETE = 400404,

            MAS_DEPT_VIEW = 400501,
            MAS_DEPT_ADD = 400502,
            MAS_DEPT_EDIT = 400503,
            MAS_DEPT_DELETE = 400504,

            MAS_UNIT_VIEW = 400601,
            MAS_UNIT_ADD = 400602,
            MAS_UNIT_EDIT = 400603,
            MAS_UNIT_DELETE = 400604,

            MAS_TITLE_VIEW = 400701,
            MAS_TITLE_ADD = 400702,
            MAS_TITLE_EDIT = 400703,
            MAS_TITLE_DELETE = 400704,

            EMP_MAS_VIEW = 500101,
            EMP_MAS_ADD = 500102,
            EMP_MAS_EDIT = 500103,
            EMP_MAS_DELETE = 500104,

            EMP_DRIV_VIEW = 500201,
            EMP_DRIV_ADD = 500202,
            EMP_DRIV_EDIT = 500203,
            EMP_DRIV_DELETE = 500204,

            EMP_DRIV_ASTN_VIEW = 500301,
            EMP_DRIV_ASTN_ADD = 500302,
            EMP_DRIV_ASTN_EDIT = 500303,
            EMP_DRIV_ASTN_DELETE = 500304,

            CUST_PRCPL_VIEW = 600101,
            CUST_PRCPL_ADD = 600102,
            CUST_PRCPL_EDIT = 600103,
            CUST_PRCPL_DELETE = 600104,
            CUST_PRCPL_CTCT_VIEW = 600105,
            CUST_PRCPL_CTCT_ADD = 600106,
            CUST_PRCPL_CTCT_EDIT = 600107,
            CUST_PRCPL_CTCT_DELETE = 600108,

            CUST_BRN_VIEW = 600201,
            CUST_BRN_ADD = 600202,
            CUST_BRN_EDIT = 600203,
            CUST_BRN_DELETE = 600204,

            CUST_ORD_LOC_VIEW = 600301,
            CUST_ORD_LOC_ADD = 600302,
            CUST_ORD_LOC_EDIT = 600303,
            CUST_ORD_LOC_DELETE = 600304,

            VEH_MAS_VIEW = 700101,
            VEH_MAS_ADD = 700102,
            VEH_MAS_EDIT = 700103,
            VEH_MAS_DELETE = 700104,

            VEH_TYPE_VIEW = 700201,
            VEH_TYPE_ADD = 700202,
            VEH_TYPE_EDIT = 700203,
            VEH_TYPE_DELETE = 700204,

            VEH_MDL_VIEW = 700301,
            VEH_MDL_ADD = 700302,
            VEH_MDL_EDIT = 700303,
            VEH_MDL_DELETE = 700304,

            VEH_USR_VIEW = 700401,
            VEH_USR_ADD = 700402,
            VEH_USR_EDIT = 700403,
            VEH_USR_DELETE = 700404,
        }

        public enum ApprovalStatus
        {
            New = 1,
            Approved = 2,
            Rejected = 3,
        }

        public enum EmployeeType
        {
            Pengemudi = 1,
            Kenek = 2,
            Kantor = 3
        }

        public enum VehicleStatus
        {
            Siap_Digunakan = 1,
            Dalam_Penggunaan = 2,
            Dalam_Perbaikan = 3,
            Tidak_Dapat_Digunakan = 4
        }

        public enum VehicleUsageType
        {
            TidakDigunakan = -99,
            Permanent = 1,
            Temporary = 2,
        }

        public enum RegisterEmployeeAs
        {
            StandardEmployee = 1,
            Driver = 2,
            DriverAssistant = 3,
        }
    }
}
