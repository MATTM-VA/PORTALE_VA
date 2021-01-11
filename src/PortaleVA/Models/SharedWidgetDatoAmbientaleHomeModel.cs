using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class SharedWidgetDatoAmbientaleHomeModel
    {
        public List<DatoAmbientaleHome> DatiAmbientali { get; set; }

        public int NumeroElementi { get; set; }

        public string Titolo { get; set; }

        public VoceMenu VoceMenu { get; set; }
    }
}