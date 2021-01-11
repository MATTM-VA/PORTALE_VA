using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Models
{
    public class ProcedureElencoModel : PaginazioneModel
    {
        public string Testo { get; set; }

        public int Parametro { get; set; }

        public int Id { get; set; }

        public IEnumerable<OggettoElencoProcedura> Oggetti { get; set; }

        public Procedura Procedura { get; set; }

    }
}