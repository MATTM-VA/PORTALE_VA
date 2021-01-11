using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Models;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VAPortale.Filters;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Services;
using VALib.Helpers;
using System.Configuration;
using System.Threading.Tasks;
using VALib.Web;
using Joel.Net;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class ComunicazioneController : Controller
    {
        //
        // GET: /Comunicazione/

      

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Cittadino()
        {
            PagineVoceModel model = new PagineVoceModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("cittadino");

            if (model.VoceMenu != null)
            {
                model.PaginaStatica = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(model.VoceMenu.ID);
                return PartialView(model);
            }
            else return HttpNotFound();
        }



        public ActionResult Proponente()
        {
            ComunicazioneProponenteModel model = new ComunicazioneProponenteModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("proponente");

            List<WidgetCorrelato> widget = new List<WidgetCorrelato>();

            model.VoceMenu = voce;
            model.Links = VoceMenuRepository.Instance.RecuperaVociMenuFigliFrontEnd(voce.ID);

            return View(model);
        }

        [ChildActionOnly]
        [HttpGet]
        public ActionResult FormContatto(string tipo)
        {
            ComunicazioneFormContattoModel model = new ComunicazioneFormContattoModel();

            model.Tipo = tipo;
            model.EmailInviata = false;

            return PartialView(model);
        }


        [ValidateAntiForgeryToken]
        //[ChildActionOnly]
        [HttpPost]
        public JsonResult FormContatto(ComunicazioneFormContattoModel model)
        {
            JsonResult result = null;
            int i = 0;
            string messaggio = "";
            bool bok = true;
            bool postSpam = true;

            postSpam = false;

            if (ModelState.IsValid && !postSpam)
            {
                try
                {
                    EmailService.InvioEmail(model.IndirizzoMail, model.Testo, model.Tipo);
                }
                catch (Exception ex)
                {
                    bok = false;
                }

                if (bok)
                    i = EmailRepository.Instance.InserisciEmail(model.Testo, model.IndirizzoMail, model.Tipo, DateTime.Now);

                if (i > 0)
                {
                    model.EmailInviata = true;
                    messaggio = "Messaggio inviato con successo";
                }
                else
                {
                    model.EmailInviata = false;
                    messaggio = "Si è verificato un errore, la mail non è stata inviata";
                }
            }
            else
            {
                messaggio = "errore";
                model.EmailInviata = false;
            }

            result = Json(new object[] { model.EmailInviata, messaggio });

            return result;
        }

        private bool ContattoIsSpam(string tipo, string testo, string email, HttpContext context)
        {
            bool result = false;
            Akismet api = new Akismet(ConfigurationManager.AppSettings["AkismetApiKey"], UrlUtility.VADomain, context.Request.UserAgent);
            if (!api.VerifyKey()) throw new Exception("Akismet API key invalid.");

            //Now create an instance of AkismetComment, populating it with values
            //from the POSTed form collection.
            AkismetComment akismetComment = new AkismetComment
            {
                Blog = UrlUtility.VADomain,
                UserIp = context.Request.UserHostAddress,
                UserAgent = context.Request.UserAgent,
                CommentContent = testo,
                CommentType = "comment",
                CommentAuthor = tipo,
                CommentAuthorEmail = email,
                CommentAuthorUrl = ""
            };

            //Check if Akismet thinks this comment is spam. Returns TRUE if spam.
            result = api.CommentCheck(akismetComment);

            return result;
        }
               
        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult EventiNotizie(ComunicazioneNotizieModel model)
        {
            ActionResult result = null;
            result = ElencoNotizie(model, CategoriaNotiziaEnum.EventiENotizie);
            return result;
        }

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult DirezioneInforma(ComunicazioneNotizieModel model)
        {
            ActionResult result = null;
            result = ElencoNotizie(model, CategoriaNotiziaEnum.LaDirezioneInforma);
            return result;
        }

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AreaGiuridica(ComunicazioneNotizieModel model)
        {
            ActionResult result = null;

            result = ElencoNotizie(model, CategoriaNotiziaEnum.AreaGiuridica);
            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult UltimiProvvedimenti(ComunicazioneProvvedimentiModel model)
        {
            model.AnnoInCorso = true;
            model.Anno = DateTime.Now.Year;
            int totale = 0;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie(CultureHelper.GetCurrentCultureShortName(), model.Testo, model.AnnoInCorso, model.Anno, (int)CategoriaNotiziaEnum.UltimiProvvedimenti, true, StatoNotiziaEnum.Pubblicabile, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.Notizie = notizie;
            model.TotaleRisultati = totale;

            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("UltimiProvvedimenti");

            model.VoceMenu = voce;

            return View(model);
        }

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ArchivioProvvedimenti(ComunicazioneProvvedimentiModel model)
        {
            model.AnnoInCorso = false;
            model.Anno = DateTime.Now.Year;
            int totale = 0;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie(CultureHelper.GetCurrentCultureShortName(), model.Testo, model.AnnoInCorso, model.Anno, (int)CategoriaNotiziaEnum.UltimiProvvedimenti, true, StatoNotiziaEnum.Pubblicabile, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.Notizie = notizie;
            model.TotaleRisultati = totale;

            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("ArchivioProvvedimenti");

        
            model.VoceMenu = voce;

            return View(model);
        }

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        private ActionResult ElencoNotizie(ComunicazioneNotizieModel model, CategoriaNotiziaEnum categoriaEnum)
        {
            int categoriaID = (int)categoriaEnum;
            CategoriaNotizia categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(categoriaID);
            int totale = 0;

            List<Notizia> notizie = NotiziaRepository.Instance.RecuperaNotizie(CultureHelper.GetCurrentCultureShortName(), model.Testo, null, null, categoria.ID, true, StatoNotiziaEnum.Pubblicabile, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.Notizie = notizie;
            model.Categoria = categoria;
            model.TotaleRisultati = totale;

            switch (categoriaEnum)
            {
                case CategoriaNotiziaEnum.Nessuna:
                    break;
                case CategoriaNotiziaEnum.EventiENotizie:
                    model.ActionDettaglio = "DettaglioNotizia";
                    model.ActionAttuale = "EventiNotizie";
                    break;
                case CategoriaNotiziaEnum.LaDirezioneInforma:
                    model.ActionDettaglio = "DettaglioDirezione";
                    model.ActionAttuale = "DirezioneInforma";
                    break;
                case CategoriaNotiziaEnum.AreaGiuridica:
                    model.ActionDettaglio = "DettaglioAreaGiuridica";
                    model.ActionAttuale = "AreaGiuridica";
                    break;
                case CategoriaNotiziaEnum.UltimiProvvedimenti:
                    model.ActionDettaglio = "DettaglioUltimiProvvedimenti";
                    model.ActionAttuale = "UltimiProvvedimenti";
                    break;
                default:
                    break;
            }

            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu(model.ActionAttuale);

            model.VoceMenu = voce;

            return View("NotizieElenco", model);
        }

        public ActionResult DettaglioNotizia(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioNotizia");

            result = Dettaglio(id, voceMenu);
            return result;
        }

        public ActionResult DettaglioDirezione(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioDirezione");

            result = Dettaglio(id, voceMenu);
            return result;
        }

        public ActionResult DettaglioAreaGiuridica(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioAreaGiuridica");

            result = Dettaglio(id, voceMenu);
            return result;
        }

        public ActionResult DettaglioUltimiProvvedimenti(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioUltimiProvvedimenti");

            result = Dettaglio(id, voceMenu);
            return result;
        }

        public ActionResult DettaglioArchivioProvvedimenti(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioArchivioProvvedimenti");

            result = Dettaglio(id, voceMenu);
            return result;
        }

        public ActionResult ElencoNotizieFeed(string categoria)
        {
            ActionResult result = null;
            string action = "";

            CategoriaNotiziaEnum cat = CategoriaNotiziaEnum.Nessuna;

            if (Enum.IsDefined(typeof(CategoriaNotiziaEnum), categoria))
            {
                cat = (CategoriaNotiziaEnum)Enum.Parse(typeof(CategoriaNotiziaEnum), categoria);

                switch (cat)
                {
                    case CategoriaNotiziaEnum.Nessuna:
                        break;
                    case CategoriaNotiziaEnum.EventiENotizie:
                        action = "EventiNotizie";
                        break;
                    case CategoriaNotiziaEnum.LaDirezioneInforma:
                        action = "DirezioneInforma";
                        break;
                    case CategoriaNotiziaEnum.AreaGiuridica:
                        action = "AreaGiuridica";
                        break;
                    case CategoriaNotiziaEnum.UltimiProvvedimenti:
                        action = "UltimiProvvedimenti";
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(action))
                result = RedirectToActionPermanent(action);
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult DettaglioNotizieFeed(string categoria, int id)
        {
            ActionResult result = null;
            string action = "";

            CategoriaNotiziaEnum cat = CategoriaNotiziaEnum.Nessuna;

            if (Enum.IsDefined(typeof(CategoriaNotiziaEnum), categoria))
            {
                cat = (CategoriaNotiziaEnum)Enum.Parse(typeof(CategoriaNotiziaEnum), categoria);

                switch (cat)
                {
                    case CategoriaNotiziaEnum.Nessuna:
                        break;
                    case CategoriaNotiziaEnum.EventiENotizie:
                        action = "DettaglioNotizia";
                        break;
                    case CategoriaNotiziaEnum.LaDirezioneInforma:
                        action = "DettaglioDirezione";
                        break;
                    case CategoriaNotiziaEnum.AreaGiuridica:
                        action = "DettaglioAreaGiuridica";
                        break;
                    case CategoriaNotiziaEnum.UltimiProvvedimenti:
                        action = "DettaglioUltimiProvvedimenti";
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(action))
                result = RedirectToActionPermanent(action, new { id = id });
            else
                result = HttpNotFound();

            return result;
        }

        private ActionResult Dettaglio(int id, VoceMenu voceMenu)
        {
            ActionResult result = null;

            ComunicazioneDettaglioModel model = new ComunicazioneDettaglioModel();
            Notizia notizia = NotiziaRepository.Instance.RecuperaNotizia(id);

            if (notizia != null && notizia.Pubblicata)
            {
                model.Notizia = notizia;
                model.VoceMenu = voceMenu;
                result = View("Dettaglio", model);
            }
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult DomandeFrequenti()
        {
            ComunicazioneCittadinoModel model = new ComunicazioneCittadinoModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("cittadino");

            List<WidgetCorrelato> widget = new List<WidgetCorrelato>();
            model.VoceMenu = voce;
            model.Links = VoceMenuRepository.Instance.RecuperaVociMenuFigliFrontEnd(voce.ID);

            return View(model);
        }

        public ActionResult OsservatorioILVA(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("DettaglioNotizia");

            result = Dettaglio(id, voceMenu);
            return result;
        }
    }
}
