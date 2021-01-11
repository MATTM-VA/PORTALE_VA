using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Management;
using VALib.Domain.Entities.Membership;

namespace VALib.Web
{
    public class VAWebRequestErrorEvent : WebRequestErrorEvent
    {
        public VAWebRequestErrorEvent(string message, object eventSource, Exception exception)
            : base(message, eventSource, VAWebEventCodes.ErroreGenerico, GetDetailCode(exception), exception)
        {
            VARequestInformation = new VAWebRequestInformation();

            VARequestInformation.EventTypeID = VAWebEventTypeEnum.Errore;
 
            if (HttpContext.Current != null)
            {
                VAPrincipal vaPrincipal = HttpContext.Current.User as VAPrincipal;

                if (vaPrincipal != null && vaPrincipal.Utente != null)
                {
                    VARequestInformation.UtenteID = vaPrincipal.Utente.ID;
                    VARequestInformation.NomeUtente = vaPrincipal.Utente.NomeUtente;
                }

                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    VARequestInformation.UrlReferrer = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                }

                VARequestInformation.UserAgent = HttpContext.Current.Request.UserAgent;
            }
        }

        public VAWebRequestInformation VARequestInformation { get; private set; }

        private static int GetDetailCode(Exception ex)
        {
            int code = 0;

            if (ex != null)
            {
                if (ex is HttpRequestValidationException)
                    code = VAWebEventCodes.ErroreAvvertimento;
            }

            return code;
        }
    }
}
