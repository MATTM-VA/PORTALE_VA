using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Repositories.Contenuti;
using System.Text;
using VALib.Web;
using VALib.Domain.Services;
using VAPortale.Code;
using VALib.Domain.Entities.Membership;
using VAPortale.Models.Support;
using VALib.Domain.Entities.UI;

namespace VAPortale.Areas.Admin.Controllers
{
    [Authorize]
    public class SharedController : VAController
    {
        //
        // GET: /Admin/Shared/
        [ChildActionOnly]
        public ActionResult MenuAdmin()
        {
            return null;
        }

        [ChildActionOnly]
        public PartialViewResult EditaTestoEditorNotiziaAvanzato(string selectorClass, string lang)
        {
            SharedEditorTestoNotiziaAvanzatoModel model = new SharedEditorTestoNotiziaAvanzatoModel();
            model.SelectorClass = selectorClass;
            model.ExternalLinkList = ElencoDocumentiPortale(lang.Substring(0, 2));
            model.ExternalImageList = ElencoImmagini();
            //model.ContentCss = Url.RSContent("agenzia-redattoreSchedaTesto.css");
            //model.ExternalLinkListUrl = "";
            model.Pulgins = "paste";

            return PartialView(model);
        }

        private string ElencoDocumentiPortale(string lang)
        {
            int c = 0;
            IEnumerable<DocumentoPortale> documenti = DocumentoPortaleRepository.Instance.RecuperaDocumentiPortale("", 0, 666, out c);
            StringBuilder sb = new StringBuilder("[");

            foreach (DocumentoPortale d in documenti)
            {
                string url = UrlUtility.VADocumentoPortale(d.ID);
                string titolo = "Documento portale - " +
                                d.GetNome(lang).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("'", "''");

                if (titolo.Length > 120)
                    titolo = titolo.Substring(0, 117) + "...";
                string item = string.Format("title: '{0}', value: '{1}'", titolo, url);
                item = "{" + item + "},";

                //sb.AppendFormat("{" + "title: \"{0}\",value: \"{1}\"" + "}" +",", titolo, url);
                sb.Append(item);
            }

            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");


            return sb.ToString();
        }

        private string ElencoImmagini()
        {
            List<Immagine> immagini = ImmagineRepository.Instance.RecuperaImmaginiLibere();
            StringBuilder sb = new StringBuilder("[");

            foreach (Immagine i in immagini)
            {
                string url = UrlUtility.VAImmagine(i.ID);
                string titolo = i.Nome_IT.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("'", "''");

                if (titolo.Length > 120)
                    titolo = titolo.Substring(0, 117) + "...";
                string item = string.Format("title: '{0}', value: '{1}'", titolo, url);
                item = "{" + item + "},";

                //sb.AppendFormat("{" + "title: \"{0}\",value: \"{1}\"" + "}" +",", titolo, url);
                sb.Append(item);
            }

            if (sb[sb.Length - 1] == ',')
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");


            return sb.ToString();
        }

        [ChildActionOnly]
        public PartialViewResult BarraUtente()
        {
            BarraUtenteModel model = new BarraUtenteModel();
            Utente utente = User.Utente;
            model.NomeUtente = string.Format("{0} {1}", utente.Nome, utente.Cognome);
            return PartialView(model);
        }
        
        [ChildActionOnly]
        public PartialViewResult MenuPrincipale()
        {
            MenuPrincipaleModel model = new MenuPrincipaleModel();

            // l'utente deve aver resettato la password per poter vedere le voci menu
            if (User.Utente.DataUltimoCambioPassword != null)
            {
                if (User.IsInRole(RuoloUtenteCodici.GestoreNotizie))
                {
                    LinkItem item1 = new LinkItem() { Url = "Notizia", Title = "Notizie", Text = "Notizie" };
                    model.ListaLink.Add(item1);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreImmagine))
                {
                    LinkItem item2 = new LinkItem() { Url = "Immagine", Title = "Immagini", Text = "Immagini" };
                    model.ListaLink.Add(item2);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreDocumentoPortale))
                {
                    LinkItem item3 = new LinkItem() { Url = "DocumentoPortale", Title = "Documenti portale", Text = "Documenti portale" };
                    model.ListaLink.Add(item3);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreCaroselloHome))
                {
                    LinkItem item4 = new LinkItem() { Url = "Carosello", Title = "Carosello home", Text = "Carosello home" };
                    model.ListaLink.Add(item4);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreDatiAmbientaliHome))
                {
                    LinkItem item5 = new LinkItem() { Url = "DatoAmbientaleHome", Title = "Dati Ambientale Home", Text = "Dati Ambientale Home" };
                    model.ListaLink.Add(item5);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreWidget))
                {
                    LinkItem item6 = new LinkItem() { Url = "Widget", Title = "Widget", Text = "Widget" };
                    model.ListaLink.Add(item6);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestorePagine))
                {
                    LinkItem item7 = new LinkItem() { Url = "Pagina", Title = "Pagine", Text = "Pagine" };
                    model.ListaLink.Add(item7);
                }
                if (User.IsInRole(RuoloUtenteCodici.GestoreVariabili))
                {
                    LinkItem item8 = new LinkItem() { Url = "Variabile", Title = "Variabili", Text = "Variabili" };
                    model.ListaLink.Add(item8);
                }

                if (User.IsInRole(RuoloUtenteCodici.GestoreUtenti))
                {
                    LinkItem item9 = new LinkItem() { Url = "Utente", Title = "Gestione Utenti", Text = "Gestione Utenti" };
                    model.ListaLink.Add(item9);
                }

                LinkItem item10 = new LinkItem() { Url = "Widget", Title = "Reset Cache", Text = "ResetCache" };
                model.ListaLink.Add(item10);
            }

            return PartialView(model);
        }

    }
}
