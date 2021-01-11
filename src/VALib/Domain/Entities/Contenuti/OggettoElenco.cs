using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoElenco : OggettoBase
    {
        public DateTime Data { get; internal set; }

        public string Proponente { get; internal set; }

        public Procedura Procedura { get; internal set; }

        public int OggettoProceduraID { get; internal set; }

        public string ViperaAiaID { get; internal set; }

        public string StatoImpianto { get; internal set; }

        public string AttivitaIPPC { get; internal set; }

        public string Territorio { get; internal set; }

        public int Competenza { get; internal set; }
    }
}
