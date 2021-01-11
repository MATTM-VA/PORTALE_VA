using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Models
{
    public class PaginaEditaModel
    {
        public int ID { get; set; }

        [DisplayName("Testo italiano")]
        [Required(ErrorMessage = "Testo italiano obbligatorio.")]
        public string EditaTesto_IT { get; set; }

        [DisplayName("Testo inglese")]
        [Required(ErrorMessage = "Testo inglese obbligatorio.")]
        public string EditaTesto_EN { get; set; }

        public VoceMenu VoceMenu { get; set; }

        public PaginaStatica Pagina { get; set; }

    }
}