using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class EventiDocumentazioneModel : PaginazioneModel
    {
        public int ID { get; set; }

        public int? RaggruppamentoID { get; set; }

        public string Testo { get; set; }

        public EventoModel Evento { get; set; }
        public IEnumerable<DocumentoElenco> Documenti { get; set; }


    }
}