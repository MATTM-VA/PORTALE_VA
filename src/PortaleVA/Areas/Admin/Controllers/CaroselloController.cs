using System;
using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Repositories.Contenuti;
using VALib.Domain.Entities.UI;
using VALib.Domain.Services;
using VAPortale.Code;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreCaroselloHome)]
    public class CaroselloController : Controller
    {
        //
        // GET: /Admin/Carosello/

        public ActionResult Index(CaroselloIndexModel model)
        {
            model.OggettiCarosello = OggettoCaroselloRepository.Instance.RecuperaOggettiCarosello(null);

            return View(model);
        }

        public ActionResult Crea()
        {
            CaroselloEditaModel model = new CaroselloEditaModel();

            model.EditaData = DateTime.Now;
            model.ImmaginiSelectList = ModelUtils.CreaImmaginiCaroselloSelectList(true);
            model.EditaTipoContenutoID = ContenutoOggettoCaroselloTipo.Oggetto;

            return View("Edita", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            CaroselloEditaModel model = new CaroselloEditaModel();
            OggettoCarosello oggettoCarosello = null;

            oggettoCarosello = OggettoCaroselloRepository.Instance.RecuperaOggettoCarosello(id);

            if (oggettoCarosello != null)
            {
                model.OggettoCarosello = oggettoCarosello;

                model.ID = id;
                model.EditaContenutoID = oggettoCarosello.ContenutoID;
                model.EditaImmagineID = oggettoCarosello.ImmagineID;
                model.EditaData = oggettoCarosello.Data;
                model.EditaNome_IT = oggettoCarosello.Nome_IT;
                model.EditaNome_EN = oggettoCarosello.Nome_EN;
                model.EditaDescrizione_IT = oggettoCarosello.Descrizione_IT;
                model.EditaDescrizione_EN = oggettoCarosello.Descrizione_EN;
                model.EditaLinkProgettoCartografico = oggettoCarosello.LinkProgettoCartografico;
                model.EditaPubblicato = oggettoCarosello.Pubblicato;
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiCaroselloSelectList(true);
                model.ImmagineMasterID = ImmagineRepository.Instance.RecuperaImmagine((int)model.EditaImmagineID).ImmagineMasterID;
                model.EditaTipoContenutoID = oggettoCarosello.TipoContenuto;
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
        public ActionResult Edita(CaroselloEditaModel model)
        {
            ActionResult result = null;
            OggettoCarosello oggettoCarosello = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();

                if (model.ID != 0)
                {
                    oggettoCarosello = OggettoCaroselloRepository.Instance.RecuperaOggettoCarosello(model.ID);
                    oggettoCarosello.ContenutoID = model.EditaContenutoID.Value;
                    oggettoCarosello.ImmagineID = model.EditaImmagineID.Value;
                    oggettoCarosello.Data = model.EditaData;
                    oggettoCarosello.Nome_IT = model.EditaNome_IT;
                    oggettoCarosello.Nome_EN = model.EditaNome_EN;
                    oggettoCarosello.Descrizione_IT = model.EditaDescrizione_IT;
                    oggettoCarosello.Descrizione_EN = model.EditaDescrizione_EN;
                    oggettoCarosello.LinkProgettoCartografico = model.EditaLinkProgettoCartografico;
                    oggettoCarosello.TipoContenuto = model.EditaTipoContenutoID;
                }
                else
                {
                    oggettoCarosello = cs.CreaOggettoCarosello(model.EditaContenutoID.Value, model.EditaTipoContenutoID, model.EditaData,
                                                            model.EditaNome_IT, model.EditaNome_EN,
                                                            model.EditaDescrizione_IT, model.EditaDescrizione_EN);
                }

                oggettoCarosello.LinkProgettoCartografico = model.EditaLinkProgettoCartografico;
                oggettoCarosello.ImmagineID = model.EditaImmagineID.Value;

                id = cs.SalvaOggettoCarosello(oggettoCarosello);

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    oggettoCarosello = OggettoCaroselloRepository.Instance.RecuperaOggettoCarosello(model.ID);
                    model.OggettoCarosello = oggettoCarosello;
                }
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiCaroselloSelectList(true);

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public JsonResult EditaPubblicato(int id, bool editaPubblicato)
        {
            JsonResult result = null;
            OggettoCarosello oggettoCarosello = OggettoCaroselloRepository.Instance.RecuperaOggettoCarosello(id);

            if (oggettoCarosello != null)
            {
                oggettoCarosello.Pubblicato = editaPubblicato;

                ContenutoService cs = new ContenutoService();
                cs.SalvaOggettoCarosello(oggettoCarosello);

                result = Json(new object[] { oggettoCarosello.Pubblicato, oggettoCarosello.DataUltimaModifica.ToString("dd/MM/yyyy HH:mm") });
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

            OggettoCaroselloRepository.Instance.Elimina(id);

            result = RedirectToAction("Index");

            return result;
        }

    }
}
