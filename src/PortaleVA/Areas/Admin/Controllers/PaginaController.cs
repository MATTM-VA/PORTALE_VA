using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Web;
using VALib.Domain.Services;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;
using VAPortale.Code;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestorePagine)]
    public class PaginaController : Controller
    {
        //
        // GET: /Admin/PaginaStatica/

        public ActionResult Index()
        {
            PaginaIndexModel model = new PaginaIndexModel();
            List<VoceMenu> vociMenu = VoceMenuRepository.Instance.RecuperaVociMenu();
            List<Tuple<VoceMenu, int?, string, PaginaStatica>> vociMenuPlus = new List<Tuple<VoceMenu, int?, string, PaginaStatica>>();

            List<int> VociMenuID = PaginaStaticaElencoRepository.Instance.RecuperaPagineStaticheVoceMenuIdElenco();

            //VoceMenu voceMenuStudiEIndaginiDiSettore = VoceMenuRepository.Instance.RecuperaVoceMenu("StudiEIndaginiDiSettore");

            foreach (VoceMenu v in vociMenu)
            {
                int? id = null;
                string action = "";
                PaginaStatica p = null;

                if (v.Editabile || v.WidgetAbilitati || VociMenuID.Contains(v.ID))
                {
                    p = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(v.ID);
                    if (p != null)
                    {
                        id = p.ID;
                        action = "Edita";
                    }
                    else
                    {
                        id = v.ID;
                        action = "Crea";
                    }

                    vociMenuPlus.Add(new Tuple<VoceMenu, int?, string, PaginaStatica>(v, id, action, p));
                }
            }

            model.VociMenu = vociMenuPlus
                .OrderBy(x => x.Item1.Sezione)
                .ThenBy(x => x.Item1.Ordine)
                .ToList();

            return View(model);
        }

        public ActionResult Crea(int id)
        {
            PaginaEditaModel model = new PaginaEditaModel();
            ActionResult result = null;

            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(id);
            if (voceMenu != null)
            {
                model.ID = id;
                model.VoceMenu = voceMenu;
                model.EditaTesto_IT = null;
                model.EditaTesto_EN = null;
                result = View("Edita", model);
            }
            else
                result = RedirectToAction("Index");

            return result;
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            PaginaEditaModel model = new PaginaEditaModel();
            PaginaStatica pagina = null;
            VoceMenu voceMenu = null;

            voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(id);

            if (voceMenu != null)
                pagina = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(voceMenu.ID);

            if (pagina != null)
            {
                model.Pagina = pagina;
                model.VoceMenu = voceMenu;

                model.ID = id;
                model.EditaTesto_IT = UrlUtility.VAHtmlReplacePseudoUrls(pagina.GetTesto("it"));
                model.EditaTesto_EN = UrlUtility.VAHtmlReplacePseudoUrls(pagina.GetTesto("en"));

                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edita(PaginaEditaModel model)
        {
            ActionResult result = null;
            PaginaStatica pagina = null;
            VoceMenu voceMenu = null;

            voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(model.ID);

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    pagina = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(model.ID);
                    pagina.Testo_IT = UrlUtility.VAHtmlReplaceRealUrls(model.EditaTesto_IT);
                    pagina.Testo_EN = UrlUtility.VAHtmlReplaceRealUrls(model.EditaTesto_EN);
                }
                else
                {
                    pagina = cs.CreaPaginaStatica(voceMenu, UrlUtility.VAHtmlReplaceRealUrls(model.EditaTesto_IT), UrlUtility.VAHtmlReplaceRealUrls(model.EditaTesto_EN));
                }

                id = cs.SalvaPaginaStatica(pagina);

                result = RedirectToAction("Edita", new { id = model.ID });
            }
            else
            {
                if (model.ID != 0)
                {
                    pagina = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(model.ID);
                    model.Pagina = pagina;
                }

                model.VoceMenu = voceMenu;

                result = View(model);
            }

            return result;
        }

        [HttpGet]
        public ActionResult EditaWidget(int id)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(id);
            PaginaStatica pagina = null;

            if (voceMenu != null)
            {
                PaginaEditaWidgetModel model = new PaginaEditaWidgetModel();
                pagina = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(id);
                int c = 0;

                model.ID = id;
                model.VoceMenu = voceMenu;
                model.Pagina = pagina;
                model.WidgetCorrelati = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(id);
                model.Widget = WidgetRepository.Instance.RecuperaWidget("", null, 0, 666, out c);

                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        [HttpPost]
        public ActionResult EditaWidget(PaginaEditaWidgetModel model)
        {
            ActionResult result = null;
            VoceMenu voceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(model.ID);

            if (voceMenu != null)
            {
                List<WidgetCorrelato> editaWidget = new List<WidgetCorrelato>();

                if (model.EditaWidget != null && model.EditaWidget.Length > 0)
                {
                    int c = 1;
                    foreach (int i in model.EditaWidget)
                    {
                        if (!editaWidget.Exists(x => x.WidgetID == i))
                        {
                            editaWidget.Add(new WidgetCorrelato() { VoceMenuID = model.ID, WidgetID = i, Ordine = c });
                            c++;
                        }
                    }
                }

                WidgetCorrelatoRepository.Instance.Elimina(model.ID);

                foreach (WidgetCorrelato w in editaWidget)
                {
                    WidgetCorrelatoRepository.Instance.InserisciWidgetCorrelato(w);
                }

                result = RedirectToAction("EditaWidget", new { id = model.ID });
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

    }
}
