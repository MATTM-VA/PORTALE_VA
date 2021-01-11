using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Repositories.Membership;

namespace VaPortale.Areas.Admin.Filters
{
    public class UtenteBaseAutorizeAttribute : AuthorizeAttribute
    {
        public UtenteBaseAutorizeAttribute()
            : base()
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorized = false;

            if (base.AuthorizeCore(httpContext))
            {
                string nomeUtente = httpContext.User.Identity.Name;
                Utente utente = UtenteRepository.Instance.RecuperaUtente(nomeUtente);

                if (utente != null)
                {
                    //if (utente.Ruolo == Ruolo.Amministratore || utente.Ruolo == Ruolo.Area || utente.Ruolo == Ruolo.Rep || utente.Ruolo == Ruolo.Sede || utente.Ruolo == Ruolo.CallCenter)
                    authorized = true;
                }
            }

            return authorized;
        }
    }
}