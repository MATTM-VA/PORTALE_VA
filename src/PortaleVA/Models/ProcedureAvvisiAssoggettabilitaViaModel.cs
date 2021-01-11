using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureAvvisiAssoggettabilitaViaModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public IEnumerable<DocumentoElenco> Documenti { get; set; }

    }
}