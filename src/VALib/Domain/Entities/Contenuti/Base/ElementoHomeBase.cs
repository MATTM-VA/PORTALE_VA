using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti.Base
{
    public abstract class ElementoHomeBase : OggettoBase
    {
        public string LinkLocalizzazione { get; internal set; }

        public Tipologia Tipologia { get; internal set; }

        public CategoriaImpianto CategoriaImpianto { get; internal set; }

        public String ProponenteGestore { get; internal set; }

    }
}
