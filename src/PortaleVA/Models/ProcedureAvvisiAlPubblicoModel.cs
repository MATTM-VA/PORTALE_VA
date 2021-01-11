using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Models
{
    public class ProcedureAvvisiAlPubblicoModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public Attributo Attributo { get; set; }

        public string NomeAttributo { get; set; }

        public IEnumerable<DocumentoElenco> Documenti { get; set; }
        public PaginaStatica PaginaStatica { get; set; }

        public string TitoloGriglia { get; set; }

        public string NomeColonnaOggetto { get; set; }
    }
}