using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.Contenuti
{
    public class ProceduraCollegata
    {
        public int OggettoProceduraID { get; internal set; }
        
        public Procedura Procedura { get; internal set; }

        public StatoProcedura StatoProcedura { get; internal set; }

        public DateTime? Data { get; internal set; }

        public int NumeroDocumenti { get; internal set; }

        public string ViperaAiaID { get; internal set; }
    }
}
