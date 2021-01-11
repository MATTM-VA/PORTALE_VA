using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ComunicazioneNotizieModel : PaginazioneModel
    {
        public List<Notizia> Notizie { get; set; }

        public CategoriaNotizia Categoria { get; set; }

        public string Testo { get; set; }
        
        public string ActionDettaglio { get; set; }

        public string ActionAttuale { get; set; }
    }
}