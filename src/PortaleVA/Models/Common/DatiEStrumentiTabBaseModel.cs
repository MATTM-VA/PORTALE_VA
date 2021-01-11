using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Models.Common
{
    public class DatiEStrumentiTabBaseModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public Elenco Elenco { get; set; }

        public string NomeElenco { get; set; }
    }
}