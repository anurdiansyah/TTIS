﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.WebUi.Configuration
{
    public class TtsiApi
    {
        public string AuthUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string ApiUrl { get; set; }
    }
}
