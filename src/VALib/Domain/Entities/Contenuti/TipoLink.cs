using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using System.Threading;

namespace VALib.Domain.Entities.Contenuti
{
    public class TipoLink : MultilingualEntity
    {
        public TipoLinkEnum Enum { get; internal set; }
    }
}
