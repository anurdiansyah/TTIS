using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTIS.IServ4.Configuration
{

    /// <summary>
    /// Provides strong-typed access to the the application settings that are loaded through the 'appSettings.json' file.
    /// </summary>
    //[UsedImplicitly]
    public class AppSettings
    {
        public Oidc Oidc { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }

        public DatabaseServer Database { get; set; } = DatabaseServer.LocalDb;

        public Url Url { get; set; }


    }
}
