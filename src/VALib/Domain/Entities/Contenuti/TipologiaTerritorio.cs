using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipologiaTerritorio : MultilingualEntity
    {
        public TipologiaTerritorioEnum Enum { get; internal set; }

        public bool MostraRicerca { get; internal set; }
    }
}
