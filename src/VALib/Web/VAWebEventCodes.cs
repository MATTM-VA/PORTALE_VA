using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Management;

namespace VALib.Web
{
    public sealed class VAWebEventCodes
    {
        public const int ErroreGenerico = WebEventCodes.WebExtendedBase + 1;

        public const int LoginUtente = WebEventCodes.WebExtendedBase + 2;

        public const int DownloadDocumento = WebEventCodes.WebExtendedBase + 3;

        public const int ErroreAvvertimento = WebEventCodes.WebExtendedBase + 1001; // Codice di dettaglio per gli eventi ErroreGenerico.
    }
}
