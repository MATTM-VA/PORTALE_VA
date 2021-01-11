using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class HomeElencoProvvedimentiModel
    {
        public HomeElencoProvvedimentiModel()
        {
            Provvedimenti = new List<ProvvedimentoHome>();
        }

        public List<ProvvedimentoHome> Provvedimenti { get; set; }

        public string MessaggioElencoVuoto { get; set; }

        public TipoElencoEnum TipoElenco { get; set; }
    }
}