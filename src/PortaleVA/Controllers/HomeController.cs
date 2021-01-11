using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VAPortale.Models;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.UI;
using VAPortale.Filters;
using VALib.Domain.Services;
using VALib.Domain.Entities.UI;
using VALib.Helpers;
using VALib.Domain.Repositories.Contenuti;
using System.Net.Mail;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string lang)
        {
            ActionResult result = null;
          
            if (!string.IsNullOrWhiteSpace(lang))
            {
                HomeIndexModel model = new HomeIndexModel();

                List<List<OggettoHome>> listeOggetti = OggettoHomeRepository.Instance.RecuperaOggettiHome();
           
                List<WidgetCorrelato> widget = new List<WidgetCorrelato>();

                widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(1);

                model.Widget = widget;
                
                model.Oggetti = listeOggetti[0];
            
                result = View(model);
            }
            else
                result = RedirectToRoutePermanent("Home_Index", new { lang = "it-IT" });


            return result;
        }

        [ChildActionOnly]
        public ActionResult ElencoOggetti(HomeElencoOggettiModel model)
        {
            switch (model.MacroTipoOggetto)
            {
                case MacroTipoOggettoEnum.Via:
                    model.MessaggioElencoVuoto = DizionarioService.HOME_OggettiConsultazioneNoRisultatiVIA;
                    break;
                case MacroTipoOggettoEnum.Vas:
                    model.MessaggioElencoVuoto = DizionarioService.HOME_OggettiConsultazioneNoRisultatiVAS;
                    break;
                case MacroTipoOggettoEnum.Aia:
                    model.MessaggioElencoVuoto = DizionarioService.HOME_OggettiConsultazioneNoRisultatiAIA;
                    break;

                default:
                    model.MessaggioElencoVuoto = "";
                    break;
            }
            
            
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ElencoProvvedimenti(HomeElencoProvvedimentiModel model)
        {
            model.MessaggioElencoVuoto = DizionarioService.HOME_ProvvedimentiConsultazioneNoRisultati;

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Carosello()
        {
            HomeCaroselloModel model = new HomeCaroselloModel();

            model.Elementi = OggettoCaroselloRepository.Instance.RecuperaOggettiCaroselloHome();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Icone()
        {
            HomeIconeModel model = new HomeIconeModel();
            model.VociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();

            return PartialView(model);
        }

    
        public ActionResult Mappa()
        {
            HomeMappaModel model = new HomeMappaModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("Mappa");
            model.VoceMenu = voce;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult VociMenuRootNodes()
        {
            VociMenuTreeViewModel model = new VociMenuTreeViewModel();
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu().Where(x => x.VisibileMappa).ToList();

            model.VociMenu = vociMenu.Where(x => x.GenitoreID == 0).ToList();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult VociMenuChildNodes(int genitoreID)
        {
            VociMenuTreeViewModel model = new VociMenuTreeViewModel();
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu().Where(x => x.VisibileMappa).ToList();

            model.VociMenu = vociMenu.Where(x => x.GenitoreID == genitoreID).ToList();

            return PartialView(model);
        }

        public ActionResult CercaSito(HomeCercaSitoModel model)
        {
            int totale = 0;
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("CercaSito");
            model.Widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(model.VoceMenu.ID);

            List<PaginaStaticaElenco> pagine = PaginaStaticaElencoRepository.Instance.RecuperaPagineStaticheElenco(CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
            model.TotaleRisultati = totale;
            model.Pagine = pagine;


            return View(model);
        }

        public ActionResult Robots()
        {
            ActionResult result = null;

            string filepath = Server.MapPath("~/Content/RootContent/robots.txt");

            if (System.IO.File.Exists(filepath))
            {
                result = File(filepath, "text/plain");
            }
            else
            {
                 result = HttpNotFound();
            }

            return result;
        }

        public ActionResult Feed()
        {
            HomeFeedModel model = new HomeFeedModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("Feed");
            model.Widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(voce.ID);

            model.VoceMenu = voce;

            return View(model);
        }

    }
}
