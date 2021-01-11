using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipoAttributo : MultilingualEntity
    {
        public int Ordine { get; internal set; }

        public VoceMenu VoceMenu { get; internal set; }
    }
}
