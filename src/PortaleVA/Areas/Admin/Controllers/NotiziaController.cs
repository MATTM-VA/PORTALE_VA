using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Repositories.Contenuti;
using VAPortale.Code;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Services;
using VALib.Web;
using System.Web.Script.Serialization;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles=RuoloUtenteCodici.GestoreNotizie)]
    public class NotiziaController : Controller
    {
        //
        // GET: /Admin/Notizie/

        public ActionResult Index(NotiziaIndexModel model)
        {
            ContenutoService cs = new ContenutoService();
            int totale = 0;
            string testo = string.IsNullOrWhiteSpace(model.Testo) ? "" : model.Testo;
            model.DefaultImmagineID = cs.DefaultImmagineID;

            model.Notizie = NotiziaRepository.Instance.RecuperaNotizie(testo, model.CategoriaNotiziaID, model.Pubblicato, model.Stato, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);
            
            model.BooleanSelectList = ModelUtils.CreaBooleanSelectList();
            model.CategoriaSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);
            model.StatoSelectList = ModelUtils.CreaStatoNotiziaSelectList(true);

            model.TotaleRisultati = totale;
            
            return View(model);
        }

        public ActionResult Crea(int immagineID = 0, int categoriaNotiziaID = 0)
        {
            NotiziaEditaModel model = new NotiziaEditaModel();

            model.Data = DateTime.Now;
            model.StatoNotizia = StatoNotiziaEnum.Bozza;
            model.CategoriaNotiziaID = categoriaNotiziaID;
            model.ImmagineID = immagineID;

            model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);
            model.ImmaginiSelectList = ModelUtils.CreaImmaginiSelectList(true);
            model.StatiSelectList = ModelUtils.CreaStatoNotiziaSelectList(false);

            return View("Edita", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            NotiziaEditaModel model = new NotiziaEditaModel();
            Notizia notizia = null;

            notizia = NotiziaRepository.Instance.RecuperaNotizia(id);

            if (notizia != null)
            {
                model.Notizia = notizia;

                model.ID = id;
                model.CategoriaNotiziaID = notizia.Categoria.ID;
                model.ImmagineID = notizia.ImmagineID;
                model.Data = notizia.Data;
                model.Titolo_IT = notizia.GetTitolo("it");
                model.Titolo_EN = notizia.GetTitolo("en");
                model.TitoloBreve_IT = notizia.GetTitoloBreve("it");
                model.TitoloBreve_EN = notizia.GetTitoloBreve("en");
                model.Abstract_IT = notizia.GetAbstract("it");
                model.Abstract_EN = notizia.GetAbstract("en");
                model.Testo_IT = UrlUtility.VAHtmlReplacePseudoUrls(notizia.GetTesto("it"));
                model.Testo_EN = UrlUtility.VAHtmlReplacePseudoUrls(notizia.GetTesto("en"));
                model.Pubblicata = notizia.Pubblicata;
                model.StatoNotizia = notizia.Stato;
                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiSelectList(true);
                model.StatiSelectList = ModelUtils.CreaStatoNotiziaSelectList(false);
                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edita(NotiziaEditaModel model)
        {
            ActionResult result = null;
            Notizia notizia = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                CategoriaNotizia categoria = null;
                ContenutoService cs = new ContenutoService();

                categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(model.CategoriaNotiziaID.Value);

                if (model.ID != 0)
                {
                    notizia = NotiziaRepository.Instance.RecuperaNotizia(model.ID);
                    notizia.Categoria = categoria;
                    notizia.ImmagineID = model.ImmagineID.Value;
                    notizia.Data = model.Data;
                    notizia.Titolo_IT = model.Titolo_IT;
                    notizia.Titolo_EN = model.Titolo_EN;
                    notizia.TitoloBreve_IT = model.TitoloBreve_IT;
                    notizia.TitoloBreve_EN = model.TitoloBreve_EN;
                    notizia.Abstract_IT = model.Abstract_IT;
                    notizia.Abstract_EN = model.Abstract_EN;
                    notizia.Testo_IT = UrlUtility.VAHtmlReplaceRealUrls(model.Testo_IT);
                    notizia.Testo_EN = UrlUtility.VAHtmlReplaceRealUrls(model.Testo_EN);
                    //notizia.Stato = model.StatoNotizia;
                }
                else
                {
                    notizia = cs.CreaNotizia(categoria, model.ImmagineID.Value, model.Data, model.Titolo_IT, model.Titolo_EN, model.TitoloBreve_IT, model.TitoloBreve_EN, model.Abstract_IT, model.Abstract_EN, model.Testo_IT, model.Testo_EN);
                }

                id = cs.SalvaNotizia(notizia);

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    notizia = NotiziaRepository.Instance.RecuperaNotizia(model.ID);
                    model.Notizia = notizia;
                }
                model.CategorieSelectList = ModelUtils.CreaCategoriaNotiziaSelectList(true);
                model.ImmaginiSelectList = ModelUtils.CreaImmaginiSelectList(true);
                model.StatiSelectList = ModelUtils.CreaStatoNotiziaSelectList(false);

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public JsonResult EditaPubblicato(int id, bool editaPubblicato)
        {
            JsonResult result = null;
            Notizia notizia = NotiziaRepository.Instance.RecuperaNotizia(id);
            bool bOk = false;

            if (notizia != null)
            {
                if (!editaPubblicato)
                    bOk = true;
                else
                {
                    if (notizia.Stato == StatoNotiziaEnum.Pubblicabile)
                        bOk = true;
                }

                if (bOk)
                {
                    notizia.Pubblicata = editaPubblicato;

                    ContenutoService cs = new ContenutoService();
                    cs.SalvaNotizia(notizia);

                    result = Json(new object[] { notizia.Pubblicata, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "ok" });
                }
                else
                    result = Json(new object[] { notizia.Pubblicata, notizia.DataUltimaModifica.ToString("dd/MM/yyyy HH:mm"), "La notizia non può essere pubblicata perchè non è nello stato 'pubblicabile'" });

            }
            else
            {
                result = Json(new object[] { null, null, "error" });
            }

            return result;
        }

        [HttpPost]
        public JsonResult EditaStato(int id, StatoNotiziaEnum statoNotizia)
        {
            JsonResult result = null;
            Notizia notizia = NotiziaRepository.Instance.RecuperaNotizia(id);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<string> messaggi = new List<string>();
            bool bOk = false;

            if (notizia != null)
            {
                ContenutoService cs = new ContenutoService();

                if (statoNotizia == StatoNotiziaEnum.Bozza)
                {
                    bOk = true;
                    notizia.Pubblicata = false;
                }
                else if (statoNotizia == StatoNotiziaEnum.Pubblicabile)
                {
                    messaggi = cs.NotiziaPubblicabile(notizia);
                }

                if (messaggi == null || messaggi.Count == 0)
                    bOk = true;

                if (bOk)
                {
                    messaggi.Insert(0, "ok");
                    notizia.Stato = statoNotizia;
                    cs.SalvaNotizia(notizia);

                    result = Json(new object[] { notizia.Pubblicata, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), serializer.Serialize(messaggi) });
                }
                else
                {
                    messaggi.Insert(0, "validation errors");


                    result = Json(new object[] { notizia.Pubblicata, notizia.DataUltimaModifica.ToString("dd/MM/yyyy HH:mm"), serializer.Serialize(messaggi) });
                }
            }
            else
            {
                messaggi.Insert(0, "error");
                result = Json(new object[] { null, null, serializer.Serialize(messaggi) });
            }

            return result;
        }

        [HttpPost]
        public ActionResult Elimina(int id)
        {
            ActionResult result = null;

            NotiziaRepository.Instance.Elimina(id);

            result = RedirectToAction("Index");

            return result;
        }

    }
}
