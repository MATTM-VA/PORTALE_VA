using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Domain.Entities.DatiAmbientali
{
    public class StratoMetadatiLink
    {
        public string Denominazione { get; set; }

        public string LinkTesto { get; set; }

        public string LinkTarget { get; set; }

        public string LinkUrl { get; set; }

        public string LinkTooltip { get; set; }
    }
}
