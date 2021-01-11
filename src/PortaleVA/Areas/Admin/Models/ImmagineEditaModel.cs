using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VALib.Domain.Entities.Contenuti;

namespace VAPortale.Areas.Admin.Models
{
    public class ImmagineEditaModel
    {
        public int ID { get; set; }

        [DisplayName("Nome IT")]
        [Required(ErrorMessage = "Nome IT obbligatorio.")]
        [StringLength(128, ErrorMessage = "Nome IT max 128 caratteri.")]
        public string EditaNome_IT { get; set; }

        [DisplayName("Nome EN")]
        [Required(ErrorMessage = "Nome EN obbligatorio.")]
        [StringLength(128, ErrorMessage = "Nome EN max 128 caratteri.")]
        public string EditaNome_EN { get; set; }

        [DisplayName("Immagine")]
        public HttpPostedFileBase EditaFileImmagine { get; set; }

        public bool MostraLinkInserimentoNotizia { get; set; }

        public Immagine Immagine { get; set; }
    }
}