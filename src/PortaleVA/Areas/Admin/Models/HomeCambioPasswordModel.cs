using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VAPortale.Areas.Admin.Models
{
    public class HomeCambioPasswordModel
    {
        [Required]
        [DisplayName("Vecchia Password")]
        public string VecchiaPassword { get; set; }

        [Required]
        [DisplayName("Nuova Password")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "La password deve contenere un minimo di 6 e un massimo di 20 caratteri  ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[.,/%&$!?§=°ç*#@^+]).{6,20}$", ErrorMessage = "La password  deve contenere un minimo di 6 e un massimo di 20 caratteri, almeno un carattere speciale, almeno un numero e almeno una lettera maiuscola. ")]
        public string NuovaPassword { get; set; }

        //[StringLength(20, MinimumLength = 4, ErrorMessage = "La password deve contenere un minimo di 4 e un massimo di 20 caratteri ")]
        //[RegularExpression(@"\S*\d\S*", ErrorMessage = "La password deve contenere almeno un carattere speciale e almeno un numero")]
        [Required]   
        [Compare("NuovaPassword", ErrorMessage="Le due password non coincidono")]
        [DisplayName("Conferma Nuova Password")]
        public string ConfermaNuovaPassword { get; set; }
    }
}