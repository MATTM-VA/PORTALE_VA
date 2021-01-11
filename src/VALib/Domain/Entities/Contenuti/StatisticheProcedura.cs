using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class StatisticheProcedura
    {
        public Procedura Procedura { get; internal set; }

        public int InCorso { get; internal set; }

        public int Avviate { get; internal set; }

        public int Concluse { get; internal set; }
    }
}
