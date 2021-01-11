using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.Contenuti;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Services;
using VALib.Helpers;
using VALib.Web;
using VAPortale.Filters;
using VAPortale.Models;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class FeedController : Controller
    {
        //
        // GET: /Feed/

        
        public ActionResult Notizie(string categoria)
        {
            ActionResult result = HttpNotFound();

            CategoriaNotiziaEnum c = CategoriaNotiziaEnum.Nessuna;

            if (Enum.TryParse(categoria, out c) && c != CategoriaNotiziaEnum.Nessuna)
            {
                List<Notizia> notizie = null;
                int numeroTotale = 0;

                notizie = NotiziaRepository.Instance.RecuperaNotizie("", (int)c, true, StatoNotiziaEnum.Pubblicabile, 0, 20, out numeroTotale);

                result = Content(CreaTipologiaNotiziaRss(notizie, c), "text/xml", Encoding.UTF8);

                return result;
            }
            else {
                return View("NotFound");
            }

           
        }

        private string CreaTipologiaNotiziaRss(List<Notizia> notizie, CategoriaNotiziaEnum categoria)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            CultureInfo ci = CultureHelper.GetCurrentCultureInfo();

            // rss
            XmlElement root = doc.CreateElement("rss");
            root.Attributes.Append(doc.CreateAttribute("version"));
            root.Attributes["version"].Value = "2.0";

            // channel
            XmlElement channel = doc.CreateElement("channel");
            channel.AppendChild(doc.CreateElement("title"));
            channel.GetElementsByTagName("title")[0].InnerText = "www.va.minambiente.it - " + EnumUtility.CategoriaNotiziaToString(categoria);
            channel.AppendChild(doc.CreateElement("description"));
            channel.GetElementsByTagName("description")[0].InnerText = EnumUtility.CategoriaNotiziaToString(categoria) + " - www.va.minambiente.it " + DizionarioService.SITO_TitoloParte1 + " " + DizionarioService.SITO_TitoloParte2;
            channel.AppendChild(doc.CreateElement("link"));
            channel.GetElementsByTagName("link")[0].InnerText = UrlUtility.VASite(ci.Name + "/Comunicazione/ElencoNotizieFeed/" + categoria.ToString());
            channel.AppendChild(doc.CreateElement("copyright"));
            channel.GetElementsByTagName("copyright")[0].InnerText = string.Format("Copyright {0:yyyy}, " + DizionarioService.SITO_TitoloParte1, DateTime.Today);
            channel.AppendChild(doc.CreateElement("language"));
            channel.GetElementsByTagName("language")[0].InnerText = CultureHelper.GetCurrentCultureInfo().ToString();
            channel.AppendChild(doc.CreateElement("managingEditor"));
            channel.GetElementsByTagName("managingEditor")[0].InnerText = "webmaster.dva@minambiente.it";
            channel.AppendChild(doc.CreateElement("webMaster"));
            channel.GetElementsByTagName("webMaster")[0].InnerText = "webmaster.dva@minambiente.it";
            channel.AppendChild(doc.CreateElement("category"));
            channel.GetElementsByTagName("category")[0].InnerText = EnumUtility.CategoriaNotiziaToString(categoria);

            if (notizie.Count() > 0)
            {
                channel.AppendChild(doc.CreateElement("pubDate"));
                channel.GetElementsByTagName("pubDate")[0].InnerText = notizie[0].DataUltimaModifica.ToString(@"ddd, d MMM yyyy HH\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture) + " GMT";

                channel.AppendChild(doc.CreateElement("lastBuildDate"));
                channel.GetElementsByTagName("lastBuildDate")[0].InnerText = notizie[0].DataUltimaModifica.ToString(@"ddd, d MMM yyyy HH\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture) + " GMT";
            }

            foreach (Notizia contenuto in notizie.OrderByDescending(x => x.Data))
            {
                XmlElement item = doc.CreateElement("item");

                item.AppendChild(doc.CreateElement("guid"));
                item.GetElementsByTagName("guid")[0].InnerText = UrlUtility.VASite(ci.Name + "/Comunicazione/NotiziaFeed/" + categoria.ToString() + "/" + contenuto.ID);

                item.AppendChild(doc.CreateElement("title"));
                item.GetElementsByTagName("title")[0].InnerText = contenuto.GetTitolo();

                item.AppendChild(doc.CreateElement("link"));
                item.GetElementsByTagName("link")[0].InnerText = UrlUtility.VASite(ci.Name + "/Comunicazione/NotiziaFeed/" + categoria.ToString() + "/" + contenuto.ID);

                item.AppendChild(doc.CreateElement("description"));
                item.GetElementsByTagName("description")[0].InnerText = contenuto.GetAbstract();

                item.AppendChild(doc.CreateElement("pubDate"));
                item.GetElementsByTagName("pubDate")[0].InnerText = contenuto.DataUltimaModifica.ToString(@"ddd, d MMM yyyy HH\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture) + " GMT";
                
                item.AppendChild(doc.CreateElement("category"));
                item.GetElementsByTagName("category")[0].InnerText = EnumUtility.CategoriaNotiziaToString(categoria);

                channel.AppendChild(item);
            }

            root.AppendChild(channel);
            doc.AppendChild(root);

            return doc.OuterXml;
        }


    }
}
