using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Areas.Admin.Models
{
    public class WidgetNotiziaEditaModel : WidgetBaseEditaModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome obbligatorio.")]
        public string EditaNome { get; set; }

        [DisplayName("Categoria notizie")]
        [Required(ErrorMessage = "Categoria notizie obbligatoria.")]
        public int? EditaCategoriaNotiziaID { get; set; }

        [DisplayName("Numero elementi")]
        [Required(ErrorMessage = "Indicare il numero degli elementi.")]
        [Range(1, 15)]
        public int EditaMax { get; set; }

        public CategoriaNotizia CategoriaNotizia { get; set; }

        public IEnumerable<SelectListItem> CategorieSelectList { get; set; }

    }
}