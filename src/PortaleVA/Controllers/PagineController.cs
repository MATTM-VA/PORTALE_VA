using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VAPortale.Models;
using VAPortale.Filters;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class PagineController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            String Controller = filterContext.RouteData.Values["Controller"] as String;
            String Action = filterContext.RouteData.Values["Action"] as String;
            String Sezione = filterContext.RouteData.Values["NomeSezione"] as String;
            String Voce = filterContext.RouteData.Values["NomeVoce"] as String;

            var Redirect = new[]{
                new {Sezione = "DatiEStrumenti",  Voce = "SpecificheTecnicheELineeGuida", result = RedirectToRoute(new { controller = "Pagine", action = "Voce", nomeSezione = "DatiEStrumenti", nomeVoce = "Modulistica" })},
                new {Sezione = "Procedure",  Voce = "Statistiche", result = RedirectToRoute(new { controller = "Procedure", action = "Statistiche", anno = DateTime.Now.Year })},
                new {Sezione = "Comunicazione",  Voce = "QualiSonoLePrincipaliDifferenze", result = RedirectToRoute(new { controller = "Comunicazione", action = "Cittadino" })}
            };
            ActionResult result = Redirect.Where(x => x.Sezione.Equals(Sezione) && x.Voce.Equals(Voce)).Select(x => (ActionResult)x.result).FirstOrDefault();
            if (result != null) filterContext.Result = result;

        }


        // GET: /Pagine/
        public ActionResult Voce(string nomeSezione, string nomeVoce)
        {
            ActionResult result = null;
            PagineVoceModel model = new PagineVoceModel();
            VoceMenu voceMenu = null;
            PaginaStatica paginaStatica = null;
            List<WidgetCorrelato> widget = new List<WidgetCorrelato>();

            voceMenu = VoceMenuRepository.Instance.RecuperaVociMenu().SingleOrDefault(x => x.Sezione.Equals(nomeSezione, StringComparison.InvariantCultureIgnoreCase) && x.Voce.Equals(nomeVoce, StringComparison.InvariantCultureIgnoreCase));

            if (voceMenu != null)
            {
                paginaStatica = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(voceMenu.ID);
                widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(voceMenu.ID);

                model.PaginaStatica = paginaStatica;
                model.VoceMenu = voceMenu;
                model.Widget = widget;
                result = View(model);
            }
            else
                result = HttpNotFound();

            return result;
        }


    }
}
