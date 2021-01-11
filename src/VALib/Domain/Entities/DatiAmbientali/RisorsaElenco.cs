using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.DatiAmbientali
{
    public class RisorsaElenco : GuidEntity
    {
        public TipoContenutoRisorsa TipoContenuto { get; internal set; }

        public Tema Tema { get; internal set; }

        public string Titolo { get; internal set; }

        public string Scala { get; internal set; }

        public string Url { get; internal set; }

        public string UrlWms { get; internal set; }

        public string UrlGoogleEarth { get; internal set; }

        public string Tipo { get; internal set; }
    }
}
