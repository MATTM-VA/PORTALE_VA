using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.UI;
using VALib.Domain.Services;
using VAPortale.Code;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreDatiAmbientaliHome)]
    public class DatoAmbientaleHomeController : Controller
    {
        //
        // GET: /Admin/DatoAmbientaleHome/

        public ActionResult Index(DatoAmbientaleHomeIndexModel model)
        {
            int totale = 0;
            string testo = string.IsNullOrWhiteSpace(model.Testo) ? "" : model.Testo;

            model.DatiAmbientaliHome = DatoAmbientaleHomeRepository.Instance.RecuperaDatiAmbientaliHome(testo, model.Pubblicato, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.BooleanSelectList = ModelUtils.CreaBooleanSelectList();

            model.TotaleRisultati = totale;

            return View(model);
        }

        public ActionResult Crea()
        {
            DatoAmbientaleHomeEditaModel model = new DatoAmbientaleHomeEditaModel();

            model.ImmaginiSelectList = ModelUtils.CreaImmaginiDatoAmbientaleHomeSelectList(true);

            return View("Edita", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            DatoAmbientaleHomeEditaModel model = new DatoAmbientaleHomeEditaModel();
            DatoAmbientaleHome datiAmbientaleHome = null;

            datiAmbientaleHome = DatoAmbientaleHomeRepository.Instance.RecuperaDatoAmbientaleHome(id);

            if (datiAmbientaleHome != null)
            {
                model.DatoAmbientaleHome = datiAmbientaleHome;

                model.ID = id;
                model.EditaImmagineID = datiAmbientaleHome.ImmagineID;
                model.EditaTitolo_IT = datiAmbientaleHome.Titolo_IT;
                model.EditaTitolo_EN = datiAmbientaleHome.Titolo_EN;
                model.EditaLink = datiAmbientaleHome.Link;
                model.EditaPubblicato = datiAmbientaleHome.Pubblicato;
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiDatoAmbientaleHomeSelectList(true);
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
        public ActionResult Edita(DatoAmbientaleHomeEditaModel model)
        {
            ActionResult result = null;
            DatoAmbientaleHome datiAmbientaleHome = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    datiAmbientaleHome = DatoAmbientaleHomeRepository.Instance.RecuperaDatoAmbientaleHome(model.ID);
                    datiAmbientaleHome.ImmagineID = model.EditaImmagineID.Value;
                    datiAmbientaleHome.Titolo_IT = model.EditaTitolo_IT;
                    datiAmbientaleHome.Titolo_EN = model.EditaTitolo_EN;
                    datiAmbientaleHome.Link = model.EditaLink;
                }
                else
                {
                    datiAmbientaleHome = cs.CreaDatoAmbientaleHome(model.EditaImmagineID.Value, model.EditaTitolo_IT.Trim(), model.EditaTitolo_EN.Trim(), model.EditaLink.Trim());
                }

                id = cs.SalvaDatoAmbientaleHome(datiAmbientaleHome);

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    datiAmbientaleHome = DatoAmbientaleHomeRepository.Instance.RecuperaDatoAmbientaleHome(model.ID);
                    model.DatoAmbientaleHome = datiAmbientaleHome;
                }
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiDatoAmbientaleHomeSelectList(true);

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public JsonResult EditaPubblicato(int id, bool editaPubblicato)
        {
            JsonResult result = null;
            DatoAmbientaleHome datiAmbientaleHome = DatoAmbientaleHomeRepository.Instance.RecuperaDatoAmbientaleHome(id);

            if (datiAmbientaleHome != null)
            {
                datiAmbientaleHome.Pubblicato = editaPubblicato;

                ContenutoService cs = new ContenutoService();
                cs.SalvaDatoAmbientaleHome(datiAmbientaleHome);

                result = Json(new object[] { datiAmbientaleHome.Pubblicato, datiAmbientaleHome.DataUltimaModifica.ToString("dd/MM/yyyy HH:mm") });
            }
            else
            {
                result = Json(null);
            }

            return result;
        }

        [HttpPost]
        public ActionResult Elimina(int id)
        {
            ActionResult result = null;

            DatoAmbientaleHomeRepository.Instance.Elimina(id);

            result = RedirectToAction("Index");

            return result;
        }

    }
}
