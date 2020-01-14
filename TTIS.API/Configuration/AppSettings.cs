using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.API.Configuration
{

    /// <summary>
    /// Provides strong-typed access to the the application settings that are loaded through the 'appSettings.json' file.
    /// </summary>
    //[UsedImplicitly]
    public class AppSettings
    {
        public ConnectionString ConnectionString { get; set; }
        public Auth Auth { get; set; }
        public API API { get; set; }
    }
}
