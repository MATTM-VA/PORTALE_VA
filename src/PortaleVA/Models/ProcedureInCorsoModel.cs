using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureInCorsoModel : PaginaDinamicaModel
    {
        public ProcedureInCorsoModel()
        {
            Righe = new List<ConteggioProcedura>();
        }

        public IEnumerable<ConteggioProcedura> Righe { get; set; }

        public int Totale { get; set; }
    }
}