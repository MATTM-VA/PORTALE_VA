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
    public class WidgetInEvidenzaEditaModel : WidgetBaseEditaModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome obbligatorio.")]
        public string EditaNome { get; set; }

        [DisplayName("Categoria notizie")]
        [Required(ErrorMessage = "Categoria notizie obbligatoria.")]
        public int? EditaCategoriaNotiziaID { get; set; }

        //[DisplayName("Numero elementi")]
        //[Required(ErrorMessage = "Indicare il numero degli elementi.")]
        //[Range(1, 3)]
        //public int EditaMax { get; set; }

        public CategoriaNotizia CategoriaNotizia { get; set; }

        public IEnumerable<SelectListItem> CategorieSelectList { get; set; }

        public IEnumerable<SelectListItem> NotizieSelectList { get; set; }


        [DisplayName("Selezionare la notizia")]
        [Required(ErrorMessage = "Notizie obbligatoria.")]
        public int? EditaNotiziaInEvidenza { get; set; }

        public int? ImmagineID { get; set; }

        //public int ordineInEvidenza { get; set; }

        //public IEnumerable<ListaInEvidenza> InEvidenzaList { get; set; }
    }
}