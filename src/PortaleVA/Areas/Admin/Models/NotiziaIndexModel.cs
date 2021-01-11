using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Contenuti;
using System.Web.Mvc;
using System.ComponentModel;

namespace VAPortale.Areas.Admin.Models
{
    public class NotiziaIndexModel : PaginazioneModel
    {
        [DisplayName("Testo")]
        public string Testo { get; set; }

        [DisplayName("Pubblicata")]
        public bool? Pubblicato { get; set; }

        [DisplayName("Categoria")]
        public int? CategoriaNotiziaID { get; set; }

        [DisplayName("Stato")]
        public StatoNotiziaEnum? Stato { get; set; }

        public int DefaultImmagineID { get; set; }

        public IEnumerable<SelectListItem> BooleanSelectList { get; set; }

        public IEnumerable<SelectListItem> CategoriaSelectList { get; set; }

        public IEnumerable<SelectListItem> StatoSelectList { get; set; }

        public List<Notizia> Notizie { get; set; }
    }
}