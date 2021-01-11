using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Membership;

namespace VAPortale.Areas.Admin.Models
{
    public class UtenteIndexModel
    {
        public UtenteIndexModel()
        {
            Utenti = new List<Utente>();
        }

        public List<Utente> Utenti { get; set; }
    }
}