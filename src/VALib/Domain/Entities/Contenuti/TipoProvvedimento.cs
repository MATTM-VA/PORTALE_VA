using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipoProvvedimento : MultilingualEntity
    {
        public AreaTipoProvvedimento Area { get; internal set; }

        public VoceMenu VoceMenu { get; internal set; }

        public int Ordine { get; internal set; }
    }
}
