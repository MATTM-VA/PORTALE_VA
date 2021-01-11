using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Common;

namespace VALib.Domain.Entities.UI
{
    public class VoceMenu : MultilingualEntityWDescription
    {
        public int GenitoreID { get; internal set; }

        public string Sezione { get; internal set; }

        public string Voce { get; internal set; }

        public bool Link { get; internal set; }

        public bool Editabile { get; internal set; }

        public bool VisibileFrontEnd { get; internal set; }

        public int Ordine { get; internal set; }

        public int TipoMenu { get; internal set; }

        public bool VisibileMappa { get; internal set; }

        public bool WidgetAbilitati { get; internal set; }
    }
}
