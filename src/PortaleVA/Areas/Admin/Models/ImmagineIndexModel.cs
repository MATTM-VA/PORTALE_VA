using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Areas.Admin.Models
{
    public class ImmagineIndexModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public List<Immagine> Immagini { get; set; }
    }
}