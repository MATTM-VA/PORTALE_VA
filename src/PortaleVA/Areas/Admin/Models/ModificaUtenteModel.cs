using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VAPortale.Areas.Admin.Models
{
    public class ModificaUtenteModel
    {
        public int Id { get; set; }

        //[DisplayName("Nominativo")]
        //[Required(ErrorMessage="Campo obbligatorio")]
        //public string Nominativo { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Nome { get; set; }

        [DisplayName("Cognome")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Cognome { get; set; }

        //[DisplayName("Nome Utente")]
        //[Required(ErrorMessage = "Campo obbligatorio")]
        //public string NomeUtente { get; set; }

        [System.Web.Mvc.Remote("ValidaEmail", "Utente", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Email già esistente")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email non valida")]
        [Required(ErrorMessage = "Campo obbligatorio")]
        public string Email { get; set; }

        public bool Abilitato { get; set; }

        public bool UtenteCorrente { get; set; }

        [DisplayName("Nome utente")]
        public string NomeUtente { get; set; }

        [DisplayName("Data ultimo Login")]
        public DateTime? DataUltimoLogin { get; set; }

        [DisplayName("Data ultimo cambio password")]
        public DateTime? DataUltimoCambioPassword { get; set; }
    }
}