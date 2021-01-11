using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Management;
using VALib.Domain.Entities.Membership;

namespace VALib.Web
{
    public class VAWebRequestDocumentoDownloadEvent : WebRequestEvent
    {
        public VAWebRequestDocumentoDownloadEvent(string message, object eventSource, object documentoID, VAWebEventTypeEnum tipo)
            : base(message, eventSource, VAWebEventCodes.DownloadDocumento)
        {
            VARequestInformation = new VAWebRequestInformation();

            if (tipo == VAWebEventTypeEnum.DownloadDocumentoCondivisione)
                VARequestInformation.GuidEntityID = (Guid?)documentoID;
            else
                VARequestInformation.IntEntityID = (int)documentoID;
            
            VARequestInformation.EventTypeID = tipo;

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
    }
}
