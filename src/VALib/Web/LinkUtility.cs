using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web;
using System.IO;
using VALib.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace VALib.Web
{
    public static class LinkUtility
    {
        private static bool IndirizzoWebValido(string indirizzo)
        {
            bool result = false;

            if (indirizzo.StartsWith("http://", true, CultureInfo.InvariantCulture) ||
                indirizzo.StartsWith("https://", true, CultureInfo.InvariantCulture) ||
                indirizzo.StartsWith("ftp://", true, CultureInfo.InvariantCulture))
                result = true;

            return result;
        }

        public static string LinkLocalizzazione(string immagineLocalizzazione, int oggettoID)
        {
            string result = "";

            if (!string.IsNullOrWhiteSpace(immagineLocalizzazione))
            {
                if (IndirizzoWebValido(immagineLocalizzazione))
                {
                    result = immagineLocalizzazione;
                }
                else if (immagineLocalizzazione.EndsWith(".wmc", false, CultureInfo.InvariantCulture))
                {
                    result = "http://cart.ancitel.it/index.html?context=" + HttpUtility.UrlEncode(UrlUtility.VALocalizzazioneWmc(oggettoID));
                }
                else if (immagineLocalizzazione.EndsWith(".pdf", false, CultureInfo.InvariantCulture))
                {
                    result = UrlUtility.VALocalizzazionePdf(oggettoID);
                }
                else
                {
                    result = UrlUtility.VAImmagineLocalizzazione(oggettoID);
                }
            }

            return result;
        }
    }

    public static class HtmlExtensions
    {
        public static MvcHtmlString MyActionLink(
            this HtmlHelper html,
            string linkText,
            string action,
            string controller,
            object routeValues,
            object htmlAttributes
        )
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var url = urlHelper.Action(action, controller, routeValues);
            var anchor = new TagBuilder("a");
            anchor.InnerHtml = linkText;
            anchor.Attributes["href"] = url;
            anchor.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(anchor.ToString());
        }
    }
}
