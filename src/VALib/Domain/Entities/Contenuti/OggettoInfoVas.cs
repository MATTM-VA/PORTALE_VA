using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Entities.Contenuti
{
    public class OggettoInfoVas : OggettoInfoBase
    {
        public Settore Settore { get; internal set; }
    }
}
