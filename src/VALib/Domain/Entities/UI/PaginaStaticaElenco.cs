using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.UI
{
    public class PaginaStaticaElenco : MultilingualEntity
    {
        public VoceMenu VoceMenu { get; internal set; }
    }
}
