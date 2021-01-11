using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.UI;
using VAPortale.Models.Common;

namespace VAPortale.Models
{
    public class PagineVoceModel : PaginaDinamicaModel
    {
        public PaginaStatica PaginaStatica { get; set; }

        public List<WidgetCorrelato> Widget { get; set; }
    }
}