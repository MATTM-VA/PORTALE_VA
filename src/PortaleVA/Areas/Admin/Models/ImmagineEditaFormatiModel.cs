using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Areas.Admin.Models
{
    public class ImmagineEditaFormatiModel
    {
        public Immagine ImmagineMaster { get; set; }

        public int ID { get; set; }

        public Dictionary<FormatoImmagine, Immagine> FormatiImmagine { get; set; }

        public FormatoImmagine FormatoImmagine { get; set; }

        public Immagine Immagine { get; set; }

        public int Modalita { get; set; }
    }
}