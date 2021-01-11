using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace VALib.Domain.Entities.Membership
{
    public class VAPrincipal : IPrincipal
    {
        public VAPrincipal(Utente utente)
        {
            if(utente!=null)
            {
                this.Identity = new GenericIdentity(utente.NomeUtente);
                this.Utente = utente;
            }
            else
            {
                this.Identity = new GenericIdentity("");
            }
        }

        public IIdentity Identity
        {
            get ;//{ throw new NotImplementedException(); }
            private set;
        }

        public bool IsInRole(string role)
        {
            return Utente != null ? Utente.RuoliUtente.Any(x => x.Codice == role) : false;
        }
        
        public Utente Utente 
        { 
            get; 
            private set; 
        }
    }
}