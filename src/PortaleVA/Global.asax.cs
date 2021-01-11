/*
    *************************************************************************************
    ************* PortaleVA - Portale delle valutazioni ambientali VAS-VIA**************
    *************************************************************************************

    Copyright MATTM Ministero dell'Ambiente e della Tutela del Territorio e del Mare
    Concesso in licenza a norma dell'EUPL(la "Licenza"), versione 1.2;
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Repositories.Membership;
using VALib.Domain.Services;
using VALib.Web;
using VAPortale.Controllers;
using VAPortale.Code;
using System.Web.Configuration;

namespace VAPortale
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // Remove ASP.Net MVC Default HTTP Headers
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Remove("Server");
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            String[] allowedIPs = ConfigurationManager.AppSettings["MaintenanceAllowedIPs"].Split(',');
            String[] allowedExtensions = new String[] { ".css", ".ico", ".png", ".jpg", ".woff", ".woff2" };

            if (ConfigurationManager.AppSettings["MaintenanceMode"] == "true" && Request.RawUrl.IndexOf("maintenance.aspx") == -1)
            {
                if (!Request.IsLocal &&
                    !allowedIPs.Contains(Request.UserHostAddress) &&
                    !allowedExtensions.Contains(HttpContext.Current.Request.CurrentExecutionFilePathExtension))
                {
                    HttpContext.Current.RewritePath("~/maintenance.aspx");
                   
                }
            }
        }

        protected void Application_PostAuthenticateRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                Utente utente = UtenteRepository.Instance.RecuperaUtente(User.Identity.Name);
                if (utente != null && utente.Abilitato)
                {
                    HttpContext.Current.User = new VAPrincipal(utente);
                }
                else
                {
                    HttpContext.Current.User = new VAPrincipal(null);
                }
            }
            else
            {
                HttpContext.Current.User = new VAPrincipal(null);
            }
        }


        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();

            if (ex != null)
            {
                //Server.ClearError();

                string detailError = GetError.getHtmlError(HttpContext.Current.Request, HttpContext.Current.Session, ex);

                if (Convert.ToBoolean(WebConfigurationManager.AppSettings["SendErrorMail"]))
                {
                    // INVIO EMAIL DI ERRORE
                    Mail.Send(Mail.getMailMessage(new String[] { ConfigurationManager.AppSettings["MailTo"], ConfigurationManager.AppSettings["MailToCC"] },
                          "Avviso da Portale VA", detailError));
                }
                

                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var redirectUrl = url.RouteUrl("Error", new { Controller = "Error", Action = "NotFound" });
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

    }
}