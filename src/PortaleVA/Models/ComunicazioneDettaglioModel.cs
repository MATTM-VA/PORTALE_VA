using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VAPortale.Models.Common;

namespace VAPortale.Models
{
    public class ComunicazioneDettaglioModel : PaginaDinamicaModel
    {
        public Notizia Notizia { get; set; }
    }
}