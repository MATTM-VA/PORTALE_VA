using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VAPortale.Models
{
    public class ComunicazioneFormContattoModel
    {
        [Required(ErrorMessage = "Scrivere un testo per l'email.")]
        public string Testo { get; set; }

        [Email]
        [Required(ErrorMessage = "Indicare l'indirizzo email.")]
        [StringLength(128)]
        public string IndirizzoMail { get; set; }

        public string Tipo { get; set; }

        public bool EmailInviata { get; set; }
    }
}