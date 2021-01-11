using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureAttributiModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public Attributo Attributo { get; set; }

        public string NomeAttributo { get; set; }

        public List<OggettoElenco> Oggetti { get; set; }

        public string TitoloGriglia { get; set; }

        public string NomeColonnaOggetto { get; set; }
    }
}