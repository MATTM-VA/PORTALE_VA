using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedWidgetInEvidenzaModel
    {
        public List<Notizia> Notizie { get; set; }

        public CategoriaNotizia CategoriaNotizie { get; set; }

        public VoceMenu VoceMenu { get; set; }
    }
}

