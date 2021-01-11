using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipoOggetto : MultilingualEntity
    {
        public string Descrizione { get; internal set; }

        public TipoOggettoEnum Enum { get; internal set; }

        public MacroTipoOggetto MacroTipoOggetto { get; internal set; }
    }
}
