using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class ConteggioTipoProvvedimento
    {
        public TipoProvvedimento TipoProvvedimento { get; internal set; }

        public int Conteggio { get; internal set; }
    }
}
