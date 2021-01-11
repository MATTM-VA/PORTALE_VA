using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureViaVasInCorsoModel : PaginaDinamicaModel
    {
        public ProcedureViaVasInCorsoModel()
        {
            RigheVia = new List<ConteggioProcedura>();
            RigheVas = new List<ConteggioProcedura>();
            RigheAia = new List<ConteggioProcedura>();
        }

        public IEnumerable<ConteggioProcedura> RigheVia { get; set; }
        public IEnumerable<ConteggioProcedura> RigheVas { get; set; }
        public IEnumerable<ConteggioProcedura> RigheAia { get; set; }

        public int TotaleVia { get; set; }
        public int TotaleVas { get; set; }
        public int TotaleAia { get; set; }
    }
}