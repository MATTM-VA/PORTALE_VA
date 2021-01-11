using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.DatiAmbientali
{
    public class TipoContenutoRisorsa : MultilingualEntity
    {
        public string Estensioni { get; internal set; }

        public string FileIcona { get; internal set; }
    }
}
