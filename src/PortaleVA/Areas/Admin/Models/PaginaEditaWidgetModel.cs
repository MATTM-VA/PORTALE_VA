using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class PaginaEditaWidgetModel
    {
        public int ID { get; set; }

        public int[] EditaWidget { get; set; }

        public List<WidgetCorrelato> WidgetCorrelati { get; set; }

        public List<Widget> Widget { get; set; }

        public VoceMenu VoceMenu { get; set; }

        public PaginaStatica Pagina { get; set; }
    }
}