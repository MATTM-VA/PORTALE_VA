using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class EntitaCollegata
    {
        public Entita Entita { get; internal set; }

        public RuoloEntita Ruolo { get; internal set; }
    }
}
