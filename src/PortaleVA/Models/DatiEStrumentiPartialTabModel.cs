using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Models
{
    public class DatiEStrumentiPartialTabModel
    {
        public List<Elenco> Voci { get; set; }

        public int ElencoSelezionatoID { get; set; }

        public string Voce { get; set; }
    }
}