using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class HomeElencoOggettiModel
    {
        public HomeElencoOggettiModel()
        {
            Oggetti = new List<OggettoHome>();
        }

        public List<OggettoHome> Oggetti { get; set; }

        public string MessaggioElencoVuoto { get; set; }

        public TipoElencoEnum TipoElenco { get; set; }

        public MacroTipoOggettoEnum MacroTipoOggetto { get; set; }
    }
}