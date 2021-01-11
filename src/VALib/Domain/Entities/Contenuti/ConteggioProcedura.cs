using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class ConteggioProcedura
    {
        public Procedura Procedura { get; internal set; }

        public int Conteggio { get; internal set; }

        public int Parametro { get; internal set; }
    }
}
