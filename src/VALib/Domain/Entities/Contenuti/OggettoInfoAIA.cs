using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoInfoAIA : OggettoInfoBase
    {
        public CategoriaImpianto CategoriaImpianto { get; internal set; }
        public StatoImpianto StatoImpianto { get; internal set; }

        public string IndirizzoImpianto { get; internal set; }

        public List<AttivitaIPPC> AttivitaIPPC { get; internal set; }
    }
}
