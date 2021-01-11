using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureOsservatoriModel : PaginaDinamicaModel
    {
        public List<Tuple<OggettoElenco, LinkCollegato>> Oggetti { get; set; }
    }
}