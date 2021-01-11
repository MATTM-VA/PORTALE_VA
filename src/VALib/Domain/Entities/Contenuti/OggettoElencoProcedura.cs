using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoElencoProcedura : OggettoElenco
    {
        public StatoProcedura StatoProcedura { get; internal set; }
    }
}
