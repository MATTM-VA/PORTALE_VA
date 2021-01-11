using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.Contenuti;

namespace VALib.Domain.Entities.UI
{
    public class ContenutoOggettoCarosello : MultilingualEntityWDescription
    {
        public CategoriaNotiziaEnum CategoriaNotizia { get; set; }
    }
}
