using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VAPortale.Code;
using VAPortale.Models.Common;

namespace VAPortale.Models
{
    public class ProcedureStatisticheModel : PaginaDinamicaModel
    {
        public ProcedureStatisticheModel()
        {
            Tabelle = new List<TabellaStatisticheProcedure>();
        }

        public int? Anno { get; set; }

        public IEnumerable<SelectListItem> AnniSelectList { get; set; }

        public List<TabellaStatisticheProcedure> Tabelle { get; set; }

        public bool AnnoCorrente { get; set; }
    }
}