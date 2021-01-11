using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti.Base
{
    public class OggettoBase : MultilingualEntityWDescription
    {
        public TipoOggetto TipoOggetto { get; internal set; }
    }
}
