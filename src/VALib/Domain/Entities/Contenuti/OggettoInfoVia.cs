using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoInfoVia : OggettoInfoBase
    {
        public Opera Opera { get; internal set; }

        public string Cup { get; internal set; }

        public List<OggettoElenco> AltriOggetti { get; internal set; }
    }
}
