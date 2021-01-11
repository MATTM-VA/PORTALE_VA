using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Models
{
    public class DatiEStrumentiMetadatoStratoModel : PaginaDinamicaModel
    {
        public DatiEStrumentiMetadatoStratoModel()
        {
            Tabs = new List<Tuple<string, string>>();
        }
        
        public Strato Strato { get; set; }

        public List<Tuple<string, string>> Tabs { get; set; }
    }
}