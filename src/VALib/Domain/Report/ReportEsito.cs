using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Report
{
    public class ReportEsito
    {

        public ReportEsito()
        {
            ListaEsiti = new List<ReportEsitoItem>();
        }

        public List<ReportEsitoItem> ListaEsiti { get; private set; }

        public string Title { get; set; }
    }
}
