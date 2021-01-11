using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models.Common
{
    public class PaginaDinamicaModel
    {
        public VoceMenu VoceMenu { get; set; }

        public List<WidgetCorrelato> Widget { get; set; }
    }
}