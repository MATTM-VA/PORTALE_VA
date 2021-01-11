using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Report
{
    public class ReportTipologia
    {
        public ReportTipologia()
        {
            TipoProvvedimento = new Dictionary<int, TipoProvvedimento>();
            RtpItem = new List<ReportTipologiaItem>();
            ReportMT = new ReportMacroTipologie();
        }

        public Dictionary<int, TipoProvvedimento> TipoProvvedimento { get; private set; }

        public List<ReportTipologiaItem> RtpItem { get; private set; }

        public ReportMacroTipologie ReportMT { get; private set; }

    }
}
