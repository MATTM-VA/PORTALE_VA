using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Areas.Admin.Models
{
    public class DocumentoPortaleIndexModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public IEnumerable<DocumentoPortale> DocumentiPortale { get; set; }
    }
}