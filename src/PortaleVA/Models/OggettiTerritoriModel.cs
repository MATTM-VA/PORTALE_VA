using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettiTerritoriModel
    {
        public string Regioni { get; set; }

        public string Province { get; set; }

        public string Comuni { get; set; }

        public string AreeMarine { get; set; }

        public bool ImmagineLocalizzazione { get; set; }

        public int OggettoID { get; set; }

        public string LinkLocalizzazione { get; set; }
    }
}