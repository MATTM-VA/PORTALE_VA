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
    public class DocumentoPortaleEditaModel
    {
        public int ID { get; set; }

        [DisplayName("Nome italiano")]
        [Required(ErrorMessage = "Nome italiano obbligatorio.")]
        public string EditaNome_IT { get; set; }

        [DisplayName("Nome inglese")]
        [Required(ErrorMessage = "Nome inglese obbligatorio.")]
        public string EditaNome_EN { get; set; }

        [DisplayName("Nome file")]
        [Required(ErrorMessage = "File obbligatorio.")]
        public string EditaNomeFileOriginale { get; set; }

        public int EditaDimensione { get; set; }

        public bool EditaNuovoFile { get; set; }

        public DocumentoPortale Documento { get; set; }

    }
}