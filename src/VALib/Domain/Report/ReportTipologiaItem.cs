using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Report
{
    public class ReportTipologiaItem
    {
        public ReportTipologiaItem()
        {
            TotaliTipi = new Dictionary<int, int>();
        }

        public Tipologia Tipologia { get; set; }

        public Dictionary<int, int> TotaliTipi { get; private set; }



    }
}
