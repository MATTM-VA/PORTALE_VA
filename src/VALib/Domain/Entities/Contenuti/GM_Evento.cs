using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class GM_Evento : MultilingualEntity
    {
        public TipoEventoEnum TipoEvento { get; internal set; }
        public DateTime? DataInizio { get; internal set; }
        public DateTime? DataFine { get; internal set; }
    }
}
