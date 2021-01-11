using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Models;
using System.Globalization;
using VAPortale.Code;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Helpers;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using System.Web.Routing;
using System.Collections.Specialized;

namespace VAPortale.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        [ChildActionOnly]
        public ActionResult MenuServizio(SharedMenuNavigazioneModel model)
        {
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd().Where(x => x.TipoMenu == 0).ToList();

            RouteData.Values["nomeAttributo"] = "";

            string nomeSezione = "";
            string nomeVoce = "";

            nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
            nomeVoce = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();

            if (nomeSezione.Equals("Pagine"))
            {
                nomeSezione = Request.RequestContext.RouteData.Values["nomeSezione"].ToString();
                nomeVoce = Request.RequestContext.RouteData.Values["nomeVoce"].ToString();
            }

            model.NomeSezione = nomeSezione;
            model.NomeVoce = nomeVoce;
            model.VociMenu = vociMenu;

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MenuNavigazione(SharedMenuNavigazioneModel model)
        {
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd().Where(x => x.TipoMenu == 1).ToList();

            RouteData.Values["nomeAttributo"] = "";

            string nomeSezione = ""; 
            nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
            if (nomeSezione.Equals("Pagine"))
                nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["NomeSezione"].ToString();

            model.NomeSezione = nomeSezione;
            model.VociMenu = vociMenu; 

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MenuNavigazioneMobile(SharedMenuNavigazioneMobileModel model)
        {
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd().Where(x => x.TipoMenu == 1).ToList();
            List<VoceMenu> vociMenuServizio = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd().Where(x => x.TipoMenu == 0).ToList();

            RouteData.Values["nomeAttributo"] = "";

            string nomeSezione = "";
            nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
            if (nomeSezione.Equals("Pagine"))
                nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["NomeSezione"].ToString();

            model.NomeSezione = nomeSezione;
            model.VociMenu = vociMenu;
            model.VociMenuServizio = vociMenuServizio;

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MenuNavigazioneFooter(SharedMenuNavigazioneModel model)
        {
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd().Where(x => x.TipoMenu == 1).ToList();

            RouteData.Values["nomeAttributo"] = "";

            string nomeSezione = "";
            nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
            if (nomeSezione.Equals("Pagine"))
                nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["NomeSezione"].ToString();

            model.NomeSezione = nomeSezione;
            model.VociMenu = vociMenu;

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult MenuSezione(SharedMenuSezioneModel model)
        {
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenuFrontEnd();

            RouteData.Values["nomeAttributo"] = "";

            string nomeSezione = nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString(); 
            string nomeVoce = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();

            if (nomeSezione.Equals("Pagine"))
            {
                nomeSezione = Request.RequestContext.RouteData.Values["nomeSezione"].ToString();
                nomeVoce = Request.RequestContext.RouteData.Values["nomeVoce"].ToString(); 
            }

            model.NomeSezione = nomeSezione;
            model.NomeVoce = nomeVoce;
            model.VociMenu = vociMenu.Where(x => x.Sezione.Equals(nomeSezione, StringComparison.OrdinalIgnoreCase) && x.GenitoreID > 0);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult SelezioneLingua()
        {
            SharedSelezioneLinguaModel model = new SharedSelezioneLinguaModel();
            RouteValueDictionary rvd = new RouteValueDictionary(Request.RequestContext.RouteData.Values);
            NameValueCollection qnv = Request.RequestContext.HttpContext.Request.QueryString;

            foreach (string key in qnv)
            {
                if (!rvd.ContainsKey(key))
                    rvd.Add(key, qnv[key] ?? "5");
            }

            model.LinkLinguaInglese = "#";
            model.LinkLinguaItaliana = "#";
            bool useRouteUrl = false;

            string currentLangRoute = rvd["lang"].ToString();
            string currentController = "";
            string currentAction = "";
            string currentId = "";

            currentController = rvd["controller"].ToString();
            currentAction = rvd["action"].ToString();

            if (currentController.Equals("Pagine"))
            {
                useRouteUrl = true;
                currentController = rvd["nomeSezione"].ToString();
                currentAction = rvd["nomeVoce"].ToString();
            }


            if (!string.IsNullOrWhiteSpace(currentLangRoute) && CultureHelper.IsValidCulture(currentLangRoute))
            {
                if (currentLangRoute.Equals(CultureHelper._it, StringComparison.OrdinalIgnoreCase))
                {
                    if (useRouteUrl)
                    {
                        model.LinkLinguaInglese = Url.RouteUrl("PaginaStatica", new { lang = CultureHelper._en, nomeSezione = currentController, nomeVoce = currentAction });
                    }
                    else
                    {
                        rvd.Remove("lang");
                        rvd.Add("lang", CultureHelper._en);
                        model.LinkLinguaInglese = Url.Action(currentAction, currentController, rvd);
                    }
                }
                else if (currentLangRoute.Equals(CultureHelper._en, StringComparison.OrdinalIgnoreCase))
                {
                    if (useRouteUrl)
                    {
                        model.LinkLinguaItaliana = Url.RouteUrl("PaginaStatica", new { lang = CultureHelper._it, nomeSezione = currentController, nomeVoce = currentAction });
                    }
                    else
                    {
                        rvd.Remove("lang");
                        rvd.Add("lang", CultureHelper._it);
                        model.LinkLinguaItaliana = Url.Action(currentAction, currentController, rvd);
                    }
                }
            }
            else
            {
                if (useRouteUrl)
                {
                    model.LinkLinguaInglese = Url.RouteUrl("PaginaStatica", new { lang = CultureHelper._en, nomeSezione = currentController, nomeVoce = currentAction });
                    model.LinkLinguaItaliana = Url.RouteUrl("PaginaStatica", new { lang = CultureHelper._it, nomeSezione = currentController, nomeVoce = currentAction });
                }
                else
                {
                    model.LinkLinguaInglese = Url.Action(currentAction, currentController, new { lang = CultureHelper._en, id = currentId });
                    model.LinkLinguaItaliana = Url.Action(currentAction, currentController, new { lang = CultureHelper._it, id = currentId });
                }
            }

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Breadcrumbs()
        {
            SharedBreadcrumbsModel model = new SharedBreadcrumbsModel();

            VoceMenu voce = null;
            List<VoceMenu> vociMenu = new List<VoceMenu>();

            string nomeSezione = nomeSezione = HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString();
            string nomeVoce = HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString();

            if (nomeSezione.Equals("Pagine"))
            {
                nomeSezione = Request.RequestContext.RouteData.Values["nomeSezione"].ToString();
                nomeVoce = Request.RequestContext.RouteData.Values["nomeVoce"].ToString();
            }

            voce = VoceMenuRepository.Instance.RecuperaVoceMenu(nomeVoce);

            if (voce != null)
                vociMenu = VoceMenuRepository.Instance.RecuperaGenitori(voce);

            model.Genitori = vociMenu.Distinct();
            model.Voce = voce;

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Widget(Widget widget)
        {
            ActionResult result = null;

            switch (widget.Tipo)
            {
                case TipoWidget.NonDefinito:
                    break;
                case TipoWidget.Notizie:
                    result = WidgetNotizia(widget);
                    break;
                case TipoWidget.DatiAmbientali:
                    result = WidgetDatoAmbientaleHome(widget);
                    break;
                case TipoWidget.Transfrontaliero:
                    result = WidgetAttributi();
                    break;
                case TipoWidget.Embed:
                    result = WidgetEmbed(widget);
                    break;
                case TipoWidget.InEvidenza:
                    result = WidgetInEvidenza(widget);
                    break;
                case TipoWidget.Sezione:
                    result = WidgetSezione(widget);
                    break;

                default:
                    break;
            }

            return result;
        }
        
        [ChildActionOnly]
        public ActionResult WidgetAttributi()
        {
            SharedWidgetAttributiModel model = new SharedWidgetAttributiModel();

            RouteData.Values["nomeAttributo"] = "";
            
            List<OggettoWidgetAttributo> oggetti = OggettoWidgetAttributoRepository.Instance.RecuperaOggettiWidgetAttributi();
            List<TipoAttributo> tipiAttributi = oggetti.Select(x => x.TipoAttributo).Distinct().ToList();
            List<Attributo> attributi = oggetti.Select(x => x.Attributo).Distinct().ToList();

            model.TipiAttributi = tipiAttributi;
            model.Attributi = attributi;
            model.Oggetti = oggetti;

            return PartialView("WidgetAttributi", model);
        }

        [ChildActionOnly]
        public ActionResult WidgetNotizia(Widget widget)
        {
            SharedWidgetNotiziaModel model = new SharedWidgetNotiziaModel();
            int c = 0;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie("", widget.Categoria.ID, true, StatoNotiziaEnum.Pubblicabile, 0, widget.NumeroElementi.Value, out c);

            model.CategoriaNotizie = widget.Categoria;
            model.Notizie = notizie;
            model.NumeroElementi = widget.NumeroElementi.Value;
            model.VoceMenu = widget.VoceMenu;

            bool home = false;
            string controller = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string action = HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            if (controller.Equals("home", StringComparison.InvariantCultureIgnoreCase) && action.Equals("index", StringComparison.InvariantCultureIgnoreCase))
                home = true;

            model.Home = home;
            
            return PartialView("WidgetNotizia", model);
        }

        [ChildActionOnly]
        public ActionResult WidgetDatoAmbientaleHome(Widget widget)
        {
            SharedWidgetDatoAmbientaleHomeModel model = new SharedWidgetDatoAmbientaleHomeModel();

            List<DatoAmbientaleHome> datiAmbientali = DatoAmbientaleHomeRepository.Instance.RecuperaDatiAmbientaliHomeIndex(widget.NumeroElementi.Value);

            model.DatiAmbientali = datiAmbientali;
            model.NumeroElementi = datiAmbientali.Count;
            model.Titolo = widget.GetNome();
            model.VoceMenu = widget.VoceMenu;

            return PartialView("WidgetDatoAmbientaleHome", model);
        }

        [ChildActionOnly]
        public ActionResult WidgetEmbed(Widget widget)
        {
            return PartialView("WidgetEmbed", widget);
        }


        [ChildActionOnly]
        public ActionResult WidgetInEvidenza(Widget widget)
        {
            SharedWidgetInEvidenzaModel model = new SharedWidgetInEvidenzaModel();
            
            List<Notizia> notizie =  InEvidenzaRepository.Instance.RecuperaAllInEvidenza(widget.ID);

            model.CategoriaNotizie = widget.Categoria;
            model.Notizie = notizie;

            model.VoceMenu = widget.VoceMenu;

            return PartialView("WidgetInEvidenza", model);
        }

        [ChildActionOnly]
        public ActionResult WidgetSezione(Widget widget)
        {
            return PartialView("WidgetSezione", widget);
        }


    }
}
