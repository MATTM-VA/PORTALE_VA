using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Membership;

namespace VAPortale.Areas.Admin.Models
{
    public class GestioneRuoliModel
    {
        public GestioneRuoliModel()
        {
            RuoliUtente = new List<RuoloUtente>();
        }

        public int UtenteID { get; set; }

        public Utente Utente { get; set; }

        public List<RuoloUtente> RuoliUtente { get; private set; }

        //public List<bool> HasRole { get; set; }

        public bool UtenteCorrente { get; set; }
    }
}