using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VAPortale.Models.Common;
using System.Web.Mvc;
using VALib.Domain.Entities.DatiAmbientali;

namespace VAPortale.Models
{
    public class DatiEStrumentiRicercaCatalogoModel : PaginazioneModel
    {
        public IEnumerable<SelectListItem> TemiSelectList { get; set; }

        public int? TemaID { get; set; }

        public string Testo { get; set; }

        public List<RisorsaElenco> Risorse { get; set; }

        public List<Elenco> Elenchi { get; set; }
    }
}