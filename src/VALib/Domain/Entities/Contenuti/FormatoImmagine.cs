using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class FormatoImmagine
    {
        public int ID { get; internal set; }

        public string Nome { get; internal set; }

        public int AltezzaMax { get; internal set; }

        public int AltezzaMin { get; internal set; }

        public int LarghezzaMax { get; internal set; }

        public int LarghezzaMin { get; internal set; }

        public FormatoImmagineEnum Enum { get; internal set; }

        public bool Abilitato { get; internal set; }
    }
}
