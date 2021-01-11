using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.Contenuti.Base;

namespace VAPortale.Models.Common
{
    public class OggettiDocumentazioneBaseModel : PaginazioneModel
    {
        public int ID { get; set; }

        public int OggettoProceduraID { get; set; }

        public int? RaggruppamentoID { get; set; }

        public string Testo { get; set; }

        public IEnumerable<DocumentoElenco> Documenti { get; set; }

        public OggettoDocumentazioneBase Oggetto { get; set; }

        //public IEnumerable<ProceduraCollegata> ProcedureCollegate { get; set; }

        //public ProceduraCollegata ProceduraSelezionata { get; set; }

    }
}