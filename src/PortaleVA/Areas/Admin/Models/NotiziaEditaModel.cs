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
    public class NotiziaEditaModel
    {
        public int ID { get; set; }

        public bool Pubblicata { get; set; }
        
        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Categoria obbligatoria.")]
        public int? CategoriaNotiziaID { get; set; }

        public IEnumerable<SelectListItem> CategorieSelectList { get; set; }

        [DisplayName("Immagine")]
        [Required(ErrorMessage = "Immagine obbligatoria.")]
        public int? ImmagineID { get; set; }

        public IEnumerable<SelectListItem> ImmaginiSelectList { get; set; }

        [DisplayName("Stato")]
        //[Required(ErrorMessage = "Immagine obbligatoria.")]
        public StatoNotiziaEnum StatoNotizia { get; set; }

        public IEnumerable<SelectListItem> StatiSelectList { get; set; }

        [DisplayName("Data")]
        [Required(ErrorMessage = "Data obbligatorio.")]
        public DateTime Data { get; set; }

        [DisplayName("Titolo italiano")]
        [Required(ErrorMessage = "Titolo italiano obbligatorio.")]
        public string Titolo_IT { get; set; }

        [DisplayName("Titolo inglese")]
        //[Required(ErrorMessage = "Titolo inglese obbligatorio.")]
        public string Titolo_EN { get; set; }

        [DisplayName("Titolo breve italiano")]
        //[Required(ErrorMessage = "Titolo breve italiano obbligatorio.")]
        public string TitoloBreve_IT { get; set; }

        [DisplayName("Titolo breve inglese")]
        //[Required(ErrorMessage = "Titolo breve inglese obbligatorio.")]
        public string TitoloBreve_EN { get; set; }

        [DisplayName("Abstract italiano")]
        //[Required(ErrorMessage = "Abstract italiano obbligatorio.")]
        public string Abstract_IT { get; set; }

        [DisplayName("Abstract inglese")]
        //[Required(ErrorMessage = "Abstract inglese obbligatorio.")]
        public string Abstract_EN { get; set; }

        [DisplayName("Testo italiano")]
        //[Required(ErrorMessage = "Testo italiano obbligatorio.")]
        public string Testo_IT { get; set; }

        [DisplayName("Testo inglese")]
        //[Required(ErrorMessage = "Testo inglese obbligatorio.")]
        public string Testo_EN { get; set; }

        public Notizia Notizia { get; set; }

    }
}