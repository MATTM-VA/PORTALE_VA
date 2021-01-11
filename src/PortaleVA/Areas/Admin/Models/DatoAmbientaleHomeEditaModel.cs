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
    public class DatoAmbientaleHomeEditaModel
    {
        public int ID { get; set; }

        public bool EditaPubblicato { get; set; }

        [DisplayName("Immagine")]
        [Required(ErrorMessage = "Immagine obbligatoria.")]
        public int? EditaImmagineID { get; set; }

        public IEnumerable<SelectListItem> ImmaginiSelectList { get; set; }

        [DisplayName("Titolo italiano")]
        [Required(ErrorMessage = "Titolo italiano obbligatorio.")]
        public string EditaTitolo_IT { get; set; }

        [DisplayName("Titolo inglese")]
        [Required(ErrorMessage = "Titolo inglese obbligatorio.")]
        public string EditaTitolo_EN { get; set; }

        [DisplayName("Link")]
        public string EditaLink { get; set; }

        public DatoAmbientaleHome DatoAmbientaleHome { get; set; }

    }
}