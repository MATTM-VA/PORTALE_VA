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
    public class CaroselloEditaModel
    {
        public int ID { get; set; }

        public bool EditaPubblicato { get; set; }

        [DisplayName("Tipo contenuto")]
        [Required(ErrorMessage = "Tipo contenuto obbligatorio.")]
        public ContenutoOggettoCaroselloTipo EditaTipoContenutoID { get; set; }

        [DisplayName("Oggetto ID")]
        [Required(ErrorMessage = "Oggetto obbligatorio.")]
        public int? EditaContenutoID { get; set; }

        [DisplayName("Immagine")]
        [Required(ErrorMessage = "Immagine obbligatoria.")]
        public int? EditaImmagineID { get; set; }

        public int? ImmagineMasterID { get; set; }

        public IEnumerable<SelectListItem> ImmaginiSelectList { get; set; }

        [DisplayName("Data")]
        [Required(ErrorMessage = "Data obbligatorio.")]
        public DateTime EditaData { get; set; }

        [DisplayName("Nome italiano")]
        [Required(ErrorMessage = "Nome italiano obbligatorio.")]
        public string EditaNome_IT { get; set; }

        [DisplayName("Nome inglese")]
        [Required(ErrorMessage = "Nome inglese obbligatorio.")]
        public string EditaNome_EN { get; set; }

        [DisplayName("Descrizione italiano")]
        [Required(ErrorMessage = "Descrizione italiano obbligatorio.")]
        public string EditaDescrizione_IT { get; set; }

        [DisplayName("Descrizione inglese")]
        [Required(ErrorMessage = "Descrizione inglese obbligatorio.")]
        public string EditaDescrizione_EN { get; set; }

        [DisplayName("Link progetto cartografico")]
        public string EditaLinkProgettoCartografico { get; set; }

        public OggettoCarosello OggettoCarosello { get; set; }

    }
}