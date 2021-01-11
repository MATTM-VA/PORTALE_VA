using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.UI
{
    public class WidgetCorrelato
    {
        public int VoceMenuID { get; set; }

        public int WidgetID { get; set; }

        public int Ordine { get; set; }

        public Widget Widget { get; set; }
    }
}
