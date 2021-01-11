using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VAPortale.Areas.Admin.Models
{
    public class HomeLoginModel
    {
        [Required(ErrorMessage = "Inserire il nome utente")]
        public string NomeUtente { get; set; }

        [Required(ErrorMessage = "Inserire la password")]
        public string Pswd { get; set; }
    }
}