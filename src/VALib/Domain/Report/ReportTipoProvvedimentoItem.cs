using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Report
{
    public class ReportTipoProvvedimentoItem
    {
        public ReportTipoProvvedimentoItem() 
        {
            TotaliTipi = new Dictionary<int, int>();
        }

        public Dictionary<int, int> TotaliTipi { get; private set; }

        public int Anno { get; set; }
    }
}
