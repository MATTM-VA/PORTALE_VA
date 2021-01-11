using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Code
{
    public class TabellaStatisticheProcedure
    {
        public TabellaStatisticheProcedure()
        {
            Righe = new List<StatisticheProcedura>();
        }
        
        public AmbitoProcedura AmbitoProcedura { get; set; }

        public List<StatisticheProcedura> Righe { get; set; }

    }
}