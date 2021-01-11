using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Repositories.Contenuti;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Services;
using VALib.Web;
using System.IO;
using System.Globalization;
using VALib.Domain.Entities.Membership;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreDocumentoPortale)]
    public class DocumentoPortaleController : Controller
    {
        //
        // GET: /Admin/DocumentoPortale/

        public ActionResult Index(DocumentoPortaleIndexModel model)
        {
            int totale = 0;
            model.DocumentiPortale = DocumentoPortaleRepository.Instance.RecuperaDocumentiPortale(model.Testo, model.IndiceInizio, model.IndiceInizio + model.DimensionePagina, out totale);

            model.TotaleRisultati = totale;

            return View(model);
        }

        public ActionResult Crea()
        {
            DocumentoPortaleEditaModel model = new DocumentoPortaleEditaModel();

            model.EditaNuovoFile = true;

            return View("Edita", model);
        }

        [HttpGet]
        public ActionResult Edita(int id)
        {
            ActionResult result = null;
            DocumentoPortaleEditaModel model = new DocumentoPortaleEditaModel();
            DocumentoPortale documentoPortale = null;

            documentoPortale = DocumentoPortaleRepository.Instance.RecuperaDocumentoPortale(id);

            if (documentoPortale != null)
            {
                model.Documento = documentoPortale;

                model.ID = id;
                model.EditaNome_IT = documentoPortale.Nome_IT;
                model.EditaNome_EN = documentoPortale.Nome_EN;
                model.EditaNomeFileOriginale = documentoPortale.NomeFileOriginale;
                model.EditaDimensione = documentoPortale.Dimensione;
                model.EditaNuovoFile = false;

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
        public ActionResult Edita(DocumentoPortaleEditaModel model)
        {
            ActionResult result = null;
            DocumentoPortale documentoPortale = null;

            if (ModelState.IsValid)
            {
                int id = 0;
                ContenutoService cs = new ContenutoService();
                string ext = Path.GetExtension(model.EditaNomeFileOriginale);
                TipoFile tipoFile = TipoFileRepository.Instance.RecuperaTipoFile(ext);

                if (model.ID != 0)
                {
                    documentoPortale = DocumentoPortaleRepository.Instance.RecuperaDocumentoPortale(model.ID);
                    documentoPortale.Nome_IT = model.EditaNome_IT.Trim();
                    documentoPortale.Nome_EN = model.EditaNome_EN.Trim();
                }
                else
                {
                    documentoPortale = cs.CreaDocumentoPortale(model.EditaNome_IT.Trim(), model.EditaNome_EN.Trim());
                }

                if (model.EditaNuovoFile)
                {
                    documentoPortale.TipoFile = tipoFile;
                    documentoPortale.NomeFileOriginale = model.EditaNomeFileOriginale;
                    documentoPortale.Dimensione = model.EditaDimensione;
                }

                id = cs.SalvaDocumentoPortale(documentoPortale);

                 
                if (model.EditaNuovoFile)
                {
                    FileUtility.VASpostaDocumentoPortaleTemp(id, model.EditaNomeFileOriginale);
                }

                result = RedirectToAction("Edita", new { id = id });
            }
            else
            {
                if (model.ID != 0)
                {
                    documentoPortale = DocumentoPortaleRepository.Instance.RecuperaDocumentoPortale(model.ID);
                    model.Documento = documentoPortale;
                }

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public JsonResult DocumentoPortaleUpload(int chunk, int chunks, string name, HttpPostedFileBase file)
        {
            JsonResult result = null;
            object resultObj = null;
            bool fileCompletato = false;

            try
            {
                string filePath = FileUtility.VADocumentoPortaleTemp(name);
                fileCompletato = FileUtility.VASalvaDocumentoPortaleTempChunk(filePath, chunk, chunks, file);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;

                resultObj = new { Status = "error", Message = "Si è verificato un errore sil server. Il file " + name + " non è stato caricato.", NomeFile = name };
            }

            if (resultObj == null)
            {
                
                if (fileCompletato)
                {
                    if (resultObj == null)
                        resultObj = new { Status = "ok", Message = "Caricamento file completato.", NomeFile = name };
                }

                if (resultObj == null)
                    resultObj = new { Status = "ok", Message = "Caricamento chunk completato.", NomeFile = name };
            }

            Response.TrySkipIisCustomErrors = true;

            result = Json(resultObj);

            return result;
        }

        [HttpPost]
        public ActionResult Elimina(int id)
        {
            ActionResult result = null;

            DocumentoPortale d = DocumentoPortaleRepository.Instance.RecuperaDocumentoPortale(id);

            if (d != null)
            {
                string ext = Path.GetExtension(d.NomeFileOriginale);
                FileUtility.VAEliminaDocumentoPortale(d.ID + ext);
                DocumentoPortaleRepository.Instance.Elimina(id);
            }
            
            result = RedirectToAction("Index");

            return result;
        }

    }
}
