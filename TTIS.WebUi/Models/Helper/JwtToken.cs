using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.WebUi.Models.Helper
{
    public class JwtToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiredTime { get; set; }

        public JwtToken(string p_sAccessToken, int p_iExpiresIn, DateTime p_dtExpiredTime)
        {
            AccessToken = p_sAccessToken;
            ExpiresIn = p_iExpiresIn;
            ExpiredTime = p_dtExpiredTime;
        }
        public JwtToken()
        {
        }
    }
}
