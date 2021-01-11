using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class OggettiRicercaDocumentiModel : PaginazioneModel
    {
        public int MacroTipoOggettoID { get; set; }

        public int OggettoProceduraID { get; set; }

        public int? RaggruppamentoID { get; set; }

        public string Testo { get; set; }

        public List<DocumentoElenco> Documenti { get; set; }

    }
}