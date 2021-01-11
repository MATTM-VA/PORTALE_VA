using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Report
{
    public class ReportTipoProvvedimento
    {
        public ReportTipoProvvedimento()
        {
            TipoProvvedimento = new Dictionary<int, TipoProvvedimento>();
            RtpItem = new List<ReportTipoProvvedimentoItem>();
        }

        public Dictionary<int,TipoProvvedimento> TipoProvvedimento { get; private set; }

        public List<ReportTipoProvvedimentoItem> RtpItem { get; private set; }
    }
}
