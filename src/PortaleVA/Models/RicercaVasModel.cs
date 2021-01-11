using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using VAPortale.Models.Common;

namespace VAPortale.Models
{
    public class RicercaVasModel : PaginaDinamicaModel
    {
        public IEnumerable<SelectListItem> ProcedureSelectList { get; set; }

        public IEnumerable<SelectListItem> SettoriSelectList { get; set; }
    }
}