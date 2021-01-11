using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VAPortale.Models.Common;

namespace VAPortale.Models
{
    public class ComunicazioneProvvedimentiModel : PaginazioneModel
    {
        public List<Notizia> Notizie { get; set; }

        public string Testo { get; set; }

        public bool AnnoInCorso { get; set; }

        public int Anno { get; set; }

        public string TitoloPagina { get; set; }

        public int NumeroTotale { get; set; }
    }
}