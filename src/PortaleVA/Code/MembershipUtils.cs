using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Repositories.Membership;

namespace VAPortale.Code
{
    public class MembershipUtils
    {
        public static string NomeUtente
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public static bool UtenteAutenticato
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }

        public static Utente RecuperaUtenteCorrente()
        {
            return ((VAPrincipal)HttpContext.Current.User).Utente;
            
        }
    }
}