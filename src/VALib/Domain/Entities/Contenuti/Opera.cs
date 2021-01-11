using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Opera : MultilingualEntity
    {
        public Tipologia Tipologia { get; internal set; }
    }
}
