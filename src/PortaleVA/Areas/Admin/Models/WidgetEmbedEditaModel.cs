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
    public class WidgetEmbedEditaModel : WidgetBaseEditaModel
    {
        [DisplayName("Nome italiano")]
        [Required(ErrorMessage = "Nome italiano obbligatorio.")]
        public string EditaNome_IT { get; set; }

        [DisplayName("Nome inglese")]
        [Required(ErrorMessage = "Nome inglese obbligatorio.")]
        public string EditaNome_EN { get; set; }

        [DisplayName("Contenuto italiano")]
        [Required(ErrorMessage = "Contenuto italiano obbligatorio.")]
        public string EditaContenuto_IT { get; set; }

        [DisplayName("Contenuto inglese")]
        [Required(ErrorMessage = "Contenuto inglese obbligatorio.")]
        public string EditaContenuto_EN { get; set; }

        [DisplayName("Mostra titolo")]
        [Required(ErrorMessage = "Selezionare se mostratre il titolo o no.")]
        public bool? MostraTitolo { get; set; }

        public IEnumerable<SelectListItem> BooleanSelectList { get; set; }

    }
}