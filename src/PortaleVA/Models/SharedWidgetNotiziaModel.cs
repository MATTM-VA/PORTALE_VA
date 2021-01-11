using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedWidgetNotiziaModel
    {
        public List<Notizia> Notizie { get; set; }

        public CategoriaNotizia CategoriaNotizie { get; set; }

        public int NumeroElementi { get; set; }

        public bool Home { get; set; }

        public VoceMenu VoceMenu { get; set; }
    }
}