using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.Contenuti
{
    public class Procedura : MultilingualEntity
    {
        public MacroTipoOggettoEnum MacroTipoOggettoEnum { get; internal set; }

        public AmbitoProcedura AmbitoProcedura { get; internal set; }

        public int Ordine { get; internal set; }
    }
}
