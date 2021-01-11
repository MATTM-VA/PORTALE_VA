using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Attributo : MultilingualEntity
    {
        public int Ordine { get; internal set; }

        public TipoAttributo TipoAttributo { get; internal set; }

        public MacroTipoOggetto MacroTipoOggetto { get; internal set; }
    }
}
