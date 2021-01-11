using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;

namespace VALib.Domain.Entities.Contenuti
{
    public class CategoriaNotizia : MultilingualEntity
    {
        public CategoriaNotiziaEnum Enum { get; internal set; }
    }
}
