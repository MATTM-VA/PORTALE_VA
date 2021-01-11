using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class LinkCollegato
    {
        public Link Link { get; internal set; }

        public TipoLink Tipo { get; internal set; }
    }
}
