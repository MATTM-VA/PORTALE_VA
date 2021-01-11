using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipoFile : Entity
    {
        public string FileIcona { get; internal set; }

        public string Estensione { get; internal set; }

        public string TipoMime { get; internal set; }

        public string Software { get; internal set; }
    }
}
