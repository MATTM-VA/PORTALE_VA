using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class WidgetSezioneEditaModel : WidgetBaseEditaModel
    {
        [DisplayName("Nome italiano")]
        [Required(ErrorMessage = "Nome italiano obbligatorio.")]
        public string EditaNome_IT { get; set; }

        [DisplayName("Nome inglese")]
        [Required(ErrorMessage = "Nome inglese obbligatorio.")]
        public string EditaNome_EN { get; set; }

        [Required(ErrorMessage = "Selezione tipo obbligatorio.")]
        public string SelezioneLinkVoce { get; set; }

        [DisplayName("Link italiano")]
        //[Required(ErrorMessage = "Link obbligatorio.")]
        public string EditaLinkIT { get; set; }

        [DisplayName("Link inglese")]
        //[Required(ErrorMessage = "Link obbligatorio.")]
        public string EditaLinkEN { get; set; }

        [DisplayName("Elenco voci menu")]
        //[Required(ErrorMessage = "Voce menu obbligatoria.")]
        public int? EditaVoceMenuID { get; set; }

        [DisplayName("Voce menu")]
        public IEnumerable<SelectListItem> VociMenuSelectList { get; set; }
        [Required(ErrorMessage = "Icona obbligatoria.")]
        public string EditaIcona { get; set; }

        [DisplayName("Elenco icone disponibili")]
        public List<String> IconeList { get; set; }

    }
}