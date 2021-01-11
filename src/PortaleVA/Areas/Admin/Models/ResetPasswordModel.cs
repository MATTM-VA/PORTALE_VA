using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VAPortale.Areas.Admin.Models
{
    public class ResetPasswordModel
    {
        public bool TokenValido { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,/%&$!?§=°ç*#@^+]).{6,20}$", ErrorMessage = "La password  deve contenere un minimo di 6 e un massimo di 20 caratteri, almeno un carattere speciale, almeno un numero e almeno una lettera maiuscola. ")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Conferma Password")]
        [Compare("Password", ErrorMessage = "Le due password non coincidono")]
        public string ConfermaPassword { get; set; }

        public int UtenteId { get; set; }
    }
}