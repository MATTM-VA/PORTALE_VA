using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Web
{
    public enum VAWebEventTypeEnum
    {
        DownloadDocumentoOggetto = 1, 
        DownloadDocumentoPortale = 2,
        DownloadDocumentiProvvedimento = 3,
        DownloadDocumentoCondivisione = 4,
        DownloadDocumentoMedia = 5, 
        DownloadAltroDocumento = 99,
        Errore = 500
    }
}
