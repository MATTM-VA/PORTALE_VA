using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;

namespace VALib.Domain.Entities.Contenuti
{
    public class AmbitoProcedura : MultilingualEntity
    {
        public int Ordine { get; internal set; }
    }
}
