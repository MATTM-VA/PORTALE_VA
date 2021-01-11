using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using System.ComponentModel.DataAnnotations;

namespace VAPortale.Models
{
    public class RicercaTerritorioModel : PaginazioneModel
    {
        public IEnumerable<SelectListItem> TipologieTerritorioSelectList { get; set; }

        public int MacroTipoOggettoID { get; set; }

        public int? TipologiaTerritorioID { get; set; }

        [Required(ErrorMessage = "Inserire un testo da ricercare.")]
        public string Testo { get; set; }

        public List<OggettoElenco> Oggetti { get; set; }
    }
}