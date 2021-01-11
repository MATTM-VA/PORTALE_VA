using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VAPortale.Code;

namespace VAPortale.Models
{
    public class ProcedureProvvedimentiModel : PaginaDinamicaModel
    {
        public ProcedureProvvedimentiModel()
        {
            Tabelle = new List<TabellaConteggioProvvedimenti>();
        }
        
        public List<TabellaConteggioProvvedimenti> Tabelle { get; set; }
    }
}